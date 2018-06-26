using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverviewRow : MonoBehaviour {

    public GameObject icon;
    public GameObject name;
    public GameObject type;
    public GameObject alliance;
    public Sprite defaultIcon;

    public int IndexToWatch;

    public float lastUpdate = 0;
}
