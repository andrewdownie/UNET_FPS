using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class Vitals : Vitals_Base {
	[SerializeField]
	Collider reviveCollider;

	[SerializeField]
	RigidbodyFirstPersonController playerControllerScript;	

	[SerializeField][SyncVar(hook="UpdateHealthGUI")]
	float curHealth = 100;
	[SerializeField][SyncVar]
	float maxHealth = 200;

	[SerializeField][SyncVar]
	float curStamina, maxStamina;

	[SerializeField][SyncVar]
	float curMana, maxMana;

	[SerializeField][SyncVar]
	bool hasHealthpack;

	private AudioSource audioSource;

	[SerializeField]
	private AudioClip healSound;

	[SerializeField]
	private Image healthBar, healthBarBG;

	private Player_Base player;

	//TODO: even if this works 100% correctly, it's not setup as cleanly as it could be
	//      	-> ChangeHealth will be called by all clients, but not do anything because of the syncvar (I think...)
	//          -> Actually this might be the normal way to use syncvar's, while the clients will have unused work to do, its work they would have to do in single player anyway, and it saves the server a little work
	void UpdateHealthGUI(float curHealth){
		healthBar.fillAmount = curHealth / maxHealth;


		bool dead = (curHealth == 0);
		bool previouslyDead = (this.curHealth == 0);
		if(hasAuthority){
			HUD.SetHealth(curHealth, maxHealth);
			playerControllerScript.enabled = !dead;
			player.enabled = !dead;

		}

		//
		// Handle the transition from dead to alive and vice versa
		//
		if(dead){
			if(!previouslyDead){
				transform.Rotate(0, 0, 90);
				player.GunSlot.EquippedGun.SetVisible(false);
				healthBarBG.enabled = false;
			}
		}
		else{
			if(previouslyDead){
				transform.Rotate(0, 0, -90);
				player.GunSlot.EquippedGun.SetVisible(true);
				healthBarBG.enabled = true;
			}
		}


		this.curHealth = curHealth;
	}

	public override void OnStartAuthority(){

		if(hasAuthority){
			HUD.SetHealth(curHealth, maxHealth);
			HUD.SetHealthPackVisible(hasHealthpack);

			HUD.SetDeathMessageVisible(curHealth == 0);


		}
	}

	void Start(){
		healthBar.fillAmount = curHealth / maxHealth;

		audioSource = GetComponent<AudioSource>();
		if(audioSource == null){
			Debug.LogWarning("Vitals: audio source not found.");
		}

		player = GetComponent<Player_Base>();
	}

	public override void ChangeHealth(float amount){
		curHealth = Mathf.Clamp(curHealth + amount, 0, maxHealth);

	}

	public override void Revive(){
		ChangeHealth(70);
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

	public override bool alive{
		get{return curHealth > 0;}
	}

	public override bool dead{
		get{return curHealth == 0;}
	}

}
