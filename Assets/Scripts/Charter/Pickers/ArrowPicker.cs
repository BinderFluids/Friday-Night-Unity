using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ArrowPicker : MonoBehaviour
{
    public int index;
    public TMP_Text text;
    public Cursor cursor;
    public SpriteRenderer arrowRenderer;
    public List<ArrowObject> arrowTypes;
    public ArrowObject currentArrow;

    private void Start()
    {
        arrowRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        //cycle
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (index == 0)
            {
                index = arrowTypes.Count - 1;
            }
            else
            {
                index--;
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (index == arrowTypes.Count - 1)
            {
                index = 0;
            }
            else
            {
                index++;
            }
        }

        //sets arrow type
        currentArrow = arrowTypes[index];
        //writes name
        text.text = arrowTypes[index].editorName;
        //sets image to up arrow for preview
        arrowRenderer.sprite = arrowTypes[index].editorImages[2];
        //sets list of images for the cursor
        cursor.arrowImages = arrowTypes[index].editorImages;
    }
}
