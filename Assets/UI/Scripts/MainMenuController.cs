using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Q9Core;
using System;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

    [System.Serializable]
	public enum States
    {
        select_profile,
        new_profile
    }

    public States _menuState;
    public Canvas _newProfileUI;
    public Canvas _selectProfileUI;
    public Text _newProfileInputField;
    public Button _newProfileStartButton;
    public Text Error1;
    public Text Error2;
    public Text[] _profileSelectionText;

    private string[] foundProfiles;
    private string _lastEnteredNameText;


    private void Awake()
    {
        GetProfileList();
    }

    private void Update()
    {
        _newProfileUI.gameObject.SetActive(_menuState == States.new_profile);
        _selectProfileUI.gameObject.SetActive(_menuState == States.select_profile);

        if (_menuState == States.new_profile)
        {
            if (_newProfileInputField.text != _lastEnteredNameText)
            {
                _lastEnteredNameText = _newProfileInputField.text;
                NewProfileEnteredNameChanged(_lastEnteredNameText);
            }
        }
        else if(_menuState == States.select_profile)
        {
            for(int i = 0; i < 3; i++)
            {
                if (i < foundProfiles.Length)
                {
                    _profileSelectionText[i].text = foundProfiles[i];
                }
                else
                {
                    _profileSelectionText[i].text = "CREATE NEW";
                }
            }
        }

    }

    private void NewProfileEnteredNameChanged(string s)
    {
        bool exists = false;
        bool notNull = false;

        if(s != null && s != "")
        {
            notNull = true;
        }

        foreach(string l in foundProfiles)
        {
            if(s == l)
            {
                exists = true;
            }
        }

        if(!exists && notNull)
        {
            _newProfileStartButton.interactable = true;
        }
        else
        {
            _newProfileStartButton.interactable = false;
        }

        if (!notNull)
        {
            if (exists)
            {
                Error1.enabled = true;
                Error2.enabled = false;
            }
            else
            {
                Error2.enabled = true;
                Error1.enabled = false;
            }
        }
        else
        {
            Error2.enabled = false;
            Error1.enabled = false;
        }
    }

    public void CreateNewProfile()
    {
        SaveManager.WriteNewProfile(_newProfileInputField.text);
        GetProfileList();
    }

    private void GetProfileList()
    {
        foundProfiles = SaveManager.GetProfiles();
        print(foundProfiles.Length);
        if (foundProfiles.Length == 0)
        {
            _menuState = States.new_profile;
        }
        else
        {
            _menuState = States.select_profile;
        }
    }

    public void ProfileSelected(int i)
    {
        if(foundProfiles.Length > i)
        {
            if (SaveManager.ReadProfile(foundProfiles[i]))
            {
                SceneManager.LoadScene("scalespace");
            }
        }
        else
        {
            _menuState = States.new_profile;
        }
    }
}