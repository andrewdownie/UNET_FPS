using UnityEngine.Networking;
using UnityEngine;

/// <summary>
/// A bag of extension methods for the class: Gun_Base
/// Used to implement common methods for this virtual interface / Base class
/// </summary>
public static class Gun_Base_Extension
{
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
    public static void ETriggerEnterPickup(this Gun_Base gun, Collider coll)
    {
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


    /// <summary>
    /// Sets the owner of this gun. Destroys Rigidbody, enables gun script, enables colliders, aligns gun.
    /// </summary>
    /// <param name="gun">The gun to set the owner of, and to configure to be used by a player.</param>
    /// <param name="sentOwner">The player we want to try to set as the owner of the gun.</param>
    /// <returns>Returns the sentOwner, set the guns player reference to this return.</returns>
    public static Player_Base ESetOwningPlayer(this Gun_Base gun, Player_Base sentOwner)
    {
        if (sentOwner != null)
        {
            gun.transform.parent = sentOwner.GunSlot.transform;

            GameObject.Destroy(gun.GetComponent<Rigidbody>());
            gun.enabled = true;

            gun.gameObject.EEnableCollidersInChildren(false);

            gun.transform.localPosition = Vector3.zero;
            gun.transform.localRotation = Quaternion.Euler(0, 180, 0);
            gun.AlignGun();

        }

        
        return sentOwner;
    }

}
