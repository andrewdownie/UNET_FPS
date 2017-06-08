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
	float curStamina = 100, maxStamina = 100;

	[SerializeField][SyncVar]
	float curMana = 100, maxMana = 100;

	[SerializeField][SyncVar]
	bool hasHealthpack;

	[SerializeField]
	private AudioSource audioSource;

	[SerializeField]
	private AudioClip healSound;

	[SerializeField]
	private Image healthBar, healthBarBG;

	private Player_Base player;


	[SerializeField]
	AudioClip[] painSounds;

	float timeSinceLastAttack;
	[SerializeField]
	float damageIndicatorTime = 0.6f;


	void Update(){


		if(timeSinceLastAttack >= damageIndicatorTime){
			HUD.SetDamageDirectionIndicatorVisible(false);
		}
		else{
			HUD.SetDamageDirectionIndicatorAlpha((damageIndicatorTime - timeSinceLastAttack)/damageIndicatorTime);
			timeSinceLastAttack += Time.deltaTime;
		}
	}

	float RelativeAngleOfAttack(Transform attacker){
		Vector3 dir = attacker.position - transform.position;
		dir = transform.InverseTransformDirection(dir);
		float angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
		return angle - 90;
	}

	//TODO: even if this works 100% correctly, it's not setup as cleanly as it could be
	//      	-> ChangeHealth will be called by all clients, but not do anything because of the syncvar (I think...)
	//          -> Actually this might be the normal way to use syncvar's, while the clients will have unused work to do, its work they would have to do in single player anyway, and it saves the server a little work
	void UpdateHealthGUI(float newCurHealth){
		healthBar.fillAmount = newCurHealth / maxHealth;


		bool dead = (newCurHealth == 0);
		bool previouslyDead = (this.curHealth == 0);
		if(hasAuthority){
			HUD.SetHealth(newCurHealth, maxHealth);
			HUD.SetDeathMessageVisible(dead);
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


		this.curHealth = newCurHealth;
	}

	public override void OnStartAuthority(){

		if(hasAuthority){
			HUD.SetHealth(curHealth, maxHealth);
			HUD.SetHealthPackVisible(hasHealthpack);

			HUD.SetDeathMessageVisible(curHealth == 0);

		}
	}

	void Start(){
		timeSinceLastAttack = 0;

		healthBar.fillAmount = curHealth / maxHealth;

		if(audioSource == null){
			Debug.LogWarning("Vitals: audio source not found.");
		}

		player = GetComponent<Player_Base>();
	}

	public override void DamageHealth(float amount, Transform attacker){
		curHealth = Mathf.Clamp(curHealth - amount, 0, maxHealth);

		if(amount > 0){
			PlayPainSound();

			timeSinceLastAttack = 0;

			if(attacker != null){
				HUD.SetDamageDirectionIndicatorRotation(RelativeAngleOfAttack(attacker));
			}

		}

	}

	public override void HealHealth(float amount){
		curHealth = Mathf.Clamp(curHealth + amount, 0, maxHealth);

	}

	void PlayPainSound(){
		int rnd = Random.Range(0, painSounds.Length);
		audioSource.clip = painSounds[rnd];
		audioSource.pitch = Random.Range(0.9f, 1.1f);
		audioSource.Play();
	}

	public override void Revive(){
		HealHealth(70);
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
		HealHealth(120);
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
