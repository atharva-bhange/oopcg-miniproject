using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Range(1, 50)][SerializeField] public int rows = 20;
    [Range(1, 50)][SerializeField] public int cols = 20;
    [SerializeField] float blockSize = 1;
    private Cell cellComponent;
    public List<Cell> grid = new List<Cell>();
    public Cell current;
    Stack<Cell> stack = new Stack<Cell>();

    void Start()
    {
        GenerateGrid();
        GenerateMaze();
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
                float zPos = row * blockSize;
                float xPos = col * blockSize;
                cell.transform.position = new Vector3(xPos , 0f , zPos);
                cell.name = $"({row}, {col})";
                cellComponent.i = row;
                cellComponent.j = col;
                grid.Add(cellComponent);
                DrawWalls(referenceWall, cell);
            }
        }
        Destroy(referenceCell);
        Destroy(referenceWall);
	}

	private void DrawWalls(GameObject referenceWall, GameObject cell)
	{
        bool [] wall = cellComponent.walls;
        for (int i = 0; i < wall.Length; i++) {
            var parentPosition = cell.transform.position;
            GameObject newWall = Instantiate(referenceWall, cell.transform);  
            newWall.name = i.ToString();
            Wall wallComponent = newWall.GetComponent<Wall>();
            cellComponent.wallObjects.Add(newWall);
            wallComponent.wallId = i;
            wallComponent.setTransforAndAngle(parentPosition);
        }
    }

    private void GenerateMaze()
    {
        current = grid[0];
        current.isVisited = true;
        stack.Push(current);
        current.SetTopColor(Color.red);
 
        current.SetTopColor(Color.blue);
        StartCoroutine(FindNextCell());
	}

    IEnumerator FindNextCell()
	{
        while (true)
        {
            Cell next = current.CheckNeighbour();
            if (next != null)
            {
                next.isVisited = true;
                FindWallId(current, next);
                Cell temp = current;
                current = next;
                stack.Push(current);
                current.SetTopColor(Color.red);
                temp.SetTopColor(Color.blue);
            }
            else
            {
                current.SetTopColor(Color.blue);
                if (stack.Count > 0)
                {
                    current = stack.Pop();
                    current.SetTopColor(Color.red);
                }
                else
                {
                    break;
                }
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

	private void FindWallId(Cell current, Cell next)
	{
        int xdif = current.i - next.i;
        int ydif = current.j - next.j;
        if (xdif == 0 && ydif == -1)
        {
            current.DestroyWall(1);
            next.DestroyWall(3);
        }
        else if (xdif == 0 && ydif == 1)
        {
            current.DestroyWall(3);
            next.DestroyWall(1);
        }
        else if (ydif == 0 && xdif == -1)
        {
            current.DestroyWall(0);
            next.DestroyWall(2);
        }
        else {
            current.DestroyWall(2);
            next.DestroyWall(0);
        }
	}
}
