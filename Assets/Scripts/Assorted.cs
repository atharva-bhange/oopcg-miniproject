using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assorted : MonoBehaviour
{
	private GridManager gridManager;
	public void OnResetClicked() {
		gridManager = GameObject.Find("/Grid").GetComponent<GridManager>();
		if (!gridManager.isProcessing && gridManager.isGenerated) {
			gridManager.ResetIsVisited();
			gridManager.ResetColors();
		}
	}

	public void OnQuitClicked() {
		Application.Quit();
	}

}
