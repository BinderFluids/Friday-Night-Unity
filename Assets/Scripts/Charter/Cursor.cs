using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    private Vector3 mousePos;
    private float mousePosX;
    private float mousePosY;
    private RaycastHit hit;
    private Ray ray;
    private bool inGrid;

    public RuntimeChartSection chartSection;
    public ArrowPicker arrowPicker;
    public int currentImage;
    public List<Sprite> arrowImages;
    public SpriteRenderer arrowRenderer;

    public SongInfo songInfo;

    private void Update()
    {
        //make sure the cursor is on the grid
        CheckPos();
        

        if (Input.GetMouseButtonDown(0) && inGrid && chartSection != null)
        {
            chartSection.AddArrow(new Vector2(mousePosX, mousePosY), arrowPicker.currentArrow, false);
        }
        if (Input.GetMouseButtonDown(1) && inGrid && chartSection != null)
        {
        }
    }

    //sets position and image
    private void CheckPos()
    {
        mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //makes sure the mouse is on the grid
        if (Physics.Raycast(ray, out hit, 100) && hit.transform.gameObject.GetComponent<RuntimeChartSection>() != null)
        {
            transform.parent = hit.transform; 
            chartSection = hit.transform.gameObject.GetComponent<RuntimeChartSection>();
            //sets up grid locking positions
            mousePosX = CustomMath.RoundToNearestFactorOf(1 * transform.parent.transform.localScale.x, Camera.main.ScreenToWorldPoint(mousePos).x - transform.parent.transform.position.x);
            mousePosY = CustomMath.RoundToNearestFactorOf(1 * transform.parent.transform.localScale.y, Camera.main.ScreenToWorldPoint(mousePos).y - transform.parent.transform.position.y);

            inGrid = true;
            transform.localPosition = new Vector2(mousePosX, mousePosY);
        }
        else
        {
            inGrid = false;
            chartSection = null;
        }
    }
}