using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AmmoInventory : AmmoInventory_Base {

    private Action CB_AmmoChanged;
    
    private Dictionary<GunType, int> bullets = new Dictionary<GunType, int>();

    [SerializeField]
    int sniper, pistol, shotgun, assualtRifle, smg;




    void Start(){
        bullets = new Dictionary<GunType, int>();

        //TODO: custom unity inspector gui applied to a dictionary (unsupported by unity), 
        //      does not remember the values entered (values get forgotten at runtime)
        //TODO: this way is actually not bad, requiring only three lines of code to be added for a new
        //      weapon / ammo type to be setup: the Weapon type enum, the above serializedfield and
        //      the below add to dictionary.
        bullets.Add(GunType.sniper, sniper);
        bullets.Add(GunType.pistol, pistol);
        bullets.Add(GunType.assaultRifle, assualtRifle);
        bullets.Add(GunType.smg, smg);
        bullets.Add(GunType.shotgun, shotgun);
    }

    public override int Count(GunType gunType){
        return bullets[gunType];
    }


    public override void Add(GunType gunType, int ammoAmount){
        if(ammoAmount > 0){
            bullets[gunType] += ammoAmount;

            if(CB_AmmoChanged != null){
                CB_AmmoChanged();
            }

        }
    }


    public override int Request(GunType gunType, int amountRequested){
        int returnAmount = 0;

        if(bullets[gunType] >= amountRequested){
            bullets[gunType] -= amountRequested;
            returnAmount = amountRequested;
        }
        else{
            returnAmount = bullets[gunType];
            bullets[gunType] = 0;
        }

        if(CB_AmmoChanged != null){
            CB_AmmoChanged();
        }

        return returnAmount; 
    }
    
    public override void SetCB_AmmoChanged(Action action){
        CB_AmmoChanged = action;
    }

}
