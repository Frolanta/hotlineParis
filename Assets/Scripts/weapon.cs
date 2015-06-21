using UnityEngine;
using System.Collections;

public class weapon : MonoBehaviour {

	public Sprite attachBodySprite;
	
	public string weaponName;
	private SpriteRenderer sr;

	private Collider2D ignoreCollider;


	// Use this for initialization
	void Awake () {
		sr = gameObject.GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public Sprite getAttachedSprite() {
		return attachBodySprite;
	}

	public void hide() {
		sr.enabled = false;
	}

	public void show() {
		sr.enabled = true;
	}

	public void drop(float force, Collider2D ignore) {

		float drag = 5.0f;

		show ();
		ignoreCollider = ignore;
		this.transform.SetParent (null);
		this.gameObject.GetComponent<Collider2D> ().isTrigger = false;
		if (!this.gameObject.GetComponent<Rigidbody2D> ())
			this.gameObject.AddComponent<Rigidbody2D> ();

		Rigidbody2D rb = this.gameObject.GetComponent<Rigidbody2D> ();

		rb.drag = drag;
		rb.angularDrag = 1;
		rb.AddTorque (force / 2.0f, ForceMode2D.Impulse);
		rb.AddRelativeForce (Vector3.down * force * drag, ForceMode2D.Impulse);

		Physics2D.IgnoreCollision (ignoreCollider, gameObject.GetComponent<Collider2D> (), true);

		Invoke ("pickable", 2);
	}

	public void pickable() {
		this.gameObject.GetComponent<Collider2D> ().isTrigger = true;
		if (this.gameObject.GetComponent<Rigidbody2D> ())
			Destroy(this.gameObject.GetComponent<Rigidbody2D> ());
		Physics2D.IgnoreCollision (ignoreCollider, gameObject.GetComponent<Collider2D> (), false);
	}


}
