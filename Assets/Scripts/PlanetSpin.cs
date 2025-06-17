using UnityEngine;

public class PlanetSpin : MonoBehaviour
{
    public float rotationSpeed = 30f; //rotation speed in degrees per second
   

    // Update is called once per frame
    void Update()
    {
        //rotate the planet around the z axis
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}
