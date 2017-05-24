using UnityEngine;

/// <summary>
/// A bag of extension methods for the Unity class: Transform
/// </summary>
public static class ExtensionMethods
{
    /// <summary>
    /// Aligns the Transform to point forward in the direction the main camera faces.  
    /// </summary>
    /// <param name="alignObject"></param>
    /// <param name="additionalRotation"></param>
    /// <param name="alignMask">A raycast will be sent out in the direction of the main camera, if anything with this LayerMask is hit, this object will point directly at the object hit by the raycast. (Example: if the LayerMask is set to "Everything", the object will point at any object the camera looks).</param>
    public static void AlignWithMainCamera(this Transform alignObject, Vector3 additionalRotation, LayerMask alignMask)
    {
        Transform camera = Camera.main.transform;
        Vector3 point = camera.position + (camera.forward * 10000);


        if (alignMask == LayerMask.GetMask("Nothing"))
        {
            RaycastHit hit;
            Physics.Raycast(camera.position, camera.forward * 1000, out hit, 1000f, alignMask);

            if (hit.point != Vector3.zero)
            {
                point = hit.point;
            }
        }


        alignObject.LookAt(point);
        alignObject.Rotate(additionalRotation);
    }

    /// <summary>
    /// Aligns the Transform to point forward in the direction the main camera faces.  
    /// </summary>
    public static void AlignWithMainCamera(this Transform alignObject, Vector3 additionalRotation)
    {
        AlignWithMainCamera(alignObject, additionalRotation, LayerMask.GetMask("Nothing"));
    }

    
    /// <summary>
    /// Aligns the Transform to point forward in the direction the main camera faces.  
    /// </summary>
    public static void AlignWithMainCamera(this Transform alignObject)
    {
        AlignWithMainCamera(alignObject, Vector3.zero, LayerMask.GetMask("Nothing"));
    }

}
