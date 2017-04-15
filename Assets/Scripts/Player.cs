using UnityEngine;
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
        HUD.SetInventoryAmmo(ammo.Count(gunSlot.EquippedGun.GunType)); //TODO: change GetGunType() to getter
        HUD.SetClipAmmo(gunSlot.EquippedGun.BulletsInClip, gunSlot.EquippedGun.ClipSize);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            vitals.UseHealthpack();
        }

        if(Input.GetKeyDown(KeyCode.E)){
            gunSlot.Drop();
        }


        if(Input.GetKeyDown(KeyCode.R)){
            gunSlot.Reload();
        }

        if(Input.GetKeyDown(KeyCode.Q)){
            gunSlot.NextWeapon();
        }


        ///
        /// Handle shooting
        ///
        if(Input.GetKey(KeyCode.Mouse0)){
            gunSlot.Shoot(Input.GetKeyDown(KeyCode.Mouse0));
        } 
                
        
    }
}
