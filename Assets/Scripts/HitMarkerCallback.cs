using UnityEngine;
using System.Collections;

public class HitMarkerCallback : MonoBehaviour {

    private AudioSource audioSource;

    [SerializeField]
    private AudioClip hitMarkerSound;


	void Start () {
        audioSource = GetComponent<AudioSource>();
        HUD.SetHitMarkerVisible(false);
	}
	
    public void ConfirmHit()
    {
        audioSource.PlayOneShot(hitMarkerSound);
        StartCoroutine(ShowHideHitMarker());
    }

    private IEnumerator ShowHideHitMarker()
    {
        HUD.SetHitMarkerVisible(true);
        yield return new WaitForSeconds(0.15f);
        HUD.SetHitMarkerVisible(false);
    }
}
