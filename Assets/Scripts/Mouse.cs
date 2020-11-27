using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Mouse : MonoBehaviour
{
    public GridManager gridManager;
    public MeshRenderer topMeshRenderer;
    public int rows;
    public int cols;
    void Start()
    {
        gridManager = GameObject.Find("/Grid").GetComponent<GridManager>();
        rows = gridManager.rows;
        cols = gridManager.cols;
        
    }
    void OnMouseOver() {
        MeshRenderer topMeshRenderer = transform.GetComponent<MeshRenderer>();
        if (gridManager.isGenerated && !gridManager.isProcessing && Input.GetKey(KeyCode.LeftControl))
        {            
            if (Input.GetKeyDown(KeyCode.Mouse0) && !Input.GetKey(KeyCode.LeftShift)) {
                gridManager.startPoint.SetTopColor(Color.blue);
                string parentName = transform.parent.parent.name;
                Cell start = GameObject.Find($"/Grid/{parentName}").GetComponent<Cell>();
                gridManager.startPoint = start;
                topMeshRenderer.material.color = Color.red;
                print("Start point selected !!!!!!!!");  
            }
            else if(Input.GetKeyDown(KeyCode.Mouse0) && Input.GetKey(KeyCode.LeftShift))
            {
                gridManager.endPoint.SetTopColor(Color.blue);
                string parentName = transform.parent.parent.name;
                Cell end = GameObject.Find($"/Grid/{parentName}").GetComponent<Cell>();
                gridManager.endPoint = end;
                topMeshRenderer.material.color = Color.green;
                print("End point selected !!!!!!!!");
            }

        }
    }
    
}
