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

	public static Net_Manager instance;


	void Start(){
		instance = this;
	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		Debug.Log("Adding player to game...");
		GameObject newPlayer = Instantiate(playerPrefab);

		NetworkServer.Spawn(newPlayer);
		NetworkIdentity newIdentity = newPlayer.GetComponent<NetworkIdentity>();
		newIdentity.AssignClientAuthority(conn);


		NetPlayer np = gameObject.AddComponent<NetPlayer>();
		np.Constructor(conn, newPlayer.GetComponent<Player_Base>(), startingPrimaryWeapon, startingSecondaryWeapon);
		netPlayerList.Add(np);

		LocalSetup ls = newPlayer.GetComponent<LocalSetup>();
		ls.TargetSetupPlayer(conn);


		ConnectWeapons();
	}

	public void PickupPrimary(NetworkIdentity playerID, NetworkIdentity gunID){
		//TODO: this
	}
	public void DropPrimary(NetworkIdentity playerID, NetworkIdentity gunID){
		//TODO: this
	}


	private void ConnectWeapons(){
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



}


