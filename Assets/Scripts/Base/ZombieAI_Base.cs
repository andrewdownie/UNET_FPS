using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ZombieAI_Base : MonoBehaviour {
	public abstract void JustGotSpawned(MonsterSpawner_Base parentSpawner);
	public abstract void GotAttacked();
}
