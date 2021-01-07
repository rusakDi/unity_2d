using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour {
	public float moveTime = 1000f;
	public LayerMask blockingLayer;

	[SerializeField]
	protected float speenP;

	private BoxCollider2D boxCollider;
	private Rigidbody2D rb2D;
	private float inverseMoveTime;

	protected virtual void Start() {
		boxCollider = GetComponent<BoxCollider2D>();
		rb2D = GetComponent<Rigidbody2D>();
		inverseMoveTime = 1f / moveTime;
	}

	protected bool Move (float xDir, float yDir, out RaycastHit2D hit, float deltaTime) {
		Vector2 start = transform.position;
		Vector2 end = transform.position + new Vector3(xDir, yDir) * speenP * deltaTime;
		boxCollider.enabled = false;
		hit = Physics2D.Linecast(start, end, blockingLayer);
		boxCollider.enabled = true;

		if (hit.transform == null) {
			transform.position += new Vector3(xDir, yDir) * speenP * deltaTime;
			return true;
		} else {
			return false;
		}
	}

	protected IEnumerator SmoothMovement (Vector3 end) {
		float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
		while (sqrRemainingDistance > float.Epsilon) {
			Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
			rb2D.MovePosition(newPosition);
			sqrRemainingDistance = (transform.position - end).sqrMagnitude;
			yield return null;
		}
	}

	protected virtual void AttemptMove<T>(float xDir, float yDir, float deltaTime)
		where T : Component {
		RaycastHit2D hit;
		bool canMove = Move(xDir, yDir, out hit, deltaTime);
	}
	
	protected abstract void OnAttack<T>(T component)
		where T : Component;
}
