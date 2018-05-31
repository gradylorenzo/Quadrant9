using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Q9Core;

public class Libraries : MonoBehaviour {

    public Sprite[] ThumbnailLibrary;
    public Sprite[] IconLibrary;
    public string[] AllianceLibrary;

    public void Awake()
    {
        GUIManager.ThumbnailLibrary = ThumbnailLibrary;
        GUIManager.IconLibrary = IconLibrary;
        GUIManager.AllianceLibrary = AllianceLibrary;
    }
}
