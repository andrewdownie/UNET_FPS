using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Gun : Gun_Base
{
    ///
    /// 
    ///                                     Instance Variables
    /// 
    ///
    [SerializeField]
    Transform modelParent;
    [SerializeField]
    LayerMask alignMask;

    [SerializeField]
    private GunType gunType;

    [SerializeField]
    private Player_Base player;

    [Header("Sound Clips")]
    [SerializeField]
    private AudioClip shoot, reload, outOfAmmo;


    [Header("Weapon Firing")]
    [SerializeField]
    private int clipSize = 5;
    [SerializeField]
    [SyncVar]
    private int bulletsInClip = 5;


    [SerializeField]
    private bool automatic;

    [SerializeField]
    float timeBetweenShots = 0.3f;
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
        //      the current value (then this wouldn't have to eat cpu time in the update method)
        timeSinceLastShot += Time.deltaTime;
    }


    IEnumerator DropGunTimer()
    {
        //TODO: this needs to be networked, or there is a small chance an invalid state can happen locally
        yield return new WaitForSeconds(1.3f);
        player = null;
    }


    void OnTriggerEnter(Collider coll)
    {
        if (player == null)
        {
            this.ETriggerEnterPickup(coll);
        }
    }






    ///
    /// 
    ///                                     Public Methods
    /// 
    ///
    public override void SetOwningPlayer(Player_Base newOwner)
    {
        if (this.ESetOwningPlayer(newOwner))
        {
            player = newOwner;
        }
    }


    public override void SetVisible(bool visible)
    {
        modelParent.gameObject.EEnableRenderersInChildren(visible);
        muzzleFlash.HideFlash();
    }


    public override void Drop()
    {
        this.EDrop();
        StartCoroutine(DropGunTimer());
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

        if (timeSinceLastShot >= timeBetweenShots)
        {
            timeSinceLastShot = 0;

            if (bulletsInClip > 0)
            {

                AudioSource.PlayClipAtPoint(shoot, player.transform.position);
                muzzleFlash.ShowFlash();

                ///
                /// Create the bullet
                ///
                Bullet bullet = ((GameObject)Instantiate(bulletPrefab)).GetComponent<Bullet>();
                bullet.transform.EAlignWithCamera(transform.parent.parent);
                bullet.transform.position = bulletSpawnPoint.position;
                bullet.InitBulletTrail(bullet.transform.position);
                bullet.SetHitMarkerCallBack(hitMarkerCallback);

                bulletsInClip -= 1;

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


        if (bulletsInClip < clipSize && timeSinceLastShot >= timeBetweenShots)
        {
            int bulletsFromInventory = player.Ammo.Request(gunType, clipSize - bulletsInClip);

            if (bulletsFromInventory > 0)
            {
                timeSinceLastShot = -(reload.length - timeBetweenShots);
                player.AudioSource.PlayOneShot(reload);
                bulletsInClip += bulletsFromInventory;
            }


        }
    }



}

