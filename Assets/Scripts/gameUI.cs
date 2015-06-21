using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class gameUI : MonoBehaviour {

	public Texture2D crosshair;
	public Text weaponName;
	public Text ammoCount;
	public endPanel end;


	public static gameUI instance;

	void Awake () {
		if (instance == null) {
			instance = this;
		}
	}

	// Use this for initialization
	void Start () {
		hideWeaponInfo ();
		Cursor.SetCursor (crosshair, Vector2.zero, CursorMode.Auto);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void dead()
	{
		//Time.timeScale = 0;
		end.activate (true);
	}

	public void endLevel()
	{
		//Time.timeScale = 0;
		end.activate (false);
	}

	public void setWeapon(string str) {
		weaponName.text = str;
	}

	public void setAmmo(int nb) {
		if (nb == -1)
			ammoCount.text = "inf";
		else
			ammoCount.text = "" + nb;
	}

	public void hideWeaponInfo() {
		weaponName.text = "";
		ammoCount.text = "";
	}
	
}
