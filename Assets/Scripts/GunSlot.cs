using UnityEngine.Networking;
using UnityEngine;
using System.Collections;
using System;

public class GunSlot : GunSlot_Base {

    [SerializeField]
    Player player;

    private Action CB_AmmoChanged;

    [SerializeField]
    private Gun_Base primaryGun;
    
    [SerializeField]
    private Gun_Base secondaryGun;



    private Gun_Base equippedGun;


	void Awake () {
        primaryGun = null;
        secondaryGun = null;

    }
	
    public override void Drop(){

        if(primaryGun != null && equippedGun != secondaryGun)
        {
            equippedGun = secondaryGun;
            secondaryGun.gameObject.SetActive(true);
//            UpdateAmmoHUD();

            primaryGun.transform.parent = null;
            primaryGun.Drop();
            primaryGun = null;
        }

        //CB_AmmoChanged();//////////////////
    }

    public override void SetSecondaryActive(bool active){
        //secondaryGun.gameObject.SetActive(active);
        secondaryGun.gameObject.SetActive(true);
    }

    public override void SetPrimaryActive(bool active){
        //primaryGun.gameObject.SetActive(active);
        primaryGun.gameObject.SetActive(true);
    }

    public override Gun_Base PrimaryGun{
        get{return primaryGun;}
    }
    public override Gun_Base SecondaryGun{
        get{return secondaryGun;}
    }

    public override void SetSecondary(Gun_Base _gun){
        
        secondaryGun = _gun;
        equippedGun = secondaryGun;
        equippedGun.AlignGun();


//        CB_AmmoChanged();//////////////////////////////////////
    }


    public override void SetPrimary(Gun_Base _gun){

        
        primaryGun = _gun;
        equippedGun = primaryGun;
        equippedGun.AlignGun();

//        CB_AmmoChanged();//////////////////////////////////////
    }


    public override bool TryPickup(Gun_Base gun){

        if(primaryGun == null){
            primaryGun = gun;
            equippedGun = primaryGun;
            equippedGun.gameObject.SetActive(true);

            if(secondaryGun != null){
                //secondaryGun.gameObject.SetActive(false);////////////////////////////
            }
            //CB_AmmoChanged();////////////////////////////////

            player.GunPickedUp();
            return true;
        }
        return false;
    }

    public override void PreviousWeapon(){
        ToggleEquip();
    }

    public override bool NextWeapon(){
        return ToggleEquip();
    }

    private bool ToggleEquip(){
        if(primaryGun == null){
            return false;
        }

        //equippedGun.gameObject.SetActive(false);
        if(equippedGun == primaryGun){
            equippedGun = secondaryGun;
        }
        else{
            equippedGun = primaryGun;
        }
        equippedGun.gameObject.SetActive(true);
        //CB_AmmoChanged();//////////////
        equippedGun.AlignGun();

        if(equippedGun == primaryGun){
            return true;
        }
        return false;
    }

    public override void EquipPrimary(){
        if(primaryGun != null){
            equippedGun = primaryGun;
        }
        else{
            Debug.LogError("EquipPrimary(): primary gun is null");
        }
    }

    public override void EquipSecondary(){
        if(secondaryGun != null){
            equippedGun = secondaryGun;
        }
        else{
            Debug.LogError("EquipSecondary(): secondary gun is null");
        }

    }


    public override void Reload(){
        equippedGun.Reload();
        //CB_AmmoChanged();////////////
    }


    public override void Shoot(bool firstDown){
        if(equippedGun == null){
            return;
        }

        equippedGun.Shoot(firstDown);
        //CB_AmmoChanged();//////////////
    }

    public override int BulletsInClip{
        get{ return equippedGun.BulletsInClip;}
    }

    public override int ClipSize{
        get{return equippedGun.ClipSize;}
    }


    public override void SetCB_AmmoChanged(Action action){
        CB_AmmoChanged = action;
    }


    public override Gun_Base EquippedGun{
        get{
            return equippedGun;
        }
    }

    public override Player Player{
        get{return player;}
    }
}
