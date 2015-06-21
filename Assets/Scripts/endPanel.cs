using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class endPanel : MonoBehaviour {

	public Text headline;
	public Text next;
	public Text exit;
	public CanvasGroup cg;
	private bool menu;
	public bool lost;
	private int selected;
	private Color textColor;

	public AudioSource aWin;
	public AudioSource aLose;

	// Use this for initialization
	void Start () {
		textColor = next.color;
		next.color = Color.blue;
		selected = 1;
	}
	
	// Update is called once per frame
	void Update () {
	

		if (menu) {
		
			if (Input.GetKeyDown (KeyCode.S) || Input.GetKeyDown (KeyCode.DownArrow)) {
				
				if (selected == 1) {
					selected = 2;
					next.color = textColor;
					exit.color = Color.blue;
				}
				else
				{
					selected = 1;
					next.color = Color.blue;
					exit.color = textColor;
					
				}
			}
			
			if (Input.GetKeyDown (KeyCode.W) || Input.GetKeyDown (KeyCode.UpArrow)) {
				
				if (selected == 1) {
					selected = 2;
					next.color = textColor;
					exit.color = Color.blue;
				}
				else
				{
					selected = 1;
					next.color = Color.blue;
					exit.color = textColor;
					
				}
			}

			if (Input.GetKeyDown (KeyCode.Return))
			{
				if (lost) {


					if (selected == 1)
						Application.LoadLevel(Application.loadedLevel);
					else
						Application.LoadLevel(0);
		
				}
				else
				{
					if (selected == 1)
						Application.LoadLevel(Application.loadedLevel + 1);
					else
						Application.LoadLevel(0);
				}

			}
		}

	}

	public void activate (bool l)
	{
		cg.alpha = 1;
		menu = true;
		lost = l;
		cg.blocksRaycasts = true;
		if (lost) {
			aLose.Play();
			headline.text = "GAME OVER !";
			next.text = "Retry";
		}
		else
		{
			aWin.Play();
			headline.text = "GOOD JOB";
			next.text = "Next Level";
		}
	}

	public void deactivate ()
	{
		menu = false;
		cg.alpha = 0;
		cg.blocksRaycasts = false;
	}
}
