using UnityEngine;
using System.Collections.Generic;

public class AmmoPickup : MonoBehaviour {
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
        if (coll.tag == "Player")
        {
            Player player = coll.GetComponent<Player>();


            player.PickupAmmo(typeOfAmmo, numberOfbullets);
            Destroy(gameObject, pickupSound.length + 1f);

            audioSource.PlayOneShot(pickupSound);
            hide.Hide();

        }


    }


}
