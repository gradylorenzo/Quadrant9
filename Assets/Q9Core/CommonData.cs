using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;

namespace Q9Core.CommonData
{
    #region Attributes
    [System.Serializable]
    public struct ResistanceProfile
    {
        public float thermal;
        public float kinetic;
        public float electro;
        public float explosive;
    }

    [System.Serializable]
    public struct Shields
    {
        public float _capacity;
        public float _rechargeRate;
        public ResistanceProfile _resistances;
    }

    [System.Serializable]
    public struct Integrity
    {
        public float _capacity;
        public ResistanceProfile _resistances;
    }

    [System.Serializable]
    public struct Capacitor
    {
        public float _capacity;
        public float _rechargeRate;
    }

    [System.Serializable]
    public struct Attributes
    {
        public Shields _shield;
        public Integrity _integrity;
        public Capacitor _capacitor;
    }
    #endregion
}
