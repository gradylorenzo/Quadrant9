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
        public bool _enableOverviewVisiblity;
        public Q9OverviewData _overview;
        public string _id;
        public string _description;
        public bool _isTargetable;
        public bool _isDockable;
        public bool _isMinable;
        public bool _canBridge;
        public bool _isBridging;
        public bool _isAlwaysVisibleInOverview;

        public Q9Object[] _cargo;
    }
}