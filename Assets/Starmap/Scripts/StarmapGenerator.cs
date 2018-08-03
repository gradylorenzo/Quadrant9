using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarmapGenerator : MonoBehaviour {

    public GameObject blockPrefab;
    public Vector2 mapSize;

    public void Awake()
    {
        BuildMap();
    }

    public void BuildMap()
    {
        for(int x = 0; x <= mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                Vector3 pos = new Vector3(x - (mapSize.x / 2), 0, y - (mapSize.y / 2));
                GameObject newBlock = Instantiate(blockPrefab, pos, transform.rotation);
            }
        }
    }
}
