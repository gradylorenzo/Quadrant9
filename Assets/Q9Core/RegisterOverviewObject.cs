using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;

public class RegisterOverviewObject : MonoBehaviour {

    public Vector3 positionOffset;
    public OverviewObject info;

    private OverviewGUI Overview;

    public void Start()
    {
        info.position = transform.position + positionOffset;
        if (gameObject.GetComponent<ScaleSpaceLocation>())
        {
            info.scale = gameObject.GetComponent<ScaleSpaceLocation>().scale;
        }

        if (GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>().Overview)
        {
            Overview = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>().Overview;
        }

        Register();
    }

    private void Register()
    {
        if (Overview)
        {
            Overview.RegisterOverviewObject(info);
        }
    }

    public void Unregister()
    {
        if (Overview)
        {
            Overview.UnregisterOverviewObject(info);
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + positionOffset, .1f);
    }
}
