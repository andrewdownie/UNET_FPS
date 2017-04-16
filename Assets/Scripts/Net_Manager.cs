using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;


public class Net_Manager : NetworkManager{
	[SerializeField]
	List<NetPlayer> netPlayerList;
	
	[SerializeField]
	Gun_Base startingPrimaryWeapon;

	[SerializeField]
	Gun_Base startingSecondaryWeapon;

	//Trying to create a new branch for adding networking stuff....
	// Adding this random content to this file so git will actually commit something to the remote


	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		Debug.Log("Adding player to game...");
		GameObject newPlayer = Instantiate(playerPrefab);

		NetworkServer.Spawn(newPlayer);
		NetworkIdentity newIdentity = newPlayer.GetComponent<NetworkIdentity>();
		newIdentity.AssignClientAuthority(conn);


		//NetPlayer np = new NetPlayer(conn, newPlayer.GetComponent<Player>(), startingPrimaryWeapon, startingSecondaryWeapon);	
		NetPlayer np = gameObject.AddComponent<NetPlayer>();
		np.Constructor(conn, newPlayer.GetComponent<Player_Base>(), startingPrimaryWeapon, startingSecondaryWeapon);
		netPlayerList.Add(np);

		LocalSetup ls = newPlayer.GetComponent<LocalSetup>();
		ls.TargetSetupPlayer(conn);


		foreach(NetPlayer netPlayer in netPlayerList){
			NetworkIdentity primaryWeapon = netPlayer.PrimaryWeapon;
			NetworkIdentity secondaryWeapon = netPlayer.SecondaryWeapon;
			netPlayer.Player.RpcConnectWeapons(primaryWeapon, secondaryWeapon);
		}

	}




	public override void OnServerReady(NetworkConnection conn){
		NetworkServer.SetClientReady(conn);
		Debug.Log("Server is now ready");
		
	}


	/// TODO: Need a way of sending a message to the player who just joined, that contains each
	/// players gameobject and the two guns they have in their inventory.	

}


