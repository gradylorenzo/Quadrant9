using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;
using Q9Core.CommonData;

public class ShipManager : MonoBehaviour {

    public bool isPlayerShip;
    public Q9Ship defaultShipData;
    public Attributes baseAttributes;
    public Attributes modifiedAttributes;
    public Attributes currentAttributes;

    public GameObject shipModel;
    public GameObject target;

    private void Awake()
    {
        if (isPlayerShip)
        {
            Q9GameManager._playerShip = this;
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
        CalculateModifiedAttributes();
    }

    private void CalculateModifiedAttributes()
    {
        //This method will later calculate the modified attributes of the ships considering all passive modules and, eventually, skills.
        //For now, it just mirrors the base stats.
        modifiedAttributes = baseAttributes;
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

    public void FixedUpdate()
    {
        #region Standard ship loops. Passive Capacitor/Shield recharge. Integrity does not passively recharge.
        if (currentAttributes._shield._capacity < modifiedAttributes._shield._capacity)
        RepairShield(modifiedAttributes._shield._rechargeRate * Time.deltaTime);

        if(currentAttributes._capacitor._capacity < modifiedAttributes._capacitor._capacity)
        RechargeCapacitor(modifiedAttributes._capacitor._rechargeRate * Time.deltaTime);

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

#endregion
    }
}
