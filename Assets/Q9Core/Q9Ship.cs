using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;
using Q9Core.CommonData;

[CreateAssetMenu(fileName = "New Ship", menuName = "Ship")]
public class Q9Ship : Q9Object
{
    [Header("Ship Attributes")]
    public Attributes _attributes;
    public GameObject _model;
    public float _minCameraDistance;
}
