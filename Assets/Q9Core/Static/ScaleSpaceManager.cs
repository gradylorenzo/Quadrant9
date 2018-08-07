using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Q9Core
{
    public static class ScaleSpaceManager
    {
        private static DoubleVector3 _apparentPosition;
        
        public static DoubleVector3 apparentPosition
        {
            get
            {
                return _apparentPosition;
            }
        }

        public static void SetApparentPosition(DoubleVector3 pos)
        {
            _apparentPosition = new DoubleVector3(pos.x, pos.y, pos.z);
        }

        public static void Translate(DoubleVector3 pos)
        {
            SetApparentPosition(apparentPosition + pos);
        }

        public static void Translate(Vector3 pos)
        {
            DoubleVector3 v = new DoubleVector3();
            v.x = apparentPosition.x + pos.x;
            v.y = apparentPosition.y + pos.y;
            v.z = apparentPosition.z + pos.z;
            SetApparentPosition(v);

        }

        public static void Warp(DoubleVector3 d, double s)
        {
            SetApparentPosition(DoubleVector3.MoveTowards(apparentPosition, d, s * 149597870700));
        }

        [System.Serializable]
        public enum ScaleSpaceLevel
        {
            ScaleSpace0 = 1,
            ScaleSpace1 = 100000000
        }
    }
}
