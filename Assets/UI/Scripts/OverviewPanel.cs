﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;

public class OverviewPanel : MonoBehaviour {

    public GameObject                   OverviewObjectPrefab;
    public List<OverviewObjectData>     DataList;
    public List<GameObject>             ObjectList;


    private void Awake()
    {
        GUIManager.OverviewPanel = this.gameObject;
    }

    public void AddObject(OverviewObjectData d)
    {
        if (!DataList.Contains(d))
        {
            DataList.Add(d);
            ResetObjects();
        }
    }

    public void RemoveObject(OverviewObjectData d)
    {
        if (DataList.Contains(d))
        {
            DataList.Remove(d);
            ResetObjects();
        }
    }

    private void ResetObjects()
    {
        //Clear current overview objects
        foreach(GameObject go in ObjectList)
        {
            Destroy(go);
        }
        ObjectList.Clear();

        float i = 387.5f;
        //Create new overview objects
        foreach(OverviewObjectData d in DataList)
        {
            GameObject newObject;
            Vector3 newPos = new Vector3(0, i, 0);
            newObject = Instantiate(OverviewObjectPrefab, transform.position, transform.rotation);
            newObject.GetComponent<OverviewObject>().Data = d;
            newObject.transform.parent = this.transform;
            newObject.GetComponent<RectTransform>().localPosition = newPos;
            ObjectList.Add(newObject);
            i += -22.2f;
        }
    }
}
