using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadChart : MonoBehaviour
{
    public static LoadChart instance;
    public List<GameObject> gridGameObjects = new List<GameObject>();
    public List<ArrowGrid> gridObjects = new List<ArrowGrid>();
    public Sprite gridCellImage; 
    public SongInfo songInfo;

    public int numberOfGrids; 
    [SerializeField] private GameObject quarterNoteThing;


    public void Awake()
    {
        instance = this;
    }

    public void LoadGridRows(int gridCount)
    {
        foreach(GameObject grid in gridGameObjects)
        {
            songInfo.grids.Remove(grid.GetComponent<ArrowGrid>());
            Destroy(grid);
            Destroy(grid.GetComponent<ArrowGrid>());
        }

        gridObjects.Clear(); 
        gridGameObjects.Clear();

        ArrowGrid.numberOfGrids = 0; 

        //int gridCount = songInfo.grids.Count(); 
        int rows = Mathf.RoundToInt(songInfo.songObject.music.length * ((songInfo.bpm / 60) * 4));

        //creat grid makrers
        for (int i = 0; i < rows; i++)
        {
            if (i % 4 == 1)
            {
                GameObject marker = Instantiate(quarterNoteThing, transform);
                marker.transform.localPosition = new Vector3(4, -i, 0);
            }
        }

        //create grids
        for (int i = 0; i < gridCount; i++)
        {
            GameObject newGridGameObject = new GameObject("grid");
            newGridGameObject.transform.parent = transform;
            gridGameObjects.Add(newGridGameObject);

            ArrowGrid newGridObject = newGridGameObject.AddComponent<ArrowGrid>();
            gridObjects.Add(newGridObject);
            newGridObject.SetInfo(4, rows);
        }
    }
}
