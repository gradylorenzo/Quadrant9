using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;

public class ScaleSpaceObject : MonoBehaviour {
    
    public ScaleSpace.ScaleSpaceLevel PlacementScale = ScaleSpace.ScaleSpaceLevel.ScaleSpace0;
    public ScaleSpace.ScaleSpaceLevel ActualScale = ScaleSpace.ScaleSpaceLevel.ScaleSpace0;
    public DoubleVector3 initialPosition;

    private void Start()
    {
        initialPosition = (DoubleVector3.FromVector3(transform.position) + ScaleSpace.apparentPosition) * (int)PlacementScale;
    }

    private void Update()
	{
        transform.position = DoubleVector3.ToVector3(initialPosition - ScaleSpace.apparentPosition) / (int)ActualScale;
	}
}
