using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    Stack<Cell> stack = new Stack<Cell>();
    private GridManager gridManager;
    private List<Cell> grid;
    private int selected;
    private int rows;
    private int cols;

    public void onButtonClick() {
        StartCoroutine(GenerateMaze());
    }

    IEnumerator GenerateMaze()
    {
        gridManager = GameObject.Find("/Grid").GetComponent<GridManager>();
        grid = gridManager.grid;
        rows = gridManager.rows;
        cols = gridManager.cols;
        selected = gridManager.generator;
        gridManager.ResetGrid();
        if (!gridManager.isProcessing)
        {
            gridManager.isProcessing = true;
            gridManager.isGenerated = false;
            switch (selected)
            {
                case 0:
                    yield return StartCoroutine(DFS_Backtracking());
                    break;
                case 1:
                    yield return StartCoroutine(Double_DFS_Backtracking());
                    break;
                default:
                    break;
            }
            gridManager.isGenerated = true;
            gridManager.isProcessing = false;
            // Setting Default Start and EndPoint
            gridManager.startPoint = grid[0];
            gridManager.endPoint = grid[(rows * cols) - 1];
            gridManager.startPoint.SetTopColor(gridManager.startPointColor);
            gridManager.endPoint.SetTopColor(gridManager.endPointColor);
        }
    }
    IEnumerator Double_DFS_Backtracking()
	{
        yield return StartCoroutine(DFS_Backtracking());
        yield return StartCoroutine(DFS_Backtracking());
	}
	IEnumerator DFS_Backtracking()
	{
        gridManager.ResetIsVisited();
		Cell current = grid[UnityEngine.Random.Range(0, rows*cols -1)];
		current.isVisited = true;
		stack.Push(current);
		current.SetTopColor(gridManager.resetGridColor);

		yield return StartCoroutine(MineMaze(current));
	}
	IEnumerator MineMaze(Cell current)
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
                current.SetTopColor(gridManager.headColor);
                temp.SetTopColor(gridManager.resetGridColor);
            }
            else
            {
                current.SetTopColor(gridManager.resetGridColor);
                if (stack.Count > 0)
                {
                    current = stack.Pop();
                    current.SetTopColor(gridManager.headColor);
                }
                else
                {
                    break;
                }
            }
            yield return new WaitForSeconds(gridManager.delay);
        }
    }
    public void DestroyWallBetween(Cell current, Cell next)
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
        else
        {
            current.DestroyWall(2);
            next.DestroyWall(0);
        }
    }
}
