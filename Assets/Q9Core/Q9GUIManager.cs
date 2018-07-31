using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Q9Core;
using Q9Core.CommonData;
using System;

public class Q9GUIManager : MonoBehaviour
{
    public Canvas _notificationCanvas;
    public GameObject[] _notifications;
    public StatusRingController _statusRing;
    public HotbarButton[] _hotbarButtons;

    private float nextUpdate = 0;

    private ShipController _playerShip;


    private void Start()
    {
        GameManager._guiManager = this;
        foreach(HotbarButton hbb in _hotbarButtons)
        {
            hbb.ResetTargetModule();
        }
        EventManager.NotifyLockLimitReached += NotifyLockLimitReached;
        EventManager.NotifyShipStopped += NotifyShipStopped;
        EventManager.NotifyTargetInvulnerable += NotifyTargetInvulnerable;
        EventManager.NotifyModuleRequiresActiveTarget += NotifyModuleRequiresActiveTarget;
    }

    private void NotifyModuleRequiresActiveTarget()
    {
        CreateNotification(3);
    }

    private void NotifyTargetInvulnerable()
    {
        CreateNotification(2);
    }

    public void NotifyLockLimitReached()
    {
        CreateNotification(0);
    }

    public void NotifyShipStopped()
    {
        CreateNotification(1);
    }

    public void CreateNotification(int i)
    {
        GameObject newNotification = Instantiate(_notifications[i], _notifications[i].transform.position, _notifications[i].transform.rotation);
        newNotification.transform.parent = _notificationCanvas.gameObject.transform;
        newNotification.transform.position = new Vector3(Screen.width / 2, newNotification.transform.position.y, 0);
    }

    public void FixedUpdate()
    {

        if(Time.time > nextUpdate)
        {
            //Make sure a player ship is assigned.
            if (_playerShip != null)
            {
                #region Status Ring Updater
                if (_statusRing != null)
                {

                    _statusRing.SetShield(_playerShip.modifiedAttributes._shield._capacity, _playerShip.currentAttributes._shield._capacity);
                    _statusRing.SetIntegrity(_playerShip.modifiedAttributes._integrity._capacity, _playerShip.currentAttributes._integrity._capacity);
                    _statusRing.SetCapacitorFill(_playerShip.modifiedAttributes._capacitor._capacity, _playerShip.currentAttributes._capacitor._capacity);
                }
                else
                {
                    print("status ring not assigned");
                }
                #endregion
            }
            else
            {
                if (GameManager._playerShip != null)
                {
                    _playerShip = GameManager._playerShip;
                }
            }
            nextUpdate = Time.time + 1;
        }
    }
}
