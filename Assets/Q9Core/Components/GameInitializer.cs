using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;
using Q9Core.CommonData;
using Q9Core.PlayerData;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameInitializer : MonoBehaviour {

    public Text _loadingText;
    public Button _start;
    public Q9InitialLibrary _library;
    public float delay;
    public Texture2D noise;

    private bool loaded = false;

    public void Update()
    {
        if(Time.time > delay && !loaded)
        {
            _start.interactable = true;
        }

        if(LibraryManager.isInitialized && !loaded)
        {
            loaded = true;
            _loadingText.text = "Starting..";
            _start.interactable = false;
            loaded = true;
            SceneManager.LoadScene("main_menu");
        }
    }

    public void LaunchDiscord()
    {
        Application.OpenURL("https://discord.gg/nYnVzFU");
    }

    public void LaunchGitHub()
    {
        Application.OpenURL("https://github.com/gradylorenzo/Quadrant9");
    }

    public void LaunchBlog()
    {
        Application.OpenURL("https://q9dev.wordpress.com/");
    }

    public void StartGame()
    {
        LibraryManager.Initialize(_library);
        NavigationManager.InitializeMapData(BuildMapData());
    }

    public StarSystem[] BuildMapData()
    {
        List<StarSystem> _starSystems = new List<StarSystem>();
        for (int x = 0; x <= 32; x++)
        {
            for (int y = 0; y < 32; y++)
            {
                if (noise.GetPixel(x, y).r >= .6f)
                {
                    StarSystem newSystem = new StarSystem();
                    newSystem.xCoord = x;
                    newSystem.yCoord = y;
                    newSystem.starSize = Mathf.Clamp(noise.GetPixel(x, y).g, .1f, 1f);
                    newSystem.starMass = Mathf.Clamp(noise.GetPixel(x, y).b, .1f, 1f);
                    newSystem.starDensity = (newSystem.starMass / newSystem.starSize);
                    newSystem.name = GenerateSystemName(x, y);
                    _starSystems.Add(newSystem);
                }
            }
        }

        return _starSystems.ToArray();
    }

    private string GenerateSystemName(int x, int y)
    {
        const string allChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        string systemName = "";
        int tacPosition = Convert.ToInt32(Mathf.PingPong(x, 3)) + 1;

        for (int i = 0; i < 6; i++)
        {
            if (i == tacPosition)
            {
                systemName += "-";
            }
            else
            {
                int r = Convert.ToInt32(Mathf.Repeat((x + 1) + (y + 5) + i ^ 2 + 20, 36));
                char newChar = allChars[r];
                systemName += newChar;
            }
        }
        return systemName;
    }
}
