using UnityEngine.Networking;


public abstract class Gun_Base : NetworkBehaviour
{
    /// 
    /// 
    ///                                     Getters
    /// 
    /// 
    

    /// <summary>
    /// The maximum amount of bullets that can be loaded into this guns clip.
    /// </summary>
    public abstract int ClipSize { get; }

    /// <summary>
    /// How many bullets are currently in this guns clip.
    /// </summary>
    public abstract int BulletsInClip { get; }

    /// <summary>
    /// The type of gun, this gun is.
    /// </summary>
    public abstract GunType GunType { get; }





    /// 
    /// 
    ///                                    Methods 
    /// 
    /// 

    /// <summary>
    /// Attempt to reload this weapons clip. The difference between this weapons BulletsInClip and ClipSize should be Requested(...) from an AmmoInventory.
    /// </summary>
    public abstract void Reload();

    /// <summary>
    /// Shoots this weapon. Should instantiate a bullet, and a shell.
    /// </summary>
    /// <param name="firstDown">Whether the shoot button is being pressed for the first time. (False if the shoot button is being held).</param>
    public abstract void Shoot(bool firstDown);

    /// <summary>
    /// Drops this weapon, and removes the current player as its owner.
    /// </summary>
    public abstract void Drop();

    /// <summary>
    /// Sets this guns barrel to be in line with the players camera that is currently holding it.
    /// </summary>
    public abstract void AlignGun();

    /// <summary>
    /// Sets the given player to be the owner of this gun, which allow the player to shoot/reload/ect... 
    /// </summary>
    /// <param name="newOwner">The player that will own the weapon.</param>
    public abstract void SetOwningPlayer(Player_Base newOwner);

    /// <summary>
    /// Turns all the guns renderers on/off
    /// </summary>
    /// <param name="visible"></param>
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