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
            GUILayout.BeginArea(new Rect(5, 5, 400, Screen.height));
            GUILayout.BeginVertical();
            if (SaveManager.profileLoaded)
            {
                GUILayout.Label(SaveManager.currentPlayer._identity._name + "   CR= " + SaveManager.currentPlayer._identity._credits);
                GUILayout.Label(GameManager._playerShip.GetComponent<Q9Entity>()._overview._name);
            }
            else
            {
                GUILayout.Label("No Profile Loaded");
            }
            GUILayout.Label("ScaleSpace.apparentPosition");
            GUILayout.Label("X= " + ScaleSpaceManager.apparentPosition.x);
            GUILayout.Label("Y= " + ScaleSpaceManager.apparentPosition.y);
            GUILayout.Label("Z= " + ScaleSpaceManager.apparentPosition.z);
            GUILayout.Label("Warp Target Distance= ");
            GUILayout.Space(10);
            GUILayout.Label("NavigationManager.activeSystem");
            GUILayout.Label("X= " + NavigationManager.activeSystem.x);
            GUILayout.Label("Y= " + NavigationManager.activeSystem.y);
            GUILayout.Space(10);
            if (GameManager._playerShip)
            {
                ShipTransform st = GameManager._playerShip.GetComponent<ShipTransform>();
                GUILayout.Label("PlayerShipAngularVelocity");
                GUILayout.Label("X= " + st.rotationalVelocity.x);
                GUILayout.Label("Y= " + st.rotationalVelocity.y);
                GUILayout.Label("Z= " + st.rotationalVelocity.z);
                GUILayout.Space(10);
            }
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
