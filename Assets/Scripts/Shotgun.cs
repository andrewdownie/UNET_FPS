using System.Collections;
using UnityEngine;

public class Shotgun : Gun_Base
{
    ///
    /// 
    ///                                     Instance Variables
    /// 
    /// 
    [SerializeField]
    Transform modelParent;

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





    ///
    ///
    ///                                     Getters
    /// 
    /// 
    public override int BulletsInClip
    {
        get { return bulletsInClip; }
    }
    public override int ClipSize
    {
        get { return clipSize; }
    }
    public override GunType GunType
    {
        get { return gunType; }
    }





    ///
    ///
    ///                                     Unity Methods
    /// 
    /// 
    void Update()
    {
        //TODO: record the time since last shot once, and compare the saved value to
        //      the current value
        timeSinceLastShot += Time.deltaTime;
        timeSinceLastReload += Time.deltaTime;

        if (timeSinceLastShot >= timeBetweenShots)
        {
            canShoot = true;
        }

        if (timeSinceLastReload >= timeBetweenReloads)
        {
            canReload = true;
        }

        if (reloading)
        {
            Reload();
        }
    }


    void OnTriggerEnter(Collider coll)
    {   
        if(player == null){
            this.ETriggerEnterPickup(coll);
        }
    }

    IEnumerator DropGunTimer()
    {
        yield return new WaitForSeconds(1.3f);
        player = null;
        gunSlot = null;
    }





    ///
    ///     
    ///                                     Public Methods
    /// 
    ///
    public override void Drop()
    {
        this.EDrop();
        StartCoroutine(DropGunTimer());
    }  
    
    public override void SetOwningPlayer(Player_Base newOwner)
    {

        if (newOwner != null)
        {

            GunSlot_Base _gunSlot = newOwner.GunSlot;

            player = newOwner;
            gunSlot = _gunSlot;
            gameObject.transform.parent = _gunSlot.transform;

            Destroy(GetComponent<Rigidbody>());
            enabled = true;

            gameObject.EEnableCollidersInChildren(false);



            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(0, 180, 0);
            AlignGun();


        }
    }



    public override void SetVisible(bool visible){
        modelParent.gameObject.EEnableRenderersInChildren(visible);
        muzzleFlash.HideFlash();
    }

    public override void AlignGun()
    {
        transform.EAlignWithCamera(transform.parent.parent, new Vector3(0, 90, 0));
    }

    public override void Shoot(bool firstDown)
    {
        if (!automatic && !firstDown)
        {
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

                for (int i = 0; i < pelletCount; i++)
                {
                    ShotgunPellet bullet = ((GameObject)Instantiate(shotgunPelletPrefab.gameObject)).GetComponent<ShotgunPellet>();
                    bullet.transform.position = bulletSpawnPoint.position;
                    bullet.transform.rotation = bulletSpawnPoint.rotation;
                    bullet.SetHitMarkerCallBack(hitMarkerCallback);
                    bullet.InitBulletTrail(bullet.transform.position);
                    bullet.SetupBulletVelocity(i == 0);

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

    public override void Reload()
    {


        if (bulletsInClip < clipSize)
        {
            if (canShoot && canReload)
            {
                reloading = true;
                int bulletsFromInventory = player.Ammo.Request(gunType, 1);

                if (bulletsFromInventory > 0)
                {
                    player.AudioSource.PlayOneShot(reload);
                    bulletsInClip += bulletsFromInventory;
                    timeSinceLastReload = 0;
                    canReload = false;

                    if(hasAuthority){
                        HUD.SetClipAmmo(bulletsInClip, clipSize);
                    }

                }
                else
                {
                    reloading = false;
                }
            }


        }
        else
        {
            reloading = false;
        }
    }



}
