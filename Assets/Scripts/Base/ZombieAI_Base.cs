using UnityEngine;

//TODO: significantly more work on zombies is needed before cleaning and then commenting will occur
public abstract class ZombieAI_Base : MonoBehaviour {
	public abstract void JustGotSpawned(MonsterSpawner_Base parentSpawner);
	public abstract void GotAttacked();
}
