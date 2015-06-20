using UnityEngine;
using System.Collections;

public class weaponSpawner : MonoBehaviour {

	public GameObject[] weapons;

	// Use this for initialization
	void Start () {
		Instantiate (weapons [Random.Range (0, weapons.Length)], transform.position, Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawIcon(transform.position, "weaponSpawner.png", true);
	}
}
