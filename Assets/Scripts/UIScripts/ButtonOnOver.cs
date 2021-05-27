using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonOnOver : MonoBehaviour, IPointerEnterHandler
{
    public AK.Wwise.Event uiMouseOverSFX;

    public void OnPointerEnter(PointerEventData eventData)
    {
        uiMouseOverSFX.Post(gameObject);
    }
}
