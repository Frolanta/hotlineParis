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
}
