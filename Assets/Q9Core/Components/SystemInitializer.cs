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
        InitializeJumpgates();
    }

    private void InitializeJumpgates()
    {
        #region north gate
        if (NavigationManager.GetSystemName(NavigationManager._activeSystem.xCoord, NavigationManager._activeSystem.yCoord + 1) != "")
        {
            JumpbridgePositiveZ.GetComponent<Q9Entity>()._overview._name = 
                NavigationManager.GetSystemName(NavigationManager._activeSystem.xCoord, NavigationManager._activeSystem.yCoord + 1);
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
        if (NavigationManager.GetSystemName(NavigationManager._activeSystem.xCoord, NavigationManager._activeSystem.yCoord - 1) != "")
        {
            JumpbridgeNegativeZ.GetComponent<Q9Entity>()._overview._name =
                NavigationManager.GetSystemName(NavigationManager._activeSystem.xCoord, NavigationManager._activeSystem.yCoord - 1);
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
        if (NavigationManager.GetSystemName(NavigationManager._activeSystem.xCoord + 1, NavigationManager._activeSystem.yCoord) != "")
        {
            JumpbridgePositiveX.GetComponent<Q9Entity>()._overview._name =
                NavigationManager.GetSystemName(NavigationManager._activeSystem.xCoord + 1, NavigationManager._activeSystem.yCoord);
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
        if (NavigationManager.GetSystemName(NavigationManager._activeSystem.xCoord - 1, NavigationManager._activeSystem.yCoord) != "")
        {
            JumpbridgeNegativeX.GetComponent<Q9Entity>()._overview._name =
                NavigationManager.GetSystemName(NavigationManager._activeSystem.xCoord - 1, NavigationManager._activeSystem.yCoord);
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
