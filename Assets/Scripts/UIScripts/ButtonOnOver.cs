using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonOnOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public AK.Wwise.Event uiMouseOverSFX;
    public AK.Wwise.Event clicOnButtonSFX;

    public Sprite myMouseOverImage;
    private Sprite defaultImage;

    public bool isTextColorChanging = false;
    public bool emitSpecificSoundOnClick = false;
    public bool isButtonSwitchingScene = false;

    public Color overTextColor;
    private Color initialTextColor;
    private TextMeshProUGUI buttonText;

    public void Start()
    {
        defaultImage = GetComponent<Image>().sprite;

        if (isTextColorChanging)
        {
            buttonText = GetComponentInChildren<TextMeshProUGUI>();
            initialTextColor = buttonText.color;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        uiMouseOverSFX.Post(gameObject);

        if (myMouseOverImage)
        {
            GetComponent<Image>().sprite = myMouseOverImage;

            if (isTextColorChanging)
            {
                buttonText.color = overTextColor;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (myMouseOverImage)
        {
            GetComponent<Image>().sprite = defaultImage;

            if (isTextColorChanging)
            {
                buttonText.color = initialTextColor;
            }
        }
    }

    public void PlayClickSound()
    {
        if (emitSpecificSoundOnClick)
        {
            clicOnButtonSFX.Post(gameObject);
        }
    }
}
