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
        public Q9Ship _currentShip;
        public Q9Ship[] _allShips;

        public PlayerProfile()
        {

        }

        public PlayerProfile (Identity _id, Alliances _all, Standing[] _stand, Q9Ship _q9s, Q9Ship[] _allq9s)
        {
            _identity = _id;
            //_alliance = _all;
            //_standings = _stand;
            _currentShip = _q9s;
            _allShips = _allq9s;
        }
    }
    #endregion
}
