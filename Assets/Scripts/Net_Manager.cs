using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;


public class Net_Manager : NetworkManager{
	[SerializeField]
	List<NetPlayer> netPlayerList;
	




	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		Debug.Log("Adding player to game...");
		GameObject newPlayer = Instantiate(playerPrefab);
		NetworkServer.Spawn(newPlayer);
		NetworkIdentity newIdentity = newPlayer.GetComponent<NetworkIdentity>();
		newIdentity.AssignClientAuthority(conn);


		NetPlayer np = new NetPlayer(conn, newPlayer.GetComponent<Player>());	
		netPlayerList.Add(np);

		LocalSetup ls = newPlayer.GetComponent<LocalSetup>();
		ls.TargetSetupPlayer(conn);
	}




	public override void OnServerReady(NetworkConnection conn){
		NetworkServer.SetClientReady(conn);
		Debug.Log("Server is now ready");
		
	}


}


[System.Serializable]
public struct NetPlayer{
	[SerializeField]
	private NetworkConnection conn;
	[SerializeField]
	private Player player;

	public NetPlayer(NetworkConnection playerConn, Player player){
		conn = playerConn;
		this.player = player;
	}

	public NetworkConnection Conn{
		get{return conn;}
	}
	public Player Player{
		get{return player;}
	}

}

