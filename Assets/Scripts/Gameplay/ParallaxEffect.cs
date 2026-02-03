using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    private float length, startPos;
    private float heigth;
    public float parallaxFactor;
    public GameObject cam;

    private void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        heigth = GetComponent<SpriteRenderer>().bounds.size.y; 
    }

    private void Update()
    {
        float temp = cam.transform.position.x * (1 - parallaxFactor);
        float distance = cam.transform.position.x * parallaxFactor;

        Vector3 newPosition = new Vector3(startPos + distance, transform.position.y, transform.position.z);
        transform.position = newPosition; 

        if (temp > startPos + (length/2))
        {
            startPos += length;
        }
        else if (temp < startPos - (length / 2))
        {
            startPos -= length;
        }
    }
}
