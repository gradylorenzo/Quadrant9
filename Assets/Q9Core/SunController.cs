using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunController : MonoBehaviour {

    private void Update()
    {
        transform.LookAt(Vector3.zero - this.transform.position);
    }
}
