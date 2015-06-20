using UnityEngine;
using System.Collections;

public class character : MonoBehaviour {

	public float speed = 2.0f;
	public Animator legAnimator;
	public GameObject attachWeapon;

	private GameObject weapon = null;
	private Rigidbody2D rb;
	private bool walking = false;

	// Use this for initialization
	void Start () {
		rb = gameObject.GetComponent<Rigidbody2D> ();
	}

	// Update is called once per frame
	void Update () {
	
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


		//rotation
		Vector3 mousePos = Input.mousePosition;
		
		Vector3 objectPos = Camera.main.WorldToScreenPoint (transform.position);
		mousePos.x = mousePos.x - objectPos.x;
		mousePos.y = mousePos.y - objectPos.y;
		
		float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90.0f));

		//weapon pikup
		if (Input.GetKeyDown (KeyCode.E)) {

			RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero, 0f);

			if (hit.collider)
				Debug.Log (hit.collider.tag);

			if (hit.collider && hit.collider.tag == "weapon" && weapon == null) {
				weapon = hit.collider.gameObject;
				weapon.transform.position = Vector3.zero;
				weapon.transform.SetParent(attachWeapon.transform, false);
				weapon.GetComponent<weapon>().hide ();
				attachWeapon.GetComponent<SpriteRenderer>().sprite = weapon.GetComponent<weapon>().getAttachedSprite();
			}

		}

		if (Input.GetMouseButtonDown (0) && weapon) {
			Debug.Log ("mouse down");
			weapon.SendMessage("startAttack", null, SendMessageOptions.DontRequireReceiver);
		}

		if (Input.GetMouseButtonUp (0) && weapon) {
			weapon.SendMessage("stopAttack", null, SendMessageOptions.DontRequireReceiver);
		}

	}
}
