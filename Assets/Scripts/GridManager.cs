using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Range(1, 50)][SerializeField] public int rows = 20;
    [Range(1, 50)][SerializeField] public int cols = 20;
    [SerializeField] float blockSize = 1;
    [Range(0, 1)][SerializeField] float delay = 0.01f;
    private Cell cellComponent;
    public List<Cell> grid = new List<Cell>();
    public Cell current;
    Stack<Cell> stack = new Stack<Cell>();
    Dictionary<Cell, Cell> parentMap = new Dictionary<Cell, Cell>();

    IEnumerator Start()
    {
        GenerateGrid();
        yield return GenerateMaze();
        yield return dfs();
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

    IEnumerator GenerateMaze()
    {
        current = grid[0];
        current.isVisited = true;
        stack.Push(current);
        current.SetTopColor(Color.red);
 
        current.SetTopColor(Color.blue);
        yield return StartCoroutine(MineMaze());
	}

    IEnumerator MineMaze()
	{
        while (true)
        {
            Cell next = current.FindRandomNeighbour();
            if (next != null)
            {
                next.isVisited = true;
                DestroyWallBetween(current, next);
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
            yield return new WaitForSeconds(delay);
        }
    }

	private void DestroyWallBetween(Cell current, Cell next)
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

    IEnumerator dfs() {
        ResetIsVisited();
        stack.Clear();
        List<Cell> neighbours;

        Cell temp;
        Cell start = grid[0];
        Cell current = start;
        stack.Push(current);
        current.SetTopColor(Color.black);

        Cell end = grid[(cols * rows) -1];
        start.SetTopColor(Color.red);
        end.SetTopColor(Color.green);
		while (stack.Count > 0)
		{
            temp = current;
			current = stack.Pop();
            temp.SetTopColor(Color.magenta);
            current.SetTopColor(Color.black);
			current.isVisited = true;
            neighbours = current.FindNeighbours();
            foreach (Cell neighbour in neighbours) {
                if (neighbour == end)
                {
                    parentMap[neighbour] = current;
                    break;
                }
                else
                {
                    stack.Push(neighbour);
                    parentMap[neighbour] = current;
                }
            }
            yield return new WaitForSeconds(delay);
		}
        end.SetTopColor(Color.red);
        current = parentMap[end];
        while (current != start) {
            current.SetTopColor(Color.yellow);
            current = parentMap[current];
            yield return new WaitForSeconds(delay);
        }
        start.SetTopColor(Color.green);
		yield return new WaitForEndOfFrame();
    }

    public void ResetIsVisited() {
        foreach (Cell cell in grid) {
            cell.isVisited = false;
        }

    }
}
