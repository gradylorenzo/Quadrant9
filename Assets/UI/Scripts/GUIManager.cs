using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Q9Core;

namespace Q9Core
{
    public static class GUIManager
    {
        public static GameObject SelectedItemPanel;
        public static GameObject OverviewPanel;
        public static Ship playerShip;

        public static Sprite[] IconLibrary;
        public static Sprite[] ThumbnailLibrary;
        public static string[] AllianceLibrary;

        //Call the SelectedItemPanel to show data for the currently selected object
        public static void SetSelectedItem(OverviewObjectData o)
        {
            SelectedItemPanel.GetComponent<SelectedItemPanel>().SetInfo(o);
        }

        //Call the overview panel to add a new object
        public static void AddOverviewObject(OverviewObjectData o)
        {
            OverviewPanel.GetComponent<OverviewPanel>().AddObject(o);
        }

        //Call the overview panel to remove an object
        public static void RemoveOverviewObject(OverviewObjectData o)
        {
            OverviewPanel.GetComponent<OverviewPanel>().RemoveObject(o);
        }
    }
}