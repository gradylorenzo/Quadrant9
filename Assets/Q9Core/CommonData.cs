using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Q9Core;

namespace Q9Core
{
    [Serializable]
    public class OverviewObjectData
    {
        [Serializable]
        public enum objectCategory
        {
            Null,
            Frigate,
            Destroyer,
            Cruiser,
            Battleship,
            Carrier,
            Dreadnought,
            Supercarrier,
            MiningShip,
            Fighter,
            Sentry,
            Wreck,
            Station,
            Jumpgate,
            Star,
            Planet,
            Moon,
            AsteroidBelt,
            Asteroid,
            Wormhole,
            Container,
            Beacon,
            JumpBeacon,
        }

        [Serializable]
        public enum objectType
        {
            Null,
            CommerceStation,
            Tartarus,
            Star,
            Planet,
            Moon
        }

        [Serializable]
        public enum alliance
        {
            Null,
            Neutral,
            YLTGR,
            Unfriendly
        }

        public string guid;
        public string name;
        public bool isTargetable;
        public objectCategory Category;
        public objectType Type;
        public alliance Alliance;

        public DoubleVector3 position;
        //Set to 0 if object is visible system-wide
        public float range;

        public void initialize (DoubleVector3 pos)
        {
            position = pos;
            guid = Guid.NewGuid().ToString();
        }
    }

    [Serializable]
    public struct Location
    {
        public string name;
        public DoubleVector3 position;
        public string PrefabResource;
    }

    [Serializable]
    public struct StarSystem
    {
        public GameObject SSO;
        public GameObject SS1;
        public List<Location> Locations;
    }

}
