using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class Player_Base : NetworkBehaviour {
	public abstract void PickupAmmo(GunType gunType, int amount);


	public abstract bool TryPickupGun(Gun_Base gun);


	public abstract AudioSource AudioSource{get;}


	public abstract Vitals_Base Vitals{get;}
	public abstract GunSlot_Base GunSlot{get;} 
	public abstract AmmoInventory_Base Ammo{get;}


	public abstract Rigidbody Rigidbody{get;}

}
