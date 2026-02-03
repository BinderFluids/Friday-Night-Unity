using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLine : MonoBehaviour
{
    [SerializeField] private AudioSource hitSound; 

    private void OnTriggerEnter(Collider other)
    {
        hitSound.Play(); 
    }
}
