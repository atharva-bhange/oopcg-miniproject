using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Range(1, 50)][SerializeField] int rows = 20;
    [Range(1, 50)][SerializeField] int cols = 20;
    [SerializeField] float blockSize = 1;
    private Cell cellComponent;
    void Start()
    {
        GenerateGrid();
    }

	private void GenerateGrid()
	{
        GameObject referenceCell = (GameObject)Instantiate(Resources.Load("Cell"));
        GameObject referenceWall = (GameObject)Instantiate(Resources.Load("Wall"));
        for (int row = 0; row < rows; row++) {
            for (int col = 0; col < cols; col++)
            {
                GameObject cell = Instantiate(referenceCell, transform);
                cellComponent = cell.GetComponent<Cell>();
                float zPos = col * blockSize;
                float xPos = row * blockSize;
                cell.transform.position = new Vector3(xPos , 0f , zPos);
                
                DrawWalls(referenceWall, cell);
            }
        }
        Destroy(referenceCell);
        Destroy(referenceWall);
	}

	private void DrawWalls(GameObject referenceWall, GameObject cell)
	{
        bool [] wall = cellComponent.walls;
        if (wall[0]) {
            var parentPosition = cell.transform.position;
            GameObject newWall = Instantiate(referenceWall, cell.transform);
            newWall.transform.position = new Vector3(parentPosition.x ,parentPosition.y + 1.5f , parentPosition.z + 0.5f);
            newWall.transform.Rotate(0 ,90,0);
        }

        if (wall[1]) {
            var parentPosition = cell.transform.position;
            GameObject newWall = Instantiate(referenceWall, cell.transform);
            newWall.transform.position = new Vector3(parentPosition.x+0.5f, parentPosition.y + 1.5f, parentPosition.z);
            newWall.transform.Rotate(0, 180, 0);
        }

        if (wall[2])
        {
            var parentPosition = cell.transform.position;
            GameObject newWall = Instantiate(referenceWall, cell.transform);
            newWall.transform.position = new Vector3(parentPosition.x, parentPosition.y + 1.5f, parentPosition.z - 0.5f);
            newWall.transform.Rotate(0, 270, 0);
        }
        if (wall[3])
        {
            var parentPosition = cell.transform.position;
            GameObject newWall = Instantiate(referenceWall, cell.transform);
            newWall.transform.position = new Vector3(parentPosition.x-0.5f, parentPosition.y + 1.5f, parentPosition.z);
            newWall.transform.Rotate(0, 180, 0);
        }
    }
}
