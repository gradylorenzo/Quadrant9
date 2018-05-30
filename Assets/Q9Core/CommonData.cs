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
            Friendly,
            Unfriendly,
            Neutral
        }

        public string guid;
        public string name;
        public Sprite thumbnail;
        public objectCategory Category;
        public objectType Type;
        public alliance Alliance;

        public Vector3 position;
        public float scale;

        public void generateID ()
        {
            guid = Guid.NewGuid().ToString();
        }
    }
}
