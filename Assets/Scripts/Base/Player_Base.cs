using UnityEngine;
using UnityEngine.Networking;

public abstract class Player_Base : NetworkBehaviour {
    /// 
    /// 
    ///                                     Getters
    /// 
    /// 

	/// <summary>
	/// A reference to the players audio source.
	/// </summary>
	public abstract AudioSource AudioSource{get;}

	/// <summary>
	/// A reference to the players vitals.
	/// </summary>
	public abstract Vitals_Base Vitals{get;}

	/// <summary>
	/// A reference to the players GunSlot.
	/// </summary>
	public abstract GunSlot_Base GunSlot{get;} 

	/// <summary>
	/// A reference to the players AmmoInventory.
	/// </summary>
	public abstract AmmoInventory_Base Ammo{get;}

	/// <summary>
	/// A reference to the players RigidBody.
	/// </summary>
	public abstract Rigidbody Rigidbody{get;}





    /// 
    /// 
    ///                                     Methods
    /// 
	/// 

	/// <summary>
	/// Adds ammo to the players AmmoInventory.
	/// </summary>
	/// <param name="gunType">The type of gun that the ammo is.</param>
	/// <param name="amount">The amount of bullets to add.</param>
	public abstract void PickupAmmo(GunType gunType, int amount);

	/// <summary>
	/// Connects and sets this player as the owner of a weapon. The gun occupies the players primary weapon slot.
	/// </summary>
	/// <param name="primaryWeapon">The Network ID of the weapon to equip.</param>
	[ClientRpc]
	public abstract void RpcConnectPrimary(NetworkIdentity primaryWeapon);

	/// <summary>
	/// Connects and sets this player as the owner of a weapon. The gun occupies the players secondary weapon slot.
	/// </summary>
	/// <param name="primaryWeapon">The Network ID / reference to the weapon.</param>
	[ClientRpc]
	public abstract void RpcConnectSecondary(NetworkIdentity secondaryWeapon);

	/// <summary>
	/// Set the players gameObject, and the GUI text field for their name.
	/// </summary>
	/// <param name="playerName">The name of the player.</param>
	[ClientRpc]
	public abstract void RpcSetPlayerName(string playerName);
	
	/// <summary>
	/// Method that tells the player that they picked up a gun. Updates related to picking up a gun should happen here.
	/// </summary>
	public abstract void GunPickedUp();


}
