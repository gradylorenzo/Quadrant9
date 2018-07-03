using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;
using Q9Core.CommonData;
using UnityEngine.UI;

namespace Q9Core
{
    public class Q9Entity : MonoBehaviour
    {

        public Q9OverviewData _overview;
        public int _id;
        public string _description;
        public bool _isTargetable;
        public bool _isDockable;
        public bool _isMinable;

        public bool _isAlwaysVisible;

        public Q9Object[] _cargo;
    }
}