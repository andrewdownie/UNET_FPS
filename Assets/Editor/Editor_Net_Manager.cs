using System.Collections.Generic;

using UnityEngine.Networking;
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
			EditorGUILayout.ObjectField("Player", np.PlayerID, typeof(NetworkIdentity), true);
			EditorGUILayout.ObjectField("Primary Gun", np.PrimaryWeapon, typeof(NetworkIdentity), true);
			EditorGUILayout.ObjectField("Secondary Gun", np.SecondaryWeapon, typeof(NetworkIdentity), true);

		}



	}


}
