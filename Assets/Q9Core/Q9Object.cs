using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;
using Q9Core.CommonData;

namespace Q9Core
{
    [System.Serializable]
    public struct PassiveBonuses
    {
        public Shields _shieldBonus;
        public Integrity _integrityBonus;
        public Capacitor _capacitorBonus;
        public Offensive _offensiveBonus;
        public ResistanceProfile _resistanceBonus;
        public Cargo _cargoBonus;
    }

    public class Q9Object : ScriptableObject
    {
        public string _name;
        public int _id;
        public int _quantity;
        public int _value;
        public float _volume;
        public string _description;
        public Sprite _thumbnail;
    }



    public class Q9Module : Q9Object
    {
        [System.Serializable]
        public enum Slots
        {
            High,
            Mid,
            Low,
            Rig
        }

        public bool _isPassive;
        public float _cooldown;
        public float _capacitorUse;
        public int _size;
        public Slots _slot;

        public float _nextCycle = 0;
        public float _lastCycle = 0;
        public bool _activated;
        public bool _queueDeactivation;
        public GameObject _user;
        public GameObject _target;

        public PassiveBonuses _bonuses;

        public void Activate(GameObject u, GameObject t)
        {
            if (!_isPassive)
            {
                _user = u;
                _target = t;
                _activated = true;
                _nextCycle = 0;
                _lastCycle = 0;
            }
        }

        public void Deactivate()
        {
            _user = null;
            _target = null;
            _queueDeactivation = true;
        }

        public void ModuleUpdate()
        {
            if (!_isPassive)
            {
                if (_activated)
                {
                    if(Time.time > _lastCycle + _cooldown || _lastCycle == 0)
                    {
                        if (_queueDeactivation)
                        {
                            _activated = false;
                            _queueDeactivation = false;
                        }
                        else
                        {
                            if (_user.GetComponent<ShipManager>().currentAttributes._capacitor._capacity > _capacitorUse)
                            {
                                _user.GetComponent<ShipManager>().ConsumeCapacitor(_capacitorUse);
                                doEffect();
                            }
                            else
                            {
                                Q9GameManager._announcer.QueueClip(Q9Announcer.VoicePrompts.InsufficientPower);
                                Deactivate();
                            }
                            _lastCycle = Time.time;
                        }
                    }
                }
            }
        }

        public virtual void doEffect()
        {

        }
    }
}