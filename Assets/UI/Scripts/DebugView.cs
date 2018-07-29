using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;

public class DebugView : MonoBehaviour {

    public GameObject grid;
    private bool showDebugView = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            print("Debug View Toggled");
            showDebugView = !showDebugView;
            grid.SetActive(showDebugView);
        }
    }

    private void OnGUI()
    {
        if (showDebugView)
        {
            GUILayout.BeginArea(new Rect(5, 5, 400, 400));
            GUILayout.BeginVertical();
            if (SaveManager.profileLoaded)
            {
                GUILayout.Label(SaveManager.currentPlayer._identity._name + "   CR= " + SaveManager.currentPlayer._identity._credits);
                GUILayout.Label(GameManager._playerShip.GetComponent<Q9Entity>()._overview._name);
                GUILayout.Label("Seed");
                GUILayout.TextField(SaveManager.currentPlayer._identity._seed);
            }
            else
            {
                GUILayout.Label("No Profile Loaded");
            }
            GUILayout.Label("ScaleSpace.apparentPosition");
            GUILayout.Label("X= " + ScaleSpace.apparentPosition.x);
            GUILayout.Label("Y= " + ScaleSpace.apparentPosition.y);
            GUILayout.Label("Z= " + ScaleSpace.apparentPosition.z);
            GUILayout.Space(10);
            GUILayout.Label("No information is shared by this game with any party");
            GUILayout.Label("Nerds don't get invited to parties anyway.");
            GUILayout.Space(10);
            GUILayout.Label("System Information:");
            GUILayout.Label("OS= " + SystemInfo.operatingSystem);
            GUILayout.Label("CPU= " + SystemInfo.processorType + "  @" + SystemInfo.processorFrequency + "MHz");
            GUILayout.Label("RAM= " + SystemInfo.systemMemorySize + " MB");
            GUILayout.Label("Graphics Vendor= " + SystemInfo.graphicsDeviceVendor);
            GUILayout.Label("Graphics Device= " + SystemInfo.graphicsDeviceName);
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
    }
}
