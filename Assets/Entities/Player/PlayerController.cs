using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	public float ShipSpeed = 5.0f;
	public float padding = 1f;
	public GameObject projectile;
	public float projectileSpeed;
	public float firingRate = 0.2f;
	public float health = 250f;
	
	public AudioClip fireSound;
	
	float xmin;
	float xmax;
		
	// Use this for initialization
	void Start () {
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftmost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
		Vector3 rightmost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));
		xmin = leftmost.x + padding;
		xmax = rightmost.x - padding;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetKeyDown(KeyCode.Space)) {
			InvokeRepeating("LaunchProjectile", 0.00001f, firingRate);
		}
		
		if (Input.GetKeyUp(KeyCode.Space)) {
			CancelInvoke("LaunchProjectile");
		}
		
		if (Input.GetKey(KeyCode.LeftArrow)) {
			 MoveLeft();
		} else if (Input.GetKey(KeyCode.RightArrow)) {
			MoveRight();
		}
		
		//restricts PLayer to PLayspcae
		float newX = Mathf.Clamp(transform.position.x, xmin, xmax);
		transform.position = new Vector3(newX, transform.position.y, transform.position.z);
	}
	
		void OnTriggerEnter2D(Collider2D collider){
			Projectile missile = collider.gameObject.GetComponent<Projectile>();
			if (missile) {
				health -= missile.getDamage();
				missile.Hit();
				if (health <= 0) {
					Die();
				}
			}
		}
	
	void MoveLeft() {
		transform.position += Vector3.left * ShipSpeed * Time.deltaTime;
	}
	
	void MoveRight() {
		transform.position += Vector3.right * ShipSpeed * Time.deltaTime;
	}
	
	void LaunchProjectile() {
		Vector3 offset = new Vector3(0f,1f,0f);
		GameObject laserbeam = Instantiate(projectile, this.transform.position + offset, Quaternion.identity) as GameObject;
		laserbeam.rigidbody2D.velocity = new Vector3 (0, projectileSpeed, 0);
		AudioSource.PlayClipAtPoint(fireSound,this.transform.position);
	}
	
	void Die() {
		LevelManager lvlmanager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
		lvlmanager.LoadLevel("Win Screen");
		Destroy(gameObject);
	}
}
