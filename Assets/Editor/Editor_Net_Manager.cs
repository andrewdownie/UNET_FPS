using System.Collections.Generic;
using System;

using UnityEngine.Networking;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Net_Manager))]
public class Editor_Net_Manager : Editor {
	


	public override void OnInspectorGUI(){
		DrawDefaultInspector ();	
		Net_Manager myTarget = (Net_Manager)target;
		List<NetPlayer> netPlayerList = myTarget.NetPlayerList;


		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		EditorGUILayout.LabelField("Net_Manager internal representation of players", EditorStyles.boldLabel);
		foreach(NetPlayer np in netPlayerList){
			EditorGUILayout.Separator();

			EditorGUILayout.LabelField("Player Name", np.PlayerName, EditorStyles.boldLabel);
			EditorGUILayout.ObjectField("Player", np.PlayerID, typeof(NetworkIdentity));
			EditorGUILayout.ObjectField("Primary Gun", np.PrimaryWeapon, typeof(NetworkIdentity));
			EditorGUILayout.ObjectField("Secondary Gun", np.SecondaryWeapon, typeof(NetworkIdentity));

		}



	}


}
