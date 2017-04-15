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



	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		Debug.Log("Adding player to game...");
		GameObject newPlayer = Instantiate(playerPrefab);

		NetworkServer.Spawn(newPlayer);
		NetworkIdentity newIdentity = newPlayer.GetComponent<NetworkIdentity>();
		newIdentity.AssignClientAuthority(conn);


		NetPlayer np = new NetPlayer(conn, newPlayer.GetComponent<Player>(), startingPrimaryWeapon, startingSecondaryWeapon);	
		netPlayerList.Add(np);

		LocalSetup ls = newPlayer.GetComponent<LocalSetup>();
		ls.TargetSetupPlayer(conn);
	}




	public override void OnServerReady(NetworkConnection conn){
		NetworkServer.SetClientReady(conn);
		Debug.Log("Server is now ready");
		
	}


}


public class NetPlayer : NetworkBehaviour{
	[SerializeField]
	private NetworkConnection conn;
	[SerializeField]
	private Player player;

	[SerializeField]
	private NetworkIdentity primaryWeapon;
	[SerializeField]
	private NetworkIdentity secondaryWeapon;

	public NetPlayer(NetworkConnection playerConn, Player player, Gun_Base primaryWeapon, Gun_Base secondaryWeapon){
		conn = playerConn;
		this.player = player;


		this.primaryWeapon = null;
		this.secondaryWeapon = null;


		if(primaryWeapon != null){
			this.primaryWeapon = SpawnGun(primaryWeapon);
		}

		if(secondaryWeapon != null){
			this.secondaryWeapon = SpawnGun(secondaryWeapon);
		}

	}

	private NetworkIdentity SpawnGun(Gun_Base gun){
		GameObject gunGO = Instantiate(gun.gameObject);
		NetworkServer.Spawn(gunGO);

		NetworkIdentity gunIdentity = gunGO.GetComponent<NetworkIdentity>();
		gunIdentity.AssignClientAuthority(conn);

		return gunIdentity;
	}

	public NetworkConnection Conn{
		get{return conn;}
	}
	public Player Player{
		get{return player;}
	}

}

