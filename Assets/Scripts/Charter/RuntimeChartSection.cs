using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RuntimeChartSection : MonoBehaviour {

    public int section;
    public int columnCount = 4;
    public int timeSigTop = 4;
    public int timeSigBottom = 4;

    public List<EditorArrow> editorArrows = new List<EditorArrow>();
    public ChartSection chartClass; 

    [SerializeField] SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.size = new Vector2(columnCount, timeSigTop * timeSigBottom);

        gameObject.AddComponent<BoxCollider>();
    }

    public void NewSection(ChartSection chartInfo)
    {
        chartClass = chartInfo; 
        spriteRenderer.size = new Vector2(chartInfo.columnCount, chartInfo.timeSigTop * chartInfo.timeSigBottom);
        section = chartInfo.section; 

        foreach (ArrowArray arrowGroup in chartInfo.columns)
        {
            foreach (ChartArrow arrow in arrowGroup.arrows)
            {
                AddArrow(new Vector2(chartClass.columns.IndexOf(arrowGroup), arrow.gridY), arrow.arrowObject, true);
            }
        }
    }

    public void AddArrow(Vector2 mousePos, ArrowObject arrow, bool loading)
    {
        //add to world space
        GameObject arrowGameObject = new GameObject(arrow.name);
        arrowGameObject.transform.parent = transform;
        arrowGameObject.transform.localPosition = mousePos;

        //set sprite
        Sprite arrowSprite = arrow.editorImages[Mathf.RoundToInt(mousePos.x)];
        arrowGameObject.AddComponent<SpriteRenderer>();
        arrowGameObject.GetComponent<SpriteRenderer>().sprite = arrowSprite;
        arrowGameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;

        //set song position
        float songPosition = CustomMath.WorldSpaceToSongPos(mousePos.y, timeSigTop) + CustomMath.WorldSpaceToSongPos(timeSigTop * timeSigBottom * section, timeSigTop);
        //sets the song position in the arrow object itself

        //give editor arrow info for later use
        EditorArrow editorArrow = arrowGameObject.AddComponent<EditorArrow>();
        editorArrow.songPos = songPosition; 
        editorArrows.Add(editorArrow);

        //add arrow to list
        ChartArrow chartArrow = new ChartArrow();
        chartArrow.songPos = songPosition;
        chartArrow.arrowObject = arrow;
        chartArrow.gridX = Mathf.RoundToInt(mousePos.x);
        chartArrow.gridY = Mathf.RoundToInt(mousePos.y);

        if (!loading)
        {
            chartClass.columns[Mathf.RoundToInt(mousePos.x)].arrows.Add(chartArrow);
        }
    }

    public void ClearSection()
    {
        foreach (EditorArrow arrow in editorArrows)
        {
            Destroy(arrow.gameObject); 
        }

        editorArrows.Clear(); 
    }

    public IEnumerator SaveData()
    {
        yield return null; 
    }

    public IEnumerator LoadData()
    {
        yield return null; 
    }
}

[System.Serializable]
public class ArrowArray {
    public List<ChartArrow> arrows = new List<ChartArrow>();
}
[System.Serializable]
public class ChartSection {
    public int section;
    public int columnCount = 4;
    public int timeSigTop = 4;
    public int timeSigBottom = 4;

    public List<ArrowArray> columns = new List<ArrowArray>();
}

[System.Serializable]
public class FullChart {
    public List<ChartSection> sections = new List<ChartSection>();
}
