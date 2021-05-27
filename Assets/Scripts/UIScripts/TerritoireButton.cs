using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TerritoireButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image myContour;

    public void OnPointerEnter(PointerEventData eventData)
    {
        myContour.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        myContour.enabled = false;
    }
}
