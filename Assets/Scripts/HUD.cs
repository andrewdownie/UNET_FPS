using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUD : HUD_Base<HUD> {

    /////
    ///// GUI Element References
    /////
    [Header("Ammo")]
    [SerializeField]
    Image clipAmmoImage;
    [SerializeField]
    Text clipAmmoText;

    [Header("Health")]
    [SerializeField]
    Text healthText;
    [SerializeField]
    Image healthImage;

    [Header("Health Pack")]
    [SerializeField]
    Text healthPackText;
    [SerializeField]
    Image healthPackImage;

    [Header("Bullet Inventory")]
    [SerializeField]
    Text bulletInventoryText;
    [SerializeField]
    Image bulletInventoryImage;

    [Header("Hit Marker")]
    [SerializeField]
    Image hitMarker;

    [Header("Dying and respawning")]
    [SerializeField]
    Text deathMessage;
    [SerializeField]
    Image respawnButton;
    [SerializeField]
    Text respawnButtonText;

    [Header("Aimer")]
    Image aimer;


    /////
    ///// Public Manipulation Functions
    /////


    public static void SetClipAmmo(int current, int max)
    {
        singleton.clipAmmoText.enabled = true;
        singleton.clipAmmoText.text = current + "/" + max;
    }


    public static void SetHealth(float current, float max)
    {
        singleton.healthImage.enabled = true;
        float ratio = current / max;
        singleton.healthImage.fillAmount = ratio;
        singleton.healthText.text = current + "/" + max;
    }
    

    public static void SetHealthPackVisible(bool visible)
    {
        singleton.healthPackImage.enabled = visible;
        singleton.healthPackText.enabled = visible;
    }
    

    public static void SetInventoryAmmo(int amount)
    {
        singleton.bulletInventoryImage.enabled = true;
        singleton.bulletInventoryText.text = amount.ToString();
    }


    public static void SetHitMarkerVisible(bool visible)
    {
        singleton.hitMarker.enabled = visible;
    }


    public static void SetRespawnButtonVisible(bool visible)
    {
        singleton.deathMessage.enabled = visible;
        singleton.respawnButton.enabled = visible;
        singleton.respawnButtonText.enabled = visible;
    }

    public static void SetAimerVisible(bool visible)
    {
        singleton.aimer.enabled = visible;
    }
}
