using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;
using UnityEngine.UI;

public class SelectedItemPanel : MonoBehaviour {

    public OverviewObjectData defaultData;
    public OverviewObjectData Data;
    public Image thumbnailImage;
    public Text nameLabel;
    public Text typeLabel;
    public Text allianceLabel;
    public Button dockButton;
    public Button alignButton;
    public Button warpButton;

    public void SetInfo(OverviewObjectData oo)
    {
        Data = oo;

        thumbnailImage.sprite = GUIManager.IconLibrary[(int)Data.Category];
        nameLabel.text = Data.name;
        typeLabel.text = (string)Data.Type.ToString();
        allianceLabel.text = GUIManager.AllianceLibrary[(int)Data.Alliance];
    }

    public void ClearSelectedItem()
    {
        SetInfo(defaultData);
    }

    public void Awake()
    {
        GUIManager.SelectedItemPanel = this.gameObject;
        ClearSelectedItem();
    }

    public void LateUpdate()
    {
        dockButton.interactable = (Data.Category == OverviewObjectData.objectCategory.Station);
        warpButton.interactable = (DoubleVector3.Distance(Data.position, ScaleSpace.apparentPosition) >= 150);
        alignButton.interactable = (Data.guid != defaultData.guid);

        thumbnailImage.sprite = GUIManager.ThumbnailLibrary[(int)Data.Type];
    }

    public void DockButtonClicked()
    {
        //Dock ship
    }

    public void AlignButtonClicked()
    {
        GUIManager.playerShip.Align(Data.position, false);
    }

    public void WarpButtonClicked()
    {
        GUIManager.playerShip.Align(Data.position, true);
    }
}
