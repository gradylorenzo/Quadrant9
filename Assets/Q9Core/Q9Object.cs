using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;
using Q9Core.CommonData;

public class Q9Object : ScriptableObject
{
    public string _name;
    public int _quantity;
    public int _value;
    public string _description;
    public Sprite _thumbnail;
}

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Q9Item : Q9Object
{

}

[CreateAssetMenu(fileName = "New Ship", menuName = "Ship")]
public class Q9Ship : Q9Object
{
    public Attributes _attributes;
    public GameObject _model;
}

public class Q9Module : Q9Object
{

}
