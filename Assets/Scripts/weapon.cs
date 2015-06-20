using UnityEngine;
using System.Collections;

public class weapon : MonoBehaviour {

	public Sprite attachBodySprite;
	private SpriteRenderer sr;

	// Use this for initialization
	void Start () {
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

	public void drop(float force) {
		show ();
		this.transform.SetParent (null);
		this.gameObject.GetComponent<Collider2D> ().isTrigger = false;
		this.gameObject.AddComponent<Rigidbody2D> ();
		this.gameObject.GetComponent<Rigidbody2D> ().AddRelativeForce (Vector3.down * force, ForceMode2D.Impulse);

		Invoke ("pickable", 2);
	}

	public void pickable() {
		this.gameObject.GetComponent<Collider2D> ().isTrigger = true;
		Destroy(this.gameObject.GetComponent<Rigidbody2D> ());
	}


}
