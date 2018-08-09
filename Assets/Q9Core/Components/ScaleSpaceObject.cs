using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;

public class ScaleSpaceObject : MonoBehaviour {
    
    public ScaleSpaceManager.ScaleSpaceLevel PlacementScale = ScaleSpaceManager.ScaleSpaceLevel.ScaleSpace0;
    public ScaleSpaceManager.ScaleSpaceLevel ActualScale = ScaleSpaceManager.ScaleSpaceLevel.ScaleSpace0;
    public DoubleVector3 initialPosition;

    private void Awake()
    {
        initialPosition = (DoubleVector3.FromVector3(transform.position) + ScaleSpaceManager.apparentPosition) * (int)PlacementScale;
    }

    private void Update()
	{
        transform.position = DoubleVector3.ToVector3(initialPosition - ScaleSpaceManager.apparentPosition) / (int)ActualScale;
	}
}
