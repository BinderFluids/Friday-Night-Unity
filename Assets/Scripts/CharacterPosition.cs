using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPosition : MonoBehaviour
{
    private SpriteRenderer image;

    public string character;

    private void Awake()
    {
        image = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        Destroy(image);
    }
}
