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
		animator.SetTrigger ("dead");
		Destroy (this.gameObject, 1);
		Destroy (this.gameObject.GetComponent<Rigidbody2D>());
		Destroy (this.gameObject.GetComponent<Collider2D>());
		audioSource.Play ();
	}
}
