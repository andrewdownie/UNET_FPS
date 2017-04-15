using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Characters.FirstPerson;

public class ColorSetup : NetworkBehaviour {
    [SerializeField]
    private Renderer playerRenderer;

    [SyncVar]
    private Color color;


    public override void OnStartClient()
    {
        base.OnStartClient();
        
        playerRenderer.material.color = color;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        color = RandomColor.GetRandPlayerColor();
    }


}
