using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Q9Core
{
    public enum LocationState
    {
        idle,
        running
    }

    public class LocationManager : MonoBehaviour
    {
        public GameObject LocationPrefab;
        public bool locationIsStatic;
        public bool playerTriggered;
        public float playerExitTime;
        public LocationState State;

        private GameObject LP;

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
                State = LocationState.running;
            }
        }

        public void OnPlayerExit()
        {
            playerExitTime = Time.time;
            State = LocationState.idle;
        }

        public void Update()
        {
            if (State == LocationState.idle)
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

        public void killLocation()
        {
            playerTriggered = false;
            if(LP != null)
            {
                Destroy(LP);
            }
        }
    }
}