using UnityEngine;
using UnityEngine.Networking;

public abstract class MonsterSpawner_Base : NetworkBehaviour {
	
    /// 
    /// 
    ///                                     Methods
    /// 
    /// 

	/// <summary>
	/// Adds health to the monster spawner.
	/// </summary>
	/// <param name="amount">The amount of health to add.</param>
	[Command]
	public abstract void CmdAddHealth(float amount);

	/// <summary>
	/// Subtracts health from the monster spawner, and shows a visual effect where it took damage.
	/// </summary>
	/// <param name="amount">The amount of health to subtract.</param>
	/// <param name="pointOfImpact">Where the bullet hit.</param>
	/// <param name="locationOfBullet">Where the bullet is (pointOfImpact and locationOfBullet work together to give the visual effect a direction)</param>
	[Command]
	public abstract void CmdSubtractHealth(float amount, Vector3 pointOfImpact, Vector3 locationOfBullet);

	/// <summary>
	/// Removes a monster that was spawned from this spawner. Allows another monster to be spawned from this spawner.
	/// <param name="spawnee">The monster that should be removed from the spawners current list of spawnees.</param>
	/// /// </summary>
	public abstract void RemoveSpawnee(Zombie_Base spawnee);

}
