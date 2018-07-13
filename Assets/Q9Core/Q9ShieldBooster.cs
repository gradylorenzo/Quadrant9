using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;
using Q9Core.CommonData;

[CreateAssetMenu(fileName = "New ShieldBooster", menuName = "Module/ShieldBooster")]
public class Q9ShieldBooster : Q9Module {
    public float _boostAmount;

    public override void doEffect()
    {
        if (_user)
        {
            if (_user.GetComponent<ShipManager>())
            {
                _user.GetComponent<ShipManager>().RepairShield(_boostAmount);
            }
            else
            {
                Debug.LogError("No ShipManager attached to _user. You bad. Stop hacking.");
            }
        }
    }
}
