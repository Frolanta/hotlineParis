using UnityEngine;
using System.Collections;

public class character : MonoBehaviour {

	public float speed = 2.0f;
	
	private Rigidbody2D rb;
	public Animator legAnimator;

	private bool walking = false;

	// Use this for initialization
	void Start () {
		rb = gameObject.GetComponent<Rigidbody2D> ();
	}

	// Update is called once per frame
	void Update () {

		Vector3 mp = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		this.gameObject.transform.rotation = Quaternion.LookRotation (Vector3.forward, new Vector3(mp.x, mp.y, 0.0f) - transform.position);
	
		Vector3 vel = new Vector3();
		
		if(Input.GetKey(KeyCode.W)){
			Vector3 velUp = new Vector3();
			velUp.y = 1;
			vel += velUp;
		}
		else if(Input.GetKey(KeyCode.S)){
			Vector3 velDown = new Vector3();
			velDown.y = -1;
			vel += velDown;
		}

		if(Input.GetKey(KeyCode.A)){
			Vector3 velLeft = new Vector3();
			velLeft.x = -1;
			vel += velLeft;
		}
		else if(Input.GetKey(KeyCode.D)){
			Vector3 velRight = new Vector3();
			velRight.x = 1;
			vel += velRight;
		}

		if (vel.magnitude > 0.001) {
			Vector3.Normalize(vel);
			vel *= speed;
			rb.velocity = vel;

			if (!walking) {
				walking = true;
				legAnimator.SetBool("walk", true);
			}
		} else if (walking) {
			walking = false;
			legAnimator.SetBool("walk", false);
		}

	}
}
