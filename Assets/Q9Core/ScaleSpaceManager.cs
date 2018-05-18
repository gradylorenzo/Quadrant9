using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Q9Core
{
    [System.Serializable]
    public struct doubleVector3
    {
        public double x;
        public double y;
        public double z;

        public doubleVector3(double X, double Y, double Z)
        {
            x = X;
            y = Y;
            z = Z;
        }
    }

    public static class ScaleSpace
    {
        private static doubleVector3 _apparentPosition;
        
        public static Vector3 apparentPosition
        {
            get
            {
                Vector3 v3 = new Vector3(Convert.ToSingle(_apparentPosition.x), Convert.ToSingle(_apparentPosition.y), Convert.ToSingle(_apparentPosition.z));
                return v3;
            }
        }

        public static void SetApparentPosition(doubleVector3 pos)
        {
            _apparentPosition = new doubleVector3(pos.x, pos.y, pos.z);
        }

        /*
        private static Vector3 _apparentPosition;

        public static Vector3 apparentPosition
        {
            get
            {
                return _apparentPosition;
            }
        }

        public static void SetApparentPosition(Vector3 pos)
        {
            _apparentPosition = pos;
        }
        */
    }

    public class ScaleSpaceManager : MonoBehaviour
    {
        public doubleVector3 CurrentApparentPosition = new doubleVector3();

        private void Update()
        {
            ScaleSpace.SetApparentPosition(CurrentApparentPosition);
        }
    }
}
