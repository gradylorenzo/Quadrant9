using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;

public class SSLocation : MonoBehaviour {

    public float scale = 1e+08f;
    public Location Loc;
    private SystemManager SysManager;
    public GameObject LocationPrefab;

    void Awake ()
    {
        SetLocation();	
	}

    private void SetLocation()
    {
        SysManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<SystemManager>();

        Loc.position = DoubleVector3.FromVector3(transform.position) * scale;

        transform.position = DoubleVector3.ToVector3(Loc.position);

        this.GetComponent<ScaleSpaceObject>().scale = 1000;
        transform.parent = GameObject.FindGameObjectWithTag("SS0Parent").transform;
        this.gameObject.layer = transform.parent.gameObject.layer;
    }
}
