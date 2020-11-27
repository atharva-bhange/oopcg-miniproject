using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{


    private GridManager gridManager;
	private List<Cell> grid;
    private int rows;
    private int cols;
    private int selected;
    //private bool breakFlag = false;
    Queue<Cell> queue = new Queue<Cell>();
    Stack<Cell> stack = new Stack<Cell>();

    public void onButtonClick() {
        StartCoroutine(PathFind());
    }

    IEnumerator PathFind() {
		gridManager = GameObject.Find("/Grid").GetComponent<GridManager>();
		grid = gridManager.grid;
		selected = gridManager.pathfinder;
        rows = gridManager.rows;
        cols = gridManager.cols;
        Cell startPoint = gridManager.startPoint;
        Cell endPoint = gridManager.endPoint;

        if (gridManager.isGenerated && !gridManager.isProcessing) {
            gridManager.isProcessing = true;
            switch (selected)
            {
                case 0:
                    yield return StartCoroutine(BFS(startPoint, endPoint));
                    break;
                case 1:
                    yield return StartCoroutine(DFS(startPoint, endPoint));
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
    IEnumerator BFS(Cell startPoint,Cell endPoint)
    {
        gridManager.ResetIsVisited();
        gridManager.ResetColors();
        queue.Clear();

        //grid = gridManager.grid;
        // bool breakFlag = false;

        //Cell start = grid[0];
        Cell start = startPoint;
        Cell current = start ;
        Cell end = endPoint;
        //Cell end = grid[(rows * cols) - 1];
        List<Cell> neighbours;


        start.SetTopColor(Color.green);
        end.SetTopColor(Color.red);

        queue.Enqueue(start);
        start.isVisited = true;
        

        IDictionary<int, Cell> parentTrack = new Dictionary<int, Cell>();

        while (current != end )
        {
            current = queue.Dequeue();
            neighbours = current.FindNeighbours();

            foreach (Cell next in neighbours)
            {
                //if (current == end)
                //{
                //    breakFlag = true;
                //    break;
                //}
                if (!next.isVisited)
                {
                    queue.Enqueue(next);
                    next.isVisited = true;
                    ///if (next != end)
                    ///{
                       // next.SetTopColor(Color.green);
                       // print("grenns");
                    //}

                    int indexOfParent = grid.FindIndex(node => node == next);
                    parentTrack.Add(indexOfParent, current);
                }
                //if (breakFlag) {
                 //   break;
                //}

                next.SetTopColor(Color.magenta);
            }
            
            yield return new WaitForSeconds(gridManager.delay);
            
        }

        // reverse traversal through parentTrack to find the path
        // List<Cell> path;  // can be used to store the whole path directly
        /*        start.SetTopColor(Color.green);*/
        
        Cell runner = end;
        while (runner != start)
        {
            runner.SetTopColor(Color.yellow);
            int indexOfRunner = grid.FindIndex(node => node == runner);
            runner = parentTrack[indexOfRunner];
            yield return new WaitForSeconds(gridManager.delay);
        }
        end.SetTopColor(Color.red);
        start.SetTopColor(Color.green);

        yield return new WaitForEndOfFrame();
    }


    IEnumerator DFS(Cell startPoint, Cell endPoint)
    {
        gridManager.ResetIsVisited();
        gridManager.ResetColors();
        stack.Clear();
        Dictionary<Cell, Cell> parentMap = new Dictionary<Cell, Cell>();
        //Cell start = grid[0];
        Cell start = startPoint;
        Cell current = start;
        stack.Push(current);
        current.SetTopColor(Color.black);

        //Cell end = grid[(cols * rows) - 1];
        Cell end = endPoint;
        start.SetTopColor(Color.green);
        end.SetTopColor(Color.red);

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
            temp.SetTopColor(Color.magenta);
            current.SetTopColor(Color.black);
            current.isVisited = true;
            neighbours = current.FindNeighbours();
            foreach (Cell neighbour in neighbours)
            {
                if (neighbour == end)
                {
                    current.SetTopColor(Color.magenta);
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
        end.SetTopColor(Color.red);
        current = parentMap[end];
        while (current != start)
        {
            current.SetTopColor(Color.yellow);
            current = parentMap[current];
            yield return new WaitForSeconds(gridManager.delay);
        }
        start.SetTopColor(Color.green);
    }

    IEnumerator Dijkstra()
    {
        gridManager.ResetIsVisited();
        gridManager.ResetColors();
        Cell start = grid[0];
        Cell end = grid[(rows * cols) - 1];
        Cell current;
        List<Cell> neighbours;
        Dictionary<Cell, int> totalCosts = new Dictionary<Cell, int>();
        Dictionary<Cell, Cell> preCell = new Dictionary<Cell, Cell>();
        List<(Cell cell, int priority)> priorityQueue = new List<(Cell, int)>();

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
            current.SetTopColor(Color.magenta);
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
        current.SetTopColor(Color.red);
        while (current != start) {
            current = preCell[current];
            current.SetTopColor(Color.yellow);
            yield return new WaitForSeconds(gridManager.delay);
        }
        start.SetTopColor(Color.green);

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
