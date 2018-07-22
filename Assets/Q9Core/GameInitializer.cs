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
        EventManager.OnGameInternalDataInitialize();
        //SaveManager.WriteNewProfile("Nyxton");
        _prof = SaveManager.ReadProfile("Nyxton");
        foreach(KeyValuePair<string, Q9Ship> kvp in _prof._allShips)
        {
            print(kvp.Key);
        }
    }

    private void OnGameInternalDataInitialize()
    {
        LibraryManager.Initialize(_shipLibrary, _moduleLibrary, _itemLibrary);
    }
}
