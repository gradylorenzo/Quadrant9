using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;
using Q9Core.CommonData;
using Q9Core.PlayerData;

public static class ProfileManager
{
    //This will hold static information related to the player's profile
    //Including name, standings, credit balance,
    //Home station, current ship, fitting, etc.

    //Has a profile already been loaded? This can be used by the main menu to skip the profile selection state.
    private static bool _profileLoaded = false;
    public static bool profileLoaded
    {
        get { return _profileLoaded; }
        set { _profileLoaded = value; }
    }

    private static PlayerProfile _player;
    public static PlayerProfile player
    {
        get { return _player; }
    }
    public static void SetPlayerProfile(PlayerProfile pp)
    {
        _player = pp;
    }
}
