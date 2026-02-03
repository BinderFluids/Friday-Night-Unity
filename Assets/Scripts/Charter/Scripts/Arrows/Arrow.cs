using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Arrow : MonoBehaviour
{
    public SongManager manager; 
    public List<Sprite> editorImages;
    public List<Sprite> gameImages;
    public string editorName;

    public UnityEvent miss;
    public UnityEvent hit;

    public float arrowSpeed;

    public ArrowKey arrowKey;
    public int direction;

    public bool longNote; 
    public KeyCode keybind; 

    private void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = false;
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true; 
    }

    private void Update()
    {
        transform.Translate(Vector2.down * arrowSpeed * Time.deltaTime);

        //if (transform.position.y < -12)
        //{
        //    Destroy(gameObject);
        //}

        //if (hittable && Input.GetKeyDown(keybind))
        //{
        //    arrowKey.character.PlayArrow(true, direction);
        //    Gameplay.AddCombo();
        //    Gameplay.AddPoints();
        //    Destroy(gameObject);
        //}

        //if (missedArrow)
        //{
        //    Gameplay.KillCombo();
        //    arrowKey.character.PlayArrow(false, direction);
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform == arrowKey.transform && arrowKey.playerOrEnemy)
        {
        }
        else
        {
            //arrowKey.character.PlayArrow(true, direction);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.transform == arrowKey.transform && arrowKey.playerOrEnemy)
        {
        }
    }
}
