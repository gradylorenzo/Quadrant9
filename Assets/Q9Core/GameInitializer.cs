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
    }
}
