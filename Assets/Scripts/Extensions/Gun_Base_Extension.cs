using UnityEngine;

/// <summary>
/// A bag of extension methods for the class: Gun_Base
/// Used to implement common methods for a virtual interface / Base class
/// </summary>
public static class Gun_Base_Extension{
	/// <summary>
	/// Disables the gun script, adds a Rigidbody, and enables colliders in children.
	/// </summary>
	/// <param name="gun"></param>
    public static void EDrop(this Gun_Base gun)
    {
        gun.enabled = false;
        Rigidbody rb = gun.gameObject.AddComponent<Rigidbody>();
        rb.useGravity = true;
        gun.gameObject.EEnableCollidersInChildren(true);
    }

}
