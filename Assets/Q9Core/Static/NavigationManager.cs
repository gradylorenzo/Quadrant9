using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;

public static class NavigationManager
{
    public static Dictionary<Vector2, StarSystem> _starSystems = new Dictionary<Vector2, StarSystem>();
    public static Vector2 _activeSystem;

    public static void InitializeMapData(Texture2D noise, Gradient gradient)
    {
        List<StarSystem> sys = new List<StarSystem>();
        for (int x = 0; x <= 32; x++)
        {
            for (int y = 0; y < 32; y++)
            {
                if (noise.GetPixel(x, y).r >= .6f)
                {
                    StarSystem newSystem = new StarSystem();
                    newSystem.position = new Vector2(x, y);
                    newSystem.starSize = Mathf.Clamp(noise.GetPixel(x, y).g, .1f, 1f);
                    newSystem.starMass = Mathf.Clamp(noise.GetPixel(x, y).b, .1f, 1f);
                    newSystem.starDensity = (newSystem.starMass / newSystem.starSize);
                    newSystem.starColor = gradient.Evaluate(newSystem.starDensity);
                    newSystem.name = GenerateSystemName(x, y);
                    sys.Add(newSystem);
                }
            }
        }

        int i = 0;
        foreach(StarSystem ss in sys)
        {
            Vector2 pos = ss.position;
            _starSystems.Add(pos, ss);
            i++;
        }
        Debug.Log(i + " Systems Initialized");
    }

    public static void ShiftActiveSystem(Vector2 pos)
    {
        _activeSystem += pos;
        EventManager.OnSystemChanged(pos);
    }

    public static void SetActiveSystem (Vector2 pos)
    {
        if (_starSystems.ContainsKey(pos))
        {
            _activeSystem = _starSystems[pos].position;
            SaveManager.currentPlayer._activeSystem = _activeSystem;
        }
        else
        {
            Debug.Log("No system found at position " + pos.x + " , " + pos.y);
        }
    }

    public static string GetSystemName(Vector2 pos)
    {
        if (_starSystems.ContainsKey(pos))
        {
            return _starSystems[pos].name;
        }
        else
        {
            return null;
        }
    }

    private static string GenerateSystemName(int x, int y)
    {
        const string allChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        string systemName = "";
        int tacPosition = Convert.ToInt32(Mathf.PingPong(x^2, 3)) + 1;

        for (int i = 0; i < 6; i++)
        {
            if (i == tacPosition)
            {
                systemName += "-";
            }
            else
            {
                int r = Convert.ToInt32(Mathf.Repeat(20 ^ ((x + y) ^ (2 * i)) * (x^3 + 1) + (y^2 + 5) + i ^ 2 + 20^((x + y)^ (2*i)), 36));
                char newChar = allChars[r];
                systemName += newChar;
            }
        }
        return systemName;
    }
}
