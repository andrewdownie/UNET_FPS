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
			Debug.Log("There is more than one Net_Manager instance, you fool");
		}
	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		Debug.Log("Adding player to game...");
		GameObject newPlayer = Instantiate(playerPrefab);

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
				//np.Player.RpcConnectPrimary(np.PrimaryWeapon);
				//Debug.LogError("Foudn the new owner id");
				break;
			}	
		}	
		ConnectPrimarys();
	}
	public void DropPrimary(NetworkIdentity playerID, NetworkIdentity gunID){
		//TODO: this
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


