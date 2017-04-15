using UnityEngine;
using System.Collections;

//TODO: replace player reference, to indirect references through GunSlot
public class Gun : Gun_Base {
    [SerializeField]
    LayerMask alignMask;

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
    private bool automatic;

    [SerializeField]
    float timeBetweenShoots = 0.3f;
    float timeSinceLastShot = 1f;


    [Header("Other Setup")]
    [SerializeField]
    private Transform bulletSpawnPoint;

    [SerializeField]
    private Transform shellSpawnPoint;
    
    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private Shell_Base shellPrefab;

    [SerializeField]
    //private ParticleSystem muzzleFlash;
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
	}


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

        if (timeSinceLastShot >= timeBetweenShoots)
        {
            timeSinceLastShot = 0;

            if (bulletsInClip > 0)
            {
            
                ///
                /// Create the bullet
                ///
                player.AudioSource.PlayOneShot(shoot);
                bulletsInClip -= 1;

                Bullet bullet = ((GameObject)Instantiate(bulletPrefab)).GetComponent<Bullet>();
                bullet.transform.position = bulletSpawnPoint.position;
                bullet.SetHitMarkerCallBack(hitMarkerCallback);
                bullet.InitBulletTrail(bullet.transform.position);

                Align(bullet.transform, new Vector3(0, 0, 0));

                /*Transform t = ((ParticleSystem)Instantiate(muzzleFlash)).GetComponent<Transform>();
                t.position = bulletSpawnPoint.position;
                t.rotation = bulletSpawnPoint.rotation;
                t.parent = bulletSpawnPoint;*/

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
        

        if(bulletsInClip < clipSize && timeSinceLastShot >= timeBetweenShoots)
        {
            int bulletsFromInventory = player.Ammo.Request(gunType, clipSize - bulletsInClip);

            if(bulletsFromInventory > 0)
            {
                player.AudioSource.PlayOneShot(reload);
                bulletsInClip += bulletsFromInventory;
                timeSinceLastShot = -(reload.length - timeBetweenShoots);
            }

           
        }
    }

    public override GunType GunType{
            get{return gunType;}
    }

    
}


public enum GunType
{
    sniper,
    shotgun,
    pistol,
    smg,
    assaultRifle
}