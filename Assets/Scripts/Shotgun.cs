using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: replace player reference, to indirect references through GunSlot
public class Shotgun : Gun_Base {
    [SerializeField]
    private LayerMask alignMask;

    [SerializeField]
    private GunType gunType;

    private Player_Base player;
    private GunSlot_Base gunSlot;

    [Header("Sound Clips")]
    [SerializeField]
    private AudioClip shoot, reload, outOfAmmo;

    
    [Header("Weapon Firing")]
    [SerializeField]
    private int clipSize = 5;
    [SerializeField]
    private int bulletsInClip = 5;

	[SerializeField]
	private int pelletCount = 10;


    [SerializeField]
    private bool automatic;

    [SerializeField]
    float timeBetweenShots = 0.3f;
    float timeSinceLastShot = 1f;

	[SerializeField]
	private float timeBetweenReloads;
	private float timeSinceLastReload;

	bool canShoot, canReload, reloading;


    [Header("Other Setup")]
    [SerializeField]
    private Transform bulletSpawnPoint;

    [SerializeField]
    private Transform shellSpawnPoint;
    
    [SerializeField]
    private ShotgunPellet shotgunPelletPrefab;

    [SerializeField]
    private Shell_Base shellPrefab;

    [SerializeField]
    private MuzzleFlash_Base muzzleFlash;

    [SerializeField]
    HitMarkerCallback hitMarkerCallback;
    

    void Start()
    {
        FindGunSlotAndPlayer();
    }

    void FindGunSlotAndPlayer()
    {
        Transform parent = transform.parent;

        if(parent == null)
        {
            enabled = false;
            return;
        }

        GunSlot weaponSlot = parent.GetComponent<GunSlot>();

        if(weaponSlot != null)
        {
            this.gunSlot = weaponSlot;

            Player player = weaponSlot.Player;

            if(player != null)
            {
                this.player = player;
            }
        }
        
    }

    public override int BulletsInClip{
        get{return bulletsInClip;}
    }

    public override int ClipSize{
        get{return clipSize;}
    }
	


	// Update is called once per frame
	void Update () {
        //TODO: record the time since last shot once, and compare the saved value to
        //      the current value
        timeSinceLastShot += Time.deltaTime;
		timeSinceLastReload += Time.deltaTime;

		if(timeSinceLastShot >= timeBetweenShots){
			canShoot = true;
		}

		if(timeSinceLastReload >= timeBetweenReloads){
			canReload = true;
		}

		if(reloading){
			Reload();
		}
	}



	//TODO: instead of adding a rigid body, have a rigid body on by default, and toggle 'isKinematic' (don't actually know if this will do what I want, but it would be much better than what I have if it does)
    public override void Drop()
    {
        enabled = false;

        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = true;

        Collider[] colliders = GetComponents<Collider>();
        foreach (Collider c in colliders)
        {
            c.enabled = true;
        }

        StartCoroutine(DropGunTimer());
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Player" && player == null)
        {
            Debug.Log("Collided with: " + coll.name);

            Player_Base _player = coll.GetComponent<Player_Base>();
            if (_player != null)
            {
                GunSlot_Base _gunSlot = _player.GunSlot;

                if (_gunSlot != null && _gunSlot.TryPickup(this))
                {
                    player = _player;
                    gunSlot = _gunSlot;
                    gameObject.transform.parent = _gunSlot.transform;

                    Destroy(GetComponent<Rigidbody>());
                    enabled = true;

                    Collider[] colliders = GetComponents<Collider>();
                    foreach (Collider c in colliders)
                    {
                        c.enabled = false;
                    }

                    transform.localPosition = Vector3.zero;
                    transform.localRotation = Quaternion.Euler(0, 180, 0);
                    AlignGun();
                }
            }

        }
    }

    IEnumerator DropGunTimer(){
        yield return new WaitForSeconds(1.3f);
        player = null;
        gunSlot = null;
    } 


    public override void Align(Transform alignObject, Vector3 additionalRotation){

        if(player != null){
            Transform camera = transform.parent.parent;
            RaycastHit hit;
            //Debug.DrawRay(camera.position, camera.forward * 1000, Color.red, 0.1f);
            Physics.Raycast(camera.position, camera.forward * 1000, out hit, 1000f, alignMask);
            
            Vector3 point = hit.point;

            if(point == Vector3.zero){
                point = camera.forward * 100000;
            }
            alignObject.LookAt(point);
            alignObject.Rotate(additionalRotation);
            
        }
    }


    public override void AlignGun(){
        if(player != null){
            Transform camera = transform.parent.parent;
            Vector3 point = camera.position + (camera.forward * 10000);

            transform.LookAt(point);
            transform.Rotate(new Vector3(0, 90, 0));
        }

    }


    public override void Shoot(bool firstDown){
        if(!automatic && !firstDown){
            return;
        }

        if (canShoot)
        {
            timeSinceLastShot = 0;
			canShoot = false;
			canReload = false;
			reloading = false;

            if (bulletsInClip > 0)
            {
            
                ///
                /// Create the bullets
                ///
                player.AudioSource.PlayOneShot(shoot);
                bulletsInClip -= 1;

				for(int i = 0; i < pelletCount; i++){
					ShotgunPellet bullet = ((GameObject)Instantiate(shotgunPelletPrefab.gameObject)).GetComponent<ShotgunPellet>();
					bullet.transform.position = bulletSpawnPoint.position;
					bullet.transform.rotation = bulletSpawnPoint.rotation;
					bullet.SetHitMarkerCallBack(hitMarkerCallback);
                    bullet.InitBulletTrail(bullet.transform.position);
                    bullet.SetupBulletVelocity(i == 0);

                    //Align(bullet.transform, bulletSpawnPoint.rotation.eulerAngles);
				}

                muzzleFlash.ShowFlash();

                ///
                /// Create the shell
                ///
                Shell_Base shell = (Shell_Base)Instantiate(shellPrefab, shellSpawnPoint.position, transform.rotation * shellSpawnPoint.localRotation);
                shell.AddVelocity(player.Rigidbody.velocity);
           
            
            }
            else
            {
                player.AudioSource.PlayOneShot(outOfAmmo);
            }

        }
    }

    public override void Reload(){
        

        if(bulletsInClip < clipSize)
        {
			if(canShoot && canReload){
				reloading = true;
				int bulletsFromInventory = player.Ammo.Request(gunType, 1);

				if(bulletsFromInventory > 0)
				{
					player.AudioSource.PlayOneShot(reload);
					bulletsInClip += bulletsFromInventory;
					timeSinceLastReload = 0;
					canReload = false;
					HUD.SetClipAmmo(bulletsInClip, clipSize);
				}
				else{
					reloading = false;
				}
			}

           
        }
		else{
			reloading = false;
		}
    }

    public override GunType GunType{
        get{return gunType;}
    }

    
}
