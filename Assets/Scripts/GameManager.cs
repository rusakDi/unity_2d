using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class GameManager : MonoBehaviour { 
	public static GameManager instance = null;
	public BoardManager boardScript;
	public int playerFoodPoints = 100;
	[HideInInspector] public bool playersTurn = true;
	public float turnDelay = .1f;
	public float levelStartDelay = 3f;
	private int level = 0;
	private List<Enemy> enemies;
	private Text levelText;
	public float restartDelay = 1f;
	private Text textToCut;
	private Text[] cutText;
	private GameObject levelImage;
	private bool enemiesMoving;
	private bool doingSetup;
	
	// public event Action OnResetGame;

	void Awake() {
        if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);
		}

        DontDestroyOnLoad(gameObject);
		enemies = new List<Enemy>();
		boardScript = GetComponent<BoardManager>();
	}

	public void InitGame() {
		doingSetup = true;
		levelImage = GameObject.Find("LevelImage");
		levelText = GameObject.Find("LevelText").GetComponent<Text>();
		if (level < 26) {
			textToCut = boardScript.cutText[level-1].GetComponent<Text>();
			levelText.text =  textToCut.text;
		} else {
			textToCut = boardScript.cutText[Random.Range(0, boardScript.cutText.Length)].GetComponent<Text>();
			levelText.text =  textToCut.text;
		}
		levelImage.SetActive(true);
		Invoke("HideLevelImage", levelStartDelay);
        enemies.Clear();
		boardScript.SetupScene(level);
	}

	private void HideLevelImage() {
		levelImage.SetActive(false);
		doingSetup = false;
	}

	public void GameOver() {
		levelText.text = "К сожалению, вы проиграли...  как это верно отметил \n один из героев чеховского «рассказа»: люди, воспитанные на чувствах,  \n часто вспоминают о своей прежней жизни,  \n лишь когда это  им становится выгодно.";
		levelImage.SetActive(true);
		enabled = false;
		Invoke("Restart()", restartDelay);
	}

	// void Restart()
    // {
    //     SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    // }

	void Update() {
        if (playersTurn || enemiesMoving || doingSetup) {
            return;
        }
		StartCoroutine(MoveEnemies());
	}

	void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
		level++;
		InitGame();
	}

	// public void OnRestart() {
	// 	SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	// }

	void OnEnable() {
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}

	void OnDisable() {
		SceneManager.sceneLoaded -= OnLevelFinishedLoading;
	}

    IEnumerator MoveEnemies() {
		enemiesMoving = true;
		yield return new WaitForSeconds(turnDelay);
		if (enemies.Count == 0) {
			yield return new WaitForSeconds(turnDelay);
		}
		for (int i = 0; i < enemies.Count; i++) {
			enemies[i].MoveEnemy();
			yield return new WaitForSeconds(enemies[i].moveTime);
		}
		enemiesMoving = false;
		playersTurn = true;
	}

    public void AddEnemyToList(Enemy script) {
		enemies.Add(script);
	}
}
