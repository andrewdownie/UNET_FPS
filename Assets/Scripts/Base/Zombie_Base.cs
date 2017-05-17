using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class Zombie_Base : NetworkBehaviour {
	public abstract void TakeDamage(float amount, Vector3 hitLocation, Vector3 bulletPosition);
	public abstract void SetSpawner(MonsterSpawner_Base spawner);


	public abstract bool alive{get;}
}
