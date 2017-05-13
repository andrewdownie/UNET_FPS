using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class MonsterSpawner_Base : NetworkBehaviour {
	[Command]
	public abstract void CmdAddHealth(float amount);
	[Command]
	public abstract void CmdSubtractHealth(float amount, Vector3 pointOfImpact, Vector3 locationOfBullet);

	public abstract void RemoveSpawnee();

}
