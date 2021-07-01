using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneSwitcher : MonoBehaviour
{
    public static SceneSwitcher Instance;
    public AkAmbient akAmbient;

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
        akAmbient = FindObjectOfType<AkAmbient>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchToMenuTerritoire()
    {
        akAmbient.Stop(0);
        GameManager.Instance.LoadCharismeValueBetweenScenes();
        SceneManager.LoadScene("MenuTerritoire_Scene");
    }

    public void SwitchToFirstScreen()
    {
        akAmbient.Stop(0);
        GameManager.Instance.LoadCharismeValueBetweenScenes();
        SceneManager.LoadScene("GameFirstScreen_Scene");
    }

    public void SwitchToTerritoire01Cinematique()
    {
        akAmbient.Stop(0);
        GameManager.Instance.LoadCharismeValueBetweenScenes();
        SceneManager.LoadScene("Territoire01_Cinematic_Scene");
    }

    public void SwitchToTerritoire01()
    {
        akAmbient.Stop(0);
        GameManager.Instance.LoadCharismeValueBetweenScenes();
        SceneManager.LoadScene("Territoire01_Scene");
    }

    public void SwitchToTerritoire02Cinematique()
    {
        akAmbient.Stop(0);
        GameManager.Instance.LoadCharismeValueBetweenScenes();
        SceneManager.LoadScene("Territoire02_Cinematic");
    }

    public void SwitchToTerritoire02()
    {
        akAmbient.Stop(0);
        GameManager.Instance.LoadCharismeValueBetweenScenes();
        SceneManager.LoadScene("Territoire02_Scene");
    }

    public void SwitchToScene(string scene)
    {
        akAmbient.Stop(0);
        GameManager.Instance.LoadCharismeValueBetweenScenes();
        SceneManager.LoadScene(scene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
