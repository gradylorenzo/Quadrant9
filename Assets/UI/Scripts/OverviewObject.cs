using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Q9Core;

public class OverviewObject : MonoBehaviour
{
    public OverviewObjectData Data;
    public Image iconLabel;
    public Text nameLabel;
    public Text typeLabel;
    public Text allianceLabel;

    private void Start()
    {
        iconLabel.sprite = Data.thumbnail; //Needs to be replaced later as icon lookup library is incorporated
        nameLabel.text = Data.name;
        typeLabel.text = (string)Data.Type.ToString();
        allianceLabel.text = (string)Data.Alliance.ToString();
    }

    public void OverviewObjectClick()
    {
        GUIManager.SetSelectedItem(Data);
    }
}
