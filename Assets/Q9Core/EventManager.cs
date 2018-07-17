using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;
using Q9Core.CommonData;

public static class EventManager
{
    public static bool isPlayerLocking = false;

    public delegate void _onTargetLocking();
    public static _onTargetLocking OnTargetLocking;

    public delegate void _onTargetLockComplete();
    public static _onTargetLockComplete OnTargetLockComplete;

    public delegate void _onShipLocked(GameObject go);
    public static _onShipLocked OnShipLocked;

    public delegate void _onShipUnlocked(GameObject go);
    public static _onShipUnlocked OnShipUnlocked;

    public delegate void _onModuleInsufficientPower();
    public static _onModuleInsufficientPower OnModuleInsufficientPower;

    public delegate void _onLockLimitReached();
    public static _onLockLimitReached OnLockLimitReached;

    public delegate void _onShipDestroyed(bool wasPlayerShip, GameObject go);
    public static _onShipDestroyed onShipDestroyed;

    public delegate void _onShipSelected(GameObject go);
    public static _onShipSelected OnShipSelected;

    public delegate void _onShipDamaged(GameObject go, float damage, DamageTypes type);
    public static _onShipDamaged OnShipDamaged;
}
