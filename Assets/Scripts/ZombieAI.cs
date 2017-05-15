using UnityEngine;
using UnityEngine.Networking;
using System.Collections;


public class ZombieAI : MonoBehaviour {
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
    private float attackDamage = -10;


    [SerializeField]
    private AudioClip[] zombieAttackSounds;


    private Player_Base target;


    private bool readyToAttack;


    private Rigidbody rigid;
    private AudioSource audioSource;

    private MonsterSpawner_Base parentSpawner;
    private Vector3 randomSpawnDirection;

	// Use this for initialization
	void Start () {

        rigid = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        readyToAttack = true;

        currentCastRadius = baseCastRadius;
	}
	
    public void GotAttacked()
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
            else if(distanceToTarget < 2 && readyToAttack)
            {
                StartCoroutine(Attack());
            }
        }
        

        if(aiState == ZombieAIState.justGotSpawned){
            rigid.MovePosition(Vector3.MoveTowards(transform.position, transform.forward * moveSpeed, moveSpeed * Time.deltaTime));

            if(Vector3.Distance(transform.position, parentSpawner.transform.position) > 1.6f){
                aiState = ZombieAIState.idle;
            }
        }
    }

    void PlayAttackSound()
    {
        int rnd = Random.Range(0, zombieAttackSounds.Length);
        audioSource.PlayOneShot(zombieAttackSounds[rnd]);
    }

    IEnumerator Attack()
    {
        target.Vitals.ChangeHealth(attackDamage);
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

    public void JustGotSpawned(MonsterSpawner_Base parentSpawner){
        this.parentSpawner = parentSpawner;
        aiState = ZombieAIState.justGotSpawned;
        randomSpawnDirection = new Vector3(Random.value, Random.value);
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