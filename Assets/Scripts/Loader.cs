using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour {
	public GameObject gameManager;

	void Awake() {
		// void GameRestart() {
		// 	Debug.Log("Restart");
		// 	GameManager.instance = null;
		// 	Instantiate(gameManager);
		// 	GameManager.instance.OnResetGame += GameRestart;
		// };

		if (GameManager.instance == null) {
			Debug.Log("First");
			Instantiate(gameManager);
			// GameManager.instance.OnResetGame += GameRestart;
			// Debug.Log("Event setted");
		}
	}
}
