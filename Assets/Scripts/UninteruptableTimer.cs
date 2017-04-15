using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class UninteruptableTimer : MonoBehaviour {

	public void Time(Action action, float time){
		StartCoroutine(Timer(action, time));
	}

	private IEnumerator Timer(Action action, float time){
		yield return new WaitForSeconds(time);	
		action();
	}
}
