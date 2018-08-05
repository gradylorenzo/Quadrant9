using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NavigationManager
{
    public static StarSystem[] _starSystems;
    public static StarSystem _activeSystem;

    public static void InitializeMapData(StarSystem[] sys)
    {
        _starSystems = sys;
        foreach(StarSystem ss in _starSystems)
        {
            Debug.Log("System " + ss.name + " added to NavigationManager");
        }
    }

    public static void SetActiveSystem (int x, int y)
    {
        bool found = false;
        foreach (StarSystem ss in _starSystems)
        {
            if (ss.xCoord == x && ss.yCoord == y)
            {
                _activeSystem = ss;
                Debug.Log("Active System set to " + ss.name);
                found = true;
            }
        }
        if (!found)
        {
            Debug.Log("System not found at coordinates " + x + ", " + y);
        }
    }

    public static string GetSystemName(int x, int y)
    {
        string name = "";
        bool found = false;
        foreach(StarSystem ss in _starSystems)
        {
            if(ss.xCoord == x && ss.yCoord == y)
            {
                name = ss.name;
                found = true;
            }
        }
        if (!found)
        {
            Debug.Log("System not found at coordinates " + x + ", " + y);
        }
        return name;
    }
}
