using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Title : MonoBehaviour {

	private int selected;
	public Text newGame;
	public Text exit;


	private Color textColor;
	// Use this for initialization
	void Start () {
		textColor = newGame.color;
		newGame.color = Color.blue;
		selected = 1;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.DownArrow)) {

			if (selected == 1) {
				Debug.Log(selected);
				selected = 2;
				newGame.color = textColor;
				exit.color = Color.blue;
			}
			else
			{
				Debug.Log(selected);
				selected = 1;
				newGame.color = Color.blue;
				exit.color = textColor;

			}
		}

		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			
			if (selected == 1) {
				Debug.Log(selected);
				selected = 2;
				newGame.color = textColor;
				exit.color = Color.blue;
			}
			else
			{
				Debug.Log(selected);
				selected = 1;
				newGame.color = Color.blue;
				exit.color = textColor;
				
			}
		}

		if (Input.GetKeyDown (KeyCode.Return)) {
			if (selected == 1)
				Application.LoadLevel(1);
			else
				Application.Quit();
		}

	}


}
