using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;
public class ScaleSpaceObject : MonoBehaviour {
    
    [System.Serializable]
    public enum ScaleSpaceLevel
    {
        ScaleSpace0 = 1,
        ScaleSpace1 = 100000000
    }

    public ScaleSpaceLevel PlacementScale = ScaleSpaceLevel.ScaleSpace0;
    public ScaleSpaceLevel ActualScale = ScaleSpaceLevel.ScaleSpace0;
    private DoubleVector3 initialPosition;

    private void Start()
    {
        initialPosition = (DoubleVector3.FromVector3(transform.position) + ScaleSpace.apparentPosition) * (int)PlacementScale;
    }

    private void Update()
	{
        transform.position = DoubleVector3.ToVector3(initialPosition - ScaleSpace.apparentPosition) / (int)ActualScale;
	}
}
