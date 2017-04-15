using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public abstract class HUD_Base<T> : MonoBehaviour where T : HUD_Base<T>  {

    public static T singleton;

    [SerializeField]
    private bool disableCanvasOnScene0;
    
    
    protected Canvas canvas;

    public static void CanvasEnabled(bool enabled)
    {
        if(singleton != null)
        {
            singleton.canvas.enabled = enabled;
        }
        
    }


    /////
    ///// Unity Events
    /////
    void Awake()
    {
        if (FindObjectsOfType(typeof(Canvas)).Length > 1)
        {
            Debug.LogWarning("Destroying excess Canvas's...");
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        canvas = GetComponent<Canvas>();
    }
    
    void Start()
    {
        singleton = (T)this;
        CheckIfCanvasShouldBeDisabled();
    }

    void OnLevelWasLoaded()
    {
        CheckIfCanvasShouldBeDisabled();
    }



    /////
    ///// Helper Functions
    /////
    void CheckIfCanvasShouldBeDisabled()
    {
        CanvasEnabled(true);


        if (disableCanvasOnScene0)
        {
            if(SceneManager.GetActiveScene().buildIndex == 0)
            {
                CanvasEnabled(false);
                return;
            }
        }

    }


}
