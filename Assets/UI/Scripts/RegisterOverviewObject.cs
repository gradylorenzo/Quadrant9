using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;

[Serializable]
public class RegisterOverviewObject {

    public OverviewObjectData Data;

    public void Initialize(DoubleVector3 pos)
    {
        Data.initialize(pos);
    }

    public void Add()
    {
        GUIManager.AddOverviewObject(Data);
    }

    public void Remove()
    {
        GUIManager.RemoveOverviewObject(Data);
    }
}
