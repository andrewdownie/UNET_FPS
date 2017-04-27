using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;


/// <summary>
/// The player class has three main responsibilities.
///     1. Act as a centralized organizer of all the components that make up a player (vitals, gunslot, ect.) 
///     2. Act as an interface for picking items up in the world.
///     3. Handle user input (in the update method)
/// </summary>
public class Player : Player_Base {

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private Vitals_Base vitals;

    [SerializeField]
    private GunSlot_Base gunSlot;

    [SerializeField]
    private AmmoInventory_Base ammo;

    [SerializeField][SyncVar(hook="GunChanged")]
    bool primaryEquipped;

    void Start(){
        ammo.SetCB_AmmoChanged(CB_AmmoInventory);
        gunSlot.SetCB_AmmoChanged(CB_AmmoInventory);
        primaryEquipped = false;
    } 

    private void GunChanged(bool primaryEquipped){
        Debug.LogError("GunChanged for player: " + this.name);

        if(gunSlot.PrimaryGun != null){
            if(primaryEquipped){
                gunSlot.PrimaryGun.gameObject.SetActive(true);
            }
            else{
                gunSlot.PrimaryGun.gameObject.SetActive(false);
            }
        }
        else if(primaryEquipped){
            Debug.LogError("Primary is equipped, but there is no primary weapon?"); 
        }

        if(gunSlot.SecondaryGun != null){
            if(!primaryEquipped){
                gunSlot.SecondaryGun.gameObject.SetActive(true);
            }
            else{
                gunSlot.SecondaryGun.gameObject.SetActive(false);
            }
        }
        else if(!primaryEquipped){
            Debug.LogError("Secondary is equipped, but there is no secondary weapon?");
        }


        this.primaryEquipped = primaryEquipped;
    }

    public override Vitals_Base Vitals{
        get{return vitals;}
    }

    public override GunSlot_Base GunSlot{
        get{return gunSlot;}
    }

    public override AmmoInventory_Base Ammo{
        get{return ammo;}
    }

    public override AudioSource AudioSource{
        get{return audioSource;}
    }

    public override Rigidbody Rigidbody{
        get{return GetComponent<Rigidbody>();}
    }

    public override void PickupAmmo(GunType gunType, int amount)
    {
        //TODO: make the pickup ammo script target the AmmoInventory_Base class directly
        ammo.Add(gunType, amount);
    }

    public override bool TryPickupGun(Gun_Base gun){
        //TODO: make the pickup gun script target the weapon slot directly
        //TODO: it doesn't look like this is used anymore
        return gunSlot.TryPickup(gun);
    }

    public override void GunPickedUp(){
        primaryEquipped = true;
    }

    private void CB_AmmoInventory(){
        HUD.SetInventoryAmmo(ammo.Count(gunSlot.EquippedGun.GunType)); 
        HUD.SetClipAmmo(gunSlot.EquippedGun.BulletsInClip, gunSlot.EquippedGun.ClipSize);
    }


    ///
    /// Keyboard input
    ///
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            vitals.UseHealthpack();
        }

        if(Input.GetKeyDown(KeyCode.E)){
            CmdDrop();
        }


        if(Input.GetKeyDown(KeyCode.R)){
            CmdReload();
        }

        if(Input.GetKeyDown(KeyCode.Q)){
            primaryEquipped = gunSlot.NextWeapon();

            //CmdNextWeapon();
        }


        ///
        /// Handle shooting
        ///
        if(Input.GetKey(KeyCode.Mouse0)){
            CmdShoot(Input.GetKeyDown(KeyCode.Mouse0));
        } 


        
    }

    [Command]
    void CmdNextWeapon(){
        RpcNextWeapon();
    }
    [ClientRpc]
    void RpcNextWeapon(){
        gunSlot.NextWeapon();
    }


    [Command]
    void CmdDrop(){
        RpcDrop();
        Net_Manager.instance.DropPrimary(GetComponent<NetworkIdentity>(), gunSlot.EquippedGun.GetComponent<NetworkIdentity>()); // How get ID of equpped weapon?
    }
    [ClientRpc]
    void RpcDrop(){
        gunSlot.Drop();
    }


    [Command]
    void CmdShoot(bool mouseDown){
        RpcShoot(mouseDown);
    }
    [ClientRpc]
    void RpcShoot(bool mouseDown){
        gunSlot.Shoot(mouseDown);
    }


    [Command]
    void CmdReload(){
        RpcReload();
    }
    [ClientRpc]
    void RpcReload(){
        gunSlot.Reload();
    }

    [ClientRpc]
    public override void RpcSetPlayerName(string playerName){
        transform.name = playerName;
    }


    [ClientRpc]
    public override void RpcConnectSecondary(NetworkIdentity secondaryWeapon){
        
        if(secondaryWeapon != null){
            Gun_Base secondary = secondaryWeapon.gameObject.GetComponent<Gun_Base>();
            if(secondary != null){
                gunSlot.SetSecondary(secondary);
                secondary.SetSecondaryOwner(this);
            }
            else{
                Debug.LogWarning("RpcConnectWeapons: secondary gameobject was null");
            }
        }
        else{
            Debug.LogWarning("RpcConnectWeapons: secondary NETID was null");
        }

        
    }

    [ClientRpc]
    public override void RpcConnectPrimary(NetworkIdentity primaryWeapon){

        if(primaryWeapon != null){
            Gun_Base primary = primaryWeapon.gameObject.GetComponent<Gun_Base>();
            if(primary != null){
                gunSlot.SetPrimary(primary);
                primary.SetOwningPlayer(this);
                //gunSlot.TryPickup(primary);
            }
            else{
                Debug.LogWarning("RpcConnectWeapons: primary gameobject was null");
            }
        }
        else{
            Debug.LogWarning("RpcConnectWeapons: primary NETID was null");
        }
    }

    

}
