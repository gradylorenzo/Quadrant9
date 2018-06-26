using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;
using Q9Core.CommonData;

public class ShipManager : MonoBehaviour {

    public Q9Ship defaultShipData;
    public Attributes maxAttributes;
    public Attributes currentAttributes;
    private GameObject shipModel;

    private void Start()
    {
        //GameManager playerShip = this.gameObject;
        LoadShip(defaultShipData);
    }

    public void LoadShip(Q9Ship s)
    {
        if(shipModel != null)
        {
            Destroy(shipModel);
            shipModel = null;
        }

        GameObject newShip;
        newShip = Instantiate(s._model, transform.position, transform.rotation);
        newShip.transform.SetParent(gameObject.transform);
        shipModel = newShip;
        maxAttributes = s._attributes;
        currentAttributes = maxAttributes;
    }
}
