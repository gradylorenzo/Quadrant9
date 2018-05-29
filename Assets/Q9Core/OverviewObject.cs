using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;

namespace Q9Core
{
    [Serializable]
    public class OverviewObject
    {
        [Serializable]
        public enum objectCategory
        {
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
            CommerceStation,
            Tartarus,
            Star,
            Planet,
            Moon
        }

        [Serializable]
        public enum faction
        {
            friendly,
            unfriendly,
            neutral
        }

        public string name;
        public objectCategory Category;
        public objectType Type;
        public faction Faction;

        public Vector3 position;
        public float scale;
    }
}
