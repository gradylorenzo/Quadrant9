using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core.CommonData;
using Q9Core.PlayerData;
using Q9Core;
using System;

public static class LibraryManager
{
    public static bool isInitialized = false;
    public static Dictionary<string, Q9Ship> L_SHIPS = new Dictionary<string, Q9Ship>();
    public static Dictionary<string, Q9Module> L_MODULES = new Dictionary<string, Q9Module>();
    public static Dictionary<string, Q9Item> L_ITEMS = new Dictionary<string, Q9Item>();

    //use this class to initialize the libraries
    public static void Initialize(Q9Ship[] shipArr, Q9Module[] modArray, Q9Item[] itemArray)
    {
        InitializeShipLibrary(shipArr);
        InitializeModuleLibrary(modArray);
        InitializeItemLibrary(itemArray);
        isInitialized = true;
    }

    #region individual initializer methods
    //Initialize L_SHIPS
    private static void InitializeShipLibrary(Q9Ship[] shipArray)
    {
        if (shipArray.Length > 0)
        {
            foreach (Q9Ship s in shipArray)
            {
                L_SHIPS.Add(s._id, s);
            }

            foreach (KeyValuePair<string, Q9Ship> kvp in L_SHIPS)
            {
                Debug.Log("Loaded data for Q9Ship " + kvp.Key);
            }
        }
        else
        {
            Debug.LogWarning("No data was loaded to LibraryManager.L_SHIPS");
        }
    }

    //Initialize L_MODULES
    private static void InitializeModuleLibrary(Q9Module[] modArray)
    {
        if(modArray.Length > 0)
        {
            foreach(Q9Module m in modArray)
            {
                L_MODULES.Add(m._id, m);
            }

            foreach(KeyValuePair<string, Q9Module> kvp in L_MODULES)
            {
                Debug.Log("Loaded data for Q9Module " + kvp.Key);
            }
        }
        else
        {
            Debug.LogWarning("No data was loaded into LibraryManager.L_MODULES");
        }
    }

    //Initialize L_ITEMS
    private static void InitializeItemLibrary(Q9Item[] itemArray)
    {
        if (itemArray.Length > 0)
        {
            foreach (Q9Item i in itemArray)
            {
                L_ITEMS.Add(i._id, i);
            }

            foreach (KeyValuePair<string, Q9Item> kvp in L_ITEMS)
            {
                Debug.Log("Loaded data for Q9Item " + kvp.Key);
            }
        }
        else
        {
            Debug.LogWarning("No data was loaded into LibraryManager.L_ITEMS");
        }
    }
    #endregion
}