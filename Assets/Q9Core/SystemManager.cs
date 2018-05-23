using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Q9Core
{
    [Serializable]
    public struct Location
    {
        public string name;
        public DoubleVector3 position;
        public string PrefabResource;
    }

    [Serializable]
    public struct StarSystem
    {
        public GameObject SSO;
        public GameObject SS1;
        public Location[] Locations;
    }

    public class SystemManager : MonoBehaviour
    {
        public StarSystem System;

        public void Start()
        {
            LoadStarSystem();
        }

        public void LoadStarSystem()
        {
            foreach(Location lt in System.Locations)
            {
                GameObject go = Resources.Load("LocationTriggerPrefabs/" + lt.PrefabResource) as GameObject;
                GameObject cp = Instantiate(go, DoubleVector3.ToVector3(lt.position), go.transform.rotation);
                cp.transform.parent = System.SSO.transform;
            }
        }
    }
}
