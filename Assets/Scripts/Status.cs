using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Status : MonoBehaviour
{
    private TMPro.TextMeshProUGUI textbox;
    private GridManager gridManager;
    int count = 0;
    void Start()
    {
        textbox = transform.GetComponent<TMPro.TextMeshProUGUI>();
        gridManager = GameObject.Find("/Grid").GetComponent<GridManager>();
    }

    void Update()
    {
        if (!gridManager.isGenerated && !gridManager.isProcessing) {
            textbox.text = "Click Generate!";
        } else if (!gridManager.isGenerated && gridManager.isProcessing) {
            if (count > 80) {
                count = 0;
            }
            textbox.text = "Generating " + string.Concat(Enumerable.Repeat(".", count / 20));
            count++;
        } else if (gridManager.isGenerated && !gridManager.isProcessing) {
            textbox.text = "Maze is ready!";
        } else if (gridManager.isGenerated && gridManager.isProcessing) {
            if (count > 80)
            {
                count = 0;
            }
            textbox.text = "Finding Path " + string.Concat(Enumerable.Repeat(".", count / 20));
            count++;
        }
        
    }
}
