using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MovingObject {
	public int wallDamage = 1;
	public int pointsPerSoda = 20;
	public float restartLevelDelay = 1f;
	public Text foodText;
	private Animator animator;
	private int food;
	public AudioSource shootSound;
	public AudioSource walkSound;
	public AudioSource hitSound;
	public AudioSource attackSound;
	public AudioSource loseSound;

	protected override void Start() {
		animator = GetComponent<Animator>();
		food = GameManager.instance.playerFoodPoints;

		foodText.text = "WaterBalance: " + food;

		base.Start();
	}

	void Update()
	{
		if (!GameManager.instance.playersTurn) {
			return;
		}

		int horizontal = 0;
		int vertical = 0;

		horizontal = (int) Input.GetAxisRaw("Horizontal");
		vertical = (int) Input.GetAxisRaw("Vertical");

		if (horizontal != 0) {
			vertical = 0;
		}

		if (horizontal != 0 || vertical != 0) {
			AttemptMove<Wall>(horizontal, vertical);
		}
	}
		
	public void LoseFood (int loss) {
		animator.SetTrigger("playerHit");
		hitSound.Play();
		food -= loss;
		foodText.text = "-" + loss + " WaterBalance: " + food;
		CheckIfGameOver();
	}

	protected override void AttemptMove <T> (int xDir, int yDir) {
		food--;
		foodText.text = "WaterBalance: " + food;
		walkSound.Play();
		base.AttemptMove <T>(xDir, yDir);

		CheckIfGameOver();

		GameManager.instance.playersTurn = false;
	}

	protected override void OnCantMove <T> (T component) {
		Wall hitWall = component as Wall;
		hitWall.DamageWall(wallDamage);
		attackSound.Play();
		animator.SetTrigger("playerChop");
	}

	private void OnTriggerEnter2D (Collider2D other) {
		if (other.CompareTag("Exit")) {
			Invoke("Restart", restartLevelDelay);
			enabled = false;
		}
		else if (other.CompareTag("Soda")) {
			food += pointsPerSoda;
			shootSound.Play();
			foodText.text = "+" + pointsPerSoda + " WaterBalance: " + food;
			other.gameObject.SetActive(false);
		}
	}

	private void Restart() {
		SceneManager.LoadScene("Main");
	}

	private void OnDisable() {
		GameManager.instance.playerFoodPoints = food;
	}

	private void CheckIfGameOver() {
		if (food <= 0) {
			loseSound.Play();
			GameManager.instance.GameOver();
		}
	}

}
