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
	private const int playerFoodPoints = 10000;
	private Animator animator;
	private float food;
	public AudioSource shootSound;
	public AudioSource walkSound;
	public AudioSource hitSound;
	public AudioSource attackSound;
	public AudioSource loseSound;

    public float Food { get => food; set => food = value; }

    protected override void Start() {
		animator = transform.GetChild(0).GetComponent<Animator>();
		Food = playerFoodPoints;
		foodText.text = "WaterBalance: " + Food;
		base.Start();
	}

	void Update()
	{
		if ((bool)!GameManager.instance?.PlayersTurn) {
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
			AttemptMove<Wall>(horizontal, vertical, Time.deltaTime);
		}
	}
		
	public void LoseFood (int loss) {
		animator.SetTrigger("playerHit");
		hitSound.Play();
		Food -= loss;
		foodText.text = "-" + loss + " WaterBalance: " + Food;
		CheckIfGameOver();
	}

	protected override void AttemptMove <T> (float xDir, float yDir, float deltaTime) {
		Food -= Time.deltaTime;
		foodText.text = "WaterBalance: " + Food;
		walkSound.Play();
		base.AttemptMove <T>(xDir, yDir, deltaTime);
		CheckIfGameOver();
		GameManager.instance.PlayersTurn = false;
	}

	protected override void OnAttack <T> (T component) {
		Wall hitWall = component as Wall;
		hitWall.DamageWall(wallDamage);
		attackSound.Play();
		animator.SetTrigger("playerChop");
	}

	private void OnTriggerEnter2D (Collider2D other) {
		switch (other.tag) {
			case "Exit": {
				Invoke("Restart", restartLevelDelay);
                enabled = false;
                break;
			}
			case "Soda": {
				Food += pointsPerSoda;
                shootSound.Play();
                foodText.text = "+" + pointsPerSoda + " WaterBalance: " + Food;
                other.gameObject.SetActive(false);
                break;
            }
			case "InWall": {
				OnAttack(other.transform.GetComponent<Wall>());
				break;
            }
        }
	}

	private void Restart() {
		SceneManager.LoadScene("Main");
	}

	private void CheckIfGameOver() {
		if (Food <= 0) {
			loseSound.Play();
			GameManager.instance.GameOver();
		}
	}
}
