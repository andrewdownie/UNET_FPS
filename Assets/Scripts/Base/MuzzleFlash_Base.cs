using UnityEngine;

public abstract class MuzzleFlash_Base : MonoBehaviour {
    /// 
    /// 
    ///                                     Methods
    /// 
    /// 

	/// <summary>
	/// Enables renderers so the MuzzleFlash is visible.
	/// </summary>
	public abstract void ShowFlash();

	/// <summary>
	/// Disables renderers so the MuzzleFlash is hidden.
	/// </summary>
	public abstract void HideFlash();

	/// <summary>
	/// Shows the MuzzleFlash, and then hides it after a short time.
	/// </summary>
	/// <param name="duration">How long to wait until hiding the MuzzleFlash.</param>
	public abstract void ShowFlash(float duration);
}
