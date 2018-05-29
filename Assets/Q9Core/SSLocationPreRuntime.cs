using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;

public class SSLocationPreRuntime : MonoBehaviour {

    public float scale = 1e+08f;
    public Location Loc;
    private SystemManager SysManager;

    void Awake ()
    {
        SetLocation();	
	}

    private void SetLocation()
    {
        SysManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<SystemManager>();

        Loc.position = DoubleVector3.FromVector3(transform.position) * scale;

        SysManager.System.Locations.Add(Loc);
    }
}
