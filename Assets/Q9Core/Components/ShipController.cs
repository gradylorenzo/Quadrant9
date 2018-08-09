using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;
using Q9Core.CommonData;

[RequireComponent(typeof(ShipMotionController))]
[RequireComponent(typeof(Q9Entity))]
[RequireComponent(typeof(ShipTransform))]
public class ShipController : MonoBehaviour {

    private ShipTransform sTrans;

    #region variables
    [Header("Attributes")]
    public bool isPlayerShip;
    public Q9Ship defaultShipData;
    public Attributes baseAttributes;
    public Attributes modifiedAttributes;
    public Attributes currentAttributes;

    

    #region
    [Header("Combat")]
    [NonSerialized]
    public GameObject _activeTarget;
    [NonSerialized]
    public List<TargetInfo> _lockedTargets = new List<TargetInfo>();

    

    [NonSerialized]
    public string guid;
    private GameObject shipModel;
    private GameObject ExplosionPrefab;
    private Camera mainCamera;
    private bool isReady = false;
    
    #endregion
    #endregion

    private void Awake()
    {
        sTrans = GetComponent<ShipTransform>();
        sTrans.isPlayerShip = isPlayerShip;
        guid = Guid.NewGuid().ToString();
        mainCamera = Camera.main;
        if (!isPlayerShip)
        {
            LoadShip(defaultShipData);
        }
        else
        {
            GameManager._playerShip = this;
            
            if (SaveManager.profileLoaded)
            {
                LoadShip(SaveManager.currentPlayer._allShips[SaveManager.currentPlayer._currentShip]);
            }
            else
            {
                LoadShip(defaultShipData);
            }
        }
        //EventManager.OnObjectLocked += OnObjectLocked;
        EventManager.OnObjectSelected += OnObjectSelected;
        EventManager.OnObjectDamaged += TakeDamage;
        //EventManager.OnObjectUnlocked += OnObjectUnlocked;
        EventManager.OnGameInitializationComplete += OnGameInitializationComplete;
        EventManager.OnWarpEntered += OnWarpEntered;
    }

    

    public void FixedUpdate()
    {
        #region Standard ship loops. Passive Capacitor/Shield recharge. Integrity does not passively recharge.
        if (currentAttributes._shield._capacity < modifiedAttributes._shield._capacity)
            RepairShield(currentAttributes._shield._rechargeRate * Time.deltaTime);

        if (currentAttributes._capacitor._capacity < modifiedAttributes._capacitor._capacity)
            RechargeCapacitor(currentAttributes._capacitor._rechargeRate * Time.deltaTime);

        foreach (Q9Module m in currentAttributes._fitting._highSlots)
        {
            if (m != null)
                m.ModuleUpdate();
        }
        foreach (Q9Module m in currentAttributes._fitting._midSlots)
        {
            if (m != null)
                m.ModuleUpdate();
        }
        foreach (Q9Module m in currentAttributes._fitting._lowSlots)
        {
            if (m != null)
                m.ModuleUpdate();
        }
        

        //This loop determines if a target lock should be finished based on when is started.
        for (int i = 0; i < _lockedTargets.Count; i++)
        {
            if (!_lockedTargets[i]._lockComplete)
            {
                if (Time.time > _lockedTargets[i]._lockStart + _lockedTargets[i]._lockTime)
                {
                    TargetInfo newTI = new TargetInfo();
                    newTI._lockComplete = true;
                    newTI._target = _lockedTargets[i]._target;
                    _lockedTargets[i] = newTI;
                    if (_activeTarget == null)
                    {
                        SelectTarget(_lockedTargets[i]._target);
                    }
                    EventManager.OnTargetLockComplete();
                }
            }
        }
        #endregion

        if (isPlayerShip)
        {
            DoLockingCheck();
        }
    }

    #region general methods
    private void OnGameInitializationComplete(Q9Ship s)
    {
        if (isPlayerShip)
        {
            LoadShip(s);
        }
    }

    public void LoadShip(Q9Ship s)
    {
        if(shipModel != null)
        {
            Destroy(shipModel);
            shipModel = null;
        }

        if(s._explosionPrefab != null)
        ExplosionPrefab = s._explosionPrefab;

        if (mainCamera)
        {
            mainCamera.gameObject.GetComponent<MouseOrbit>().distanceMin = s._minCameraDistance;
        }
        GameObject newShip;
        newShip = Instantiate(s._model, transform.position, transform.rotation);
        newShip.transform.SetParent(gameObject.transform);
        shipModel = newShip;
        sTrans.shipModel = shipModel;
        baseAttributes = s._attributes;
        
        //Copy Fitting
        //Copy High Slots
        baseAttributes._fitting._highSlots = new Q9Module[s._attributes._fitting._highSlots.Length];
        for(int i = 0; i < baseAttributes._fitting._highSlots.Length; i++)
        {
            if (s._attributes._fitting._highSlots[i] != null)
            {
                baseAttributes._fitting._highSlots[i] = Instantiate(s._attributes._fitting._highSlots[i]);
            }
        }
        foreach (Q9Module m in currentAttributes._fitting._highSlots)
        {
            m._user = this.gameObject;
        }

        //Copy Mid Slots
        baseAttributes._fitting._midSlots = new Q9Module[s._attributes._fitting._midSlots.Length];
        for (int i = 0; i < baseAttributes._fitting._midSlots.Length; i++)
        {
            if (s._attributes._fitting._midSlots[i] != null)
            {
                baseAttributes._fitting._midSlots[i] = Instantiate(s._attributes._fitting._midSlots[i]);

            }
        }
        foreach (Q9Module m in currentAttributes._fitting._midSlots)
        {
            m._user = this.gameObject;
        }
        //Copy Low Slots
        baseAttributes._fitting._lowSlots = new Q9Module[s._attributes._fitting._lowSlots.Length];
        for (int i = 0; i < baseAttributes._fitting._lowSlots.Length; i++)
        {
            if (s._attributes._fitting._lowSlots[i] != null)
            {
                baseAttributes._fitting._lowSlots[i] = Instantiate(s._attributes._fitting._lowSlots[i]);
            }
        }
        foreach (Q9Module m in currentAttributes._fitting._lowSlots)
        {
            m._user = this.gameObject;
        }

        //Copy Rig Slots
        baseAttributes._fitting._rigSlots = new Q9Module[s._attributes._fitting._rigSlots.Length];
        for (int i = 0; i < baseAttributes._fitting._rigSlots.Length; i++)
        {
            if (s._attributes._fitting._rigSlots[i] != null)
            {
                baseAttributes._fitting._rigSlots[i] = Instantiate(s._attributes._fitting._rigSlots[i]);
            }
        }
        foreach (Q9Module m in currentAttributes._fitting._rigSlots)
        {
            m._user = this.gameObject;
        }

        modifiedAttributes = baseAttributes;
        currentAttributes = modifiedAttributes;
        
        CalculateModifiedAttributes(true);

        //Q9Entity
        GetComponent<Q9Entity>()._overview._name = s._name;
        GetComponent<Q9Entity>()._overview._alliance = s._attributes._alliance.ToString();
        GetComponent<Q9Entity>()._overview._type = s._attributes._type.ToString();
        GetComponent<Q9Entity>()._overview._thumbnail = s._thumbnail;
        GetComponent<Q9Entity>()._overview._icon = s._icon;
        GetComponent<Q9Entity>()._overview._guid = guid;
        GetComponent<Q9Entity>()._isTargetable = !isPlayerShip;
        GetComponent<Q9Entity>()._isDockable = false;
        GetComponent<Q9Entity>()._isMinable = false;
        if(isPlayerShip)
        {
            GetComponent<Q9Entity>()._visibility = Q9Entity.VisibilityFlag.Never;
        }
        else
        {
            GetComponent<Q9Entity>()._visibility = Q9Entity.VisibilityFlag.ProximityOnly; 
        }

        sTrans._torque = currentAttributes._travel._torque;
        sTrans._power = currentAttributes._travel._power;
        sTrans._burnSpeed = currentAttributes._travel._burnSpeed;
        sTrans._warpStrength = currentAttributes._travel._warpStrength;
        sTrans._warpSpeed = currentAttributes._travel._warpSpeed;
        sTrans._inertia = currentAttributes._physical._inertia;
        sTrans._mass = currentAttributes._physical._mass;

        isReady = true;

        if (isPlayerShip)
        {
            EventManager.OnGameIsReady();
        }
    }

    private void CalculateModifiedAttributes(bool ResetCurrentAttributesAfterRecalculate)
    {
        //Mirror base stats
        modifiedAttributes = baseAttributes;
        //Go through modules, determine if they are passive, and if they are, modify the ModifiedAttributes accordingly
        //Go through High Slot modules
        foreach (Q9Module m in modifiedAttributes._fitting._highSlots)
        {
            //Is the slot filled?
            if (m != null)
            {
                //Calculate new physical attributes, regardless of module passivity
                modifiedAttributes._physical._mass += m._physical._mass;
                modifiedAttributes._physical._signature += m._physical._signature;

                //Is the module in the slot passive? Active module bonuses will be ignored.
                if (m._isPassive)
                {
                    //Calculate shield modifications
                    //Calculate new shield capacity and base recharge rate
                    modifiedAttributes._shield._capacity += m._passiveBonuses._shield._capacity;
                    modifiedAttributes._shield._rechargeRate += m._passiveBonuses._shield._rechargeRate;

                    //Calculate new resistance profile for shields
                    float s_raw_therm_resist = modifiedAttributes._shield._resistances._thermal + (Mathf.Clamp01(m._passiveBonuses._shield._resistances._thermal));
                    float s_raw_kin_resist = modifiedAttributes._shield._resistances._kinetic + (Mathf.Clamp01(m._passiveBonuses._shield._resistances._kinetic));
                    float s_raw_elect_resist = modifiedAttributes._shield._resistances._electro + (Mathf.Clamp01(m._passiveBonuses._shield._resistances._electro));
                    float s_raw_explo_resist = modifiedAttributes._shield._resistances._thermal + (Mathf.Clamp01(m._passiveBonuses._shield._resistances._explosive));

                    //Resistances limited to 90%
                    modifiedAttributes._shield._resistances._thermal = Mathf.Clamp(s_raw_therm_resist, 0, .90f);
                    modifiedAttributes._shield._resistances._kinetic = Mathf.Clamp(s_raw_kin_resist, 0, .90f);
                    modifiedAttributes._shield._resistances._electro = Mathf.Clamp(s_raw_elect_resist, 0, .90f);
                    modifiedAttributes._shield._resistances._explosive = Mathf.Clamp(s_raw_explo_resist, 0, .90f);

                    //Calculate integrity modifications
                    //Calculate new resistance profile for integrity
                    modifiedAttributes._integrity._capacity += m._passiveBonuses._integrity._capacity;

                    //Calculate new resistance profile for integrity
                    float i_raw_therm_resist = modifiedAttributes._integrity._resistances._thermal + (Mathf.Clamp01(m._passiveBonuses._integrity._resistances._thermal));
                    float i_raw_kin_resist = modifiedAttributes._integrity._resistances._kinetic + (Mathf.Clamp01(m._passiveBonuses._integrity._resistances._kinetic));
                    float i_raw_elect_resist = modifiedAttributes._integrity._resistances._electro + (Mathf.Clamp01(m._passiveBonuses._integrity._resistances._electro));
                    float i_raw_explo_resist = modifiedAttributes._integrity._resistances._thermal + (Mathf.Clamp01(m._passiveBonuses._integrity._resistances._explosive));

                    //Resistances limited to 90%
                    modifiedAttributes._integrity._resistances._thermal = Mathf.Clamp(i_raw_therm_resist, 0, .90f);
                    modifiedAttributes._integrity._resistances._kinetic = Mathf.Clamp(i_raw_kin_resist, 0, .90f);
                    modifiedAttributes._integrity._resistances._electro = Mathf.Clamp(i_raw_elect_resist, 0, .90f);
                    modifiedAttributes._integrity._resistances._explosive = Mathf.Clamp(i_raw_explo_resist, 0, .90f);

                    //Calculate capacitor modifications
                    modifiedAttributes._capacitor._capacity += m._passiveBonuses._capacitor._capacity;
                    modifiedAttributes._capacitor._rechargeRate += m._passiveBonuses._capacitor._capacity;

                    //Calculate offensive modifications
                    modifiedAttributes._offensive._laser += m._passiveBonuses._offensive._laser;
                    modifiedAttributes._offensive._projectile += m._passiveBonuses._offensive._projectile;
                    modifiedAttributes._offensive._railgun += m._passiveBonuses._offensive._railgun;
                    modifiedAttributes._offensive._missile += m._passiveBonuses._offensive._missile;

                    //Calculate cargo capacity modifications
                    modifiedAttributes._cargo._capacity += m._passiveBonuses._cargo._capacity;
                }
            }
        }

        //Go through Mid Slot modules
        foreach (Q9Module m in modifiedAttributes._fitting._midSlots)
        {
            //Is the slot filled?
            if (m != null)
            {
                //Is the module in the slot passive? Active module bonuses will be ignored.
                if (m._isPassive)
                {
                    //Calculate new physical attributes
                    modifiedAttributes._physical._mass += m._physical._mass;
                    modifiedAttributes._physical._signature += m._physical._signature;

                    //Calculate shield modifications
                    //Calculate new shield capacity and base recharge rate
                    modifiedAttributes._shield._capacity += m._passiveBonuses._shield._capacity;
                    modifiedAttributes._shield._rechargeRate += m._passiveBonuses._shield._rechargeRate;

                    //Calculate new resistance profile for shields
                    float s_raw_therm_resist = modifiedAttributes._shield._resistances._thermal + (Mathf.Clamp01(m._passiveBonuses._shield._resistances._thermal));
                    float s_raw_kin_resist = modifiedAttributes._shield._resistances._kinetic + (Mathf.Clamp01(m._passiveBonuses._shield._resistances._kinetic));
                    float s_raw_elect_resist = modifiedAttributes._shield._resistances._electro + (Mathf.Clamp01(m._passiveBonuses._shield._resistances._electro));
                    float s_raw_explo_resist = modifiedAttributes._shield._resistances._thermal + (Mathf.Clamp01(m._passiveBonuses._shield._resistances._explosive));

                    //Resistances limited to 90%
                    modifiedAttributes._shield._resistances._thermal = Mathf.Clamp(s_raw_therm_resist, 0, .90f);
                    modifiedAttributes._shield._resistances._kinetic = Mathf.Clamp(s_raw_kin_resist, 0, .90f);
                    modifiedAttributes._shield._resistances._electro = Mathf.Clamp(s_raw_elect_resist, 0, .90f);
                    modifiedAttributes._shield._resistances._explosive = Mathf.Clamp(s_raw_explo_resist, 0, .90f);

                    //Calculate integrity modifications
                    //Calculate new resistance profile for integrity
                    modifiedAttributes._integrity._capacity += m._passiveBonuses._integrity._capacity;

                    //Calculate new resistance profile for integrity
                    float i_raw_therm_resist = modifiedAttributes._integrity._resistances._thermal + (Mathf.Clamp01(m._passiveBonuses._integrity._resistances._thermal));
                    float i_raw_kin_resist = modifiedAttributes._integrity._resistances._kinetic + (Mathf.Clamp01(m._passiveBonuses._integrity._resistances._kinetic));
                    float i_raw_elect_resist = modifiedAttributes._integrity._resistances._electro + (Mathf.Clamp01(m._passiveBonuses._integrity._resistances._electro));
                    float i_raw_explo_resist = modifiedAttributes._integrity._resistances._thermal + (Mathf.Clamp01(m._passiveBonuses._integrity._resistances._explosive));

                    //Resistances limited to 90%
                    modifiedAttributes._integrity._resistances._thermal = Mathf.Clamp(i_raw_therm_resist, 0, .90f);
                    modifiedAttributes._integrity._resistances._kinetic = Mathf.Clamp(i_raw_kin_resist, 0, .90f);
                    modifiedAttributes._integrity._resistances._electro = Mathf.Clamp(i_raw_elect_resist, 0, .90f);
                    modifiedAttributes._integrity._resistances._explosive = Mathf.Clamp(i_raw_explo_resist, 0, .90f);

                    //Calculate capacitor modifications
                    modifiedAttributes._capacitor._capacity += m._passiveBonuses._capacitor._capacity;
                    modifiedAttributes._capacitor._rechargeRate += m._passiveBonuses._capacitor._capacity;

                    //Calculate offensive modifications
                    modifiedAttributes._offensive._laser += m._passiveBonuses._offensive._laser;
                    modifiedAttributes._offensive._projectile += m._passiveBonuses._offensive._projectile;
                    modifiedAttributes._offensive._railgun += m._passiveBonuses._offensive._railgun;
                    modifiedAttributes._offensive._missile += m._passiveBonuses._offensive._missile;

                    //Calculate cargo capacity modifications
                    modifiedAttributes._cargo._capacity += m._passiveBonuses._cargo._capacity;
                }
            }
        }

        //Go through Low Slot modules
        foreach (Q9Module m in modifiedAttributes._fitting._lowSlots)
        {
            //Is the slot filled?
            if (m != null)
            {
                //Is the module in the slot passive? Active module bonuses will be ignored.
                if (m._isPassive)
                {
                    //Calculate new physical attributes
                    modifiedAttributes._physical._mass += m._physical._mass;
                    modifiedAttributes._physical._signature += m._physical._signature;

                    //Calculate shield modifications
                    //Calculate new shield capacity and base recharge rate
                    modifiedAttributes._shield._capacity += m._passiveBonuses._shield._capacity;
                    modifiedAttributes._shield._rechargeRate += m._passiveBonuses._shield._rechargeRate;

                    //Calculate new resistance profile for shields
                    float s_raw_therm_resist = modifiedAttributes._shield._resistances._thermal + (Mathf.Clamp01(m._passiveBonuses._shield._resistances._thermal));
                    float s_raw_kin_resist = modifiedAttributes._shield._resistances._kinetic + (Mathf.Clamp01(m._passiveBonuses._shield._resistances._kinetic));
                    float s_raw_elect_resist = modifiedAttributes._shield._resistances._electro + (Mathf.Clamp01(m._passiveBonuses._shield._resistances._electro));
                    float s_raw_explo_resist = modifiedAttributes._shield._resistances._thermal + (Mathf.Clamp01(m._passiveBonuses._shield._resistances._explosive));

                    //Resistances limited to 90%
                    modifiedAttributes._shield._resistances._thermal = Mathf.Clamp(s_raw_therm_resist, 0, .90f);
                    modifiedAttributes._shield._resistances._kinetic = Mathf.Clamp(s_raw_kin_resist, 0, .90f);
                    modifiedAttributes._shield._resistances._electro = Mathf.Clamp(s_raw_elect_resist, 0, .90f);
                    modifiedAttributes._shield._resistances._explosive = Mathf.Clamp(s_raw_explo_resist, 0, .90f);

                    //Calculate integrity modifications
                    //Calculate new resistance profile for integrity
                    modifiedAttributes._integrity._capacity += m._passiveBonuses._integrity._capacity;

                    //Calculate new resistance profile for integrity
                    float i_raw_therm_resist = modifiedAttributes._integrity._resistances._thermal + (Mathf.Clamp01(m._passiveBonuses._integrity._resistances._thermal));
                    float i_raw_kin_resist = modifiedAttributes._integrity._resistances._kinetic + (Mathf.Clamp01(m._passiveBonuses._integrity._resistances._kinetic));
                    float i_raw_elect_resist = modifiedAttributes._integrity._resistances._electro + (Mathf.Clamp01(m._passiveBonuses._integrity._resistances._electro));
                    float i_raw_explo_resist = modifiedAttributes._integrity._resistances._thermal + (Mathf.Clamp01(m._passiveBonuses._integrity._resistances._explosive));

                    //Resistances limited to 90%
                    modifiedAttributes._integrity._resistances._thermal = Mathf.Clamp(i_raw_therm_resist, 0, .90f);
                    modifiedAttributes._integrity._resistances._kinetic = Mathf.Clamp(i_raw_kin_resist, 0, .90f);
                    modifiedAttributes._integrity._resistances._electro = Mathf.Clamp(i_raw_elect_resist, 0, .90f);
                    modifiedAttributes._integrity._resistances._explosive = Mathf.Clamp(i_raw_explo_resist, 0, .90f);

                    //Calculate capacitor modifications
                    modifiedAttributes._capacitor._capacity += m._passiveBonuses._capacitor._capacity;
                    modifiedAttributes._capacitor._rechargeRate += m._passiveBonuses._capacitor._capacity;

                    //Calculate offensive modifications
                    modifiedAttributes._offensive._laser += m._passiveBonuses._offensive._laser;
                    modifiedAttributes._offensive._projectile += m._passiveBonuses._offensive._projectile;
                    modifiedAttributes._offensive._railgun += m._passiveBonuses._offensive._railgun;
                    modifiedAttributes._offensive._missile += m._passiveBonuses._offensive._missile;

                    //Calculate cargo capacity modifications
                    modifiedAttributes._cargo._capacity += m._passiveBonuses._cargo._capacity;
                }
            }
        }

        //Go through Rig Slot modules
        foreach (Q9Module m in modifiedAttributes._fitting._lowSlots)
        {
            //Is the slot filled?
            if (m != null)
            {
                //Is the module in the slot passive? Active module bonuses will be ignored.
                if (m._isPassive)
                {
                    //Calculate new physical attributes
                    modifiedAttributes._physical._mass += m._physical._mass;
                    modifiedAttributes._physical._signature += m._physical._signature;

                    //Calculate shield modifications
                    //Calculate new shield capacity and base recharge rate
                    modifiedAttributes._shield._capacity += m._passiveBonuses._shield._capacity;
                    modifiedAttributes._shield._rechargeRate += m._passiveBonuses._shield._rechargeRate;

                    //Calculate new resistance profile for shields
                    float s_raw_therm_resist = modifiedAttributes._shield._resistances._thermal + (Mathf.Clamp01(m._passiveBonuses._shield._resistances._thermal));
                    float s_raw_kin_resist = modifiedAttributes._shield._resistances._kinetic + (Mathf.Clamp01(m._passiveBonuses._shield._resistances._kinetic));
                    float s_raw_elect_resist = modifiedAttributes._shield._resistances._electro + (Mathf.Clamp01(m._passiveBonuses._shield._resistances._electro));
                    float s_raw_explo_resist = modifiedAttributes._shield._resistances._thermal + (Mathf.Clamp01(m._passiveBonuses._shield._resistances._explosive));

                    //Resistances limited to 90%
                    modifiedAttributes._shield._resistances._thermal = Mathf.Clamp(s_raw_therm_resist, 0, .90f);
                    modifiedAttributes._shield._resistances._kinetic = Mathf.Clamp(s_raw_kin_resist, 0, .90f);
                    modifiedAttributes._shield._resistances._electro = Mathf.Clamp(s_raw_elect_resist, 0, .90f);
                    modifiedAttributes._shield._resistances._explosive = Mathf.Clamp(s_raw_explo_resist, 0, .90f);

                    //Calculate integrity modifications
                    //Calculate new resistance profile for integrity
                    modifiedAttributes._integrity._capacity += m._passiveBonuses._integrity._capacity;

                    //Calculate new resistance profile for integrity
                    float i_raw_therm_resist = modifiedAttributes._integrity._resistances._thermal + (Mathf.Clamp01(m._passiveBonuses._integrity._resistances._thermal));
                    float i_raw_kin_resist = modifiedAttributes._integrity._resistances._kinetic + (Mathf.Clamp01(m._passiveBonuses._integrity._resistances._kinetic));
                    float i_raw_elect_resist = modifiedAttributes._integrity._resistances._electro + (Mathf.Clamp01(m._passiveBonuses._integrity._resistances._electro));
                    float i_raw_explo_resist = modifiedAttributes._integrity._resistances._thermal + (Mathf.Clamp01(m._passiveBonuses._integrity._resistances._explosive));

                    //Resistances limited to 90%
                    modifiedAttributes._integrity._resistances._thermal = Mathf.Clamp(i_raw_therm_resist, 0, .90f);
                    modifiedAttributes._integrity._resistances._kinetic = Mathf.Clamp(i_raw_kin_resist, 0, .90f);
                    modifiedAttributes._integrity._resistances._electro = Mathf.Clamp(i_raw_elect_resist, 0, .90f);
                    modifiedAttributes._integrity._resistances._explosive = Mathf.Clamp(i_raw_explo_resist, 0, .90f);

                    //Calculate capacitor modifications
                    modifiedAttributes._capacitor._capacity += m._passiveBonuses._capacitor._capacity;
                    modifiedAttributes._capacitor._rechargeRate += m._passiveBonuses._capacitor._capacity;

                    //Calculate offensive modifications
                    modifiedAttributes._offensive._laser += m._passiveBonuses._offensive._laser;
                    modifiedAttributes._offensive._projectile += m._passiveBonuses._offensive._projectile;
                    modifiedAttributes._offensive._railgun += m._passiveBonuses._offensive._railgun;
                    modifiedAttributes._offensive._missile += m._passiveBonuses._offensive._missile;

                    //Calculate cargo capacity modifications
                    modifiedAttributes._cargo._capacity += m._passiveBonuses._cargo._capacity;
                }
            }
        }

        if (ResetCurrentAttributesAfterRecalculate)
        {
            currentAttributes = modifiedAttributes;
        }
    }

    private void DoLockingCheck()
    {
        bool anythingLocking = false;
        foreach (TargetInfo ti in _lockedTargets)
        {
            if (!ti._lockComplete)
                anythingLocking = true;
        }

        EventManager.isPlayerLocking = anythingLocking;
    }

    public void Die()
    {
        EventManager.OnObjectDestroyed(isPlayerShip, gameObject);
        EventManager.OnObjectLocked -= OnObjectLocked;
        EventManager.OnObjectSelected -= OnObjectSelected;
        EventManager.OnObjectDamaged -= TakeDamage;
        if (!isPlayerShip)
        {
            GameManager._playerShip.UnlockTarget(gameObject);
            print("NPC Died");
            Destroy(gameObject);
            if(GetComponent<Q9Entity>())
            {
                GetComponent<Q9Entity>().Remove();
            }
        }
        else
        {

        }
        if (ExplosionPrefab)
        {
            Instantiate(ExplosionPrefab, transform.position, transform.rotation);
        }
    }
    #endregion

    #region repair methods

    public void RepairShield(float a)
    {
        currentAttributes._shield._capacity = Mathf.Clamp(currentAttributes._shield._capacity + a, 0, modifiedAttributes._shield._capacity);
    }

    public void RepairIntegrity(float a)
    {
        currentAttributes._integrity._capacity = Mathf.Clamp(currentAttributes._integrity._capacity + a, 0, modifiedAttributes._integrity._capacity);
    }

    public void RechargeCapacitor(float a)
    {
        currentAttributes._capacitor._capacity = Mathf.Clamp(currentAttributes._capacitor._capacity + a, 0, modifiedAttributes._capacitor._capacity);
    }

    public void ConsumeCapacitor(float a)
    {
        currentAttributes._capacitor._capacity -= a;
    }

    public void TakeDamage(float a)
    {

    }

    #endregion

    #region target management methods

    private void OnWarpEntered()
    {
        ClearTargets();
    }

    public void OnObjectLocked(GameObject go)
    {
        if (isPlayerShip && isReady)
        {
            if (go != this.gameObject)
            {
                LockTarget(go);
            }
        }
    }

    public void OnObjectUnlocked(GameObject go)
    {
        if(isPlayerShip && isReady)
        UnlockTarget(go);
    }

    public void OnObjectSelected(GameObject go, bool forceLock)
    {
        if (isPlayerShip && isReady)
        {
            if ((Input.GetKey(KeyCode.LeftControl) || forceLock) && go.GetComponent<Q9Entity>()._isTargetable)
            {
                LockTarget(go);
            }
            else if (Input.GetKey(KeyCode.LeftAlt))
            {
                UnlockTarget(go);
            }
            if (go != this.gameObject)
            {
                SelectTarget(go);
            }
        }
    }

    public void LockTarget(GameObject t)
    {
        bool exists = false;
        foreach (TargetInfo ti in _lockedTargets)
        {
            if (ti._target.GetComponent<Q9Entity>()._overview._guid == t.GetComponent<Q9Entity>()._overview._guid)
            {
                exists = true;
            }
        }

        if (!exists)
        {
            if (_lockedTargets.Count < 5)
            {
                TargetInfo newLT = new TargetInfo();
                newLT._lockComplete = false;
                newLT._lockStart = Time.time;
                newLT._lockTime = 5; //Replace this number later with the output of the lock time algorithm.
                newLT._target = t;
                _lockedTargets.Add(newLT);
            }
            else
            {
                EventManager.NotifyLockLimitReached();
            }
        }
        else
        {
            SelectTarget(t);
        }
    }

    public void UnlockTarget(GameObject t)
    {
        int i = 0;
        bool found = false;
        foreach(TargetInfo ti in _lockedTargets)
        {
            if(ti._target == t)
            {
                if (ti._lockComplete)
                {
                    i = _lockedTargets.IndexOf(ti);
                    found = true;
                }
            }
        }

        if (found)
        {
            _lockedTargets.RemoveAt(i);
            if(_activeTarget == t)
            {
                if(_lockedTargets.Count > 0)
                {
                    _activeTarget = _lockedTargets[0]._target;
                }
                else
                {
                    _activeTarget = null;
                }
            }
            print("Unlocked");
        }
        else
        {
            print("Object either not locked or lock incomplete");
        }
    }

    public void SelectTarget(GameObject t)
    {
        foreach (TargetInfo ti in _lockedTargets)
        {
            if (ti._target.GetComponent<Q9Entity>()._overview._guid == t.GetComponent<Q9Entity>()._overview._guid)
            {
                if (ti._lockComplete)
                {
                    _activeTarget = ti._target;
                }
            }
        }
    }

    public void ClearTargets()
    {
        _activeTarget = null;
        _lockedTargets.Clear();
    }
    #endregion

    #region damage methods
    public void TakeDamage(GameObject go, float d, DamageTypes t)
    {
        if(go == gameObject)
        {
            float f;
            switch (t)
            {
                case DamageTypes.thermal:
                    f = d * (1 - (currentAttributes._shield._resistances._thermal));
                    if(currentAttributes._shield._capacity > f)
                    {
                        DamageShields(f);
                    }
                    else
                    {
                        f = (f - currentAttributes._shield._capacity) * (1 - currentAttributes._integrity._resistances._thermal);
                        DamageShields(currentAttributes._shield._capacity);
                        DamageIntegrity(f);
                    }
                    break;
                case DamageTypes.kinetic:
                    f = d * (1 - (currentAttributes._shield._resistances._kinetic));
                    if (currentAttributes._shield._capacity > f)
                    {
                        DamageShields(f);
                    }
                    else
                    {
                        f = (f - currentAttributes._shield._capacity) * (1 - currentAttributes._integrity._resistances._kinetic);
                        DamageShields(currentAttributes._shield._capacity);
                        DamageIntegrity(f);
                    }
                    break;
                case DamageTypes.electro:
                    f = d * (1 - (currentAttributes._shield._resistances._electro));
                    if (currentAttributes._shield._capacity > f)
                    {
                        DamageShields(f);
                    }
                    else
                    {
                        f = (f - currentAttributes._shield._capacity) * (1 - currentAttributes._integrity._resistances._electro);
                        DamageShields(currentAttributes._shield._capacity);
                        DamageIntegrity(f);
                    }
                    break;
                case DamageTypes.explosive:
                    f = d * (1 - (currentAttributes._shield._resistances._explosive));
                    if (currentAttributes._shield._capacity > f)
                    {
                        DamageShields(f);
                    }
                    else
                    {
                        f = (f - currentAttributes._shield._capacity) * (1 - currentAttributes._integrity._resistances._explosive);
                        DamageShields(currentAttributes._shield._capacity);
                        DamageIntegrity(f);
                    }
                    break;
            }
        }
    }

    private void DamageIntegrity(float d)
    {
        currentAttributes._integrity._capacity -= d;
        if(currentAttributes._integrity._capacity <= 0)
        {
            Die();
        }
    }

    private void DamageShields(float d)
    {
        currentAttributes._shield._capacity -= d;
    }

    #endregion
}
