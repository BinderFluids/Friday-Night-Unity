using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RuntimeArrow : MonoBehaviour {
    public ArrowObject arrowObject;
    public EditorArrow editorArrow; 

    public UnityEvent miss;
    public UnityEvent hit;

    [SerializeField] private List<int> arrowDirections = new List<int>() { -1, 0, 1, 2, 3, 4, 5, 6, 7 };
    [SerializeField] private List<string> directionNames = new List<string> { "idle", "left", "down", "up", "right", "mleft", "mdown", "mup", "mright" };

    public float arrowSpeed;
    public float timeToKey;
    public float distanceToKey; 

    public ArrowKey arrowKey;
    public int direction;
    public float songPos; 
    public float timeFalling;
    public float timeAlive; 

    private bool hittable = false;
    private bool missedArrow = false;
    public bool longNote;
    public KeyCode keybind;
    private KeyCode[] keyBinds = new KeyCode[] {KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.UpArrow, KeyCode.RightArrow }; 


    private void Start()
    {
        distanceToKey = Vector2.Distance(transform.position, transform.parent.position);
        arrowSpeed = distanceToKey * timeToKey; 


        timeAlive = 0;
        timeFalling = 0; 

        GetComponent<SpriteRenderer>().sprite = arrowObject.gameImages[direction];
        Rigidbody rb = GetComponent<Rigidbody>();
        BoxCollider boxCollider = GetComponent<BoxCollider>();

        rb.useGravity = false;
        rb.isKinematic = false;

        transform.position = new Vector2(transform.position.x, timeToKey * arrowSpeed);

        boxCollider.isTrigger = true;
    }

    private void Update()
    {
        if (SongManager.instance.songRunning)
        {
            timeAlive += Time.deltaTime;
        }
        if (timeAlive >= songPos)
        {
            timeFalling += Time.deltaTime;
            transform.Translate(Vector2.down * arrowSpeed * Time.deltaTime);
        }

        //check if the arrow no longer becomes hittable, making it missed
        if (hittable)
        {
            if (!CustomMath.GreaterThanLessThan(timeToKey - 0.1f, timeFalling, timeToKey + 0.1f))
            {
                missedArrow = true; 
            }
        }

        //checks if arrow is within "padding" time 
        hittable = CustomMath.GreaterThanLessThan(timeToKey - 0.1f, timeFalling, timeToKey + 0.1f);

        //auto hit if enemy or bot 
        if (CustomMath.Truncate(timeFalling) >= timeToKey && !arrowKey.playerOrEnemy)
        {
            SongManager.instance.voicesPlayer.volume = 1;
            arrowKey.character.PlayArrow(true, directionNames[arrowDirections.IndexOf(direction)]);
            Destroy(gameObject);
        }

        //player hit
        if (hittable && (Input.GetKeyDown(keybind) || Input.GetKeyDown(keyBinds[direction])) && arrowKey.playerOrEnemy)
        {
            SongManager.instance.voicesPlayer.volume = 1; 
            arrowKey.character.PlayArrow(true, directionNames[arrowDirections.IndexOf(direction)]);
            ScoreManager.AddCombo();
            ScoreManager.AddPoints();
            Destroy(gameObject);
        }

        //player missed
        if (missedArrow && arrowKey.playerOrEnemy)
        {
            SongManager.instance.voicesPlayer.volume = 0;
            ScoreManager.KillCombo();
            arrowKey.character.PlayArrow(false, directionNames[arrowDirections.IndexOf(direction)]);
        }

        //destroy if off screen
        if (transform.position.y < -12)
        {
            Destroy(gameObject);
        }
    } 

    private void FixedUpdate()
    {
    }
}