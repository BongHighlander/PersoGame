using UnityEngine;

public class GameManager : MonoBehaviour
{

    public PlayManager playManager;
    
    public static GameManager instance = null;
    private Carte carte;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
    }


    protected void Start()
    {
        carte = GetComponent<Carte>();
        carte.SetupScene();
        playManager.carte = carte;
    }
    
}