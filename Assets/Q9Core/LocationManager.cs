using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Q9Core
{
    public class LocationManager : MonoBehaviour
    {
        public enum LocationState
        {
            inactive,
            active
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                StartLocation();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
            {
                PauseLocation();
            }
        }

        private void StartLocation()
        {
            print("PlayerEntered");
        }

        private void PauseLocation()
        {
            print("PlayerLeft");
        }
    }
}