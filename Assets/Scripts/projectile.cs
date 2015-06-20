using UnityEngine;
using System.Collections;

public class projectile : MonoBehaviour {

	public float speed;

	private Rigidbody2D rb;
	public float lifeTime = 2.0f;
	
	// Use this for initialization
	void Start () {
		rb = gameObject.GetComponent<Rigidbody2D> ();
		rb.AddRelativeForce (Vector3.down * speed, ForceMode2D.Impulse);
	}
	
	// Update is called once per frame
	void Update () {
		lifeTime -= Time.deltaTime;
		if (lifeTime < 0) {
			Destroy(this.gameObject);
		}
	}

	public void ignoreCollider(Collider2D c) {
		Physics2D.IgnoreCollision (this.gameObject.GetComponent<Collider2D>(), c);

	}

	void OnCollisionEnter2D(Collision2D other) {
		other.gameObject.SendMessage ("takeDamage", null, SendMessageOptions.DontRequireReceiver);
		Destroy (gameObject);
	}
}
