using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;
using Q9Core.CommonData;

namespace Q9Core
{
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

        public void Activate(GameObject u, GameObject t)
        {
            if (!_isPassive)
            {
                _user = u;
                _target = t;
                _activated = true;
            }
            Debug.Log("Activated");
        }

        public void Deactivate()
        {
            _user = null;
            _target = null;
            _queueDeactivation = true;
            Debug.Log("Deactivating..");
        }

        public void ModuleUpdate()
        {
            if (!_isPassive)
            {
                if (_activated)
                {
                    if (Time.time > _nextCycle)
                    {
                        if (!_queueDeactivation)
                        {
                            _lastCycle = _nextCycle;
                            _nextCycle += _cooldown;
                            doEffect();
                        }
                        else
                        {
                            _activated = false;
                            _queueDeactivation = false;
                        }
                    }
                }
                else
                {

                }
            }
        }

        public virtual void doEffect()
        {

        }
    }
}