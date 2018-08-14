using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;

public static class NavigationManager
{
    public static Dictionary<Vector2, StarSystem> _starSystems = new Dictionary<Vector2, StarSystem>();
    public static Vector2 activeSystem;

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

                    newSystem._planets = GenerateSystemPlanets(x, y);

                    sys.Add(newSystem);
                }
            }
        }

        int i = 0;
        foreach (StarSystem ss in sys)
        {
            Vector2 pos = ss.position;
            _starSystems.Add(pos, ss);
            i++;
        }
        Debug.Log(i + " Systems Initialized");
    }

    private static Planet[] GenerateSystemPlanets(int x, int y)
    {
        List<Planet> _planets = new List<Planet>();
        int planetCount = 0;
        planetCount = Convert.ToInt32(Mathf.Repeat(x, 4) + Mathf.Repeat(y, 4));
        for (int i = 0; i <= planetCount; i++)
        {
            Planet p = new Planet();
            int planetType = Convert.ToInt32(Mathf.Repeat((int)Mathf.Repeat(x, 5) + (int)Mathf.Repeat(y, 10) + i ^ 3, 5));
            p._planetType = planetType;
            p._moons = GenerateMoons(x, y);
            p._position = GeneratePosition(x, y, i, 1000, 9000);
            _planets.Add(p);
        }

        return _planets.ToArray();
    }

    private static Vector3 GeneratePosition(int x, int y, int i, int lower, int upper)
    {
        float seed = (x + y + 150 + (y + 5 ^ (150 * i)) * (x ^ (y + (150 * i + 1))) ^ (x + y + i + 150) ^ 150) / 12.567f;
        float xCoord = Mathf.Lerp(-upper, upper, Mathf.Repeat((x * (seed * (y + .25f))) + i * (y + 1), 1));
        float zCoord = Mathf.Lerp(-upper, upper, Mathf.Repeat((y * (-seed * (-x + -.25f))) + i * (xCoord + 1), 1));

        if (Mathf.Abs(xCoord) < lower)
        {
            if (xCoord >= 0)
            {
                xCoord = lower;
            }
            else
            {
                xCoord = -lower;
            }
        }

        if (Mathf.Abs(zCoord) <= lower)
        {
            if (zCoord > 0)
            {
                zCoord = lower;
            }
            else
            {
                zCoord = -lower;
            }
        }

        return new Vector3(xCoord, 0, zCoord);
    }

    private static Moon[] GenerateMoons(int x, int y)
    {
        List<Moon> _moons = new List<Moon>();
        int planetCount = 0;
        planetCount = Convert.ToInt32(Mathf.Repeat(x, 4) + Mathf.Repeat(y, 4));
        for (int i = 0; i <= planetCount; i++)
        {
            Moon m = new Moon();
            int moonType = Convert.ToInt32(Mathf.Repeat((int)Mathf.Repeat(x, 5) + (int)Mathf.Repeat(y, 10) + i ^ 3, 5));
            m._moonType = moonType;
            m._position = GeneratePosition(x, y, i, 100, 200);
            _moons.Add(m);
        }

        return _moons.ToArray();
    }

    public static void SetActiveSystem (Vector2 pos)
    {
        if (_starSystems.ContainsKey(pos))
        {
            activeSystem = _starSystems[pos].position;
            SaveManager.currentPlayer._activeSystem = activeSystem;
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
