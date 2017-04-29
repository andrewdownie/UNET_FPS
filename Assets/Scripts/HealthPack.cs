using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class HealthPack : NetworkBehaviour {



    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip pickupSound;

    [SerializeField]
    HideGameObject hide;

    void OnTriggerEnter(Collider coll)
    {
        
        if(coll.tag == "Player")
        {

            if(isServer){
                Player player = coll.GetComponent<Player>();
                if(player.Vitals.CanAddHealthpack()){
                    NetworkIdentity playerID = coll.GetComponent<NetworkIdentity>();

                    CmdPickup(playerID);
                }

            }
        }


    }


    [Command]
    void CmdPickup(NetworkIdentity netID){
        RpcPickup(netID);
    }


    [ClientRpc]
    void RpcPickup(NetworkIdentity netID){
        Player player = netID.GetComponent<Player>();

        //Destroy(gameObject, pickupSound.length + 1f);
        player.Vitals.AddHealthpack();
        audioSource.PlayOneShot(pickupSound);
        hide.Hide();

        if(isServer){
            StartCoroutine(DelayedDestroy());
        }
    }


    IEnumerator DelayedDestroy(float soundDuration){
        yield return new WaitForSeconds(soundDuration + 1);
        NetworkServer.Destroy(this.gameObject);
    }


}
