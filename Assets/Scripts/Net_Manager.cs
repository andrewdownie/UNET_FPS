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
		SetPlayerNames();
	}

	public void PickupPrimary(NetworkIdentity playerID, NetworkIdentity gunID){
		foreach(NetPlayer np in netPlayerList){
			if(np.PlayerID == playerID){
				np.PrimaryWeapon = gunID;
				break;
			}	
		}	
	}
	public void DropPrimary(NetworkIdentity playerID, NetworkIdentity gunID){
		//TODO: this
	}



	public void ConnectWeapons(){
		foreach(NetPlayer netPlayer in netPlayerList){
			if(netPlayer.PrimaryWeapon != null){
				NetworkIdentity primaryWeapon = netPlayer.PrimaryWeapon;
				netPlayer.Player.RpcConnectPrimary(primaryWeapon);
			}

			if(netPlayer.SecondaryWeapon != null){
				NetworkIdentity secondaryWeapon = netPlayer.SecondaryWeapon;
				netPlayer.Player.RpcConnectSecondary(secondaryWeapon);
			}
		}
	}

	private void SetPlayerNames(){
		for(int i = 0; i < netPlayerList.Count; i++){
			netPlayerList[i].Player.RpcSetPlayerName("Player" + i); 
		}
	}


	public override void OnServerReady(NetworkConnection conn){
		NetworkServer.SetClientReady(conn);
		Debug.Log("Server is now ready");
		
	}



}


