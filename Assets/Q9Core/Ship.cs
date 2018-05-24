using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;

public class Ship : MonoBehaviour {

    public ShipAttributes Attributes;
    public ShipState State;

    private bool WantToWarp = false;
    private Quaternion wantedRotation;
    private Quaternion currentRotation;
    private DoubleVector3 WarpDestination;
    private float WantedThrottle;
    private float CurrentThrottle;

    public void MoveShip()
    {
        ScaleSpace.Translate(transform.forward * (Attributes.Control.maxBurnSpeed * (Mathf.Clamp01(CurrentThrottle))));
    }

    public void Align(DoubleVector3 pos, bool warp)
    {
        SetWantedRotation(Quaternion.LookRotation(transform.position - DoubleVector3.ToVector3(pos)));
        WantToWarp = warp;
    }

    public void SetWantedRotation (Quaternion r)
    {
        if (State != ShipState.warping)
        {
            State = ShipState.aligning;
            wantedRotation = r;
        }
    }

    public void SetWantedThrottle(float t)
    {
        WantedThrottle = Mathf.Clamp01(t);
    }

    public void FixedUpdate()
    {
        CurrentThrottle = Mathf.Lerp(CurrentThrottle, WantedThrottle, Attributes.Control.acceleration);

        currentRotation = Quaternion.Lerp(currentRotation, wantedRotation, Attributes.Control.maxTurnSpeed);
        if(Quaternion.Angle(currentRotation, wantedRotation) <= 5f)
        {
            State = ShipState.aligned;
        }

        if(State == ShipState.aligned && WantToWarp)
        {
            State = ShipState.warping;
        }

        if(State == ShipState.warping)
        {
            ScaleSpace.Warp(WarpDestination, Attributes.Control.maxWarpSpeed);

            if (DoubleVector3.Distance(ScaleSpace.apparentPosition, WarpDestination) <= .001f)
            {
                State = ShipState.idle;
            }
        }
    }
}
