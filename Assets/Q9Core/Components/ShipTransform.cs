using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;
using Q9Core.CommonData;

public class ShipTransform : MonoBehaviour {

    [Serializable]
    public enum ShipState
    {
        Idle,
        Aligning,
        Warping
    }

    public bool isPlayerShip;

    [Header("Stats")]
    public float _torque;
    public float _power;
    public float _burnSpeed;
    public double _warpStrength;
    public double _warpSpeed;
    public float _inertia;
    public float _mass;

    [Header("Travel")]
    private ShipState state = ShipState.Idle;
    private bool wantToWarp;
    private float currentThrottle = 0;
    public float CurrentThrottle
    {
        get { return currentThrottle; }
    }
    private float wantedThrottle = 0;
    private Quaternion currentRotation;
    private Quaternion wantedRotation;
    private GameObject alignmentTarget;
    private bool aligned;

    [Header("Debug")]
    private Quaternion lastFrameRotation;
    private Quaternion thisFrameRotation;
    public Vector3 rotationalVelocity;
    public float wantedBank;
    public float currentBank;
    public float bankMultiplier;
    public GameObject shipModel;

    //Warping
    private float warpStartTime;
    private float wantedWarpSpeed;
    private float currentWarpSpeed;
    #region warpCurve
    public AnimationCurve warpAccelerationCurve = new AnimationCurve(new Keyframe(0, 0),
                                                                     new Keyframe(.1f, .000000001f),
                                                                     new Keyframe(.2f, .00000001f),
                                                                     new Keyframe(.3f, .0000001f),
                                                                     new Keyframe(.4f, .000001f),
                                                                     new Keyframe(.5f, .00001f),
                                                                     new Keyframe(.6f, .0001f),
                                                                     new Keyframe(.7f, .001f),
                                                                     new Keyframe(.8f, .01f),
                                                                     new Keyframe(.9f, .1f),
                                                                     new Keyframe(1, 1));
    #endregion

    private void Awake()
    {
        EventManager.OnSystemChanged += OnSystemChanged;
        EventManager.OnObjectSelectedAsAlignmentTarget += Align;

        currentRotation = transform.rotation;
        wantedRotation = transform.rotation;
    }

    private void FixedUpdate()
    {
        if (isPlayerShip)
        {
            ScaleSpaceManager.Translate(DoubleVector3.FromVector3(transform.forward) * 0);
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.Space))
            {
                if (state != ShipState.Idle)
                {
                    AllStop();
                }
            }

            if (state != ShipState.Warping)
            {
                if (Quaternion.Angle(transform.rotation, wantedRotation) <= 2.5f && CurrentThrottle >= .75f && wantToWarp)
                {
                    warpStartTime = Time.time;
                    state = ShipState.Warping;
                    print("Warping");
                }
            }
            else
            {
                Warp();
            }
        }

        #region calculate rotation speed
        lastFrameRotation = thisFrameRotation;
        thisFrameRotation = transform.rotation;
        rotationalVelocity = Quaternion.ToEulerAngles(thisFrameRotation) - Quaternion.ToEulerAngles(lastFrameRotation);
        #endregion

        Move();
        Rotate();
        Bank();
    }

    private void OnSystemChanged(Vector2 dir)
    {
        Flip(dir);
    }

    private void Flip(Vector2 dir)
    {
        double x = ScaleSpaceManager.apparentPosition.x * -Mathf.Abs(dir.x);
        double y = ScaleSpaceManager.apparentPosition.y;
        double z = ScaleSpaceManager.apparentPosition.z * -Mathf.Abs(dir.y);

        DoubleVector3 newPos = new DoubleVector3(x, y, z);
        ScaleSpaceManager.SetApparentPosition(newPos);
    }

    private void AllStop()
    {
        EventManager.NotifyShipStopped();
        SetThrottle(0);
        wantToWarp = false;
        state = ShipState.Idle;
    }

    private void Align(GameObject target, bool warpWhenReady)
    {
        wantToWarp = (Vector3.Distance(target.transform.position, Vector3.zero) > 150 && warpWhenReady);

        alignmentTarget = target;
        SetThrottle(1);
        state = ShipState.Aligning;
    }

    public void SetThrottle(float newThrottle)
    {
        wantedThrottle = Mathf.Clamp01(newThrottle);
    }

    private void Rotate()
    {
        if (state == ShipState.Aligning)
        {
            if (alignmentTarget)
            {
                wantedRotation = Quaternion.LookRotation(alignmentTarget.transform.position);
            }
            else
            {
                print("Error: no target for alignment");
                state = ShipState.Idle;
            }
        }

        currentRotation = Quaternion.Lerp(currentRotation, wantedRotation, _torque);
        transform.rotation = currentRotation;
    }

    private void Move()
    {
        if (state != ShipState.Warping)
        {
            currentThrottle = Mathf.Lerp(currentThrottle, wantedThrottle, _power);
            if (isPlayerShip)
            {
                ScaleSpaceManager.Translate((DoubleVector3.FromVector3(transform.forward) * (_burnSpeed * currentThrottle)) * Time.deltaTime);
            }
            else
            {
                transform.Translate((transform.forward) * (currentThrottle * _burnSpeed));
            }
        }
        else
        {
            //Warp instead
        }
    }

    private void Bank()
    {
        wantedBank = Mathf.Clamp(-rotationalVelocity.y * bankMultiplier, -70, 70);

        currentBank = Mathf.Lerp(currentBank, wantedBank, _torque * 4);
        Vector3 rot = new Vector3(0, 0, currentBank);
        if (shipModel)
        {
            shipModel.transform.localRotation = Quaternion.Euler(rot);
        }
    }

    private void Warp()
    {
        if (state == ShipState.Warping)
        {
            DoubleVector3 warpTarget = alignmentTarget.GetComponent<ScaleSpaceObject>().initialPosition;
            double wantedWarpSpeed = warpAccelerationCurve.Evaluate((Time.time - warpStartTime) * (float)_warpStrength);
            double warpSpeedLimiter = warpAccelerationCurve.Evaluate(((float)DoubleVector3.Distance(warpTarget, ScaleSpaceManager.apparentPosition) / (149597870700 * _inertia)));
            double actualWarpSpeed;
            
            actualWarpSpeed = Mathf.Min((float)wantedWarpSpeed, (float)warpSpeedLimiter) * _warpSpeed;

            ScaleSpaceManager.Warp(warpTarget, wantedWarpSpeed * _warpSpeed);

            if (DoubleVector3.Distance(ScaleSpaceManager.apparentPosition, warpTarget) <= 1)
            {
                state = ShipState.Idle;
                currentThrottle = 0;
                wantedThrottle = 0;
                print("warp complete");
            }
        }
    }

    private double WarpSpeedAtTime()
    {
        float time = Time.time - warpStartTime;
        return (_warpStrength * (1 - Mathf.Exp((-time * Mathf.Pow(10, 6)) / (_inertia * _mass))));
    }
}
