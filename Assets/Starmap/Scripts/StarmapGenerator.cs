using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class Starmap
{
    public List<StarSystem> _starSystems;
}

public class StarmapGenerator : MonoBehaviour {

    public GameObject blockPrefab;
    public Vector2 mapSize;
    public Texture2D noise;
    public List<StarSystem> _starSystems;
    public Gradient gradient;

    public int i { get; private set; }

    public void Awake()
    {
        BuildStarmap();
    }

    private void BuildStarmap()
    {
        foreach(StarSystem ss in _starSystems)
        {
            Vector3 newPos = new Vector3(ss.xCoord, 0, ss.yCoord);
            GameObject newBlock = Instantiate(blockPrefab, newPos, transform.rotation);
            newBlock.GetComponent<StarmapBlock>().camera = Camera.main;
            newBlock.GetComponent<StarmapBlock>().system = ss;
            newBlock.transform.parent = this.transform;
            if(ss.xCoord == mapSize.x/2 && ss.yCoord == mapSize.y / 2)
            {
                newBlock.GetComponent<StarmapBlock>().OnClick();
            }
        }
    }
}
