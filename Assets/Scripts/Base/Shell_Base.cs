using UnityEngine;

public abstract class Shell_Base : MonoBehaviour {

    /// 
    /// 
    ///                                     Methods
    /// 
	/// 

	/// <summary>
	/// Adds velocity to a shell to make it appear to fly out of the gun, used to add the players movement velocity to the bullet. 
	/// </summary>
	/// <param name="velocity">The amount of velocity to add to the shell.</param>
	public abstract void AddVelocity(Vector3 velocity);
}
