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

    public delegate void _onObjectLocked(GameObject go);
    public static _onObjectLocked OnObjectLocked;

    public delegate void _onObjectUnlocked(GameObject go);
    //public static _onObjectUnlocked OnObjectUnlocked;

    public delegate void _onObjectDestroyed(bool wasPlayerShip, GameObject go);
    public static _onObjectDestroyed OnObjectDestroyed;

    public delegate void _onObjectSelected(GameObject go, bool forceLock);
    public static _onObjectSelected OnObjectSelected;

    public delegate void _onObjectDamaged(GameObject go, float damage, DamageTypes type);
    public static _onObjectDamaged OnObjectDamaged;

    public delegate void _onModuleInsufficientPower();

    public delegate void _CreateAnnouncement();
    public static _CreateAnnouncement NotifyLockLimitReached;
    public static _CreateAnnouncement NotifyShipStopped;
    public static _CreateAnnouncement NotifyModuleInsufficientPower;
    public static _CreateAnnouncement NotifyModuleRequiresActiveTarget;
    public static _CreateAnnouncement NotifyTargetInvulnerable;

    public delegate void _onGameInitialize();
    public static _onGameInitialize OnGameInternalDataInitialize;

    public delegate void _onGameInitializationComplete(Q9Ship PlayerShipData);
    public static _onGameInitializationComplete OnGameInitializationComplete;

    public delegate void _onGameIsReady();
    public static _onGameIsReady OnGameIsReady;

    public delegate void _ObjectSelectedAsAlignmentTarget(GameObject targetGO, bool WarpWhenReady);
    public static _ObjectSelectedAsAlignmentTarget OnObjectSelectedAsAlignmentTarget;

    public delegate void _OverviewDataChange(Q9OverviewData data);
    public static _OverviewDataChange addOverviewData;
    public static _OverviewDataChange removeOverviewData;

    public delegate void _OnOverviewRowClicked(int i);
    public static _OnOverviewRowClicked OnOverviewRowClicked;
}
