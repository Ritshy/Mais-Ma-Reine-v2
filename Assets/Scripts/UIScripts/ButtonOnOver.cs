using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonOnOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public AK.Wwise.Event uiMouseOverSFX;

    public Sprite myMouseOverImage;
    private Sprite defaultImage;

    public void Start()
    {
        defaultImage = GetComponent<Image>().sprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        uiMouseOverSFX.Post(gameObject);

        if (myMouseOverImage)
        {
            GetComponent<Image>().sprite = myMouseOverImage;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (myMouseOverImage)
        {
            GetComponent<Image>().sprite = defaultImage;
        }
    }

}
