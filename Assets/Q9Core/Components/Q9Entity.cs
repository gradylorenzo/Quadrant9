using System;
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
        [Header("Overview Data")]
        public Q9OverviewData _overview = new Q9OverviewData();
        [Header("Interaction Flags")]
        public bool _canWarpTo;
        public bool _isTargetable;
        public bool _isDockable;
        public bool _isJumpable;
        public bool _isMinable;
        public bool _isLootable;
        public bool _canBridge;
        public bool _isBridging;
        public Q9Object[] _cargo;
        [Header("Visibility Flags")]
        public bool _isAlwaysVisibleInOverview;
        public bool _hideInOverview;
        private bool _started;

        //public DoubleVector3 ScaleSpacePosition;

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

                if(_overview._guid == "" || _overview._guid == null)
                {
                    _overview._guid = Guid.NewGuid().ToString();
                }
                if (GetComponent<ScaleSpaceObject>())
                {

                }
                _started = true;
            }
        }

        public void Remove()
        {
            EventManager.removeOverviewData(_overview);
        }

        public void OnMouseUpAsButton()
        {
            OnPlayerClicked();
        }

        public void OnPlayerClicked()
        {
            EventManager.OnObjectSelected(gameObject, false);

            print("Object " + _overview._name + " clicked. _isTargetable = " + _isTargetable.ToString());
        }
    }
}