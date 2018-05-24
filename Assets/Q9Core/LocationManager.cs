using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Q9Core
{
    public enum LocationState
    {
        inactive,
        active
    }

    public class LocationManager : MonoBehaviour
    {
        public GameObject LocationPrefab;
        public bool locationIsStatic;
        public bool playerTriggered;
        public float playerExitTime;
        public LocationState State;

        private GameObject LP;
        private List<GameObject> locationObjects;


        public void OnPlayerEnter()
        {
            if (!playerTriggered)
            {
                playerTriggered = true;
                LP = Instantiate(LocationPrefab, transform.position, transform.rotation);
                LP.transform.parent = this.transform;
            }
            else
            {
                State = LocationState.active;
            }
        }

        public void OnPlayerExit()
        {
            playerExitTime = Time.time;
            State = LocationState.inactive;
        }

        public void Update()
        {
            if (State == LocationState.inactive)
            {
                if (playerTriggered)
                {
                    if (Time.time > playerExitTime + 300)
                    {
                        killLocation();
                    }
                }
            }
        }

        public void AddLocationObject(GameObject go)
        {
            locationObjects.Add(go);
        }

        public void RemoveLocationObject (GameObject go)
        {
            if (locationObjects.Contains(go))
            {
                locationObjects.Remove(go);
            }
        }

        public void killLocation()
        {
            playerTriggered = false;
            if(LP != null)
            {
                foreach(GameObject go in locationObjects)
                {
                    RemoveLocationObject(go);
                    Destroy(go);
                }
                Destroy(LP);
            }
        }
    }
}