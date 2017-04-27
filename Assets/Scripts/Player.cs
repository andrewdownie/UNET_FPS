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

    [SerializeField]
    private Gun_Base tempSecondaryGun;
    void Start(){





        ammo.SetCB_AmmoChanged(CB_AmmoInventory);
        gunSlot.SetCB_AmmoChanged(CB_AmmoInventory);

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
        return gunSlot.TryPickup(gun);
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
            CmdNextWeapon();
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
        Debug.LogError("RpcConnectSecondary");
        
        if(secondaryWeapon != null){
            Debug.LogError("1");
            Gun_Base secondary = secondaryWeapon.gameObject.GetComponent<Gun_Base>();
            tempSecondaryGun = secondary;
            if(secondary != null){
                Debug.LogError("2: " + secondary);
                gunSlot.SetSecondary(secondary);
                secondary.SetOwningPlayer(this);
            }
            else{
                Debug.LogError("RpcConnectWeapons: secondary gameobject was null");
            }
        }
        else{
            Debug.LogError("RpcConnectWeapons: secondary NETID was null");
        }

        
    }

    [ClientRpc]
    public override void RpcConnectPrimary(NetworkIdentity primaryWeapon){
        Debug.LogError("RpcConnectSecondary");

        if(primaryWeapon != null){
            Gun_Base primary = primaryWeapon.gameObject.GetComponent<Gun_Base>();
            if(primary != null){
                primary.SetOwningPlayer(this);
                gunSlot.TryPickup(primary);
            }
            else{
                Debug.LogError("RpcConnectWeapons: primary gameobject was null");
            }
        }
        else{
            Debug.LogError("RpcConnectWeapons: primary NETID was null");
        }
    }

    

}
