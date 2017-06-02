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

		if(instance == null){
			instance = this;
		}
		else{
			Debug.Log("There is more than one Net_Manager instance, (fly) you fool");
		}






	}

	Vector3 SpawnPoint(){
		Vector3 spawnPoint = Vector3.zero;

		GameObject spawnObject = GameObject.Find("SpawnPoint");

		if(spawnObject != null){
			spawnPoint = spawnObject.transform.position;
		}
		else{
			Debug.LogError("GameObject with the name: SpawnPoint was not found, player will be spawned at (0, 0, 0)");
		}


		return spawnPoint;
	}



	public override void OnServerDisconnect(NetworkConnection conn){
		Debug.LogError("OnServerDisconnect");
		NetPlayer playerThatLeft = null;

		foreach(NetPlayer np in netPlayerList){
			if(np.Conn == conn){
				playerThatLeft = np;
			}
		}

		if(playerThatLeft != null){
			NetworkServer.Destroy(playerThatLeft.Player.gameObject);
			netPlayerList.Remove(playerThatLeft);
			Destroy(playerThatLeft);
		}

		
	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		Debug.Log("Adding player to game...");
		GameObject newPlayer = Instantiate(playerPrefab, SpawnPoint(), Quaternion.identity);

		NetworkServer.Spawn(newPlayer);
		NetworkIdentity newIdentity = newPlayer.GetComponent<NetworkIdentity>();
		newIdentity.AssignClientAuthority(conn);


		NetPlayer np = gameObject.AddComponent<NetPlayer>();
		string playerName = "Player" + netPlayerList.Count;
		np.Constructor(conn, newPlayer.GetComponent<Player_Base>(), playerName, startingPrimaryWeapon, startingSecondaryWeapon);
		netPlayerList.Add(np);
		SetPlayerNames();

		LocalSetup ls = newPlayer.GetComponent<LocalSetup>();
		ls.TargetSetupPlayer(conn);


		ConnectPrimarys();
		ConnectSecondarys();
	}

	public void SetPrimary(NetworkIdentity playerID, NetworkIdentity gunID){
		//Debug.LogError("Setting primary weapon for player.");

		foreach(NetPlayer np in netPlayerList){
			if(np.PlayerID == playerID){
				np.PrimaryWeapon = gunID;
				gunID.AssignClientAuthority(np.Conn);
				//np.Player.RpcConnectPrimary(np.PrimaryWeapon);
				//Debug.LogError("Foudn the new owner id");
				break;
			}	
		}	
		ConnectPrimarys();
	}
	public void DropPrimary(NetworkIdentity playerID){
		

		foreach(NetPlayer np in netPlayerList){
			if(np.PlayerID == playerID){
				if(np.PrimaryWeapon == null){
					return;
				}

				np.PrimaryWeapon.RemoveClientAuthority(np.Conn);
				np.PrimaryWeapon = null;

				break;
			}	
		}	

		//Player_Base player = playerID.GetComponent<Player_Base>();
		//player.RpcDrop();

		ConnectPrimarys();
	}

	public void SetSecondary(NetworkIdentity playerID, NetworkIdentity gunID){
		//Debug.LogError("Setting secondary weapon for player.");

		foreach(NetPlayer np in netPlayerList){
			if(np.PlayerID == playerID){
				np.SecondaryWeapon = gunID;
				break;
			}	
		}	

	}

	public void ConnectPrimarys(){

		foreach(NetPlayer netPlayer in netPlayerList){
			NetworkIdentity primaryWeapon = netPlayer.PrimaryWeapon;
			netPlayer.Player.RpcConnectPrimary(primaryWeapon);
		}

	}

	public void ConnectSecondarys(){

		foreach(NetPlayer netPlayer in netPlayerList){
			NetworkIdentity secondaryWeapon = netPlayer.SecondaryWeapon;
			netPlayer.Player.RpcConnectSecondary(secondaryWeapon);
		}

	}


	private void SetPlayerNames(){
		for(int i = 0; i < netPlayerList.Count; i++){
			netPlayerList[i].Player.RpcSetPlayerName(netPlayerList[i].PlayerName); 
		}
	}


	public override void OnServerReady(NetworkConnection conn){
		NetworkServer.SetClientReady(conn);
		Debug.Log("Server is now ready");
		
	}

	public List<NetPlayer> NetPlayerList{
		get{return netPlayerList;}
	}



}


