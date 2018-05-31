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
    public Image background;

    private void Start()
    {
        iconLabel.sprite = GUIManager.IconLibrary[(int)Data.Category]; ; //Needs to be replaced later as icon lookup library is incorporated
        nameLabel.text = Data.name;
        typeLabel.text = (string)Data.Type.ToString();
        allianceLabel.text = (string)Data.Alliance.ToString();
        background = this.GetComponent<Image>();
    }

    public void OverviewObjectClick()
    {
        GUIManager.SetSelectedItem(Data);
    }

    public void Update()
    {
        if(Data.Alliance == OverviewObjectData.alliance.Unfriendly)
        {
            background.color = new Color(((float)Math.Sin(Time.time * 3) / 2) + .5f, 0, 0);
        }
        else
        {
            background.color = Color.black;
        }
    }
}
