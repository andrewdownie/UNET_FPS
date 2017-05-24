using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public abstract class Gun_Base : NetworkBehaviour
{
    /// 
    /// 
    ///                                     Getters
    /// 
    /// 
    public abstract int ClipSize { get; }
    public abstract int BulletsInClip { get; }
    public abstract GunType GunType { get; }





    /// 
    /// 
    ///                                    Methods 
    /// 
    /// 
    public abstract void Reload();

    public abstract void Shoot(bool firstDown);

    public abstract void Drop();

    public abstract void AlignGun();

    public abstract void SetOwningPlayer(Player_Base newOwner);

    public abstract void SetVisible(bool visible);

}


public enum GunType
{
    sniper,
    shotgun,
    pistol,
    smg,
    assaultRifle
}