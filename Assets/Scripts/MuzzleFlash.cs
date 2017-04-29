using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : MuzzleFlash_Base {

	[SerializeField]
	private float defaultFlashDuration = 0.05f;

	[SerializeField]
	private MeshRenderer flash;

	void Start () {
		flash.enabled = false;
	}

	public override void ShowFlash(){
		ShowFlash(defaultFlashDuration);
	}

	public override void HideFlash(){
		flash.enabled = false;
	}
	
	public override void ShowFlash(float duration){
		transform.Rotate(0, Random.Range(-180, 180), 0);
		flash.enabled = true;
		StartCoroutine(TimeFlash());
	}

	IEnumerator TimeFlash(){
		yield return new WaitForSeconds(defaultFlashDuration);
		HideFlash();
	}

	private void ToggleFlash(){
		flash.enabled = !flash.enabled;
	}

}
