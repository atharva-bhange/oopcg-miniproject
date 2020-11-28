﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetLegendColor : MonoBehaviour
{
    private GridManager gridManager;
    private string name;
    private Image image;
    void Start()
    {
        gridManager = GameObject.Find("/Grid").GetComponent<GridManager>();
        name = transform.name;
        image = transform.GetComponent<Image>();
    }
    
    void Update()
    {
        switch (name) {
            case "start":
                image.color = gridManager.startPointColor;
                break;
            case "end":
                image.color = gridManager.endPointColor;
                break;
            case "path":
                image.color = gridManager.pathColor;
                break;
            default:
                break;
        }
        
    }
}
