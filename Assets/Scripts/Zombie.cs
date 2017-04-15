using UnityEngine;
using System.Collections;

public class Zombie : MonoBehaviour {

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

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
        zombieAI = GetComponent<ZombieAI>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void TakeDamage(float amount, Vector3 hitLocation, Vector3 bulletPosition)
    {
        curHealth = Mathf.Clamp(curHealth - amount, 0, maxHealth);

        zombieAI.GotAttacked();


        ParticleSystem ps = (ParticleSystem)Instantiate(bloodSplatter, hitLocation, Quaternion.identity);

        ps.transform.LookAt(bulletPosition);



        if (curHealth == 0)
        {
            hide.Hide();
            audioSource.PlayOneShot(zombieDie);
            Destroy(gameObject, 5);

            

            Instantiate(deathSplatter, new Vector3(hitLocation.x, 1, hitLocation.z), Quaternion.Euler(-90, 0, 0));
            Instantiate(deathSplatter, new Vector3(hitLocation.x, 0.8f, hitLocation.z), Quaternion.Euler(0, 0, 0));
            Instantiate(deathSplatter, new Vector3(hitLocation.x, 0.8f, hitLocation.z), Quaternion.Euler(0, 90, 0));
            Instantiate(deathSplatter, new Vector3(hitLocation.x, 0.8f, hitLocation.z), Quaternion.Euler(0, 180, 0));
            Instantiate(deathSplatter, new Vector3(hitLocation.x, 0.8f, hitLocation.z), Quaternion.Euler(0, 270, 0));
            
        }

        
        
        

        
    }


}
