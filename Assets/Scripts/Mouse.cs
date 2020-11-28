using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Mouse : MonoBehaviour
{
    // for selecting start and end
    GridManager gridManager;
    MeshRenderer topMeshRenderer;
    Generator generator;
    int rows;
    int cols;

    // for wall deletion
    


    void Start()
    {
        gridManager = GameObject.Find("/Grid").GetComponent<GridManager>();
        generator = GameObject.Find("/Grid").GetComponent<Generator>();
        rows = gridManager.rows;
        cols = gridManager.cols;
        
    }
    void OnMouseOver() {
        MeshRenderer topMeshRenderer = transform.GetComponent<MeshRenderer>();
        if (gridManager.isGenerated && !gridManager.isProcessing )
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
            else if (Input.GetKey(KeyCode.Mouse0) && Input.GetKey(KeyCode.LeftAlt)) { 
                string parentName = transform.parent.parent.name;
                Cell currentCell = GameObject.Find($"/Grid/{parentName}").GetComponent<Cell>();
                //print(prevCell);
                //print(currentCell);
                //print(prevCell.FindNeighbours());
                //foreach (Cell item in prevCell.FindNeighbours()) { print(item); };
                print(currentCell);
                //print(gridManager.prevCell);
                generator.DestroyWallBetween(currentCell, gridManager.prevCell);
                //if (currentCell.FindNeighbours().Contains(prevCell)) {
                    
                    //print("wall deletion block");
                //}
            
            }

        }
    }

    void OnMouseExit() {
        
            string parentName = transform.parent.parent.name;
            gridManager.prevCell = GameObject.Find($"/Grid/{parentName}").GetComponent<Cell>();
            print("Previous Cell Selected -----------------------");
            //print(prevCell);
        
    }
    
}