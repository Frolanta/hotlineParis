using UnityEngine;
using System.Collections;

public class gun : MonoBehaviour {
	
	public float fireRate;

	public float burstCount;
	public float burstFireRate;

	public int ammo;

	public string weaponName;

	public GameObject projectile;
	private IEnumerator routine;

	private Animator animator;

	private AudioSource audioSource;


	// Use this for initialization
	void Start () {
		animator = this.gameObject.GetComponent<Animator> ();
		audioSource = this.gameObject.GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void startAttack(Collider2D ignore) {
		if (ammo == 0) {
			return;
		}
		routine = attackRoutine (ignore);
		StartCoroutine (routine);
	}

	public void stopAttack() {
		StopCoroutine (routine);
	}

	IEnumerator attackRoutine (Collider2D ignore) {
		while (ammo > 0) {

			for (int i = 0 ; i < burstCount ; i++) {
				GameObject ob = (GameObject)Instantiate(projectile, this.transform.position, this.transform.rotation);
				ob.GetComponent<projectile>().ignoreCollider(ignore);
				ammo--;
				audioSource.Play();
				if (ammo == 0) {
					animator.SetBool("noAmmo", true);
				}
				yield return new WaitForSeconds(burstFireRate);
			}

			yield return new WaitForSeconds(fireRate);
		}
	}
}
