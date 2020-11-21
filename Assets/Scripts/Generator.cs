using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    Stack<Cell> stack = new Stack<Cell>();
    private GridManager gridManager;
    private List<Cell> grid;
    private int selected;

    public void onButtonClick() {
        StartCoroutine(GenerateMaze());
    }

    IEnumerator GenerateMaze()
	{
		gridManager = GameObject.Find("/Grid").GetComponent<GridManager>();
		grid = gridManager.grid;
        selected = gridManager.generator;
        gridManager.ResetGrid();
        if (!gridManager.isProcessing) {
            gridManager.isProcessing = true;
            
            switch (selected)
            {
                case 0:
                    yield return StartCoroutine(DFS_Backtracking());
                    break;
                default:
                    break;
            }
            gridManager.isGenerated = true;
            gridManager.isProcessing = false;
        }
    }

    IEnumerator DFS_Backtracking()
	{
		Cell current = grid[0];
		current.isVisited = true;
		stack.Push(current);
		current.SetTopColor(Color.red);

		current.SetTopColor(Color.blue);
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
            yield return new WaitForSeconds(gridManager.delay);
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
        else
        {
            current.DestroyWall(2);
            next.DestroyWall(0);
        }
    }
}
