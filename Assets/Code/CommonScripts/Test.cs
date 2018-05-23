using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;

public class Test : MonoBehaviour {

    public GameObject SSM;

    private void OnMouseUpAsButton()
    {
        if (SSM)
        {
            SSM.GetComponent<ScaleSpaceManager>().wantedPosition = DoubleVector3.FromVector3((transform.localPosition) * SSM.GetComponent<ScaleSpaceManager>().Scale[gameObject.tag]);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 100);
    }
}
