using UnityEngine;
using System.Collections;

public class gun : MonoBehaviour {
	
	public float fireRate;

	public float burstCount;
	public float burstFireRate;

	public int ammo;

	public string name;

	public GameObject projectile;

	private IEnumerator routine;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void startAttack() {
		if (ammo == 0)
			return ;
		routine = attackRoutine ();
		StartCoroutine (routine);
	}

	public void stopAttack() {
		StopCoroutine (routine);
	}

	IEnumerator attackRoutine () {
		while (ammo > 0) {

			for (int i = 0 ; i < burstCount ; i++) {
				Debug.Log ("shot : " + ammo);
				ammo--;
				yield return new WaitForSeconds(burstFireRate);
			}

			yield return new WaitForSeconds(fireRate);
		}
	}
}
