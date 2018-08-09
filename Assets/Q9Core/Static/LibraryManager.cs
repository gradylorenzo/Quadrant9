using System;
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
    private static Dictionary<string, Q9Ship> L_SHIPS = new Dictionary<string, Q9Ship>();
    private static Dictionary<string, Q9Module> L_MODULES = new Dictionary<string, Q9Module>();
    private static Dictionary<string, Q9Item> L_ITEMS = new Dictionary<string, Q9Item>();

    //use this class to initialize the libraries
    public static void Initialize(Q9InitialLibrary l)
    {
        InitializeShipLibrary(l._ships);
        InitializeModuleLibrary(l._modules);
        InitializeItemLibrary(l._items);

        isInitialized = true;
    }

    public static Q9Ship GetShip(string s)
    {
        if (L_SHIPS.ContainsKey(s))
        {
            return ScriptableObject.Instantiate(L_SHIPS[s]);
        }
        else
        {
            return null;
        }
    }

    public static Q9Module GetModule(string s)
    {
        if (L_MODULES.ContainsKey(s))
        {
            return ScriptableObject.Instantiate(L_MODULES[s]);
        }
        else
        {
            return null;
        }
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
        }
        else
        {
            Debug.LogWarning("No data was loaded into LibraryManager.L_ITEMS");
        }
    }
    #endregion
}