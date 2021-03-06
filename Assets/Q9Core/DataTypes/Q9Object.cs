﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;
using Q9Core.CommonData;
using System;

namespace Q9Core
{
    public class Q9Object : ScriptableObject
    {
        public string _name;
        public string _id;
        public int _quantity;
        public int _value;
        public float _volume = 0.01f;
        public string _description;
        public Tier _tier;
        public Sprite _thumbnail;
        public Sprite _icon;
        public Physical _physical;
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

        [Header("Fitting Restrictions")]
        public float _teraflops;
        public float _terawatts;
        public Slots _slot;

        [Header ("Passive Module Attributes")]
        public bool _isPassive;
        public Bonuses _passiveBonuses;

        [Header("Active Module Attributes")]
        public float _cooldown;
        public float _capacitorUse;

        #region Properties
        private bool _activated;
        private bool _queueDeactivation;
        public bool isActivated
        {
            get { return _activated; }
        }
        public bool isQueuedToDeactivate
        {
            get { return _queueDeactivation; }
        }

        private GameObject _User;
        private GameObject _Target;
        public GameObject _user
        {
            get { return _User; }
            set { _User = value; }
        }
        public GameObject _target
        {
            get { return _Target; }
            set { _Target = value; }
        }

        private float _nc = 0;
        private float _lc = 0;
        public float _nextCycle
        {
            get { return _nc; }
        }
        public float _lastCycle
        {
            get { return _lc; }
        }

        private bool initialized = false;

        #endregion


        public void Activate(GameObject u, GameObject t)
        {
            if (!_isPassive)
            {
                _user = u;
                _target = t;
                _activated = true;
                _nc = 0;
                _lc = 0;
            }
            if(_cooldown == 0)
            {
                Debug.LogWarning("Warning: Module '" + _name + "' _cooldown is set to 0");
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
            if (!initialized)
            {
                Start();
            }
            if (!_isPassive)
            {
                if (_activated)
                {
                    if(Time.time > _lc + _cooldown || _lc == 0)
                    {
                        if (_queueDeactivation)
                        {
                            _activated = false;
                            _queueDeactivation = false;
                            OnDeactivated();
                        }
                        else
                        {
                            if (_user.GetComponent<ShipController>().currentAttributes._capacitor._capacity > _capacitorUse)
                            {

                                if (_target != null)
                                {
                                    if (_target.GetComponent<ShipController>() != null)
                                    {
                                        doEffect();
                                        _user.GetComponent<ShipController>().ConsumeCapacitor(_capacitorUse);
                                        _lc = Time.time;
                                    }
                                    else
                                    {
                                        EventManager.NotifyTargetInvulnerable();
                                        Deactivate();
                                    }
                                }
                                else
                                {
                                    EventManager.NotifyModuleRequiresActiveTarget();
                                    Deactivate();
                                }
                            }
                            else
                            {
                                EventManager.NotifyModuleInsufficientPower();
                                Deactivate();
                            }
                        }
                    }
                }
            }
        }

        private void Start()
        {
            initialized = true;
            EventManager.OnObjectDestroyed += CheckIfTargetDestroyed;
        }

        private void CheckIfTargetDestroyed(bool wasPlayerShip, GameObject go)
        {
            if(go = _target)
            {
                _target = null;
            }
        }

        public virtual void doEffect()
        {

        }

        public virtual void OnDeactivated()
        {

        }
    }
}