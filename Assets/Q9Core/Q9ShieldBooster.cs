using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;
using Q9Core.CommonData;

[CreateAssetMenu(fileName = "New ShieldBooster", menuName = "ShieldBooster")]
public class Q9ShieldBooster : Q9Module {

    public float _boostAmount;

    public override void doEffect()
    {
        base.doEffect();
        if (_user)
        {
            if (_user.GetComponent<ShipManager>())
            {
                if (_user.GetComponent<ShipManager>().currentAttributes._capacitor._capacity > _capacitorUse)
                {
                    _user.GetComponent<ShipManager>().RepairShield(_boostAmount);
                    _user.GetComponent<ShipManager>().ConsumeCapacitor(_capacitorUse);
                }
                else
                {
                    Deactivate();
                }
            }
            else
            {
            }
        }
    }
}
