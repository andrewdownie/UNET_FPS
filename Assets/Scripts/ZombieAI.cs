using UnityEngine;
using UnityEngine.Networking;
using System.Collections;


public class ZombieAI : ZombieAI_Base {
    [SerializeField]
    NetworkIdentity zombieID;
    [SerializeField]
    private ZombieAIState aiState = ZombieAIState.idle;

    [SerializeField]
    private LayerMask playerLayerMask;

    [SerializeField]
    private float baseCastRadius = 13;
    [SerializeField]
    private float curiousDuration;
    [SerializeField]
    private float curiousRadius; //When the zombie gets shot, and is looking for the player 
    
    private float currentCastRadius;

    [SerializeField]
    private float moveSpeed = 4;

    [SerializeField]
    private float attackSpeed = 1;
    [SerializeField]
    private float attackDamage = 15;


    [SerializeField]
    private AudioClip[] zombieAttackSounds;


    private Player_Base target;

    [SerializeField]
    Zombie_Base zombieBase;


    private bool readyToAttack;


    private Rigidbody rigid;
    private AudioSource audioSource;

    private MonsterSpawner_Base parentSpawner;


    float timeSinceLastStep;
    float timeUntilNextStep = 0.5f;
    [SerializeField]
    float STEP_TIME = 1;
    [SerializeField]
    AudioClip[] stepSounds;

	// Use this for initialization
	void Start () {

        rigid = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        readyToAttack = true;

        currentCastRadius = baseCastRadius;
	}
	
    public override void GotAttacked()
    {
        if(aiState == ZombieAIState.idle || aiState == ZombieAIState.wandering || aiState == ZombieAIState.justGotSpawned)
        {
            StopCoroutine(BecomeCurious());
            StartCoroutine(BecomeCurious());
        }
    }

    void Update()
    {
        if(zombieID.isServer == false){
            return;
        }



        if(aiState == ZombieAIState.idle || aiState == ZombieAIState.wandering)
        {
            RaycastHit[] hitInfo;

            hitInfo = Physics.SphereCastAll(transform.position, currentCastRadius, transform.forward, playerLayerMask);

            foreach(RaycastHit rc in hitInfo){
                if(rc.transform != null)
                {
                    Player_Base target = rc.transform.gameObject.GetComponent<Player>();
                    if(target != null && target.Vitals.alive){
                        this.target = target;
                        aiState = ZombieAIState.chasing;
                        break;
                    } 
                }
            }

        }



        if(aiState == ZombieAIState.chasing)
        {
            timeSinceLastStep += Time.deltaTime;

            if(timeSinceLastStep >= timeUntilNextStep){
                timeSinceLastStep = 0;
                PlayStepSound();
                timeUntilNextStep = Random.Range(STEP_TIME - STEP_TIME * 0.1f, STEP_TIME + STEP_TIME * 0.1f);
            }

            if(target == null){
                aiState = ZombieAIState.idle;
                return; 
            }

            if(target.Vitals.dead){
                target = null;
                BecomeCurious();
                return;
            }

            rigid.MovePosition(Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime));

            float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

            if (distanceToTarget > currentCastRadius * 1.4f)
            {
                aiState = ZombieAIState.idle;
            }
            else if(distanceToTarget < 1.8f && readyToAttack && zombieBase.alive)
            {
                StartCoroutine(Attack());
            }
        }
        

        if(aiState == ZombieAIState.justGotSpawned){
            rigid.MovePosition(Vector3.MoveTowards(transform.position, transform.position + (transform.forward * 1000), moveSpeed * Time.deltaTime));

            if(Vector3.Distance(transform.position, parentSpawner.transform.position) > 3.0f){
                aiState = ZombieAIState.idle;
            }
        }


    }

    void PlayAttackSound()
    {
        int rnd = Random.Range(0, zombieAttackSounds.Length);
        audioSource.PlayOneShot(zombieAttackSounds[rnd]);
    }

    void PlayStepSound(){
        int rnd = Random.Range(0, stepSounds.Length);
        AudioSource.PlayClipAtPoint(stepSounds[rnd], transform.position);
    }

    IEnumerator Attack()
    {
        target.Vitals.DamageHealth(attackDamage, transform);
        PlayAttackSound();
        readyToAttack = false;
        yield return new WaitForSeconds(attackSpeed);
        readyToAttack = true;
    }

    IEnumerator BecomeCurious()
    {
        currentCastRadius = curiousRadius;
        yield return new WaitForSeconds(curiousDuration);
        currentCastRadius = baseCastRadius;
    }

    public override void JustGotSpawned(MonsterSpawner_Base parentSpawner){
        this.parentSpawner = parentSpawner;
        aiState = ZombieAIState.justGotSpawned;

        transform.rotation = parentSpawner.transform.rotation * Quaternion.Euler(0, -45, 0);
    }
}

public enum ZombieAIState
{
    idle,
    wandering,
    chasing,
    dying,
    justGotSpawned,
}