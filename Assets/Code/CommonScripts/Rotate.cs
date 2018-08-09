using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Q9Core;
using Q9Core.CommonData;

[ExecuteInEditMode]
public class Rotate : MonoBehaviour
{
    public Vector3 Speed;

    public void FixedUpdate()
    {
        transform.Rotate(Speed);
    }
}
