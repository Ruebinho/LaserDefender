using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {
	public float health = 150f;
	public GameObject enemyProjectile;
	public float projectileSpeed = 10f;
	public float shotsPerSeconds = 0.5f;
	public int scoreValue = 150;
	
	public AudioClip enemyFireSound;
	public AudioClip enemyDeathSound;
	
	private ScoreKeeper scoreKeeper;
	
	void Start() {
		scoreKeeper = GameObject.Find("Score").GetComponent<ScoreKeeper>();
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
	
	void Update () {
		float  probability = Time.deltaTime * shotsPerSeconds;
		if(Random.value < probability) {
			Fire();
		}
		
	}
	
	void Fire() {
		Vector3 startPosition = transform.position + new Vector3(0,-1,0);
		GameObject laserbeam = Instantiate(enemyProjectile, startPosition, Quaternion.identity) as GameObject;
		laserbeam.rigidbody2D.velocity = new Vector3 (0, -projectileSpeed, 0);
		AudioSource.PlayClipAtPoint(enemyFireSound, this.transform.position);
	}
	
	void Die() {
		AudioSource.PlayClipAtPoint(enemyDeathSound, this.transform.position);
		scoreKeeper.Score(scoreValue);
		Destroy(gameObject);
	}
}
