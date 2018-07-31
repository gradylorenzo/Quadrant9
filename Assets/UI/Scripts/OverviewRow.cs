using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverviewRow : MonoBehaviour {

    public Image icon;
    public Text name;
    public Text type;
    public Text alliance;
    public Text distance;
    public int rowNumber;

    public void OnClick()
    {
        EventManager.OnOverviewRowClicked(rowNumber);
    }
}
