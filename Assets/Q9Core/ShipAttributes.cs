using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Q9Core
{
    [Serializable]
    public class ShipControl
    {
        //Control
        public float maxWarpSpeed = 1f; //astronomical units (AU) per second
        public float currentWarpSpeed = 1f; //current speed the ship is warping
        public float maxBurnSpeed = 1f; //meters per second
        public float maxTurnSpeed = 1f; //Turning speed of the ship
        public float acceleration = 1f; //How quickly the ship accelerates

    }

    [Serializable]
    public class ShipLocking
    {
        //Locking
        public float SignatureSize = 1f; //how well can the ship prevent being locked?
        public float SensorStrength = 1f; //how well can the ship lock a target?
    }

    [Serializable]
    public class ShipOffense
    {
        //Offense, self-explanatory
        public float ProjectileDamage = 1f;
        public float LaserDamage = 1f;
        public float MissileDamage = 1f;
        public float DroneDamage = 1f;
    }

    [Serializable]
    public class ShipDefense
    {
        //Defense
        public float ShieldCapacity = 1f; //Maximum shield capacity of the ship
        public float CurrentShieldCapacity = 1f; //Current level of the ship's shields
        public float ShieldRechargeRate = 1f; //How quickly the shields passively recharge
        public float Integrity = 1f; //Maximum integrity of the ship's hull
        public float CurrentIntegrity = 1f; //Current integrity of the ship's hull
    }

    [Serializable]
    public class ShipCapacitor
    {
        //Capacitor
        public float CapcitorCapacity = 1f; //Maximum level of the ship's capacitor
        public float CurrentCapacityCapacity = 1f; //Current level of the ship's capacitor
        public float CapacitorRechargeRate = 1f; //How quickly the ship's capacitor passively recharges
    }

    [Serializable]
    public class ShipAttributes
    {
        public ShipControl Control;
        public ShipLocking Locking;
        public ShipOffense Offense;
        public ShipDefense Defense;
        public ShipCapacitor Capacitor;
    }

    [Serializable]
    public enum ShipState
    {
        idle,
        aligning,
        aligned,
        warping
    }
}