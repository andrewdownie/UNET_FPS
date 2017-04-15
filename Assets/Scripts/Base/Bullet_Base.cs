using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Bullet_Base : MonoBehaviour {
	public abstract void InitBulletTrail(Vector3 spawnPosition);
    public abstract void SetHitMarkerCallBack(HitMarkerCallback callback);
}
