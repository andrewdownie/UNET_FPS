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

	// Use this for initialization
	void Awake () {
        primaryGun = null;
        secondaryGun = null;

        /* 
        if(secondaryGun != null){
            secondaryGun.gameObject.SetActive(false);
            equippedGun = secondaryGun;
        }

        if(primaryGun != null){
            primaryGun.gameObject.SetActive(false);
            equippedGun = primaryGun;
        }

        if(equippedGun != null){
            equippedGun.gameObject.SetActive(true);        
            equippedGun.AlignGun();
            //CB_AmmoChanged();/////////////
        }
        */
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

    public override void SetSecondary(Gun_Base _gun){
        Debug.LogError(player.name + ": Set secondary: " + _gun);
        
        secondaryGun = _gun;
        secondaryGun.gameObject.SetActive(true);
        equippedGun = secondaryGun;
        equippedGun.AlignGun();

        secondaryGun.transform.localScale = new Vector3(10, 10, 10);
        
        Debug.LogError(player.name + " now has the gun: " + secondaryGun);
//        CB_AmmoChanged();//////////////////////////////////////
    }

    public override bool TryPickup(Gun_Base gun){

        if(primaryGun == null){
            primaryGun = gun;
            equippedGun = primaryGun;
            equippedGun.gameObject.SetActive(true);

            if(secondaryGun != null){
                secondaryGun.gameObject.SetActive(false);
            }
            //CB_AmmoChanged();////////////////////////////////
            return true;
        }
        return false;
    }

    public override void PreviousWeapon(){
        ToggleEquip();
    }

    public override void NextWeapon(){
        ToggleEquip();
    }

    private void ToggleEquip(){
        if(primaryGun == null){
            return;
        }

        equippedGun.gameObject.SetActive(false);
        if(equippedGun == primaryGun){
            equippedGun = secondaryGun;
        }
        else{
            equippedGun = primaryGun;
        }
        equippedGun.gameObject.SetActive(true);
        //CB_AmmoChanged();//////////////
        equippedGun.AlignGun();
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
