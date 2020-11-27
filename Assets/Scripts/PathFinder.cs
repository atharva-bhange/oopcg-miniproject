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


        start.SetTopColor(Color.red);
        end.SetTopColor(Color.green);

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
/*        start.SetTopColor(Color.green);
        end.SetTopColor(Color.red);*/
        Cell runner = end;
        while (runner != start)
        {
            runner.SetTopColor(Color.yellow);
            int indexOfRunner = grid.FindIndex(node => node == runner);
            runner = parentTrack[indexOfRunner];
            yield return new WaitForSeconds(gridManager.delay);
        }


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
        start.SetTopColor(Color.red);
        end.SetTopColor(Color.green);

        yield return StartCoroutine(StartDfsTraversing(start, end, current, parentMap));
        yield return StartCoroutine(ColorDfsPath(start, end, current, parentMap));
    }

    IEnumerator StartDfsTraversing(Cell start, Cell end, Cell current, Dictionary<Cell, Cell> parentMap) {
        List<Cell> neighbours;
        Cell temp;
        while (stack.Count > 0)
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
                    parentMap[neighbour] = current;
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

}
