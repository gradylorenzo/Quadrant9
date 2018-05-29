using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;

public class OverviewGUI : MonoBehaviour {

    public Ship ship;

    public List<OverviewObject> Objects = new List<OverviewObject>();

    private void Start()
    {
        ship = GameObject.FindGameObjectWithTag("PlayerShip").GetComponent<Ship>();
    }

    public void RegisterOverviewObject(OverviewObject o)
    {
        if (!Objects.Contains(o))
        {
            Objects.Add(o);
        }
    }

    public void UnregisterOverviewObject(OverviewObject o)
    {
        if (Objects.Contains(o))
        {
            Objects.Remove(o);
        }
    }

    public void OnGUI()
    {
        GUILayout.BeginArea(new Rect(5, 5, 200, 500));
        GUILayout.BeginVertical();
        foreach(OverviewObject o in Objects)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(o.name))
            {
                ship.Align(DoubleVector3.FromVector3(o.position) * o.scale, true);
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
}
