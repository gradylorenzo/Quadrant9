using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;

public class SystemController : MonoBehaviour {

    [Header("Jumpgates")]
    public GameObject[] Jumpgates;

    public void Awake()
    {
        GameManager._syscon = this;
        EventManager.OnSystemChanged += OnSystemChanged;
        OnSystemChanged(new Vector2(0, 0));
    }

    private void OnSystemChanged(Vector2 dir)
    {
        foreach(GameObject go in Jumpgates)
        {
            Q9Entity q9e = go.GetComponent<Q9Entity>();
            if (NavigationManager.GetSystemName(NavigationManager.activeSystem + go.GetComponent<Q9Entity>().jumpDirection) != null)
            {
                q9e._overview._name =
                    NavigationManager.GetSystemName(NavigationManager.activeSystem + q9e.jumpDirection);
                q9e._visibility = Q9Entity.VisibilityFlag.Always;
            }
            else
            {
                q9e._visibility = Q9Entity.VisibilityFlag.Never;
            }
            q9e.Refesh();
        }
    }

    public void Jump (Vector2 dir)
    {
        if (NavigationManager.GetSystemName(NavigationManager.activeSystem + dir) != null)
        {
            NavigationManager.SetActiveSystem(NavigationManager.activeSystem + dir);
            DoubleVector3 newPos = new DoubleVector3(-ScaleSpaceManager.apparentPosition.x, ScaleSpaceManager.apparentPosition.y, -ScaleSpaceManager.apparentPosition.z);
            ScaleSpaceManager.SetApparentPosition(newPos);
            EventManager.OnSystemChanged(dir);
        }
    }
}
