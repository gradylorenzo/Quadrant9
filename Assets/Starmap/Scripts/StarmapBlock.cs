using UnityEngine;

public class StarmapBlock : MonoBehaviour
{
    public Camera camera;
    public GameObject star;
    public StarSystem system;
    public Gradient gradient;

    public void OnClick()
    {
        if (camera)
        {
            camera.GetComponent<MouseOrbit>().target = this.gameObject.transform;
            
        }
    }

    private void Start()
    {
        if (star)
        {
            star.GetComponent<SpriteRenderer>().color = gradient.Evaluate(system.starDensity);
        }
    }

    private void Update()
    {
        if (star && camera)
        {
            star.transform.LookAt(camera.transform);
        }
    }
}
