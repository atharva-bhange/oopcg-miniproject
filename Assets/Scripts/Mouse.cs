using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Mouse : MonoBehaviour
{
    GridManager gridManager;
    MeshRenderer topMeshRenderer;
    int rows;
    int cols;

    void Start()
    {
        gridManager = GameObject.Find("/Grid").GetComponent<GridManager>();
        rows = gridManager.rows;
        cols = gridManager.cols;

    }
    void OnMouseOver()
    {
        MeshRenderer topMeshRenderer = transform.GetComponent<MeshRenderer>();
        if (gridManager.isGenerated && !gridManager.isProcessing)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && Input.GetKey(KeyCode.LeftControl))
            {
                gridManager.startPoint.SetTopColor(gridManager.resetGridColor);
                string parentName = transform.parent.parent.name;
                Cell start = GameObject.Find($"/Grid/{parentName}").GetComponent<Cell>();
                gridManager.startPoint = start;
                start.SetTopColor(gridManager.startPointColor);
            }
            else if (Input.GetKeyDown(KeyCode.Mouse1) && Input.GetKey(KeyCode.LeftControl))
            {
                gridManager.endPoint.SetTopColor(gridManager.resetGridColor);
                string parentName = transform.parent.parent.name;
                Cell end = GameObject.Find($"/Grid/{parentName}").GetComponent<Cell>();
                gridManager.endPoint = end;
                end.SetTopColor(gridManager.endPointColor);
            }
            

        }
    }

    
}