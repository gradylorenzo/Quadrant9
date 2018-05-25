using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;

namespace Q9Core
{
    public class SSLocation : MonoBehaviour
    {

        public float scale;
        public Location Loc;
        private SystemManager SysManager;

        public void Start()
        {
            SetLocation();
        }

        public void SetLocation()
        {
            SysManager = GameObject.FindGameObjectWithTag("SystemManager").GetComponent<SystemManager>();

            Loc.position = DoubleVector3.FromVector3(transform.position) * scale;

            SysManager.System.Locations.Add(Loc);
        }
    }
}
