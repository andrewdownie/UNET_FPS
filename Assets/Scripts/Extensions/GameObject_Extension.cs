using UnityEngine;

/// <summary>
/// A bag of extension methods for the Unity class: GameObject
/// </summary>
public static class GameObject_Extension{
 	/// <summary>
    /// Enables / disables all colliders on a GameObject and its children
    /// </summary>
    /// <param name="gameObject">The GameObject and it's children that will have colliders enabled/disabled.</param>
    /// <param name="active">Whether the colliders will be set to enabled, or set to disabled.</param>
    public static void EnableCollidersInChildren(this GameObject gameObject, bool active)
    {
        Collider[] colliders = gameObject.GetComponents<Collider>();
        foreach (Collider c in colliders)
        {
            c.enabled = active;
        }
    }

    /// <summary>
    /// Enables / disables all renderers on a GameObject and its children.
    /// </summary>
    /// <param name="gameObject">The GameObject and it's children that will have renderers enabled/disabled.</param>
    /// <param name="active">Whether the renderers will be set to enabled or set to disabled.</param>
    public static void EnableRenderersInChildren(this GameObject gameObject, bool active)
    {
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
        {
            r.enabled = active;
        }
    }

}
