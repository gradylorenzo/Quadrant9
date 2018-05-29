using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Q9Core
{
    [Serializable]
    public struct DoubleVector3
    {
        #region Members
        public double x;
        public double y;
        public double z;
        private double _magnitude { get; set; }
        #endregion

        #region Properties
        public double magnitude
        {
            get { return _magnitude; }
            set { _magnitude = value; }
        }
        #endregion

        #region Constructors
        public DoubleVector3(double X, double Y, double Z)
        {
            x = X;
            y = Y;
            z = Z;
            _magnitude = Math.Sqrt(x * x + y * y + z * z);
        }
        #endregion

        #region Static Properties
        public static DoubleVector3 zero
        {
            get { return new DoubleVector3(0, 0, 0); }
        }

        public static DoubleVector3 up
        {
            get { return new DoubleVector3(0, 1, 0); }
        }

        public static DoubleVector3 right
        {
            get { return new DoubleVector3(1, 0, 0); }
        }

        public static DoubleVector3 forward
        {
            get { return new DoubleVector3(0, 0, 1); }
        }
        #endregion

        #region Static Methods

        //Lerp
        public static DoubleVector3 Lerp(DoubleVector3 a, DoubleVector3 b, float c)
        {
            double x = (a.x + c * (b.x - a.x));
            double y = (a.y + c * (b.y - a.y));
            double z = (a.z + c * (b.z - a.z));

            return new DoubleVector3(x, y, z);
        }

        //MoveTowards
        public static DoubleVector3 MoveTowards(DoubleVector3 a, DoubleVector3 b, float c)
        {
            DoubleVector3 newPos = b - a;
            double magnitude = newPos.magnitude;
            if (magnitude <= c || magnitude == 0f)
                return b;
            return a + newPos / magnitude * c;
        }

        //FromSingleV3
        public static DoubleVector3 FromVector3(Vector3 v)
        {
            return new DoubleVector3(v.x, v.y, v.z);
        }

        //Distance
        public static double Distance(DoubleVector3 a, DoubleVector3 b)
        {
            return Math.Sqrt(((b.x - a.x) * (b.x - a.x)) + ((b.y - a.y) * (b.y - a.y)) + ((b.z - a.z) * (b.z - a.z)));
        }

        //ToSingleV3
        public static Vector3 ToVector3(DoubleVector3 v)
        {
            float x = Convert.ToSingle(v.x);
            float y = Convert.ToSingle(v.y);
            float z = Convert.ToSingle(v.z);

            return new Vector3(x, y, z);
        }


        #endregion

        #region Operators
        public static DoubleVector3 operator +(DoubleVector3 a, DoubleVector3 b)
        {
            DoubleVector3 v = new DoubleVector3(a.x + b.x, a.y + b.y, a.z + b.z);
            return v;
        }
        public static DoubleVector3 operator -(DoubleVector3 a, DoubleVector3 b)
        {
            DoubleVector3 v = new DoubleVector3(a.x - b.x, a.y - b.y, a.z - b.z);
            return v;
        }
        public static DoubleVector3 operator *(DoubleVector3 a, DoubleVector3 b)
        {
            DoubleVector3 v = new DoubleVector3(a.x * b.x, a.y * b.y, a.z * b.z);
            return v;
        }
        public static DoubleVector3 operator *(DoubleVector3 a, float b)
        {
            DoubleVector3 v = new DoubleVector3(a.x * b, a.y * b, a.z * b);
            return v;
        }
        public static DoubleVector3 operator /(DoubleVector3 a, DoubleVector3 b)
        {
            DoubleVector3 v = new DoubleVector3(a.x / b.x, a.y / b.y, a.z / b.z);
            return v;
        }
        public static DoubleVector3 operator /(DoubleVector3 a, double b)
        {
            DoubleVector3 v = new DoubleVector3(a.x / b, a.y / b, a.z / b);
            return v;
        }
        public static bool operator ==(DoubleVector3 a, DoubleVector3 b)
        {
            if (a.x == b.x && a.y == b.y && a.z == b.z)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool operator !=(DoubleVector3 a, DoubleVector3 b)
        {
            if (a.x != b.x || a.y != b.y || a.z != b.z)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Overrides
        public override bool Equals(object obj)
        {
            if (obj == null || this.GetType() != obj.GetType())
            {
                return false;
            }
            return (this.x == ((DoubleVector3)obj).x && this.y == ((DoubleVector3)obj).y && this.z == ((DoubleVector3)obj).z);
        }
        public override int GetHashCode()
        {
            return this.GetHashCode();
        }
        #endregion
    }
}
