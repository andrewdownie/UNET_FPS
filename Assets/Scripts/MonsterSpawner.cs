using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MonsterSpawner : MonsterSpawner_Base {
	[Header("Spawner Health")]
	[SerializeField]
	float maxHealth = 200f;
	[SerializeField]
	float healthRegenRate = 5f;
	[SerializeField]
	float healthRegenDelay = 5f;
	[SerializeField]
	float timeSinceAttacked;
	[SyncVar(hook="HealthChanged")]
	[SerializeField]
	float currentHealth;


	[Header("Spawn Settings")]
	[SerializeField]
	float SPAWN_DELAY = 5;
	[SerializeField]
	float timeSinceLastSpawn;
	[SerializeField]
	Zombie zombiePrefabToSpawn;
	[SerializeField]
	int maxSpawned = 1;
	[SerializeField]
	int currentlySpawned;

	[SerializeField]
	Image healthBar;

	[SerializeField]
	ParticleSystem hitSplatter;
	[SerializeField]
	ParticleSystem deathSplatter;
	[SerializeField]
	Renderer[] disableRendersOnDeath;
	[SerializeField]
	Behaviour[] disableBehavioursOnDeath;	
	[SerializeField]
	Collider[] disableCollidersOnDeath;
	

	void Start(){
		if(!isServer){
			enabled = false;
		}

		currentlySpawned = 0;
		currentHealth = maxHealth;
	}

	public override void RemoveSpawnee(){
		currentlySpawned--;
	}



	[Command]
	public override void CmdAddHealth(float amount){
		ApplyDamage(-amount);
	}

	[Command]
	public override void CmdSubtractHealth(float amount, Vector3 pointOfImpact, Vector3 locationOfBullet){
		ApplyDamage(amount);

		ParticleSystem ps = (ParticleSystem)Instantiate(hitSplatter, pointOfImpact, Quaternion.identity);

        ps.transform.LookAt(locationOfBullet);

	}
	

	void ApplyDamage(float damageAmount){
		currentHealth = Mathf.Clamp(currentHealth - damageAmount, 0, maxHealth);

		if(currentHealth == 0){
			StartCoroutine(DestroyDelay(3));
		}

		if(damageAmount > 0){
			timeSinceAttacked = 0;
		}


	}

	void HealthChanged(float currentHealth){
		healthBar.fillAmount = currentHealth / maxHealth;


		if(currentHealth == 0){
			foreach(Behaviour b in disableBehavioursOnDeath){
				b.enabled = false;
			}
			foreach(Renderer r in disableRendersOnDeath){
				r.enabled = false;
			}
			foreach(Collider c in disableCollidersOnDeath){
				c.enabled = false;
			}

			float x, z;
			x = transform.position.x;
			z = transform.position.z;

			//unity is being a buggy pos atm, wont read the SPAWN_DELAY variable from the inspector, makes it hard to test death splatter.
			//	-> however it will read a new value from the inspector if I rename SPAWN_DELAY to something else
			Instantiate(deathSplatter, new Vector3(x, 0.8f, z), Quaternion.Euler(-90, 0, 0));
            //Instantiate(deathSplatter, new Vector3(x, 0.6f, z), Quaternion.Euler(0, 90, 0));
            //Instantiate(deathSplatter, new Vector3(x, 0.6f, z), Quaternion.Euler(0, 180, 0));
            //Instantiate(deathSplatter, new Vector3(x, 0.6f, z), Quaternion.Euler(0, 0, 0));
            //Instantiate(deathSplatter, new Vector3(x, 0.6f, z), Quaternion.Euler(0, 270, 0));
		}


		this.currentHealth = currentHealth;
	}


	void Update(){
		timeSinceAttacked += Time.deltaTime;

		if(timeSinceAttacked >= healthRegenDelay && currentHealth < maxHealth){
			ApplyDamage(healthRegenRate * Time.deltaTime * -1);
		}


		if(timeSinceLastSpawn >= SPAWN_DELAY && currentHealth > 0){
			currentlySpawned++;
			timeSinceLastSpawn = 0;

			GameObject newZombie = Instantiate(zombiePrefabToSpawn.gameObject, transform.position + new Vector3(0, 3, 0), Quaternion.identity);
			newZombie.GetComponent<Zombie>().SetSpawner((MonsterSpawner_Base)this);
			NetworkServer.Spawn(newZombie);
		}
		else if(currentlySpawned < maxSpawned){
			timeSinceLastSpawn += Time.deltaTime;
		}

	}

	IEnumerator DestroyDelay(float delay){
		if(isServer){
			yield return new WaitForSeconds(delay);
			NetworkServer.Destroy(gameObject);
		}
	}

}
