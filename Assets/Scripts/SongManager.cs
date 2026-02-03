using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SongManager : MonoBehaviour
{
    public static SongManager instance;
    public AudioSource hitSource; 

    public GameObject[] playerKeys;
    public GameObject[] enemyKeys;
    private KeyCode[] keyBinds = new KeyCode[] {KeyBinds.left, KeyBinds.down, KeyBinds.up, KeyBinds.right };
    public RuntimeCharacter bf;
    public RuntimeCharacter enemy;
    [SerializeField] private GameObject arrowPrefab;

    public float bpm;
    public float arrowSpeed;
    public float songPos;
    public float songTime; 
    public float timeToKey;

    [SerializeField] public AudioSource musicPlayer;
    [SerializeField] public AudioSource voicesPlayer; 
    public AudioClip music;
    public AudioClip voices; 
    public List<ArrowGrid> grids = new List<ArrowGrid>();
    public List<FullChart> fullCharts = new List<FullChart>();

    public List<UnityAction> functions = new List<UnityAction>();


    public float timer;
    public bool songRunning; 

    private void Awake()
    {
        instance = this;
        songRunning = false;
        songPos = 0;
        timeToKey = 1;
    }

    private void Start()
    { 
        StartCoroutine(CountDown());
    }

    private IEnumerator LoadArrows()
    {
        int chartLoop = 0;
        GameObject[] arrowGameObjects = null;

        while (fullCharts.Count < 2)
        {
            yield return null; 
        }

        foreach (FullChart fullChart in fullCharts)
        {
            foreach(ChartSection section in fullChart.sections)
            {
                foreach (ArrowArray column in section.columns)
                {
                    foreach (ChartArrow arrow in column.arrows)
                    {
                        switch (chartLoop)
                        {
                            case 0:
                                arrowGameObjects = playerKeys;
                                break;
                            case 1:
                                arrowGameObjects = enemyKeys;
                                break;
                        }

                        GameObject arrowGO = Instantiate(arrowPrefab, 
                        new Vector2(arrowGameObjects[arrow.gridX].transform.position.x, arrowGameObjects[arrow.gridX].transform.position.y), Quaternion.identity, arrowGameObjects[arrow.gridX].transform);
                        RuntimeArrow runtimeArrow = arrowGO.GetComponent<RuntimeArrow>();

                        arrowGO.GetComponent<SpriteRenderer>().sprite = arrow.arrowObject.gameImages[arrow.gridX];

                        runtimeArrow.direction = arrow.gridX;
                        runtimeArrow.arrowObject = arrow.arrowObject;
                        runtimeArrow.arrowKey = arrowGameObjects[arrow.gridX].GetComponent<ArrowKey>();
                        runtimeArrow.keybind = keyBinds[arrow.gridX];
                        runtimeArrow.arrowSpeed = arrowSpeed;
                        runtimeArrow.songPos = arrow.songPos;

                        runtimeArrow.timeToKey = timeToKey; 
                    }
                }
            }
            chartLoop++; 
            yield return null;
        }
        Debug.Log("Arrows Loaded");
    }

    //public IEnumerator InstantiateArrows(float pos, GameObject[] arrowGameObjects)
    //{
    //    foreach (ArrowGrid grid in grids)
    //    {
    //        switch(grid.keysName)
    //        {
    //            case "player":
    //                arrowGameObjects = playerKeys;
    //                break;
    //            case "enemy":
    //                arrowGameObjects = enemyKeys;
    //                break;
    //        }
    //        foreach (ArrowColumn column in grid.columns)
    //        {
    //            if (column.arrowObjects.ContainsKey(pos))
    //            {
    //                GameObject arrow = Instantiate(arrowPrefab, new Vector2(arrowGameObjects[grid.columns.IndexOf(column)].transform.position.x,
    //                    arrowGameObjects[grid.columns.IndexOf(column)].transform.position.y + distanceToKey), Quaternion.identity, arrowGameObjects[grid.columns.IndexOf(column)].transform);
    //                RuntimeArrow runtimeArrow = arrow.GetComponent<RuntimeArrow>();
    //                arrow.GetComponent<SpriteRenderer>().sprite = column.arrowObjects[pos].gameImages[grid.columns.IndexOf(column)];

    //                runtimeArrow.direction = grid.columns.IndexOf(column);
    //                runtimeArrow.arrowObject = column.arrowObjects[pos];
    //                runtimeArrow.arrowKey = arrowGameObjects[grid.columns.IndexOf(column)].GetComponent<ArrowKey>();
    //                runtimeArrow.keybind = keyBinds[grid.columns.IndexOf(column)];
    //                runtimeArrow.arrowSpeed = arrowSpeed;

    //                column.arrowObjects.Remove(pos);
    //            }
    //        }
    //        yield return null; 
    //    }
    //}

    private IEnumerator HitSound()
    {
        yield return new WaitForSeconds(timeToKey);
        hitSource.Play();
    }

    private IEnumerator WaitForEndSong()
    {
        while(songPos < music.length)
        {
            songPos += Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene(0);
    }

    IEnumerator CountDown()
    {
        yield return StartCoroutine(LoadArrows());

        instance = this;
        
        yield return new WaitForSeconds((60 / bpm) * 3);

        foreach (GameObject arrowKey in playerKeys)
        {
            arrowKey.GetComponent<ArrowKey>().playerOrEnemy = true;
            arrowKey.GetComponent<ArrowKey>().character = bf;
        }
        foreach(GameObject arrowKey in enemyKeys)
        {
            arrowKey.GetComponent<ArrowKey>().playerOrEnemy = false;
            arrowKey.GetComponent<ArrowKey>().character = enemy;
        }

        StartCoroutine(WaitForEndSong());
        songRunning = true;

        yield return new WaitForSeconds(timeToKey);

        musicPlayer.clip = music;
        musicPlayer.Play();
        voicesPlayer.clip = voices;
        voicesPlayer.Play();
    }
}