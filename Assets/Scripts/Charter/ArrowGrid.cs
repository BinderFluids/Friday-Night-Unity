using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ArrowGrid : MonoBehaviour
{
    public static int numberOfGrids = 0; 

    public List<ArrowColumn> columns = new List<ArrowColumn>();
    public List<string> columnJsonFileInfos = new List<string>();
    public string keysName; 

    public void SetInfo (int columns, int rows)
    {
        numberOfGrids++;
        SongInfo.instance.grids.Add(this);

        for (int i = 0; i < columns; i++)
        {
            GameObject newColumn = new GameObject("column");
            newColumn.transform.parent = transform;

            ArrowColumn newColumnObject = newColumn.AddComponent<ArrowColumn>();
            this.columns.Add(newColumnObject);

            newColumnObject.SetInfo(rows, this);
        }

        transform.localPosition = new Vector2((numberOfGrids * 5) - 5f, 0);
    }

    public void ClearChart()
    {
        foreach(ArrowColumn column in columns)
        {
            column.ClearChart(); 
        }
    }

    private ArrowGridSave NewJSON()
    {
        ArrowGridSave save = new ArrowGridSave();

        foreach(ArrowColumn column in columns)
        {
            save.columnJsonFileInfos.Add(column.CreateJSON()); 
        }

        save.keysName = keysName; 

        return save; 
    }

    public string CreateJSON()
    {
        ArrowGridSave save = NewJSON();
        return JsonUtility.ToJson(save);
    }

    public ArrowGrid LoadJSON(string jsonFileData)
    {
        ArrowGridSave loadedArrowGrid = JsonUtility.FromJson<ArrowGridSave>(jsonFileData);
        columnJsonFileInfos = loadedArrowGrid.columnJsonFileInfos;
        keysName = loadedArrowGrid.keysName; 

        transform.localPosition = new Vector2((numberOfGrids * 5) - 5f, 0);

        for (int i = 0; i < columnJsonFileInfos.Count; i++)
        {
            columns[i].LoadJSON(columnJsonFileInfos[i]);
        }

        return this; 
    }
}

[System.Serializable]
public class ArrowGridSave
{
    public List<string> columnJsonFileInfos = new List<string>();
    public int columns;
    public string keysName; 
}