using UnityEngine;
using System.Collections;

public class oneHitDead : MonoBehaviour {

	public AudioSource audioSource;
	public Animator animator;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void takeDamage() {
		die ();
	}

	void die () {
		animator.SetTrigger ("dead");
		Destroy (this.gameObject, 1);
		Destroy (this.gameObject.GetComponent<Rigidbody2D>());
		Destroy (this.gameObject.GetComponent<Collider2D>());
		audioSource.Play ();
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.tag == "weapon" || (this.gameObject.tag != "Player" && collision.gameObject.tag == "door")) {
			if (collision.gameObject.GetComponent<Rigidbody2D>().velocity.sqrMagnitude > 20.0f) {
				die ();
			}
		}
	}
}
