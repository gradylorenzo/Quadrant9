using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;

public class SSLocation : MonoBehaviour {

    public float scale = 100000000;
    public RegisterOverviewObject Register;

    void Start ()
    {
        SetLocation();
        Register.Initialize(DoubleVector3.FromVector3(transform.position));
        gameObject.AddComponent<Rigidbody>();
        gameObject.AddComponent<SphereCollider>();
        GetComponent<SphereCollider>().radius = Register.Data.range;
        GetComponent<SphereCollider>().isTrigger = true;
        Register.Add();
    }

    private void SetLocation()
    {
        transform.position = DoubleVector3.ToVector3(DoubleVector3.FromVector3(transform.position) * scale);

        //this.GetComponent<ScaleSpaceObject>().scale = 1000;
        transform.parent = GameObject.FindGameObjectWithTag("SS0Parent").transform;
        this.gameObject.layer = transform.parent.gameObject.layer;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PlayerShip")
        {
            print("Player Entered");
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "PlayerShip")
        {
            print("Player Entered");
        }
    }
}
