using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;

namespace Q9Core.CommonData
{
    #region Ship Attributes
    [System.Serializable]
    public enum Tier
    {
        L1,
        L2,
        L3,
        MIL,
        HVT
    }

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
    public enum DamageTypes
    {
        thermal,
        kinetic,
        electro,
        explosive,
    }

    [System.Serializable]
    public struct Weapon
    {
        public Q9Charge.Type _chargeType;
        public DamageTypes _damageType;
        public float _damage;
        public float _optimal;
        public float _range;
        public float _flightSpeed;
        public float _tracking;

        public static Weapon operator + (Weapon w1, Weapon w2)
        {
            Weapon w3 = new Weapon();
            w3._damage = w1._damage + w2._damage;
            w3._optimal = w1._optimal + w2._optimal;
            w3._range = w1._range + w2._range;
            w3._flightSpeed = w1._flightSpeed + w2._flightSpeed;
            w3._tracking = w1._tracking + w2._tracking;

            return w3;
        }
    }

    [System.Serializable]
    public struct ResistanceProfile
    {
        public float _thermal;
        public float _kinetic;
        public float _electro;
        public float _explosive;
    }

    [System.Serializable]
    public struct Physical
    {
        public float _mass;
        public float _signature;
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
        public float _teraflops; //hah :D
        public float _terawatts;
        public Q9Module[] _highSlots;
        public Q9Module[] _midSlots;
        public Q9Module[] _lowSlots;
        public Q9Module[] _rigSlots;
    }

    [System.Serializable]
    public struct Offensive
    {
        public Weapon _laser;
        public Weapon _projectile;
        public Weapon _railgun;
        public Weapon _missile;
    }

    [System.Serializable]
    public struct Cargo
    {
        public float _capacity; //Cargo bay capacity in cubic meters
        public List<Q9Object> _cargo;

        public bool Contains(Q9Object item)
        {
            foreach (Q9Object o in _cargo)
            {
                if(o._name == item._name && o._quantity >= item._quantity)
                {
                    return true;
                }
            }
            return false;
        }

        public bool Contains(Q9Object item, int i)
        {
            foreach (Q9Object o in _cargo)
            {
                if (o._name == item._name && o._quantity >= i)
                {
                    return true;
                }
            }
            return false;
        }

        public void Remove(Q9Object item)
        {
            foreach (Q9Object o in _cargo)
            {
                if (o._name == item._name)
                {
                    if (o._quantity > item._quantity)
                    {
                        o._quantity -= item._quantity;
                    }
                    else if (o._quantity == item._quantity)
                    {
                        _cargo.Remove(o);
                    }
                    else
                    {
                        //Notify the player that the given item was not available in this quantity
                    }
                }
                else
                {
                    //Notify the player that the given item was not found in the cargo
                }
            }
        }

        public void Remove(Q9Object item, int i)
        {
            foreach (Q9Object o in _cargo)
            {
                if (o._name == item._name)
                {
                    if (o._quantity > i)
                    {
                        o._quantity -= i;
                    }
                    else if (o._quantity == i)
                    {
                        _cargo.Remove(o);
                    }
                    else
                    {
                        //Notify the player that the given item was not available in this quantity
                    }
                }
                else
                {
                    //Notify the player that the given item was not found in the cargo
                }
            }
        }
    }

    [System.Serializable]
    public struct Bonuses
    {
        public Shields _shield;
        public Integrity _integrity;
        public Capacitor _capacitor;
        public Offensive _offensive;
        public Cargo _cargo;
    }

    [System.Serializable]
    public struct Attributes
    {
        public Tier _tier;
        public Alliances _alliance;
        public ShipSizes _size;
        public Physical _physical;
        public Shields _shield;
        public Integrity _integrity;
        public Capacitor _capacitor;
        public Fitting _fitting;
        public Offensive _offensive;
        public Cargo _cargo;
    }
    #endregion

    #region Profile
    [System.Serializable]
    public struct Identity
    {
        public string _name;
        public int _credits;
    }

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

    [System.Serializable]
    public class PlayerProfile
    {
        public Identity _identity;
        public Alliances _alliance;
        public Standing[] _standings;
    }

    #endregion
}
