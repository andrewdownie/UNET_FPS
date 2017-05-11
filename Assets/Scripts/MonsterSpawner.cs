using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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
	float spawnDelay = 2;
	[SerializeField]
	float timeSinceLastSpawn;
	[SerializeField]
	Zombie zombiePrefabToSpawn;


	void Start(){
		if(!isServer){
			enabled = false;
		}
	}



	[Command]
	public override void CmdAddHealth(float amount){
		ApplyDamage(-amount);
	}

	[Command]
	public override void CmdSubtractHealth(float amount){
		ApplyDamage(amount);
	}

	void ApplyDamage(float damageAmount){
		currentHealth = Mathf.Clamp(currentHealth - damageAmount, 0, maxHealth);

		if(currentHealth == 0){
			NetworkServer.Destroy(gameObject);
		}

		if(damageAmount > 0){
			timeSinceAttacked = 0;
		}


	}

	void HealthChanged(float currentHealth){
		//TODO: Set health bar here

		this.currentHealth = currentHealth;

	}


	void Update(){
		timeSinceAttacked += Time.deltaTime;

		if(timeSinceAttacked >= healthRegenDelay){
			ApplyDamage(healthRegenRate * Time.deltaTime * -1);
		}

		timeSinceLastSpawn += Time.deltaTime;

		if(timeSinceLastSpawn >= spawnDelay){
			GameObject newZombie = Instantiate(zombiePrefabToSpawn.gameObject, transform.position, Quaternion.identity);
			newZombie.GetComponent<Zombie>().SetSpawner(this);
			NetworkServer.Spawn(newZombie);
			timeSinceLastSpawn = 0;
		}

	}


}
