using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Q9Core;
using Q9Core.CommonData;

[CreateAssetMenu(fileName ="NewWeaponCharge", menuName ="Charge")]
public class Q9Charge : Q9Object
{
    [System.Serializable]
    public enum Type
    {
        lens,
        projectile,
        missile
    }

    public Type _type;
    public DamageTypes _damageType;
    public Offensive _bonus;
}
