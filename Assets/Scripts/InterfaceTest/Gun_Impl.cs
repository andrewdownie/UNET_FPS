using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Impl : MonoBehaviour, IGun {
    void IGun.Shoot()
    {
		Debug.LogError("shoot in Gun_Impl");
    }


}
