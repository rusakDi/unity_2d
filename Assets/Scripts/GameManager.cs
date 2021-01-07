using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour { 
	public static GameManager instance = null;
	public BoardManager boardScript;

    [SerializeField]
	[HideInInspector]
	private bool playersTurn = true;
    
	public float turnDelay = .1f;
	public float levelStartDelay = 5f;
	private int level = 0;
	private List<Enemy> enemies;
	private Text levelText;
	public float restartDelay = 1f;
	private Text textToCut;
	private Text[] cutText;
	private GameObject levelImage;
	private bool enemiesMoving;
	private bool doingSetup;
	private Player player;

    public int Level { get => level; set => level = value; }
    public bool PlayersTurn { 
		get => playersTurn;
		set => playersTurn = value;
	}

    void Awake() {
		player = GameObject.Find("Player").GetComponent<Player>();
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
		if (Level < 26) {
			textToCut = boardScript.cutText[Level-1].GetComponent<Text>();
			levelText.text =  textToCut.text;
		} else {
			textToCut = boardScript.cutText[Random.Range(0, boardScript.cutText.Length)].GetComponent<Text>();
			levelText.text =  textToCut.text;
		}
		levelImage.SetActive(true);
		var corutin = HideLevelImage();
		StartCoroutine(corutin);
		enemies.Clear();
		boardScript.SetupScene(Level);
	}

	public IEnumerator HideLevelImage() {
		yield return new WaitForSeconds(levelStartDelay);
		levelImage.SetActive(false);
		doingSetup = false;
		yield return null;
	}

	public void GameOver() {
		levelText.text = "К сожалению, вы проиграли...  как это верно отметил \n один из героев чеховского «рассказа»: люди, воспитанные на чувствах,  \n часто вспоминают о своей прежней жизни,  \n лишь когда это  им становится выгодно.";
		levelImage.SetActive(true);
	}

	public void RestartLevel() {
		level = 0;
		PlayersTurn = true;
		SceneManager.LoadScene("Main");
		var corutin = HideLevelImage();
		StartCoroutine(corutin);
	}

	void Update() {
        if (doingSetup) {
            return;
        }
		MoveEnemies(Time.deltaTime);
	}

	void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
		Level++;
		InitGame();
	}

	void OnEnable() {
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}

	void OnDisable() {
		SceneManager.sceneLoaded -= OnLevelFinishedLoading;
	}

    void MoveEnemies(float deltaTime) {
		enemiesMoving = true;
		for (int i = 0; i < enemies.Count; i++) {
			enemies[i].MoveEnemy(deltaTime);
		}
		enemiesMoving = false;
		PlayersTurn = true;
	}

    public void AddEnemyToList(Enemy script) {
		enemies.Add(script);
	}
}
