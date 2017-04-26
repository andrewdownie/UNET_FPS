using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class NetPlayer : NetworkBehaviour{
	[SerializeField]
	private NetworkConnection conn;
	[SerializeField]
	private Player_Base player;
	[SerializeField]
	private NetworkIdentity playerID;

	[SerializeField]
	private NetworkIdentity primaryWeapon;
	[SerializeField]
	private NetworkIdentity secondaryWeapon;

	public void Constructor(NetworkConnection playerConn, Player_Base player, Gun_Base primaryWeapon, Gun_Base secondaryWeapon){
		
		conn = playerConn;
		this.player = player;
		playerID = player.GetComponent<NetworkIdentity>();


		this.primaryWeapon = null;
		this.secondaryWeapon = null;


		if(primaryWeapon != null){
			this.primaryWeapon = SpawnGun(primaryWeapon);
		}

		if(secondaryWeapon != null){
			this.secondaryWeapon = SpawnGun(secondaryWeapon);
		}

	}


	public NetworkConnection Conn{
		get{return conn;}
	}
	public Player_Base Player{
		get{return player;}
	}
	public NetworkIdentity PrimaryWeapon{
		get{return primaryWeapon;}
		set{primaryWeapon = value;}
	}
	public NetworkIdentity SecondaryWeapon{
		get{return secondaryWeapon;}
	}
	public NetworkIdentity PlayerID{
		get{return playerID;}
	}

	private NetworkIdentity SpawnGun(Gun_Base gun){
		GameObject gunGO = Instantiate(gun.gameObject);
		NetworkServer.Spawn(gunGO);

		NetworkIdentity gunIdentity = gunGO.GetComponent<NetworkIdentity>();
		gunIdentity.AssignClientAuthority(conn);

		return gunIdentity;
	}
}
