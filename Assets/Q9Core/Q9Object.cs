using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;
using Q9Core.CommonData;

namespace Q9Core
{
    public class Q9Object : ScriptableObject
    {
        public string _name;
        public int _id;
        public int _quantity;
        public int _value;
        public string _description;
        public Sprite _thumbnail;
    }

    

    public class Q9Module : Q9Object
    {

    }
}