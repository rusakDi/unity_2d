using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {
	[Serializable]
	public class Count {
		public int minimum;
		public int maximum;

		public Count(int min, int max) {
			minimum = min;
			maximum = max;
		}
	}

	public int columns = 100;
	public int rows = 100;
	public Count foodCount = new Count(1, 5);
	public Count wallCount = new Count(2, 7);
	public GameObject exit;
	public GameObject puddle;
	public GameObject[] floorTiles;
	public GameObject[] foodTiles;
	public GameObject[] outerWallTiles;
	public GameObject[] enemyTiles;
	public GameObject[] wallTiles;
	public GameObject[] cutText;

	private Transform boardHolder;
	private List<Vector3> gridPositions = new List<Vector3>();

	void InitializeList() {
		gridPositions.Clear();

		for (int x = 1; x < columns - 1; x++) {
			for (int y = 1; y < rows - 1; y++) {
				gridPositions.Add(new Vector3(x, y, 0f));
			}
		}
	}

	void BoardSetup() {
		boardHolder = new GameObject("Board").transform;
		for (int x = -1; x < columns + 1; x++) {
			for (int y = -1; y < rows + 1; y++) {
				if ((x == -1 && y == -1) || (x == columns && y == -1) || (x == -1 && y == rows) || (x == columns && y == rows)) {
					continue;
				}
				GameObject toInstantiate;
				if (x == -1) {
					toInstantiate = outerWallTiles[3];
				}
				else if (x == columns) {
					toInstantiate = outerWallTiles[2];
				}
				else if (y == -1) {
					toInstantiate = outerWallTiles[1];
				}
				else if (y == rows) {
					toInstantiate = outerWallTiles[0];
				}
				else {
					toInstantiate = floorTiles[0];
				}
				GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
				instance.transform.SetParent(boardHolder);
			}
		}
	}

	Vector3 RandomPosition() {
		int randomIndex = Random.Range(0, gridPositions.Count);
		Vector3 randomPosition = gridPositions[randomIndex];
		gridPositions.RemoveAt(randomIndex);
		return randomPosition;
	}

	void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum) {
		int objectCount = Random.Range(minimum, maximum + 1);
		for (int i = 0; i < objectCount; i++) {
			Vector3 randomPosition = RandomPosition();
			GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
			Instantiate(tileChoice, randomPosition, Quaternion.identity);
		}
	}

	public void SetupScene(int level) {
		BoardSetup();
		InitializeList();
		LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
		LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);
		int enemyCount = (int)Mathf.Log(level*12, 2f)*5;
		LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
		int exitX = Random.Range(5, columns - 2);
		int exitY = Random.Range(5, rows - 2);
		Instantiate(exit, new Vector3(exitX, exitY, 0f), Quaternion.identity);
		int countPuddle = Random.Range(5, (int) rows/8);
		for (int i = 0; i < countPuddle; i++){
			Instantiate(puddle, new Vector3(Random.Range(5, columns - 2), Random.Range(5, rows - 2), 0f), Quaternion.identity);
		}
	}
}
