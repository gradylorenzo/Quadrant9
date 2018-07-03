using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;
using Q9Core.CommonData;
using UnityEngine.UI;

[System.Serializable]
public class Q9OverviewData {

    [System.Serializable]
    public enum EntityTypes
    {
        Station,
        Wreck,
        Asteroid,
        Stargate
    }

    public string _name;
    public Alliances _alliance;
    public EntityTypes _type;
    public Sprite _thumbnail;
    public Sprite _icon;
}
