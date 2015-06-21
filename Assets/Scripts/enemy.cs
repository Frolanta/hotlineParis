using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class enemy : MonoBehaviour {


	public Transform[] checkpoints;
	public float speed = 1.0f;

	private GameObject weapon = null;
	public GameObject attachWeapon;

	public GameObject[] possibleWeapons;


	private Vector3 nextCheckpoint;
	private int checkpointIndex = 0;
	private Rigidbody2D rb;

	private bool hasCheckpoints = false;

	private Transform target;
	private Vector3 checkPosition;
	public bool shouldCheck = false;

	private Vector3 intermediatePos;
	private bool hasIntermediate;


	private List<Vector3> exits;

	public float headSoundRadius = 7.0f;


	public AudioSource audioSource;
	public Animator animator;

	public Animator legAnimator;

	private bool stun = false;

	public float StartTargetTime = 3.0f;
	private float targetTime = 0.0f;

	// Use this for initialization
	void Start () {
		rb = gameObject.GetComponent<Rigidbody2D> ();

		if (checkpoints.Length <= 1) {
			nextCheckpoint = transform.position;
		} else {
			hasCheckpoints = true;
			nextCheckpoint = checkpoints[0].position;
		}

		weapon = (GameObject)Instantiate(possibleWeapons[Random.Range(0, possibleWeapons.Length)], Vector3.zero, Quaternion.identity);
		weapon.transform.SetParent(attachWeapon.transform, false);

		weapon.GetComponent<weapon>().hide ();
		weapon.GetComponent<gun>().ammo = -1;
		attachWeapon.GetComponent<SpriteRenderer>().sprite = weapon.GetComponent<weapon>().getAttachedSprite();

		Destroy(weapon.GetComponent<Collider2D>());

		exits = new List<Vector3>();

		GameObject[] doors = GameObject.FindGameObjectsWithTag("door");

		foreach (GameObject door in doors) {
			exits.Add(door.transform.position);
		}


		GameObject[] exts = GameObject.FindGameObjectsWithTag("exit");
		
		foreach (GameObject e in exts) {
			exits.Add(e.transform.position);
		}
	}
	
	// Update is called once per frame
	void Update () {
		targetTime -= Time.deltaTime;

		if ((target || shouldCheck) && targetTime <= 0.0f) {
			weapon.SendMessage("stopAttack", null, SendMessageOptions.DontRequireReceiver);
			target = null;
			shouldCheck = false;

		}
	}

	void FixedUpdate() {

		if (stun)
			return ;

		if (target) {

			rb.MovePosition (transform.position + getDirection (target.position) * speed * Time.fixedDeltaTime);
			lookAtPosition (target.position);

			legAnimator.SetBool ("walk", true);

		} else if (hasIntermediate) {


			rb.MovePosition (transform.position + getDirection (intermediatePos) * speed * Time.fixedDeltaTime);
			lookAtPosition (intermediatePos);

			if (Vector3.Distance (transform.position, intermediatePos) <= Time.fixedDeltaTime * speed) {
				hasIntermediate = false;
			}

			legAnimator.SetBool ("walk", true);
		

		} else if (shouldCheck && richable (checkPosition)) {

			rb.MovePosition (transform.position + getDirection (checkPosition) * speed * Time.fixedDeltaTime);
			lookAtPosition (checkPosition);

			if (Vector3.Distance (transform.position, checkPosition) <= Time.fixedDeltaTime * speed) {
				shouldCheck = false;
			}

			legAnimator.SetBool ("walk", true);
		

		} else if (hasCheckpoints && richable (nextCheckpoint)) {

			rb.MovePosition (transform.position + getDirection (nextCheckpoint) * speed * Time.fixedDeltaTime);
			lookAtPosition (nextCheckpoint);

			if (Vector3.Distance (transform.position, nextCheckpoint) <= Time.fixedDeltaTime * speed) {
				if (checkpointIndex + 1 < checkpoints.Length)
					checkpointIndex++;
				else
					checkpointIndex = 0;

				nextCheckpoint = checkpoints [checkpointIndex].position;
			}

			legAnimator.SetBool ("walk", true);
		

		} else if (Vector3.Distance (transform.position, nextCheckpoint) >= speed * Time.fixedDeltaTime && richable (nextCheckpoint)) {
			rb.MovePosition (transform.position + getDirection (nextCheckpoint) * speed * Time.fixedDeltaTime);
			lookAtPosition (nextCheckpoint);
			legAnimator.SetBool ("walk", true);
		} else {
			legAnimator.SetBool("walk", false);
		}

	}


	bool richable(Vector3 pos) {

;
		LayerMask layerMask = ~((1 << LayerMask.NameToLayer ("enemy"))
		                        | (1 << LayerMask.NameToLayer ("weapon"))
		                        | (1 << LayerMask.NameToLayer ("door"))
		                        | (1 << LayerMask.NameToLayer ("Player"))
		                        );
		
		float maxRange = Vector3.Distance(transform.position, pos);
		RaycastHit2D hit = Physics2D.Raycast(transform.position, (pos - transform.position), maxRange, layerMask);


		if (hit.collider != null) {

			Debug.DrawLine(transform.position, pos, Color.red, 0.1f, false);

			;

			Vector3 exit = Vector3.zero;
			float dist = 10000.0f;
			bool hasExit = false;

			foreach (Vector3 e in exits) {

				if (Vector3.Distance (e, transform.position) > 0.05f) {
					float range = Vector3.Distance(transform.position, e);
					RaycastHit2D h = Physics2D.Raycast(transform.position, (e - transform.position), range, layerMask);
					
					if (h.collider == null) {
						if (Vector3.Distance (e, pos) < dist) {
							dist = Vector3.Distance (e, transform.position);
							exit = e;
							hasExit = true;

							Debug.DrawLine(transform.position, e, Color.cyan, 0.1f, false);

						}
					}
				}

			}

			if (hasExit) {
				hasIntermediate = true;
				intermediatePos = exit;
				return false;
			}

			return true;
	
		}

		Debug.DrawLine(transform.position, pos, Color.green, 0.1f, false);


		return true;
	}

	Vector3 getDirection(Vector3 pos) {

		Vector3 heading = pos - transform.position;
		float distance = heading.magnitude;
		return heading / distance;
	}

	void lookAtPosition(Vector3 pos) {
		Vector3 diff = pos - transform.position;
		diff.Normalize();
		
		float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0f, 0f, rot_z + 90.0f);

	}


	void OnTriggerStay2D(Collider2D other) {
		if (stun)
			return;

		if (other.tag == "Player") {
			LayerMask layerMask = ~(1 << LayerMask.NameToLayer ("enemy") | 1 << LayerMask.NameToLayer ("weapon"));

			float maxRange = 100.0f;
			RaycastHit2D hit = Physics2D.Raycast(transform.position, (other.transform.position - transform.position), maxRange, layerMask);
			if(hit.collider != null && hit.collider.gameObject.tag == "Player") {
				Debug.DrawLine(transform.position, other.transform.position, Color.blue, 0.01f, false);

				if (!target)
					weapon.SendMessage("startAttack", this.gameObject.GetComponent<Collider2D>(), SendMessageOptions.DontRequireReceiver);

				targetTime = StartTargetTime;
				target = other.transform;


			} else if (target) {
				Debug.DrawLine(transform.position, other.transform.position, Color.red, 0.5f, false);
				checkPosition = new Vector3 (target.position.x, target.position.y, target.position.z);
				shouldCheck = true;
				targetTime = StartTargetTime;
				target = null;

				weapon.SendMessage("stopAttack", null, SendMessageOptions.DontRequireReceiver);

			} else {
				target = null;
			}
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (stun)
			return;

		if(other.tag == "Player") {
			if (target) {
				weapon.SendMessage("stopAttack", null, SendMessageOptions.DontRequireReceiver);
				checkPosition = new Vector3 (target.position.x, target.position.y, target.position.z);
				shouldCheck = true;
				targetTime = StartTargetTime;
				target = null;
			}
		} 
	}

	public void takeDamage() {
		die ();
	}
	
	void die () {

		enemyManager.instance.removeEnnemy (this.gameObject);
		weapon.SendMessage("stopAttack", null, SendMessageOptions.DontRequireReceiver);
		animator.SetTrigger ("dead");
		Destroy (this.gameObject, 1);
		Destroy (this.gameObject.GetComponent<Rigidbody2D>());
		Destroy (this.gameObject.GetComponent<Collider2D>());
		Destroy (this);
		audioSource.Play ();

	}
	
	void OnCollisionEnter2D(Collision2D collision) {
		if (stun)
			return ;

		if (collision.gameObject.tag == "weapon") {
			if (collision.gameObject.name == "katana(Clone)") {
				die ();
			}

			else if (!stun && collision.gameObject.GetComponent<Rigidbody2D>().velocity.sqrMagnitude > 15.0f) {
				Stun();
			}
		} else if (collision.gameObject.tag == "door") {
			if (!stun && collision.gameObject.GetComponent<Rigidbody2D>().velocity.sqrMagnitude > 5.0f) {
				Stun();
			}
		}
	}

	public void heardSound (Vector3 pos) {
		if (!stun && !target && Vector3.Distance (transform.position, pos) <= headSoundRadius) {
			checkPosition = new Vector3 (pos.x, pos.y, pos.z);
			shouldCheck = true;
			hasIntermediate = false;
			targetTime = StartTargetTime;
		}
	}

	void Stun () {
		weapon.SendMessage("stopAttack", null, SendMessageOptions.DontRequireReceiver);
		animator.SetBool("stun", true);
		stun = true;
		target = null;
		shouldCheck = false;
		hasIntermediate = false;

		legAnimator.SetBool ("walk", false);

		Invoke ("unStun", 2);
	}

	void unStun() {
		animator.SetBool("stun", false);
		stun = false;
	}
}
