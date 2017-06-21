using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Characters.FirstPerson;

public class LocalSetup : NetworkBehaviour {

    [TargetRpc]
    public void TargetSetupPlayer(NetworkConnection targetPlayer){
        HUD.singleton.GetComponent<Canvas>().enabled = true;

        RigidbodyFirstPersonController rbc = GetComponent<RigidbodyFirstPersonController>();
        rbc.enabled = true;
        rbc.cam.enabled = true;
        rbc.cam.GetComponent<AudioListener>().enabled = true;

        GetComponent<Player>().enabled = true;
 
    }

}
