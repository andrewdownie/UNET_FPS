﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Zombie : Zombie_Base {

    [SerializeField]
    private float curHealth = 100, maxHealth = 100;

    [SerializeField]
    private HideGameObject hide;

    [SerializeField]
    private AudioClip zombieDie;

    [SerializeField]
    private ParticleSystem bloodSplatter;
    [SerializeField]
    private ParticleSystem deathSplatter;

    private ZombieAI zombieAI;
    private AudioSource audioSource;

    private Rigidbody rigid;

    [SerializeField]
    private Image healthBar;

    [SerializeField]
    Collider headCollider;

    [SerializeField]
    Collider bodyCollider;


    [SerializeField]
    MonsterSpawner_Base spawner;

	// Use this for initialization
	void Awake () {
        audioSource = GetComponent<AudioSource>();
        rigid = GetComponent<Rigidbody>();
        zombieAI = GetComponent<ZombieAI>();

        healthBar.fillAmount = curHealth / maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void SetSpawner(MonsterSpawner_Base spawner){
        this.spawner = spawner;

        if(zombieAI != null){
            zombieAI.JustGotSpawned(spawner);
        }        
    }


    public override void TakeDamage(float amount, Vector3 hitLocation, Vector3 bulletPosition, Collider c)
    {
        if(c == headCollider){
            amount *= 1.7f;
        }

        curHealth = Mathf.Clamp(curHealth - amount, 0, maxHealth);

        healthBar.fillAmount = curHealth / maxHealth;

        zombieAI.GotAttacked();


        ParticleSystem ps = (ParticleSystem)Instantiate(bloodSplatter, hitLocation, Quaternion.identity);

        ps.transform.LookAt(bulletPosition);




        if (curHealth == 0)
        {
            hide.Hide();
            audioSource.PlayOneShot(zombieDie);
            //Destroy(gameObject, 5);
            StartCoroutine(DestroyDelay(5f));

            rigid.velocity = 0.1f * rigid.velocity;
            

            Instantiate(deathSplatter, new Vector3(hitLocation.x, 1.2f, hitLocation.z), Quaternion.Euler(-90, 0, 0));
            Instantiate(deathSplatter, new Vector3(hitLocation.x, 1f, hitLocation.z), Quaternion.Euler(0, 0, 0));
            Instantiate(deathSplatter, new Vector3(hitLocation.x, 1f, hitLocation.z), Quaternion.Euler(0, 90, 0));
            Instantiate(deathSplatter, new Vector3(hitLocation.x, 1f, hitLocation.z), Quaternion.Euler(0, 180, 0));
            Instantiate(deathSplatter, new Vector3(hitLocation.x, 1f, hitLocation.z), Quaternion.Euler(0, 270, 0));
            
        }

    }


    IEnumerator DestroyDelay(float delay){
        if(isServer){
            spawner.RemoveSpawnee((Zombie_Base)this);
            yield return new WaitForSeconds(delay);
            NetworkServer.Destroy(gameObject);
        }
    }


    public override bool alive{
        get{
            return curHealth > 0;
        }
    }

}
