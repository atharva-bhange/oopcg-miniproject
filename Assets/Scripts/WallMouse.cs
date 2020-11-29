using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMouse : MonoBehaviour
{
    GridManager gridManager;
    Generator generator;

    void Start() { 
        gridManager = GameObject.Find("/Grid").GetComponent<GridManager>();
        generator = GameObject.Find("/Canvas/Generate Maze/").GetComponent<Generator>();

    }
    void OnMouseOver()
        {
        if (gridManager.isGenerated && !gridManager.isProcessing)
            {
            if (Input.GetKeyDown(KeyCode.Mouse0) && Input.GetKey(KeyCode.LeftAlt))
                {
                    Cell neighbour;
                    char wallId = transform.name[0];
                    string parentCellName = transform.parent.name;
                    Cell currentCell = GameObject.Find($"/Grid/{parentCellName}").GetComponent<Cell>();
                    if (wallId == '0')
                    {
                        int i = currentCell.i + 1;
                        try
                        {
                            neighbour = GameObject.Find($"/Grid/({i}, {currentCell.j})").GetComponent<Cell>();
                        }
                        catch
                        {
                            neighbour = null;
                        }
                        if (neighbour)
                        {
                            generator.DestroyWallBetween(currentCell, neighbour);
                        }
                    }
                    else if (wallId == '1')
                    {
                        int j = currentCell.j + 1;
                        try
                        {
                            neighbour = GameObject.Find($"/Grid/({currentCell.i}, {j})").GetComponent<Cell>();
                        }
                        catch
                        {
                            neighbour = null;
                        }
                        if (neighbour)
                        {
                            generator.DestroyWallBetween(currentCell, neighbour);
                        }
                    }
                    else if (wallId == '2')
                    {
                        int i = currentCell.i - 1;
                        try
                        {
                            neighbour = GameObject.Find($"/Grid/({i}, {currentCell.j})").GetComponent<Cell>();
                        }
                        catch
                        {
                            neighbour = null;
                        }
                        if (neighbour)
                        {
                            generator.DestroyWallBetween(currentCell, neighbour);
                        }   
                    }
                    else if (wallId == '3')
                    {
                        int j = currentCell.j - 1;
                        try
                        {
                            neighbour = GameObject.Find($"/Grid/({currentCell.i}, {j})").GetComponent<Cell>();
                        }
                        catch
                        {
                            neighbour = null;
                        }
                        if (neighbour)
                        {
                            generator.DestroyWallBetween(currentCell, neighbour);
                        }
                    }
                }
            }
        
    }
}
    

