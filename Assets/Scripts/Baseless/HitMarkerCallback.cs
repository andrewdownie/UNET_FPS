using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class HitMarkerCallback : MonoBehaviour {
    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    NetworkIdentity gunID;

    [SerializeField]
    private AudioClip hitMarkerSound;


	void Start () {
        HUD.SetHitMarkerVisible(false);
	}
	
    public void ConfirmHit()
    {
        if(!gunID.hasAuthority){
            return;
        }

        //audioSource.clip = hitMarkerSound;
        //audioSource.Play();
        AudioSource.PlayClipAtPoint(hitMarkerSound, transform.position, 1f);
        StartCoroutine(ShowHideHitMarker());
    }

    private IEnumerator ShowHideHitMarker()
    {
        HUD.SetHitMarkerVisible(true);
        yield return new WaitForSeconds(0.15f);
        HUD.SetHitMarkerVisible(false);
    }
}
