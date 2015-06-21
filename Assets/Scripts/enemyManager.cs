using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class enemyManager : MonoBehaviour {


	private List<GameObject> enemies;

	public static enemyManager instance;
	
	void Awake () {
		if (instance == null) {
			instance = this;
		}
	}

	// Use this for initialization
	void Start () {
		enemies = new List<GameObject>();

		GameObject[] ens = GameObject.FindGameObjectsWithTag("enemy");

		foreach (GameObject e in ens) {
			enemies.Add(e);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void removeEnnemy(GameObject enemy) {

		enemies.Remove (enemy);

		if (enemies.Count == 0) {
			gameUI.instance.endLevel();
		}
	}
}
