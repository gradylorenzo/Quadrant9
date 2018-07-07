using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Q9Core;
using Q9Core.CommonData;

[CreateAssetMenu(fileName ="NewWeapon", menuName ="Module/Weapon")]
public class Q9Weapon : Q9Module
{
    public Q9Charge _charges;
    public Weapon _attributes;

    public override void doEffect()
    {
        if (_user)
        {
            if (_target && _target != _user)
            {
                if (_user.GetComponent<ShipManager>())
                {
                    if (_user.GetComponent<ShipManager>().currentAttributes._cargo.Contains(_charges, 1))
                    {
                        _user.GetComponent<ShipManager>().currentAttributes._cargo.Remove(_charges, 1);
                    }
                }
            }
            else
            {
                //Notify the player that this module requires an active target
            }
        }
    }

    public void loadCharges(Q9Charge c)
    {
        if (!isActivated)
        {
            if (c._type == _attributes._chargeType)
            {
                _charges = c;
            }
            else
            {
                //Notify the player this type of ammo cannot be used with this module
            }
        }
        else
        {
            //Notify the player that the module must be deactivated before changing ammo type.
        }
    }
}
