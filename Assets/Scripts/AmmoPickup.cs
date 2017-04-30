using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Collections;

public class AmmoPickup : NetworkBehaviour {
    [SerializeField]
    private GunType typeOfAmmo;

    [SerializeField]
    private int numberOfbullets;


    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip pickupSound;

    [SerializeField]
    HideGameObject hide;


    void OnTriggerEnter(Collider coll)
    {
        if (isServer && coll.tag == "Player")
        {
            CmdPickupAmmo(coll.GetComponent<NetworkIdentity>());
        }

    }

    [Command]
    void CmdPickupAmmo(NetworkIdentity netID){
        RpcPickupAmmo(netID);
    } 

    [ClientRpc]
    void RpcPickupAmmo(NetworkIdentity netID){

        Player player = netID.GetComponent<Player>();
        Debug.LogError(player.name + " picked up ammo");


        player.PickupAmmo(typeOfAmmo, numberOfbullets);
        Destroy(gameObject, pickupSound.length + 1f);

        audioSource.PlayOneShot(pickupSound);
        hide.Hide();

        if(isServer){
            StartCoroutine(DelayedDestroy(pickupSound.length + 1f));
        }

    }

    IEnumerator DelayedDestroy(float soundDuration){
        yield return new WaitForSeconds(soundDuration + 1);
        NetworkServer.Destroy(this.gameObject);
    }


}
