using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    public TMPro.TMP_Dropdown GeneratorSelect;
    public TMPro.TMP_Dropdown AlgorithmSelect;
    public Button GenerateButton;
    public Button PathFindButton;
    public Button ResetButton;
    public Slider Slider;
    public Camera main;
    public Camera player;
    public bool isMainCamera = true;
    [Range(1, 50)][SerializeField] public int rows = 20;
    [Range(1, 50)][SerializeField] public int cols = 20;
    [SerializeField] public Color initGridColor = Color.green;
    [SerializeField] public Color resetGridColor = Color.blue;
    [SerializeField] public Color headColor = Color.black;
    [SerializeField] public Color traversalColor = Color.magenta;
    [SerializeField] public Color pathColor = Color.yellow;
    [SerializeField] public Color startPointColor = Color.green;
    [SerializeField] public Color endPointColor = Color.red;
    private float blockSize = 1;
    public float delay = 0.01f;
    public int generator = 0;
    public int pathfinder = 0;
    public bool isGenerated = false;
    public bool isProcessing = false;
    public List<Cell> grid = new List<Cell>();
    private Cell cellComponent;
    private List<GameObject> cellObjects = new List<GameObject>();
    public Cell startPoint;
    public Cell endPoint;
    public Cell prevCell;
    public Cell currentCell;

	private void Awake()
	{
       
        player.enabled = false;
        main.enabled = true;
    }

	void Start()
    {
        GenerateGrid();

    }

    public void SelectGenerator(int option) {
        generator = option;
    }

    public void SelectPathFinder(int option) {
        pathfinder = option;
    }

    public void SetSpeed(float speed) {
        delay = 1f - speed;
    }
	private void GenerateGrid()
	{
        GameObject referenceCell = (GameObject)Instantiate(Resources.Load("Cell"));
        GameObject referenceWall = (GameObject)Instantiate(Resources.Load("Wall"));
        for (int row = 0; row < rows; row++) {
            for (int col = 0; col < cols; col++)
            {
                GameObject cell = Instantiate(referenceCell, transform);
                cellObjects.Add(cell);
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
            if (!wall[i]) {
                var parentPosition = cell.transform.position;
                GameObject newWall = Instantiate(referenceWall, cell.transform);
                newWall.name = i.ToString();
                Wall wallComponent = newWall.GetComponent<Wall>();
                if (cellComponent.wallObjects.Count != 4)
                {
                    cellComponent.wallObjects.Add(newWall);
                }
                else
                {
                    cellComponent.wallObjects[i] = newWall;
                }
                wallComponent.wallId = i;
                wallComponent.setTransforAndAngle(parentPosition);
                cellComponent.walls[i] = true;
            }
        }
    }
    public void ResetIsVisited() {
            foreach (Cell cell in grid) {
                cell.isVisited = false;
            }
    }
    public void ResetColors()
    {

            foreach (Cell cell in grid)
            {
                cell.SetTopColor(resetGridColor);
            }
        startPoint.SetTopColor(startPointColor);
        endPoint.SetTopColor(endPointColor);
        
    }
    public void ResetGrid() {
        if (!isProcessing)
        {
            GameObject referenceWall = (GameObject)Instantiate(Resources.Load("Wall"));
            foreach (Cell cell in grid)
            {
                cellComponent = cell;
                GameObject cellGameObject = cellObjects[cell.index(cell.i, cell.j)];
                DrawWalls(referenceWall, cellGameObject);
                cell.SetTopColor(initGridColor);
                cell.isVisited = false;
            }
            Destroy(referenceWall);
        }
    }

	private void Update()
	{
        if (Input.GetKeyDown(KeyCode.Tab)) {
            isMainCamera = isMainCamera ? false : true;
            if (isMainCamera) {
                player.enabled = false;
                main.enabled = true;
                Cursor.lockState = CursorLockMode.None;
                GeneratorSelect.interactable = true;
                AlgorithmSelect.interactable = true;
                GenerateButton.interactable = true;
                PathFindButton.interactable = true;
                ResetButton.interactable = true;
                Slider.interactable = true;
            } else {
                main.enabled = false;
                player.enabled = true;
                Cursor.lockState = CursorLockMode.Locked;
                GeneratorSelect.interactable = false;
                AlgorithmSelect.interactable = false;
                GenerateButton.interactable = false;
                PathFindButton.interactable = false;
                ResetButton.interactable = false;
                Slider.interactable = false;
            }
        }
	}
}
