using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum PlanetType
{
    Gas = 0,
    Desert = 1,
    Terra = 2,
    Frozen = 3,
    Molten = 4
}

[Serializable]
public enum MoonType
{
    A,
    B,
    C,
    D,
    E
}

[Serializable]
public class Moon
{
    public int _moonType;
    public Vector3 _position;
}

[Serializable]
public class Planet
{
    public int _planetType;
    public Vector3 _position;
    public Moon[] _moons;
}

[System.Serializable]
public class StarSystem
{
    public string name = "NEW_SYSTEM";
    public Vector2 position = new Vector2();
    public float starSize = 0;
    public float starMass = 0;
    public float starDensity;
    public Color starColor;
    public Planet[] _planets;
}
