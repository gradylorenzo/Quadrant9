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
            if (_target)
            {
                if (_user.GetComponent<ShipManager>() && _target != _user)
                {
                    _target.GetComponent<ShipManager>().TakeDamage(_target, _attributes._damage, _attributes._damageType);
                    Debug.Log("Target Hit");
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
