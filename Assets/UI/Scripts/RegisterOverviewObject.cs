using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;

public class RegisterOverviewObject : MonoBehaviour {

    public OverviewObjectData Data;

    public void Start()
    {
        Data.generateID();
        GUIManager.AddOverviewObject(Data);
    }
}
