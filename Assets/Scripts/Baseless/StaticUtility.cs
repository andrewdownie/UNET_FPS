using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticUtility
{

    public static void SetCollidersOnChildren(GameObject gameObject, bool active)
    {
        Collider[] colliders = gameObject.GetComponents<Collider>();
        foreach (Collider c in colliders)
        {
            c.enabled = active;
        }

    }

    public static void Align(Transform alignObject, Vector3 additionalRotation, LayerMask alignMask)
    {
        Transform camera = Camera.main.transform;
        RaycastHit hit;
        Physics.Raycast(camera.position, camera.forward * 1000, out hit, 1000f, alignMask);

        Vector3 point = hit.point;

        if (point == Vector3.zero)
        {
            point = camera.forward * 100000;
        }
        alignObject.LookAt(point);
        alignObject.Rotate(additionalRotation);
    }

    public static void Align(Transform alignObject, Vector3 additionalRotation)
    {
        Align(alignObject, additionalRotation, LayerMask.GetMask("Nothing"));
    }

}
