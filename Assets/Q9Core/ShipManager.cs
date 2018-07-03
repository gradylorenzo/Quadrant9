using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;
using Q9Core.CommonData;

public class ShipManager : MonoBehaviour {

    public Q9Ship defaultShipData;
    public Attributes baseAttributes;
    public Attributes modifiedAttributes;
    public Attributes currentAttributes;

    private GameObject shipModel;
    public GameObject target;

    private void Awake()
    {
        Q9GameManager._playerShip = this;
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
        baseAttributes._fitting.m_highSlots = new Q9Module[s._attributes._fitting.m_highSlots.Length];
        for(int i = 0; i < baseAttributes._fitting.m_highSlots.Length; i++)
        {
            baseAttributes._fitting.m_highSlots[i] = Instantiate(s._attributes._fitting.m_highSlots[i]);
        }
        foreach (Q9Module m in currentAttributes._fitting.m_highSlots)
        {
            m._user = gameObject;
        }

        //Copy Mid Slots
        baseAttributes._fitting.m_midSlots = new Q9Module[s._attributes._fitting.m_midSlots.Length];
        for (int i = 0; i < baseAttributes._fitting.m_midSlots.Length; i++)
        {
            baseAttributes._fitting.m_midSlots[i] = Instantiate(s._attributes._fitting.m_midSlots[i]);
        }
        foreach (Q9Module m in currentAttributes._fitting.m_midSlots)
        {
            m._user = gameObject;
        }
        //Copy Low Slots
        baseAttributes._fitting.m_lowSlots = new Q9Module[s._attributes._fitting.m_lowSlots.Length];
        for (int i = 0; i < baseAttributes._fitting.m_lowSlots.Length; i++)
        {
            baseAttributes._fitting.m_lowSlots[i] = Instantiate(s._attributes._fitting.m_lowSlots[i]);
        }
        foreach (Q9Module m in currentAttributes._fitting.m_lowSlots)
        {
            m._user = gameObject;
        }

        //Copy Rig Slots
        baseAttributes._fitting.m_rigSlots = new Q9Module[s._attributes._fitting.m_rigSlots.Length];
        for (int i = 0; i < baseAttributes._fitting.m_rigSlots.Length; i++)
        {
            baseAttributes._fitting.m_rigSlots[i] = Instantiate(s._attributes._fitting.m_rigSlots[i]);
        }
        foreach (Q9Module m in currentAttributes._fitting.m_rigSlots)
        {
            m._user = gameObject;
        }

        currentAttributes = baseAttributes;
        CalculateModifiedAttributes();
        print("Ship Loaded");
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
        print("repaired shield for " + a + " points");
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
#endregion

    public void FixedUpdate()
    {
        #region Standard ship loops. Passive Capacitor/Shield recharge. Integrity does not passively recharge.
        if (currentAttributes._shield._capacity < modifiedAttributes._shield._capacity)
        RepairShield(modifiedAttributes._shield._rechargeRate * Time.deltaTime);

        if(currentAttributes._capacitor._capacity < modifiedAttributes._capacitor._capacity)
        RechargeCapacitor(modifiedAttributes._capacitor._rechargeRate * Time.deltaTime);

        foreach(Q9Module m in currentAttributes._fitting.m_highSlots)
        {
            m.ModuleUpdate();
        }

        foreach (Q9Module m in currentAttributes._fitting.m_midSlots)
        {
            m.ModuleUpdate();
        }

        foreach (Q9Module m in currentAttributes._fitting.m_lowSlots)
        {
            m.ModuleUpdate();
        }

#endregion
    }
}
