using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunPellet : Bullet_Base {

    [SerializeField]
    private float maxBulletTrailLength;
    
    [SerializeField]
    private float initialBulletForce = 500;
    [SerializeField]
    private Vector2 forceMultiplierRange = new Vector2(0.5f, 1.5f);
    [SerializeField][Range(0, float.MaxValue)]
    private float bulletSpread = 0.5f;

   // [SerializeField]
    //private LayerMask layerMask;

    [SerializeField]
    private float bulletDamageAmount = 36;

    [SerializeField]
    private LineRenderer lineRenderer;

    bool beingDestroyed;


    private HitMarkerCallback hitMarkerCallback;

    private Rigidbody rigid;

    Vector3 spawnPoint;
    

	//TODO: move this to abstract method in Bullet_Base class
	public void SetupBulletVelocity(bool centerPellet){
        rigid = GetComponent<Rigidbody>();
        Destroy(this, 4);

        Vector3 force;
        force = transform.forward * initialBulletForce * Random.Range(forceMultiplierRange.x, forceMultiplierRange.y);

		if(centerPellet == false){

			float xAccuracy = (1 - bulletSpread) / 2;
			float yAccuracy = xAccuracy / 6;

			force += transform.right * Random.Range(-xAccuracy, xAccuracy);
			force += transform.up * Random.Range(-yAccuracy, yAccuracy);

		}

        rigid.AddForce(force, ForceMode.Impulse);
        lineRenderer.enabled = true;
        Destroy(gameObject, 10f);
	}


    void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Zombie")
        {

            RaycastHit hit;
            Physics.Raycast(transform.position + transform.forward * -2, transform.forward, out hit);

            Zombie zombie = collision.gameObject.GetComponent<Zombie>();
            zombie.TakeDamage(bulletDamageAmount, hit.point, transform.position);

            hitMarkerCallback.ConfirmHit();
        }

        enabled = false;
        lineRenderer.enabled = false;
    }





    public override void SetHitMarkerCallBack(HitMarkerCallback callback)
    {
        hitMarkerCallback = callback;
    }

    public override void InitBulletTrail(Vector3 spawnPoint){
        this.spawnPoint = spawnPoint;
    }

    void Update(){
        SetTrailPosition();
    }
    private void SetTrailPosition(){
        ///
        /// Make the bullet trail go back toward its spawn point
        ///
        float distance = Vector3.Distance(spawnPoint, transform.position);
        float clamped = Mathf.Clamp(distance, 0, maxBulletTrailLength);
        float percent = clamped / distance;

        Vector3 endPoint = Vector3.Lerp(transform.position, spawnPoint, percent);

        lineRenderer.SetPosition(1, transform.position);
        lineRenderer.SetPosition(0, endPoint );
    }

}
