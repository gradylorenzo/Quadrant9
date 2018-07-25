using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;

public class TestAlign : MonoBehaviour {

    private void OnMouseUpAsButton()
    {
        print("Clicked");
        EventManager.OnObjectSelectedAsAlignmentTarget(DoubleVector3.FromVector3(transform.position) + ScaleSpace.apparentPosition, false);
    }
}
