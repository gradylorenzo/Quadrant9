using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;
using Q9Core.CommonData;
using Q9Core.PlayerData;

public class SystemInitializer : MonoBehaviour {

    [Header("Jumpgates")]
    public GameObject JumpbridgePositiveZ;
    public GameObject JumpbridgeNegativeZ;
    public GameObject JumpbridgePositiveX;
    public GameObject JumpbridgeNegativeX;

    public void Awake()
    {
        GameManager._sysInitializer = this;
        InitializeAll();
    }

    private void InitializeAll()
    {
        InitializeJumpgates();
    }

    private void InitializeJumpgates()
    {
        #region north gate
        if (NavigationManager.GetSystemName(NavigationManager._activeSystem + new Vector2(0, 1)) != null)
        {
            JumpbridgePositiveZ.GetComponent<Q9Entity>()._overview._name = 
                NavigationManager.GetSystemName(NavigationManager._activeSystem + new Vector2(0, 1));
            JumpbridgePositiveZ.GetComponent<Q9Entity>()._visibility = Q9Entity.VisibilityFlag.Always;
            JumpbridgePositiveZ.GetComponent<Q9Entity>()._canWarpTo = true;
            JumpbridgePositiveZ.SetActive(true);
        }
        else
        {
            JumpbridgePositiveZ.GetComponent<Q9Entity>()._visibility = Q9Entity.VisibilityFlag.Never;
            JumpbridgePositiveZ.GetComponent<Q9Entity>()._canWarpTo = false;
            JumpbridgePositiveZ.SetActive(false);
        }
        #endregion
        #region south gate
        if (NavigationManager.GetSystemName(NavigationManager._activeSystem + new Vector2(0, -1)) != null)
        {
            JumpbridgeNegativeZ.GetComponent<Q9Entity>()._overview._name =
                NavigationManager.GetSystemName(NavigationManager._activeSystem + new Vector2(0, -1));
            JumpbridgeNegativeZ.GetComponent<Q9Entity>()._visibility = Q9Entity.VisibilityFlag.Always;
            JumpbridgeNegativeZ.GetComponent<Q9Entity>()._canWarpTo = true;
            JumpbridgeNegativeZ.SetActive(true);
        }
        else
        {
            JumpbridgeNegativeZ.GetComponent<Q9Entity>()._visibility = Q9Entity.VisibilityFlag.Never;
            JumpbridgeNegativeZ.GetComponent<Q9Entity>()._canWarpTo = false;
            JumpbridgeNegativeZ.SetActive(false);
        }
        #endregion
        #region east gate
        if (NavigationManager.GetSystemName(NavigationManager._activeSystem + new Vector2(-1, 0)) != null)
        {
            JumpbridgePositiveX.GetComponent<Q9Entity>()._overview._name =
                NavigationManager.GetSystemName(NavigationManager._activeSystem + new Vector2(-1, 0));
            JumpbridgePositiveX.GetComponent<Q9Entity>()._visibility = Q9Entity.VisibilityFlag.Always;
            JumpbridgePositiveX.GetComponent<Q9Entity>()._canWarpTo = true;
            JumpbridgePositiveX.SetActive(true);
        }
        else
        {
            JumpbridgePositiveX.GetComponent<Q9Entity>()._visibility = Q9Entity.VisibilityFlag.Never;
            JumpbridgePositiveX.GetComponent<Q9Entity>()._canWarpTo = false;
            JumpbridgePositiveX.SetActive(false);
        }
        #endregion
        #region west gate
        if (NavigationManager.GetSystemName(NavigationManager._activeSystem + new Vector2(1, 0)) != null)
        {
            JumpbridgeNegativeX.GetComponent<Q9Entity>()._overview._name =
                NavigationManager.GetSystemName(NavigationManager._activeSystem + new Vector2(1, 0));
            JumpbridgeNegativeX.GetComponent<Q9Entity>()._visibility = Q9Entity.VisibilityFlag.Always;
            JumpbridgeNegativeX.GetComponent<Q9Entity>()._canWarpTo = true;
            JumpbridgeNegativeX.SetActive(true);
        }
        else
        {
            JumpbridgeNegativeX.GetComponent<Q9Entity>()._visibility = Q9Entity.VisibilityFlag.Never;
            JumpbridgeNegativeX.GetComponent<Q9Entity>()._canWarpTo = false;
            JumpbridgeNegativeX.SetActive(false);
        }
        #endregion
    }
}
