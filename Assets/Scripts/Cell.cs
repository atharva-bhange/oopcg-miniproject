using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool [] walls = {false, false, false, false}; // N , E , S , W
	public List<GameObject> wallObjects = new List<GameObject>();
	public int i;
	public int j;
    public bool isVisited;
	public int distanceFromNeighbour = 1;
	private GridManager gridManager;
	private List<Cell> grid;
	private int rows;
	private int cols;
	public void Awake()
	{
		gridManager = GameObject.Find("/Grid").GetComponent<GridManager>();
		grid = gridManager.grid;
		rows = gridManager.rows;
		cols = gridManager.cols;
	}
	public List<Cell> FindNeighbours()
	{
		List<Cell> neighbours = new List<Cell>();

		for (int wallId = 0; wallId < walls.Length; wallId++) {
			if (!walls[wallId]) {
				Cell neighbour = GetNeighbourForWall(wallId);
				if (neighbour == null) continue;
				if (!neighbour.isVisited) neighbours.Add(neighbour);
			}
		}

		return neighbours;
	}


	public Cell FindRandomNeighbour() {
		List<Cell> neighbours = new List<Cell>();
		for (int wallId = 0; wallId < 4; wallId++) {
			Cell neighbour = GetNeighbourForWall(wallId);
			if (neighbour == null) continue;
			if (!neighbour.isVisited) neighbours.Add(neighbour);
		}

		if (neighbours.Count > 0)
		{
			return neighbours[UnityEngine.Random.Range(0, neighbours.Count)];
		}
		else
		{
			return null;
		}
	}

	public int index(int newi, int newj)
	{
		if (newi < 0 || newj < 0 || newi > rows-1 || newj > cols-1) {
			return -1;
		}
		return (newi * cols) + newj;
	}

	public void SetTopColor(Color color)
	{
		
		MeshRenderer topMeshRenderer = transform.Find("Block").Find("Top").GetComponent<MeshRenderer>();
		topMeshRenderer.material.color = color;
	}

	public void DestroyWall(int wallId)
	{
		Destroy(wallObjects[wallId]);
		walls[wallId] = false;
	}

	public Cell GetNeighbourForWall(int wallId) {
		int idx;
		switch (wallId)
		{
			case 0:
				idx = index(i + 1, j);
				if (idx == -1) return null;
				return grid[idx];
			case 1:
				idx = index(i, j+1);
				if (idx == -1) return null;
				return grid[idx];
			case 2:
				idx = index(i - 1, j);
				if (idx == -1) return null;
				return grid[idx];
			case 3:
				idx = index(i, j-1);
				if (idx == -1) return null;
				return grid[idx];
			default:
				return null;
		}
	}
}
