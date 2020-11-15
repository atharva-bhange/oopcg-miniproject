using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] int rows = 10;
    [SerializeField] int cols = 10;
    [SerializeField] float blockSize = 1;
    void Start()
    {
        GenerateGrid();
    }

	private void GenerateGrid()
	{
        GameObject referenceBlock = (GameObject)Instantiate(Resources.Load("Block"));
        for (int row = 0; row < rows; row++) {
            for (int col = 0; col < cols; col++)
            {
                GameObject block = (GameObject)Instantiate(referenceBlock, transform);
                float zPos = col * blockSize;
                float xPos = row * blockSize;
                block.transform.position = new Vector3(xPos , 0f , zPos);
            }
        }
        Destroy(referenceBlock);
	}
}
