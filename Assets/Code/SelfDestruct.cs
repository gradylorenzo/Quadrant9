using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour {

    public float delay = 5.0f;
    public GameObject DestructionPrefab;

    private float SpawnTime;

    private void Start()
    {
        SpawnTime = Time.time;
    }

    private void FixedUpdate()
    {
        if(Time.time > SpawnTime + delay)
        {
            if (DestructionPrefab)
            {
                Instantiate(DestructionPrefab, transform.position, transform.rotation);
            }
            Destroy(gameObject);
        }
    }
}
