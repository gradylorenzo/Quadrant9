using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;
using Q9Core.CommonData;

public class ShipManager : MonoBehaviour {

    [Header("Attributes")]
    public bool isPlayerShip;
    public Q9Ship defaultShipData;
    public Attributes baseAttributes;
    public Attributes modifiedAttributes;
    public Attributes currentAttributes;

    public string guid = Guid.NewGuid().ToString();
    private GameObject shipModel;
    public GameObject _activeTarget;
    public List<TargetInfo> _lockedTargets = new List<TargetInfo>();

    private void Awake()
    {
        if (isPlayerShip)
        {
            Q9GameManager._playerShip = this;
            print("Player Ship Assigned!");
        }
        LoadShip(defaultShipData);
    }

    public void LoadShip(Q9Ship s)
    {
        if(shipModel != null)
        {
            Destroy(shipModel);
            shipModel = null;
        }

        GameObject newShip;
        newShip = Instantiate(s._model, transform.position, transform.rotation);
        newShip.transform.SetParent(gameObject.transform);
        shipModel = newShip;
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
            m._user = gameObject;
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
            m._user = gameObject;
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
            m._user = gameObject;
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
            m._user = gameObject;
        }

        currentAttributes = baseAttributes;
        modifiedAttributes = baseAttributes;
        CalculateModifiedAttributes(true);
    }

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
    #region combat methods
    public void LockTarget(GameObject t)
    {
        //Check to make sure the locked target limit hasn't been reached
        if(_lockedTargets.Count < 5)
        {
            bool exists = false;
            foreach (TargetInfo ti in _lockedTargets)
            {
                if (ti._target.GetComponent<ShipManager>().guid == t.GetComponent<ShipManager>().guid)
                {
                    exists = true;
                }
            }

            if (exists)
            {
                TargetInfo newLT = new TargetInfo();
                newLT._lockComplete = false;
                newLT._lockStart = Time.time;
                //Replace this number later with the output of the lock time algorithm.
                newLT._lockTime = 0;
                newLT._target = t;
                _lockedTargets.Add(newLT);

                if (_activeTarget == null)
                {
                    _activeTarget = newLT._target;
                }
            }
        }
    }

    public void UnlockTarget(GameObject t)
    {
        foreach(TargetInfo ti in _lockedTargets)
        {
            if(ti._target.GetComponent<ShipManager>().guid == t.GetComponent<ShipManager>().guid)
            {
                if (ti._lockComplete)
                {
                    _lockedTargets.Remove(ti);
                }
            }
        }
    }

    public void SelectTarget(GameObject t)
    {
        foreach (TargetInfo ti in _lockedTargets)
        {
            if (ti._target.GetComponent<ShipManager>().guid == t.GetComponent<ShipManager>().guid)
            {
                if (ti._lockComplete)
                {
                    _activeTarget = ti._target;
                }
            }
        }
    }
    #endregion

    public void FixedUpdate()
    {
        #region Standard ship loops. Passive Capacitor/Shield recharge. Integrity does not passively recharge.
        if (currentAttributes._shield._capacity < modifiedAttributes._shield._capacity)
        RepairShield(currentAttributes._shield._rechargeRate * Time.deltaTime);

        if(currentAttributes._capacitor._capacity < modifiedAttributes._capacitor._capacity)
        RechargeCapacitor(currentAttributes._capacitor._rechargeRate * Time.deltaTime);

        foreach(Q9Module m in currentAttributes._fitting._highSlots)
        {
            if(m != null)
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

        foreach (TargetInfo ti in _lockedTargets)
        {
            if (!ti._lockComplete)
            {
                if (Time.time > ti._lockStart + ti._lockTime)
                {
                    ti.CompleteLock();
                }
            }
        }
        #endregion
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
}
