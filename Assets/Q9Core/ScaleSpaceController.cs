using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;

public class ScaleSpaceController : MonoBehaviour {

	public float scale = 1;

	private void Update()
	{
        this.transform.position = (Vector3.zero - DoubleVector3.ToVector3(ScaleSpace.apparentPosition)) / scale;
	}
}
