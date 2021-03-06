using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using MilkShake;

public class CameraZooming : MonoBehaviour
{
    private CinemachineVirtualCamera myCamera;
    public GameObject mapScreen;
    private bool mapScreenPause;

    public int maxZoomInValue;
    public int maxZoomOutValue;

    private bool isMenuPauseOpenByEscape;

    private Shaker myShaker;

    public ShakePreset shakePreset;

    #region Singleton

    public static CameraZooming Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        myCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        myShaker = GetComponentInChildren<Shaker>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (mapScreenPause == false)
            {
                isMenuPauseOpenByEscape = true;
                ActivatePauseScreen();
            }
            else
            {
                isMenuPauseOpenByEscape = false;
                DesactivatePauseScreen();
            }
        }
        else if (myCamera.m_Lens.OrthographicSize >= maxZoomOutValue && Input.GetAxis("Mouse ScrollWheel") <= -0.2)
        {
            ActivatePauseScreen();
        }
        else if (isMenuPauseOpenByEscape == false && myCamera.m_Lens.OrthographicSize < maxZoomOutValue && mapScreenPause == true)
        {
            isMenuPauseOpenByEscape = false;
            DesactivatePauseScreen();
        }

        if (myCamera.m_Lens.OrthographicSize > maxZoomInValue && GameManager.Instance.isGamePaused == false)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                myCamera.m_Lens.OrthographicSize = Mathf.Max(myCamera.m_Lens.OrthographicSize - 1, 1);
            }
        }

        if (myCamera.m_Lens.OrthographicSize < maxZoomOutValue && GameManager.Instance.isGamePaused == false)
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                myCamera.m_Lens.OrthographicSize = Mathf.Max(myCamera.m_Lens.OrthographicSize + 1, 1);
            }
        }        
    }

    public void ActivatePauseScreen()
    {
        mapScreen.SetActive(true);
        mapScreenPause = true;
    }

    public void DesactivatePauseScreen()
    {
        mapScreen.SetActive(false);
        mapScreenPause = false;
    }

    public void ShakeScreen()
    {
        myShaker.Shake(shakePreset);
    }
}
