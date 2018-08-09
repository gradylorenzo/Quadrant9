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
        [Serializable]
        public enum VisibilityFlag
        {
            Always,
            Never,
            ProximityOnly
        }
        [Header("Overview Data")]
        public Q9OverviewData _overview = new Q9OverviewData();
        [Header("Interaction Flags")]
        public bool _canWarpTo;
        public bool _isTargetable;
        public bool _isDockable;
        public bool _isJumpable;
        public Vector2 jumpDirection;
        public bool _isMinable;
        public bool _isLootable;
        public bool _canBridge;
        public bool _isBridging;
        public Q9Object[] _cargo;
        [Header("Visibility Flags")]
        
        
        public VisibilityFlag _visibility;
        private bool _started;

        //public DoubleVector3 ScaleSpacePosition;

        private float DistanceAtLastFrame;
        private float DistanceAtThisFrame;

        public void Update()
        {
            if (_started)
            {
                if (_visibility == VisibilityFlag.ProximityOnly)
                {
                    DistanceAtLastFrame = DistanceAtThisFrame;
                    DistanceAtThisFrame = Vector3.Distance(transform.position, Vector3.zero);

                    if (DistanceAtLastFrame > 1000 && DistanceAtThisFrame < 1000)
                    {
                        Add();
                    }

                    if (DistanceAtLastFrame < 1000 && DistanceAtThisFrame > 1000)
                    {
                        Remove();
                    }
                }
            }
            else
            {
                DistanceAtLastFrame = DistanceAtThisFrame;
                DistanceAtThisFrame = Vector3.Distance(transform.position, Vector3.zero);
                _overview._go = this.gameObject;
                if (_visibility == VisibilityFlag.Always)
                {
                    Add();
                }
                else if(_visibility == VisibilityFlag.ProximityOnly)
                {
                    if (DistanceAtLastFrame < 1000 && DistanceAtThisFrame < 1000)
                    {
                        Add();
                    }
                }

                if(_overview._guid == "" || _overview._guid == null)
                {
                    _overview._guid = Guid.NewGuid().ToString();
                }
                _started = true;
            }
        }

        public void Refesh()
        {
            _started = false;
        }

        public void Remove()
        {
            EventManager.removeOverviewData(_overview);
        }

        public void Add()
        {
            EventManager.addOverviewData(_overview);
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