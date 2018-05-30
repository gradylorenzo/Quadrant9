using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;
public class ScaleSpaceObject : MonoBehaviour {

	public float scale = 1;
    public DoubleVector3 initialPosition;
    


    private void Start()
    {
        initialPosition = DoubleVector3.FromVector3(transform.position) * scale;
    }

    private void Update()
	{
        this.transform.position = DoubleVector3.ToVector3(initialPosition - ScaleSpace.apparentPosition) / scale;
	}
}
