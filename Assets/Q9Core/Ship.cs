using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;

public class Ship : MonoBehaviour {

    public bool isPlayerShip = false;
    public ShipAttributes Attributes;
    public ShipState State;

    private bool WantToWarp = false;
    private Quaternion WantedRotation = new Quaternion();
    private Quaternion CurrentRotation = new Quaternion();
    private DoubleVector3 WarpDestination = new DoubleVector3();
    private float WantedThrottle = 0;
    private float CurrentThrottle = 0;

    public AnimationCurve WarpOutCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
    public AnimationCurve WarpInCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 0));
    private float WarpStartTime;
    private float currentWarpLimiter = 1;
    private float currentWarpSpeed = 0;
    private float wantedWarpSpeed = 1;

    public float WarpSpeed;

    //Public method for aligning to a point
    public void Align(DoubleVector3 pos, bool warp)
    {
        if (State != ShipState.warping)
        {
            WantToWarp = warp;
            WarpDestination = pos;

            DoubleVector3 d = new DoubleVector3(pos.x - 0, pos.y - 0, pos.z - 0);
            Quaternion AlignRotation = new Quaternion();
            if (isPlayerShip)
            {
                AlignRotation = Quaternion.LookRotation(DoubleVector3.ToVector3(d) - DoubleVector3.ToVector3(ScaleSpace.apparentPosition));
            }
            else
            {
                AlignRotation = Quaternion.LookRotation(DoubleVector3.ToVector3(d) - transform.position);
            }
            WantedRotation = AlignRotation;

            State = ShipState.aligning;
        }
    }

    //Public method for aligning to a point
    public void AllStop()
    {
        WantedThrottle = 0;
        WantToWarp = false;
        State = ShipState.idle;
    }

    //Public method for setting throttle
    public void SetThrottle(float t)
    {
        WantedThrottle = Mathf.Clamp01(t);
    }

    public void Start()
    {
        WantedRotation = transform.rotation;
        CurrentRotation = transform.rotation;
    }

    public void FixedUpdate()
    {
        CurrentThrottle = Mathf.Lerp(CurrentThrottle, WantedThrottle, Attributes.Control.acceleration);
        CurrentRotation = Quaternion.Slerp(CurrentRotation, WantedRotation, Attributes.Control.maxTurnSpeed);

        if (State != ShipState.warping)
        {
            MoveShip();
        }
        RotateShip();
        
        if(State == ShipState.aligning)
        {
            if (Quaternion.Angle(CurrentRotation, WantedRotation) < 10f)
            {
                print("aligned");
                State = ShipState.aligned;
            }
        }

        if(State == ShipState.aligned && WantToWarp && CurrentThrottle >= 0.75f)
        {
            State = ShipState.warping;
            WarpStartTime = Time.time;
        }

        if (State == ShipState.warping)
        {
            print(Vector3.Distance(DoubleVector3.ToVector3(ScaleSpace.apparentPosition), DoubleVector3.ToVector3(WarpDestination)) / 1000000000);
            currentWarpLimiter = WarpOutCurve.Evaluate(Mathf.Clamp01(Vector3.Distance(DoubleVector3.ToVector3(ScaleSpace.apparentPosition), DoubleVector3.ToVector3(WarpDestination)) / 1000000000));
            currentWarpSpeed = Mathf.Clamp(WarpOutCurve.Evaluate((Time.time - WarpStartTime) / 20), 0, currentWarpLimiter);

            WarpSpeed = Attributes.Control.maxWarpSpeed * currentWarpSpeed;

            ScaleSpace.Warp(WarpDestination, WarpSpeed);

            if (DoubleVector3.Distance(ScaleSpace.apparentPosition, WarpDestination) / 1000000000 <= .0005f)
            {
                State = ShipState.idle;
                SetThrottle(0);
            }
        }
    }

    private void RotateShip()
    {
        transform.rotation = CurrentRotation;
    }

    private void MoveShip()
    {
        if (isPlayerShip)
        {
            ScaleSpace.Translate(transform.forward * (Attributes.Control.maxBurnSpeed * (Mathf.Clamp01(CurrentThrottle))));
        }
        else
        {
            transform.Translate(transform.forward * (Attributes.Control.maxBurnSpeed * (Mathf.Clamp01(CurrentThrottle))));
        }
    }
}
