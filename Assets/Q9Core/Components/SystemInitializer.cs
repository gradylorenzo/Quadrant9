using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;
using Q9Core.CommonData;
using Q9Core.PlayerData;

public class SystemInitializer : MonoBehaviour {

    [Header("Jumpgates")]
    public GameObject[] Jumpgate;
    public GameObject Star;

    public void Start()
    {
        EventManager.OnSystemChanged += OnSystemChanged;
        GameManager._sysInitializer = this;
        InitializeAll();
    }

    private void OnSystemChanged(Vector2 dir)
    {
        InitializeAll();
    }

    private void InitializeAll()
    {
        EventManager.OnClearOverviewData();
        InitializeStar();
        InitializeJumpgates();
    }

    private void InitializeStar()
    {
        if(Star != null)
        {
            Star.GetComponent<Renderer>().materials[0].SetColor("_Color", NavigationManager._starSystems[NavigationManager._activeSystem].starColor);
            Debug.Log("Star Color Set");
        }
        else
        {
            Debug.Log("No Star set on SystemInitializer");
        }
    }

    private void InitializeJumpgates()
    {
        foreach(GameObject go in Jumpgate)
        {
            Q9Entity q9e = go.GetComponent<Q9Entity>();
            if(NavigationManager._starSystems.ContainsKey(NavigationManager._activeSystem + q9e.jumpDirection))
            {
                go.SetActive(true);
                q9e._overview._name = NavigationManager.GetSystemName(NavigationManager._activeSystem + q9e.jumpDirection);
                q9e._visibility = Q9Entity.VisibilityFlag.Always;
                go.name = q9e._overview._name;
                q9e.Refesh();
            }
            else
            {
                go.SetActive(false);
                q9e._visibility = Q9Entity.VisibilityFlag.Never;
                go.name = "";
                q9e.Refesh();
            }
        }
    }
}
