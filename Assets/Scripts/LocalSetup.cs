using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Characters.FirstPerson;

public class LocalSetup : NetworkBehaviour {

    public override void OnStartLocalPlayer() // this is our player
    {
        base.OnStartLocalPlayer();


        RigidbodyFirstPersonController rbc = GetComponent<RigidbodyFirstPersonController>();
        rbc.enabled = true;
        rbc.cam.enabled = true;
        rbc.cam.GetComponent<AudioListener>().enabled = true;
    }

}
