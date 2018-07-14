using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour {

    public float delay = 5.0f;

    private float SpawnTime;

    private void Start()
    {
        SpawnTime = Time.time;
    }

    private void FixedUpdate()
    {
        if(Time.time > SpawnTime + delay)
        {
            Destroy(gameObject);
        }
    }
}
