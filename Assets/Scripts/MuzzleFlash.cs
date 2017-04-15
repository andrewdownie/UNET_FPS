using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : MuzzleFlash_Base {

	[SerializeField]
	private float defaultFlashDuration = 0.05f;

	[SerializeField]
	private MeshRenderer flash;

	[SerializeField]
	private UninteruptableTimer timer;

	void Start () {
		flash.enabled = false;
	}

	public override void ShowFlash(){
		ShowFlash(defaultFlashDuration);
	}
	
	public override void ShowFlash(float duration){
		transform.Rotate(0, Random.Range(-180, 180), 0);
		ToggleFlash();
		timer.Time(ToggleFlash, duration);
	}

	private void ToggleFlash(){
		flash.enabled = !flash.enabled;
	}

}
