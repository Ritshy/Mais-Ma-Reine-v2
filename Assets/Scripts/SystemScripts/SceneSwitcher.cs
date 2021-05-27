using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneSwitcher : MonoBehaviour
{
    public static SceneSwitcher Instance;


    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        //DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchToMenuTerritoire()
    {
        GameManager.Instance.LoadCharismeValueBetweenScenes();
        SceneManager.LoadScene("MenuTerritoire_Scene");
    }

    public void SwitchToFirstScreen()
    {
        GameManager.Instance.LoadCharismeValueBetweenScenes();
        SceneManager.LoadScene("GameFirstScreen_Scene");
    }

    public void SwitchToTerritoire01Cinematique()
    {
        GameManager.Instance.LoadCharismeValueBetweenScenes();
        SceneManager.LoadScene("Territoire01_Cinematic_Scene");
    }

    public void SwitchToTerritoire01()
    {
        GameManager.Instance.LoadCharismeValueBetweenScenes();
        SceneManager.LoadScene("Territoire01_Scene");
    }

    public void SwitchToTerritoire02Cinematique()
    {
        GameManager.Instance.LoadCharismeValueBetweenScenes();
        SceneManager.LoadScene("Territoire02_Cinematic_Scene");
    }

    public void SwitchToTerritoire02()
    {
        GameManager.Instance.LoadCharismeValueBetweenScenes();
        SceneManager.LoadScene("Territoire02_Scene");
    }
}
