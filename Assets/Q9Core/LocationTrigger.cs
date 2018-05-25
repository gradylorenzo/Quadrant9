using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Q9Core {

    public class LocationTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player")
            {

            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.tag == "Player")
            {

            }
        }

        public void Update()
        {
        }
    }
}
