using UnityEngine;
using System.Collections;

public class randomhead : MonoBehaviour {


	public Sprite[] heads;
	public SpriteRenderer sr;

	// Use this for initialization
	void Start () {
		sr.sprite = heads[Random.Range (0, heads.Length)];
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
