using UnityEngine;
using System.Collections;

public class gameUI : MonoBehaviour {

	public Texture2D crosshair;

	// Use this for initialization
	void Start () {
		Cursor.SetCursor (crosshair, Vector2.zero, CursorMode.Auto);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
