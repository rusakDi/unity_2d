using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject {
	public int playerDamage;

	[SerializeField]
	private float rangeAttack;
	
	private Animator animator;
	private Transform target;
	private bool skipMove;
	private const float eps = 0.00001f;
	protected override void Start() {
        GameManager.instance.AddEnemyToList(this);
		animator = GetComponent<Animator>();
		target = GameObject.Find("Player").transform;
		base.Start();
	}

    private void Update() => StartCoroutine(AttackCorutine(1f));

	IEnumerator AttackCorutine(float deltaTime) {
		if (Vector2.Distance(target.position, transform.position) <= rangeAttack) {
			if (target.GetComponent<Player>()) {
				OnAttack(target.GetComponent<Player>());
			}
		}
		yield return new WaitForSeconds(deltaTime);
	}

	protected override void AttemptMove<T>(float xDir, float yDir, float deltaTime) {
		if (skipMove) {
			skipMove = false;
			return;
		}

		base.AttemptMove<T>(xDir, yDir, deltaTime);
		skipMove = true;
	}

	protected override void OnAttack<T>(T component) {
		Player hitPlayer = component as Player;
		animator.SetTrigger("enemyAttack");
		hitPlayer.LoseFood(playerDamage);
	}

	public void MoveEnemy(float deltaTime) {
		float xDir = 0;
		float yDir = 0;
		var dist = Vector3.Distance(target.position, transform.position);
		if (dist > eps) {
			var direction = target.position - transform.position;
			if (deltaTime * speenP <= dist) {
				direction /= dist;
            } else {
				direction /= deltaTime * speenP;
            }
			xDir = direction.x;
			yDir = direction.y;
		} 
		AttemptMove <Player>(xDir, yDir, deltaTime);
	}
}
