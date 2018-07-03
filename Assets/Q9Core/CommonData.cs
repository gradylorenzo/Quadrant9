using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;

namespace Q9Core.CommonData
{
    #region Ship Attributes
    [System.Serializable]
    public enum ShipSizes
    {
        CommandPod,
        Corvette,
        Destroyer,
        Cruiser,
        Battleship,
        Carrier,
        Dreadnought,
        Supercarrier
    }

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
    public struct Fitting
    {
        public Q9Module[] m_highSlots;
        public Q9Module[] m_midSlots;
        public Q9Module[] m_lowSlots;
        public Q9Module[] m_rigSlots;
    }

    [System.Serializable]
    public struct Cargo
    {
        public float _capacity; //Cargo bay capacity in cubic meters
        public Q9Object[] _cargo;
    }

    [System.Serializable]
    public struct Attributes
    {
        public Alliances _alliance;
        public ShipSizes _size;
        public Shields _shield;
        public Integrity _integrity;
        public Capacitor _capacitor;
        public Fitting _fitting;
        public Cargo _cargo;
    }
    #endregion



    #region Standings
    [System.Serializable]
    public enum Alliances
    {
        Neutral,
        ShadowSyndicate,
        CrimsonUnion,
        TimirgorIndustries,
        Ascendancy,
        OracleFederation,
        PhoenixEmpire
    }

    [System.Serializable]
    public struct Standing
    {
        public Alliances _alliance;
        public float _standing;
    }
    #endregion
}
