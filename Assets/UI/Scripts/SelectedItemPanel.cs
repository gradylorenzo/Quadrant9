using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;
using UnityEngine.UI;

public class SelectedItemPanel : MonoBehaviour {

    public OverviewObjectData defaultInfo;
    public OverviewObjectData info;
    public Image thumbnailImage;
    public Text nameLabel;
    public Text typeLabel;
    public Text allianceLabel;


    public void SetInfo(OverviewObjectData oo)
    {
        info = oo;

        thumbnailImage.sprite = info.thumbnail;
        nameLabel.text = info.name;
        typeLabel.text = (string)info.Type.ToString();
        allianceLabel.text = (string)info.Alliance.ToString();
    }

    public void ClearSelectedItem()
    {
        SetInfo(defaultInfo);
    }

    public void Awake()
    {
        GUIManager.SelectedItemPanel = this.gameObject;
        ClearSelectedItem();
    }
}
