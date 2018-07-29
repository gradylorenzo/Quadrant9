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
        public Q9OverviewData _overview = new Q9OverviewData();
        public string _description;
        public bool _isTargetable;
        public bool _isDockable;
        public bool _isMinable;
        public bool _canBridge;
        public bool _isBridging;
        public bool _isAlwaysVisibleInOverview;
        public bool _hideInOverview;
        public bool _started;

        public Q9Object[] _cargo;

        private float DistanceAtLastFrame;
        private float DistanceAtThisFrame;

        public void Update()
        {
            if (_started)
            {
                if (!_isAlwaysVisibleInOverview && !_hideInOverview)
                {
                    DistanceAtLastFrame = DistanceAtThisFrame;
                    DistanceAtThisFrame = Vector3.Distance(transform.position, Vector3.zero);

                    if (DistanceAtLastFrame > 1000 && DistanceAtThisFrame < 1000)
                    {
                        EventManager.addOverviewData(_overview);
                    }

                    if (DistanceAtLastFrame < 1000 && DistanceAtThisFrame > 1000)
                    {
                        EventManager.removeOverviewData(_overview);
                    }
                }
            }
            else
            {
                DistanceAtLastFrame = DistanceAtThisFrame;
                DistanceAtThisFrame = Vector3.Distance(transform.position, Vector3.zero);
                _overview._go = this.gameObject;
                if (!_hideInOverview)
                {
                    if (_isAlwaysVisibleInOverview)
                    {
                        EventManager.addOverviewData(_overview);
                    }
                    else
                    {
                        if (DistanceAtLastFrame < 1000 && DistanceAtThisFrame < 1000)
                        {
                            EventManager.addOverviewData(_overview);
                        }
                    }
                }
                _started = true;
            }
        }

        public void Remove()
        {
            print("Removing Overview Data");
            EventManager.removeOverviewData(_overview);
        }
    }
}