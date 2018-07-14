using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    public delegate void _onTargetLocking();
    public static _onTargetLocking OnTargetLocking;

    public delegate void _onTargetLockComplete();
    public static _onTargetLockComplete OnTargetLockComplete;

    public delegate void _onShipMouseDown(GameObject go);
    public static _onShipMouseDown OnShipTargeted;

    public delegate void _onModuleInsufficientPower();
    public static _onModuleInsufficientPower OnModuleInsufficientPower;

    public delegate void _onLockLimitReached();
    public static _onLockLimitReached OnLockLimitReached;

    public delegate void _onShipDestroyed(bool wasPlayerShip, GameObject go);
    public static _onShipDestroyed onNPCShipDestroyed;
}
