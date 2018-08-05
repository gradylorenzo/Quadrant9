using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core.CommonData;

namespace Q9Core.PlayerData
{
    #region Profile
    [System.Serializable]
    public struct Identity
    {
        public string _name;
        public int _credits;
    }

    [System.Serializable]
    public struct Standing
    {
        public Alliances _alliance;
        public float _standing;
    }

    [System.Serializable]
    public class PlayerProfile
    {
        public Identity _identity;
        public string _currentShip;
        public Vector2 _activeSystem;
        public Dictionary<string, Q9Ship> _allShips = new Dictionary<string, Q9Ship>();

        public PlayerProfile()
        {

        }
    }
    #endregion
}
