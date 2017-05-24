using UnityEngine.Networking;
using UnityEngine;

/// <summary>
/// A bag of extension methods for the class: Gun_Base
/// Used to implement common methods for this virtual interface / Base class
/// </summary>
public static class Gun_Base_Extension{
	/// <summary>
	/// Disables the gun script, adds a Rigidbody, and enables colliders in children.
	/// </summary>
	/// <param name="gun"></param>
    public static void EDrop(this Gun_Base gun)
    {
        gun.enabled = false;
        Rigidbody rb = gun.gameObject.AddComponent<Rigidbody>();
        rb.useGravity = true;
        gun.gameObject.EEnableCollidersInChildren(true);
    }


    /// <summary>
    /// The server will set the gun to be owned by the player that walked over (collided with) the gun.
    /// </summary>
    /// <param name="gun">The gun that will be picked up by the player.</param>
    /// <param name="coll">The collider that caused the OnTriggerEnter event to fire.</param>
    public static void ETriggerEnterPickup(this Gun_Base gun, Collider coll){
        if (gun.isServer && coll.tag == "Player")
        {
            Player_Base _player = coll.GetComponent<Player_Base>();
            GunSlot_Base _gunSlot = _player.GunSlot;


            if (_gunSlot != null && _gunSlot.TryPickup(gun))
            {
                NetworkIdentity newOwnerID = _player.GetComponent<NetworkIdentity>();
                NetworkIdentity gunID = gun.GetComponent<NetworkIdentity>();

                Net_Manager.instance.SetPrimary(newOwnerID, gunID);
            }
        }
    }


    public static void ESetOwningPlayer(this Gun_Base gun){

    }

}
