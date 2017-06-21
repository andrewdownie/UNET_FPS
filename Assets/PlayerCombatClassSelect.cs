using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerCombatClassSelect : NetworkBehaviour {
	[SerializeField]
	Text selectedClassName;
	[SerializeField]
	Button breecherBtn, scoutBtn, survivalistBtn, riflemanBtn, selectClassBtn;
	[SerializeField]
	GameObject breecherPanel, scoutPanel, survivalistPanel, riflemanPanel;


	public override void OnStartAuthority(){
		HUD.singleton.GetComponent<Canvas>().enabled = false;	
	}
	void Start () {
		breecherBtn.onClick.AddListener(ShowBreecher);
		scoutBtn.onClick.AddListener(ShowScout);
		survivalistBtn.onClick.AddListener(ShowSurvivalist);
		riflemanBtn.onClick.AddListener(ShowRifleman);
		selectClassBtn.onClick.AddListener(SelectClass);

		//TODO: need a way to check what classes have been selected already.(RPC sent to each client when they join)
	}
	
	void Update () {
		
	}

	void SelectClass(){
		//Send command to server with what class to spawn as.
		//Spawn the player on the server.
		//Delete this gameobject.
		CmdSelectClass(selectedClassName.text);
	}


	[Command]
	void CmdSelectClass(string selectedClass){
		NetworkIdentity netID = GetComponent<NetworkIdentity>();

		Net_Manager.instance.SpawnPlayerAsClass(selectedClass, netID);

	}

	void ShowBreecher(){
		selectedClassName.text = "Breecher";
		breecherPanel.SetActive(true);
		scoutPanel.SetActive(false);
		survivalistPanel.SetActive(false);
		riflemanPanel.SetActive(false);
	}
	void ShowScout(){
		selectedClassName.text = "Scout";
		breecherPanel.SetActive(false);
		scoutPanel.SetActive(true);
		survivalistPanel.SetActive(false);
		riflemanPanel.SetActive(false);
	}
	void ShowSurvivalist(){
		selectedClassName.text = "Survivalist";
		breecherPanel.SetActive(false);
		scoutPanel.SetActive(false);
		survivalistPanel.SetActive(true);
		riflemanPanel.SetActive(false);
	}
	void ShowRifleman(){
		selectedClassName.text = "Rifleman";
		breecherPanel.SetActive(false);
		scoutPanel.SetActive(false);
		survivalistPanel.SetActive(false);
		riflemanPanel.SetActive(true);
	}

}
