﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Vitals : Vitals_Base {

	[SerializeField]
	float curHealth = 100, maxHealth = 200;

	[SerializeField]
	float curStamina, maxStamina;

	[SerializeField]
	float curMana, maxMana;

	[SerializeField]
	bool hasHealthpack;

	private AudioSource audioSource;

	[SerializeField]
	private AudioClip healSound;

	public override void OnStartAuthority(){

		if(hasAuthority){
			HUD.SetHealth(curHealth, maxHealth);
			HUD.SetHealthPackVisible(hasHealthpack);

			HUD.SetRespawnButtonVisible(curHealth == 0);

			audioSource = GetComponent<AudioSource>();
			if(audioSource == null){
				Debug.LogWarning("Vitals: audio source not found.");
			}
		}
	}

	public override void ChangeHealth(float amount){
		curHealth = Mathf.Clamp(curHealth + amount, 0, maxHealth);

		if(hasAuthority){
			HUD.SetHealth(curHealth, maxHealth);
			HUD.SetRespawnButtonVisible(curHealth == 0);
		}	


	}

	public override void ChangeStamina(float amount){
		curStamina = Mathf.Clamp(curStamina + amount, 0, maxStamina);
		//TODO: set HUD
	}

	public override void ChangeMana(float amount){
		curMana = Mathf.Clamp(curMana + amount, 0, maxMana);
		//TODO: set HUD
	}

	public override void AddHealthpack(){
		hasHealthpack = true;
		if(hasAuthority){
			HUD.SetHealthPackVisible(true);
		}
	}

	public override void UseHealthpack(){
		if(curHealth == maxHealth){
			return;
		}

		hasHealthpack = false;
		ChangeHealth(120);
		audioSource.PlayOneShot(healSound);

		if(hasAuthority){
			HUD.SetHealthPackVisible(false);
			HUD.SetHealth(curHealth, maxHealth);
		}
	}

	public override bool CanAddHealthpack(){
		return !hasHealthpack;	
	}

	public override bool HasHealthpack(){
		return hasHealthpack;
	}

}
