using UnityEngine;
using UnityEngine.Networking;

//TODO: significantly more work on zombies is needed before cleaning and then commenting will occur
public abstract class Zombie_Base : NetworkBehaviour {
	public abstract void TakeDamage(float amount, Vector3 hitLocation, Vector3 bulletPosition);
	public abstract void SetSpawner(MonsterSpawner_Base spawner);


	public abstract bool alive{get;}
}
