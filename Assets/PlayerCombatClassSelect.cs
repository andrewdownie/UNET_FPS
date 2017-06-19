using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombatClassSelect : MonoBehaviour {
	[SerializeField]
	Text selectedClassName;
	[SerializeField]
	Button breecherBtn, scoutBtn, survivalistBtn, riflemanBtn;
	[SerializeField]
	GameObject breecherPanel, scoutPanel, survivalistPanel, riflemanPanel;


	void Start () {
		breecherBtn.onClick.AddListener(ShowBreecher);
		scoutBtn.onClick.AddListener(ShowScout);
		survivalistBtn.onClick.AddListener(ShowSurvivalist);
		riflemanBtn.onClick.AddListener(ShowRifleman);
	}
	
	void Update () {
		
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
