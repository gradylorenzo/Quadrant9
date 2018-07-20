using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;
using Q9Core.CommonData;
using Q9Core.PlayerData;
using System;

public class GameInitializer : MonoBehaviour {

    public Q9Ship[] _shipLibrary;
    public Q9Module[] _moduleLibrary;
    public Q9Item[] _itemLibrary;
    public PlayerProfile _prof;

    public void Awake()
    {
        EventManager.OnGameInternalDataInitialize += OnGameInternalDataInitialize;
        SaveManager.WriteNewProfile(_prof);
    }

    private void OnGameInternalDataInitialize()
    {
        LibraryManager.Initialize(_shipLibrary, _moduleLibrary, _itemLibrary);
    }
}
