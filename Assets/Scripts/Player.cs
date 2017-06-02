using UnityEngine;
using UnityEngine.Networking;


/// <summary>
/// The player class has three main responsibilities.
///     1. Act as a centralized organizer of all the components that make up a player (vitals, gunslot, ect.) 
///     2. Act as an interface for picking items up in the world.
///     3. Handle user input (in the update method)
/// </summary>
public class Player : Player_Base
{

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private Vitals_Base vitals;

    [SerializeField]
    private GunSlot_Base gunSlot;

    [SerializeField]
    private AmmoInventory_Base ammo;

    [SerializeField]
    [SyncVar(hook = "GunChanged")]
    bool primaryEquipped;

    [SerializeField]
    [SyncVar(hook = "ReviveTimeChanged")]
    float reviveTime;

    [SerializeField]
    float REVIVE_TIME = 3f;
    [SerializeField]
    Player_Base targetRevive;
    [SerializeField]
    LayerMask playerLayerMask;
    [SerializeField]
    LayerMask enemyMask;

    bool reviving;

    float meleePercent;
    float meleeSpeed = 500;

    [SerializeField]
    AudioClip meleeSound;

    void Start()
    {
        ammo.SetCB_AmmoChanged(UpdateAmmoHUD);
        gunSlot.SetCB_AmmoChanged(UpdateAmmoHUD);
        primaryEquipped = false;

        UpdateAmmoHUD();

        if (targetRevive == null)
        {
            HUD.SetReviveImageFill(0);
            HUD.SetReviveText("");
        }
    }

    private void ReviveTimeChanged(float reviveTime)
    {
        if (hasAuthority)
        {
            HUD.SetReviveImageFill(reviveTime / REVIVE_TIME);

            if (reviveTime > 0)
            {
                HUD.SetReviveText("Hold 'j' to revive " + targetRevive.name);
            }
            else
            {
                HUD.SetReviveText("");
            }
        }

        this.reviveTime = reviveTime;
    }



    private void GunChanged(bool primaryEquipped)
    {
        //Debug.LogError("Gun Changed");

        if (gunSlot.PrimaryGun != null)
        {
            if (primaryEquipped)
            {
                gunSlot.EquipPrimary();
                gunSlot.PrimaryGun.SetVisible(true);
                UpdateAmmoHUD();
            }
            else
            {
                gunSlot.PrimaryGun.SetVisible(false);
            }
        }
        else if (primaryEquipped)
        {
            Debug.LogError("Primary is equipped, but there is no primary weapon?");
        }

        if (gunSlot.SecondaryGun != null)
        {
            if (!primaryEquipped)
            {
                gunSlot.EquipSecondary();
                gunSlot.SecondaryGun.SetVisible(true);
                UpdateAmmoHUD();
            }
            else
            {
                gunSlot.SecondaryGun.SetVisible(false);
            }
        }
        else if (!primaryEquipped)
        {
            Debug.LogError("Secondary is equipped, but there is no secondary weapon?");
        }


        this.primaryEquipped = primaryEquipped;

    }

    public override Vitals_Base Vitals
    {
        get { return vitals; }
    }

    public override GunSlot_Base GunSlot
    {
        get { return gunSlot; }
    }

    public override AmmoInventory_Base Ammo
    {
        get { return ammo; }
    }

    public override AudioSource AudioSource
    {
        get { return audioSource; }
    }

    public override Rigidbody Rigidbody
    {
        get { return GetComponent<Rigidbody>(); }
    }

    public override void PickupAmmo(GunType gunType, int amount)
    {
        //TODO: make the pickup ammo script target the AmmoInventory_Base class directly
        ammo.Add(gunType, amount);
    }


    public override void GunPickedUp()
    {
        primaryEquipped = true;
        gunSlot.SecondaryGun.SetVisible(false);
    }

    private void UpdateAmmoHUD()
    {
        HUD.SetInventoryAmmo(ammo.Count(gunSlot.EquippedGun.GunType));
        HUD.SetClipAmmo(gunSlot.EquippedGun.BulletsInClip, gunSlot.EquippedGun.ClipSize);
    }


    ///
    /// Keyboard input
    ///
    void Update()
    {




        ///
        /// Use health pack
        ///
        if (Input.GetKeyDown(KeyCode.H))
        {
            CmdUseHealthPack();
        }

        ///
        /// Drop weapon
        ///
        if (Input.GetKeyDown(KeyCode.E))
        {
            CmdDrop();
        }

        ///
        /// Reload
        ///
        if (Input.GetKeyDown(KeyCode.R))
        {
            CmdReload();
        }

        ///
        /// Switch weapons
        ///
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CmdNextWeapon();
        }


        ///
        /// Shoot 
        ///
        if (Input.GetKey(KeyCode.Mouse0) && meleePercent < -100)
        {
            CmdShoot(Input.GetKeyDown(KeyCode.Mouse0));
            UpdateAmmoHUD();
        }

        ///
        /// Start - Revive Ally
        ///
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (targetRevive != null)
            {
                reviving = true;
                reviveTime = 0;
            }
        }

        ///
        /// Stop - Revive Ally
        ///
        if (Input.GetKeyUp(KeyCode.J))
        {
            reviving = false;
            reviveTime = 0;
            HUD.SetReviveImageFill(1);
        }


        ///
        /// Melee
        ///
        if (Input.GetKeyDown(KeyCode.F) && meleePercent < -40)
        {
            meleePercent = 100;
            CmdMelee();
        }


        ReviveAlly();
        CheckForDeadAllies();

        if (meleePercent > -110)
        {
            gunSlot.MeleeAnimation(meleePercent);
            meleePercent -= meleeSpeed * Time.deltaTime;
        }

    }

    void MeleeAnimation(float percent)
    {
        Vector3 startPos = new Vector3(-0.4f, 0, 0.5f);
        Vector3 endPos = Vector3.zero;

        Vector3 newPos = Vector3.Slerp(endPos, startPos, percent / 100.0f);
        gunSlot.EquippedGun.transform.localPosition = newPos;


        Quaternion startRot = Quaternion.Euler(0, 90, 0);
        Quaternion endRot = Quaternion.Euler(30, -35, 10);

        Quaternion newRot = Quaternion.Slerp(startRot, endRot, percent / 70.0f);
        gunSlot.EquippedGun.transform.localRotation = newRot;

    }

    void ReviveAlly()
    {
        if (reviving)
        {
            reviveTime += Time.deltaTime;

            if (targetRevive == null)
            {
                reviving = false;
            }
            else if (reviveTime >= REVIVE_TIME)
            {
                NetworkIdentity targetNetID = targetRevive.GetComponent<NetworkIdentity>();
                CmdRevive(targetNetID);
                //targetRevive.Vitals.Revive();
                targetRevive = null;
                reviving = false;
            }
        }
    }

    void CheckForDeadAllies()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1.3f, playerLayerMask);

        if (colliders.Length == 1)
        {
            HUD.SetReviveText("");
            HUD.SetReviveImageFill(0);
        }
        else
        {

            foreach (Collider c in colliders)
            {
                if (c.transform == transform)
                {
                    continue;
                }

                Player_Base targetRevive = c.GetComponent<Player_Base>();
                if (targetRevive.Vitals.dead)
                {
                    this.targetRevive = targetRevive;
                    HUD.SetReviveText("Hold 'j' to revive " + c.name);

                    float revivePercent = reviveTime / REVIVE_TIME;
                    if (revivePercent == 0)
                    {
                        revivePercent = 1;
                    }
                    HUD.SetReviveImageFill(revivePercent);
                    break;
                }

            }
        }
    }

    [Command]
    void CmdRevive(NetworkIdentity reviveTarget){
        Player_Base player = reviveTarget.GetComponent<Player_Base>();
        player.Vitals.Revive();
    }

    [ClientRpc]
    void RpcRevive(NetworkIdentity reviveTarget){
        //This might not be needed, since player being alive is based on their health and health and managed by a syncvar
    }

    [Command]
    void CmdMelee()
    {
        RpcMelee();

        Collider[] enemies = Physics.OverlapSphere(transform.position + transform.forward, 2f, enemyMask);

        foreach (Collider c in enemies)
        {
            if (c.GetComponent<Zombie_Base>())
            {
                c.GetComponent<Zombie_Base>().TakeDamage(15, Vector3.zero, Vector3.zero);
                c.GetComponent<Rigidbody>().AddForce(transform.forward * 1000, ForceMode.Impulse);
            }
        }


    }

    [ClientRpc]
    void RpcMelee()
    {
        audioSource.PlayOneShot(meleeSound);
    }





    [Command]
    void CmdUseHealthPack()
    {
        if (vitals.HasHealthpack())
        {
            RpcUseHealthPack();
        }
    }

    [ClientRpc]
    void RpcUseHealthPack()
    {
        vitals.UseHealthpack();
    }

    [Command]
    void CmdNextWeapon()
    {
        primaryEquipped = gunSlot.NextWeapon();
        gunSlot.MeleeAnimation(meleePercent);
    }
    [ClientRpc]
    void RpcNextWeapon()
    {
        gunSlot.NextWeapon();
    }


    [Command]
    void CmdDrop()
    {
        RpcDrop();
        Net_Manager.instance.DropPrimary(GetComponent<NetworkIdentity>());
    }
    [ClientRpc]
    public void RpcDrop()
    {
        gunSlot.Drop();
        primaryEquipped = false;
    }


    [Command]
    void CmdShoot(bool mouseDown)
    {
        RpcShoot(mouseDown);
    }
    [ClientRpc]
    void RpcShoot(bool mouseDown)
    {
        gunSlot.Shoot(mouseDown);
    }


    [Command]
    void CmdReload()
    {
        RpcReload();
    }
    [ClientRpc]
    void RpcReload()
    {
        gunSlot.Reload();

        if (hasAuthority)
        {
            UpdateAmmoHUD();
        }
    }

    [ClientRpc]
    public override void RpcSetPlayerName(string playerName)
    {
        transform.name = playerName;
    }




    [ClientRpc]
    public override void RpcConnectSecondary(NetworkIdentity secondaryWeapon)
    {

        if (secondaryWeapon != null)
        {
            Gun_Base secondary = secondaryWeapon.gameObject.GetComponent<Gun_Base>();
            if (secondary != null)
            {
                secondary.SetOwningPlayer(this);
                gunSlot.SetSecondary(secondary);

                GunChanged(primaryEquipped);
            }
            else
            {
                Debug.LogWarning("RpcConnectWeapons: secondary gameobject was null");
            }
        }
        else
        {
            Debug.LogWarning("RpcConnectWeapons: secondary NETID was null");
        }


    }

    [ClientRpc]
    public override void RpcConnectPrimary(NetworkIdentity primaryWeapon)
    {

        if (primaryWeapon != null)
        {
            Gun_Base primary = primaryWeapon.gameObject.GetComponent<Gun_Base>();
            if (primary != null)
            {
                primary.SetOwningPlayer(this);
                gunSlot.SetPrimary(primary);
                GunChanged(primaryEquipped);
            }
            else
            {
                Debug.LogWarning("RpcConnectWeapons: primary gameobject was null");
            }
        }
        else
        {
            Debug.LogWarning("RpcConnectWeapons: primary NETID was null");
        }
    }




}


