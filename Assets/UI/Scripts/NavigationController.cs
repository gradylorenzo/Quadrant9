using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavigationController : MonoBehaviour {

    public Text active_system;

    private void Awake()
    {
        EventManager.OnSystemChanged += OnSystemChanged;
        RefreshSystemName();
    }

    private void OnSystemChanged(Vector2 dir)
    {
        RefreshSystemName();
    }

    private void RefreshSystemName()
    {
        if(NavigationManager._activeSystem != null)
        {
            active_system.text = NavigationManager.GetSystemName(NavigationManager._activeSystem);
        }
    }
}
