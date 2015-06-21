using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class enemy : MonoBehaviour {


	public Transform[] checkpoints;
	public float speed = 1.0f;


	private Vector3 nextCheckpoint;
	private int checkpointIndex = 0;
	private Rigidbody2D rb;

	private bool hasCheckpoints = false;

	private Transform target;
	private Vector3 checkPosition;
	private bool shouldCheck;

	private Vector3 intermediatePos;
	private bool hasIntermediate;


	private List<Vector3> exits;

	// Use this for initialization
	void Start () {
		rb = gameObject.GetComponent<Rigidbody2D> ();

		if (checkpoints.Length <= 1) {
			nextCheckpoint = transform.position;
		} else {
			hasCheckpoints = true;
			nextCheckpoint = checkpoints[0].position;
		}


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
	}

	void FixedUpdate() {

		if (target) {

			rb.MovePosition (transform.position + getDirection (target.position) * speed * Time.fixedDeltaTime);
			lookAtPosition (target.position);

		} else if (hasIntermediate) {

			rb.MovePosition (transform.position + getDirection (intermediatePos) * speed * Time.fixedDeltaTime);
			lookAtPosition (intermediatePos);

			if (Vector3.Distance (transform.position, intermediatePos) <= Time.fixedDeltaTime * speed) {
				Debug.Log ("intermediate finish");
				hasIntermediate = false;
			}


		} else if (shouldCheck && richable(checkPosition)) {

			rb.MovePosition (transform.position + getDirection (checkPosition) * speed * Time.fixedDeltaTime);
			lookAtPosition (checkPosition);

			if (Vector3.Distance (transform.position, checkPosition) <= Time.fixedDeltaTime * speed) {
				shouldCheck = false;
			}
		

		} else if (hasCheckpoints && richable(nextCheckpoint)) {

			rb.MovePosition (transform.position + getDirection (nextCheckpoint) * speed * Time.fixedDeltaTime);
			lookAtPosition (nextCheckpoint);

			if (Vector3.Distance (transform.position, nextCheckpoint) <= Time.fixedDeltaTime * speed) {
				Debug.Log ("change checkpoints");
				if (checkpointIndex + 1 < checkpoints.Length)
					checkpointIndex++;
				else
					checkpointIndex = 0;

				nextCheckpoint = checkpoints [checkpointIndex].position;
			}
		

		} else if (Vector3.Distance (transform.position, nextCheckpoint) >= speed * Time.fixedDeltaTime && richable(nextCheckpoint)) {
			rb.MovePosition (transform.position + getDirection (nextCheckpoint) * speed * Time.fixedDeltaTime);
			lookAtPosition (nextCheckpoint);
		} else {
			transform.rotation = Quaternion.identity;
		}

	}


	bool richable(Vector3 pos) {

		Debug.Log ("richable test");
		LayerMask layerMask = ~((1 << LayerMask.NameToLayer ("enemy"))
		                        | (1 << LayerMask.NameToLayer ("weapon"))
		                        | (1 << LayerMask.NameToLayer ("door"))
		                        | (1 << LayerMask.NameToLayer ("Player"))
		                        );
		
		float maxRange = Vector3.Distance(transform.position, pos);
		RaycastHit2D hit = Physics2D.Raycast(transform.position, (pos - transform.position), maxRange, layerMask);

		Debug.DrawLine(transform.position, pos, Color.green, 0.1f, false);

		if (hit.collider != null) {


			Debug.Log ("not richable : " + hit.collider.tag);

			Vector3 exit = Vector3.zero;
			float dist = 10000.0f;
			bool hasExit = false;

			foreach (Vector3 e in exits) {

				if (Vector3.Distance (e, transform.position) > 0.01f) {
					float range = Vector3.Distance(transform.position, e);
					RaycastHit2D h = Physics2D.Raycast(transform.position, (e - transform.position), range, layerMask);
					
					if (h.collider == null) {
						if (Vector3.Distance (e, pos) < dist) {
							dist = Vector3.Distance (e, transform.position);
							exit = e;
							hasExit = true;
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

		Debug.Log ("richable");

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
		if (other.tag == "Player") {
			LayerMask layerMask = ~(1 << LayerMask.NameToLayer ("enemy") | 1 << LayerMask.NameToLayer ("weapon"));

			float maxRange = 100.0f;
			RaycastHit2D hit = Physics2D.Raycast(transform.position, (other.transform.position - transform.position), maxRange, layerMask);
			if(hit.collider != null && hit.collider.gameObject.tag == "Player") {
				Debug.DrawLine(transform.position, other.transform.position, Color.blue, 0.01f, false);
				target = other.transform;
			} else if (target) {
				Debug.DrawLine(transform.position, other.transform.position, Color.red, 0.5f, false);
				checkPosition = new Vector3 (target.position.x, target.position.y, target.position.z);
				shouldCheck = true;
				target = null;
			} else {
				target = null;
			}
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if(other.tag == "Player") {
			if (target) {
				checkPosition = new Vector3 (target.position.x, target.position.y, target.position.z);
				shouldCheck = true;
				target = null;
			}
		} 
	}
}
