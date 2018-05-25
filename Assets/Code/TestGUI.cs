using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;

public class TestGUI : MonoBehaviour {

    public SystemManager SysManager;
    public Ship PlayerShip;
    public Location selectedLocation = new Location();
    public Vector3 clickPosition = new Vector3();
    public bool showMenu = false;
    private void Start()
    {
        SysManager = GameObject.FindGameObjectWithTag("SystemManager").GetComponent<SystemManager>();
        PlayerShip = GameObject.FindGameObjectWithTag("PlayerShip").GetComponent<Ship>();
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(0, 0, 100, 500));
        GUILayout.BeginVertical();
        foreach(Location L in SysManager.System.Locations)
        {
            if (GUILayout.Button(L.name))
            {
                selectedLocation = L;
                clickPosition = Input.mousePosition;
                showMenu = true;
            }
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();

        if (showMenu)
        {
            GUILayout.BeginArea(new Rect(120, Screen.height - clickPosition.y, 100, 200));
            GUILayout.BeginVertical();
            if (GUILayout.Button("Align"))
            {
                PlayerShip.Align(selectedLocation.position, false);
                PlayerShip.SetThrottle(1);
                showMenu = false;
            }
            if (DoubleVector3.Distance(ScaleSpace.apparentPosition, selectedLocation.position - ScaleSpace.apparentPosition) >= 150)
            {
                if (GUILayout.Button("Warp To"))
                {
                    PlayerShip.Align(selectedLocation.position, true);
                    PlayerShip.SetThrottle(1);
                    showMenu = false;
                }
            }
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
    }
}
