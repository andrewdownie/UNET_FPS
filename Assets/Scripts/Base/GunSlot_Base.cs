using UnityEngine;
using System;

public abstract class GunSlot_Base : MonoBehaviour {
 	/// 
    /// 
    ///										Getters 
    /// 
    /// 
	public abstract Gun_Base EquippedGun{get;}
	public abstract Player_Base Player{get;}

	public abstract Gun_Base PrimaryGun{get;}
	public abstract Gun_Base SecondaryGun{get;}





 	/// 
    /// 
    ///										Methods - TODO: not 100% what all of this does anymore, it needs cleaning first and then commenting
    /// 
    /// 

	/// <summary>
	/// TryPickup: the weapon slot will try to take ownership of the gun passed in.
	/// </summary>
	/// <param name="gun">The gun that should be added to the weapon slot<param>
	/// <returns>True if weaponslot now owns the gun. False otherwise.</returns>	
	public abstract bool TryPickup(Gun_Base gun); 

	/// <summary>
	/// Drop: drops the currently selected weapon. The weapon slot no longer owns the 
	///       gun after it is dropped.
	/// </summary>
	public abstract void Drop();

	/// <summary>
	/// PreviousWeapon: Cycles to the previous weapon in the list of all guns the weapon slot has.
	/// </summary>
	public abstract void PreviousWeapon();


	/// <summary>
	/// NextWeapon: cycles to the next weapon in the list of all guns the weapon slot has.
	/// </summary>
	public abstract bool NextWeapon();

	/// <summary>
	/// Reloads the currently equipped weapon.
	/// </summary>
	public abstract void Reload();


	/// <summary>
	/// Shoots the currently equipped weapon.
	/// </summary>
	/// <param name="firstDown">If this is the first frame that the shoot button is down.</param>
	public abstract void Shoot(bool firstDown);

	public abstract void SetCB_AmmoChanged(Action action);




	public abstract void SetSecondary(Gun_Base gun);
	public abstract void SetPrimary(Gun_Base gun);

	public abstract void SetSecondaryActive(bool active);
	public abstract void SetPrimaryActive(bool active);

	public abstract void EquipPrimary();
	public abstract void EquipSecondary();

	public abstract void MeleeAnimation(float meleePercent);

}
