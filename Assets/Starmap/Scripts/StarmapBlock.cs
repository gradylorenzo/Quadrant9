using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class StarmapBlock : MonoBehaviour {

    public Color normal;
    public Color hover;
    public Color active;
    public float transitionSpeed;


    private Material blockMaterial;
    private Color currentColor;
    private Color wantedColor;


    private void Start()
    {
        currentColor = normal;
        wantedColor = normal;
        blockMaterial = GetComponent<Renderer>().materials[0];
    }

    private void Update()
    {
        currentColor = Color.Lerp(currentColor, wantedColor, transitionSpeed);
        blockMaterial.color = currentColor;
    }

    private void OnMouseOver()
    {
        print("Mouse Over");
        wantedColor = hover;
    }

    private void OnMouseDown()
    {
        print("Mouse Down");
        wantedColor = active;
    }

    private void OnMouseUp()
    {
        print("Mouse Up");
        wantedColor = hover;
    }

    private void OnMouseExit()
    {
        print("Mouse Exit");
        wantedColor = normal;
    }
}
