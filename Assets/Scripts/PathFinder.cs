using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    private GridManager gridManager;
	private List<Cell> grid;
    private int selected;
    Cell startPoint;
    Cell endPoint;
    Queue<Cell> queue = new Queue<Cell>();
    Stack<Cell> stack = new Stack<Cell>();

    public void onButtonClick() {
        StartCoroutine(PathFind());
    }

    IEnumerator PathFind() {
		gridManager = GameObject.Find("/Grid").GetComponent<GridManager>();
		grid = gridManager.grid;
		selected = gridManager.pathfinder;
        startPoint = gridManager.startPoint;
        endPoint = gridManager.endPoint;

        if (gridManager.isGenerated && !gridManager.isProcessing) {
            gridManager.isProcessing = true;
            switch (selected)
            {
                case 0:
                    yield return StartCoroutine(BFS());
                    break;
                case 1:
                    yield return StartCoroutine(DFS());
                    break;
                case 2:
                    yield return StartCoroutine(Dijkstra());
                    break;
                default:
                    break;

            }
            gridManager.isProcessing = false;
        }
	}
    IEnumerator BFS()
    {
        gridManager.ResetIsVisited();
        gridManager.ResetColors();
        queue.Clear();
        Cell start = startPoint;
        Cell current = start ;
        Cell end = endPoint;
        List<Cell> neighbours;
        List<Cell> neighboursOfEnd;

        start.SetTopColor(gridManager.startPointColor);
        end.SetTopColor(gridManager.endPointColor);

        queue.Enqueue(start);
        start.isVisited = true;

        IDictionary<int, Cell> parentTrack = new Dictionary<int, Cell>();
        neighboursOfEnd = end.FindNeighbours();
        while (current != end && !neighboursOfEnd.Contains(current))
        {
            current = queue.Dequeue();
            neighbours = current.FindNeighbours();

            foreach (Cell next in neighbours)
            {
                next.SetTopColor(gridManager.traversalColor);
                if (!next.isVisited )
                { 
                    queue.Enqueue(next);
                    next.isVisited = true;
                    int indexOfParent = grid.FindIndex(node => node == next);
                    parentTrack.Add(indexOfParent, current);
                }
            } 
            yield return new WaitForSeconds(gridManager.delay);     
        }        
        
        Cell runner = end;
        while (runner != start)
        {
            runner.SetTopColor(gridManager.pathColor);
            int indexOfRunner = grid.FindIndex(node => node == runner);
            runner = parentTrack[indexOfRunner];
            yield return new WaitForSeconds(gridManager.delay);
        }
        end.SetTopColor(gridManager.endPointColor);
        start.SetTopColor(gridManager.startPointColor);

        yield return new WaitForEndOfFrame();
    }

    IEnumerator DFS()
    {
        gridManager.ResetIsVisited();
        gridManager.ResetColors();
        stack.Clear();
        Dictionary<Cell, Cell> parentMap = new Dictionary<Cell, Cell>();
        Cell start = startPoint;
        Cell current = start;
        stack.Push(current);
        current.SetTopColor(gridManager.headColor);

        Cell end = endPoint;
        start.SetTopColor(gridManager.startPointColor);
        end.SetTopColor(gridManager.endPointColor);

        yield return StartCoroutine(StartDfsTraversing(start, end, current, parentMap));
        yield return StartCoroutine(ColorDfsPath(start, end, current, parentMap));
    }

    IEnumerator StartDfsTraversing(Cell start, Cell end, Cell current, Dictionary<Cell, Cell> parentMap) {
        List<Cell> neighbours;
        Cell temp;
        bool isRunning = true;
        while (stack.Count > 0 && isRunning)
        {
            temp = current;
            current = stack.Pop();
            temp.SetTopColor(gridManager.traversalColor);
            current.SetTopColor(gridManager.headColor);
            current.isVisited = true;
            neighbours = current.FindNeighbours();
            foreach (Cell neighbour in neighbours)
            {
                if (neighbour == end)
                {
                    current.SetTopColor(gridManager.traversalColor);
                    parentMap[neighbour] = current;
                    isRunning = false;
                    break;
                }
                else
                {
                    stack.Push(neighbour);
                    parentMap[neighbour] = current;
                }
            }
            yield return new WaitForSeconds(gridManager.delay);
        }


        
    }

    IEnumerator ColorDfsPath(Cell start,Cell end, Cell current, Dictionary<Cell,Cell> parentMap) {
        end.SetTopColor(gridManager.endPointColor);
        current = parentMap[end];
        while (current != start)
        {
            current.SetTopColor(gridManager.pathColor);
            current = parentMap[current];
            yield return new WaitForSeconds(gridManager.delay);
        }
        start.SetTopColor(gridManager.startPointColor);
    }

    IEnumerator Dijkstra()
    {
        gridManager.ResetIsVisited();
        gridManager.ResetColors();
        Cell start = startPoint;
        Cell end = endPoint;
        Cell current;
        List<Cell> neighbours;
        Dictionary<Cell, int> totalCosts = new Dictionary<Cell, int>();
        Dictionary<Cell, Cell> preCell = new Dictionary<Cell, Cell>();
        List<(Cell cell, int priority)> priorityQueue = new List<(Cell, int)>();
        start.SetTopColor(gridManager.startPointColor);
        end.SetTopColor(gridManager.endPointColor);

        totalCosts[start] = 0;
        priorityQueue.Add((start, 1));

        foreach (Cell cell in grid) {
            if (cell != start) {
                totalCosts[cell] = int.MinValue;
            }
        }

        while (priorityQueue.Count > 0) {
            current = priorityQueue[0].cell;
            priorityQueue.RemoveAt(0);
			if (current == end)
			{
				break;
			}
			current.isVisited = true;
            current.SetTopColor(gridManager.traversalColor);
            neighbours = current.FindNeighbours();
            foreach (Cell neighbour in neighbours) {
                if (!neighbour.isVisited) {
                    int altPath = totalCosts[current] + neighbour.distanceFromNeighbour;
                    if (altPath > totalCosts[neighbour]) {
                        totalCosts[neighbour] = altPath;
                        preCell[neighbour] = current;
                        ChangePriority(ref priorityQueue, neighbour , altPath);
                    }
                }
            }
            yield return new WaitForSeconds(gridManager.delay);
        }
        current = end;
        current.SetTopColor(gridManager.endPointColor);
        while (current != start) {
            current = preCell[current];
            current.SetTopColor(gridManager.pathColor);
            yield return new WaitForSeconds(gridManager.delay);
        }
        start.SetTopColor(gridManager.startPointColor);

        yield return new WaitForEndOfFrame();
    }

    public void ChangePriority(ref List<(Cell cell, int priority)> pq, Cell element, int newPriority) {
        bool found = false;
        for (int i = 0; i < pq.Count; i++) {
            if (pq[i].cell == element)
            {
                pq[i] = (pq[0].cell,newPriority);
                found = true;
                break;
            }
        }
        if (!found)
        {
            pq.Add((element , newPriority));
        }
        pq.Sort(ComparePriorityQueue);
    }

    public int ComparePriorityQueue((Cell cell , int priority) element1, (Cell cell, int priority) element2) {
        if (element1.priority > element2.priority)
        {
            return -1;
        }
        else if (element1.priority < element2.priority)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
}
