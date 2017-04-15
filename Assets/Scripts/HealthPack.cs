using UnityEngine;
using System.Collections.Generic;

public class HealthPack : MonoBehaviour {



    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip pickupSound;

    [SerializeField]
    HideGameObject hide;

    void OnTriggerEnter(Collider coll)
    {
        
        /// TODO: this needs to be changed to add to the players health pack slot if the slot it empty.
        if(coll.tag == "Player")
        {
            Player combat = coll.GetComponent<Player>();


            if (combat.Vitals.AddHealthpack())
            {
                Destroy(gameObject, pickupSound.length + 1f);

                audioSource.PlayOneShot(pickupSound);

                hide.Hide();
            }
            
        }


    }
}
