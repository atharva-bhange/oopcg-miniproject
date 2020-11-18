using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool [] walls = {true, true, true, true}; // N , E , S , W
	public List<GameObject> wallObjects = new List<GameObject>();
	public int i;
	public int j;
    public bool isVisited;
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


	public Cell CheckNeighbour()
	{
		List<Cell> neighbours = new List<Cell>();
		try
		{
			Cell top = grid[index(i, j - 1)];
			if (!top.isVisited) {
				neighbours.Add(top);
			}
		}
		catch
		{
		}
		try
		{
			Cell right = grid[index(i + 1, j)];
			if (!right.isVisited)
			{
				neighbours.Add(right);
			}
		}
		catch
		{
		}
		try
		{
			Cell bottom = grid[index(i, j + 1)];
			if (!bottom.isVisited)
			{
				neighbours.Add(bottom);
			}
		}
		catch 
		{ 
		}
		try
		{
			Cell left = grid[index(i - 1, j)];
			if (!left.isVisited)
			{
				neighbours.Add(left);
			}
		}
		catch
		{
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

	private int index(int newi, int newj)
	{
		if (newi < 0 || newj < 0 || newi > rows-1 || newj > cols-1) {
			return -1;
		}
		return (newi*rows) + newj ;
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
}
