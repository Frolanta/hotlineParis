using UnityEngine;
using System.Collections;

public class character : MonoBehaviour {

	public float speed = 2.0f;
	public Animator legAnimator;
	public GameObject attachWeapon;

	private GameObject weapon = null;
	private Rigidbody2D rb;
	private bool walking = false;


	private bool up = false;
	private bool down = false;
	private bool left = false;
	private bool right = false;


	public AudioSource audioSource;
	public AudioClip pickupClip;
	public AudioClip ejectWeapon;
	public AudioClip dieClip;

	public Animator animator;


	// Use this for initialization
	void Start () {
		rb = gameObject.GetComponent<Rigidbody2D> ();
	}


	void FixedUpdate() {

		Vector3 vel = new Vector3();
		
		if(up){
			Vector3 velUp = new Vector3();
			velUp.y = 1;
			vel += velUp;
		} else if(down) {
			Vector3 velDown = new Vector3();
			velDown.y = -1;
			vel += velDown;
		}
		
		if (left) {
			Vector3 velLeft = new Vector3();
			velLeft.x = -1;
			vel += velLeft;
		} else if(right){
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

	// Update is called once per frame
	void Update () {

		if (weapon && weapon.GetComponent<gun> ()) {
			gameUI.instance.setAmmo(weapon.GetComponent<gun> ().ammo);
		}
	

		if(Input.GetKey(KeyCode.W) || Input.GetKeyDown (KeyCode.UpArrow)){
			up = true;
			down = false;
		}
		else if(Input.GetKey(KeyCode.S) || Input.GetKeyDown (KeyCode.DownArrow)){
			up = false;
			down = true;
		}
		
		if(Input.GetKey(KeyCode.A) || Input.GetKeyDown (KeyCode.LeftArrow)){
			left = true;
			right = false;
		}
		else if(Input.GetKey(KeyCode.D) || Input.GetKeyDown (KeyCode.RightArrow)){
			left = false;
			right = true;
		}

		if(Input.GetKeyUp(KeyCode.W) || Input.GetKeyDown (KeyCode.UpArrow)){
			up = false;
		}
		if(Input.GetKeyUp(KeyCode.S) || Input.GetKeyDown (KeyCode.DownArrow)){
			down = false;
		}
		
		if(Input.GetKeyUp(KeyCode.A) || Input.GetKeyDown (KeyCode.LeftArrow)){
			left = false;
		}
		if(Input.GetKeyUp(KeyCode.D) || Input.GetKeyDown (KeyCode.RightArrow)){
			right = false;
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

			RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.zero, 0f);

			foreach (RaycastHit2D hit in hits) {

				if (hit.collider && hit.collider.tag == "weapon" && weapon == null) {

					audioSource.clip = pickupClip;
					audioSource.Play();

					weapon = hit.collider.gameObject;

					if (weapon.GetComponent<Rigidbody2D>()) {
						Destroy(weapon.GetComponent<Rigidbody2D>());
					}

					weapon.transform.position = Vector3.zero;
					weapon.transform.rotation = Quaternion.identity;
					weapon.transform.SetParent(attachWeapon.transform, false);

					gameUI.instance.setWeapon(weapon.GetComponent<weapon>().weaponName);

					weapon.GetComponent<weapon>().hide ();
					attachWeapon.GetComponent<SpriteRenderer>().sprite = weapon.GetComponent<weapon>().getAttachedSprite();

				}
			}

		}

		if (Input.GetKeyDown (KeyCode.R)) {
			Application.LoadLevel(Application.loadedLevel);
		}

		if (Input.GetMouseButtonDown (0) && weapon) {
			weapon.SendMessage("startAttack", this.gameObject.GetComponent<Collider2D>(), SendMessageOptions.DontRequireReceiver);
		}

		if (Input.GetMouseButtonUp (0) && weapon) {
			weapon.SendMessage("stopAttack", null, SendMessageOptions.DontRequireReceiver);
		}

		if (Input.GetMouseButtonDown (1) && weapon) {
			//weapon.SendMessage("stopAttack", null, SendMessageOptions.DontRequireReceiver);
			audioSource.clip = ejectWeapon;
			audioSource.Play();

			gameUI.instance.hideWeaponInfo();

			float distance = Vector3.Distance(new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0.0f) , transform.position);


			weapon.GetComponent<weapon>().drop(distance, this.gameObject.GetComponent<Collider2D>());
			weapon = null;
			attachWeapon.GetComponent<SpriteRenderer>().sprite = null;
		}
	}


	public void takeDamage() {
		die ();
	}
	
	void die () {
		if (weapon)
			weapon.SendMessage("stopAttack", null, SendMessageOptions.DontRequireReceiver);
		animator.SetTrigger ("dead");
		Destroy (this.gameObject, 1);
		Destroy (this.gameObject.GetComponent<Rigidbody2D>());
		Destroy (this.gameObject.GetComponent<Collider2D>());
		Destroy (this);
		audioSource.clip = dieClip;
		audioSource.Play ();
		gameUI.instance.dead ();
	}
}
