using UnityEngine;

public abstract class Bullet_Base : MonoBehaviour {
    /// 
    /// 
    ///                                     Getters
    /// 
    /// 

    /// <summary>
    /// Sets up a bullet trail to be in the correct position.
    /// </summary>
    /// <param name="spawnPosition">The position the bullet trail should be spawned.</param>
	public abstract void InitBulletTrail(Vector3 spawnPosition);





    /// 
    /// 
    ///                                     Methods
    /// 
    /// 

    /// /// <summary>
    /// Set a reference to a players HitMarkerCallback script, so this bullet can tell the player it hit something.
    /// </summary>
    /// <param name="callback">The script that will be called when this bullet hits something.</param>
    public abstract void SetHitMarkerCallBack(HitMarkerCallback callback);
}
