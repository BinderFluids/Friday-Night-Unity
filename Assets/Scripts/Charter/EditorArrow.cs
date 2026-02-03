using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class EditorArrow : MonoBehaviour
{
    public AudioSource hitSound;
    public float songPos; 
    public bool playHit;
    public bool hasntHit; 
    public float scrollStartPos; 

    private void Start()
    {
        hitSound = GameObject.Find("Hit").GetComponent<AudioSource>();
        GetComponent<BoxCollider>().isTrigger = true; 
    }

    private void Update()
    {
        if (GridManager.instance.moving && playHit)
        {
            if (songPos > scrollStartPos)
            {
                if (GridManager.instance.songPos >= songPos)
                {
                    if (playHit)
                    {
                        hitSound.Play();
                        playHit = false;
                    }
                }
            }
        }
        else
        {
            scrollStartPos = GridManager.instance.songPos;
            playHit = true; 
        }
    }
}
