using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowColumn : MonoBehaviour
{
    //grid
    public ArrowGrid parentGrid; 
    public int direction;
    private List<GameObject> gridSquares = new List<GameObject>();

    //arrows
    public Dictionary<float, ArrowObject> arrowObjects = new Dictionary<float, ArrowObject>();
    public List<EditorArrow> editorArrows = new List<EditorArrow>();
    public List<GameObject> arrowGameObjects = new List<GameObject>();

    public List<float> newPositions = new List<float>(); 

    //first time
    public void SetInfo (int rows, ArrowGrid parent)
    {
        parentGrid = parent;
        transform.localPosition = new Vector2(parent.columns.IndexOf(this), 0);
        direction = parent.columns.IndexOf(this);

        for (int i = 0; i < rows; i++)
        {
            gridSquares.Add(new GameObject("gridSquare"));
            gridSquares[i].AddComponent<BoxCollider>();
            gridSquares[i].GetComponent<BoxCollider>().center = new Vector3(.5f, .5f, 0);
            gridSquares[i].transform.parent = transform;
            gridSquares[i].transform.localPosition = new Vector2(0, -i);
            gridSquares[i].tag = "ColumnCell";

            SpriteRenderer spriteRenderer = gridSquares[i].AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = LoadChart.instance.gridCellImage;

            //back and forth grid colors
            if (parent.columns.IndexOf(this) % 2 == 0)
            {
                if (i % 2 == 0)
                {
                    gridSquares[i].GetComponent<SpriteRenderer>().color = new Color(.5f, .5f, .5f);
                }
                else
                {

                }
            }
            else
            {
                if (i % 2 == 1)
                {
                    gridSquares[i].GetComponent<SpriteRenderer>().color = new Color(.5f, .5f, .5f);
                }
                else
                {

                }
            }
            
        }
    }
   
    public void AddArrow(float position, ArrowObject arrow)
    {
        //add to world space
        GameObject arrowGameObject = new GameObject(arrow.name);
        arrowGameObject.transform.parent = transform;
        arrowGameObject.transform.localPosition = new Vector2(0, position);
        arrowGameObjects.Add(arrowGameObject);

        //set sprite
        Sprite arrowSprite = arrow.editorImages[direction];
        arrowGameObject.AddComponent<SpriteRenderer>(); 
        arrowGameObject.GetComponent<SpriteRenderer>().sprite = arrowSprite;
        arrowGameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;

        //set song position
        position = Mathf.Abs(CustomMath.Truncate((position / 4) / (SongInfo.instance.bpm / 60)));
        arrowObjects.Add(position, arrow);

        //give editor arrow info for later use
        EditorArrow editorArrow = arrowGameObject.AddComponent<EditorArrow>();
        editorArrows.Add(editorArrow);
        //editorArrow.position = position; 
    }

    private void LoadArrow(float position, ArrowObject arrow)
    {
        float newYposition = -(((SongInfo.instance.bpm * 4) * position) / 60); 

        //add to world space
        GameObject arrowGameObject = new GameObject(arrow.name);
        arrowGameObject.transform.parent = transform;
        arrowGameObject.transform.localPosition = new Vector2(0, Mathf.RoundToInt(newYposition));
        arrowGameObjects.Add(arrowGameObject);

        //set sprite
        Sprite arrowSprite = arrow.editorImages[direction];
        arrowGameObject.AddComponent<SpriteRenderer>();
        arrowGameObject.GetComponent<SpriteRenderer>().sprite = arrowSprite;
        arrowGameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;

        //set song position
        arrowObjects.Add(position, arrow); //the problem is here

        //give editor arrow info for later use
        EditorArrow editorArrow = arrowGameObject.AddComponent<EditorArrow>();
        editorArrows.Add(editorArrow);
       // editorArrow.position = position;
    }

    //public void DeleteArrow(float pos)
    //{
    //    pos = Mathf.Abs(CustomMath.Truncate((pos / 4) / (SongInfo.instance.bpm / 60)));
    //    foreach (EditorArrow arrow in editorArrows)
    //    {
    //        if (arrow.position == pos)
    //        {
    //            arrowObjects.Remove(pos);
    //            editorArrows.Remove(arrow);
    //            arrowGameObjects.Remove(arrow.gameObject);
    //            Destroy(arrow.gameObject);
    //            break;
    //        }
    //    }
    //}

    public void ClearChart()
    {
        foreach(EditorArrow arrow in editorArrows)
        {
            arrowGameObjects.Remove(arrow.gameObject);
            //arrowObjects.Remove(arrow.position);
            Destroy(arrow.gameObject);
        }

        editorArrows.Clear(); 
    }

    private ArrowColumnSave NewJSON()
    {
        ArrowColumnSave save = new ArrowColumnSave();

        foreach(System.Collections.Generic.KeyValuePair<float, ArrowObject> arrow in arrowObjects)
        {
            save.keys.Add(CustomMath.Truncate(arrow.Key));

            string arrowObjectPath = "Arrows/" + arrow.Value.name;
            save.arrowObjectPaths.Add(arrowObjectPath);
        }

        save.direction = direction; 

        return save; 
    }


    public string CreateJSON()
    {
        ArrowColumnSave save = NewJSON();
        string json = JsonUtility.ToJson(save);
        return json; 
    }

    public void LoadJSON(string jsonFileData)
    {
        ArrowColumnSave save = JsonUtility.FromJson<ArrowColumnSave>(jsonFileData);

        direction = save.direction;

        foreach(float pos in save.keys)
        {
            newPositions.Add(CustomMath.Truncate(pos));
        }

        foreach(float pos in newPositions)
        {
            ArrowObject newArrow = Resources.Load<ArrowObject>(save.arrowObjectPaths[newPositions.IndexOf(pos)]);
            LoadArrow(pos, newArrow);
        }
    }
}

[System.Serializable]
public class ArrowColumnSave
{
    public List<float> keys= new List<float>();
    public List<string> arrowObjectPaths = new List<string>();
    public int direction; 
}
