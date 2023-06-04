using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInventoryHelper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public PlayerInventory playerInventory;
    public int helperNumber;

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        playerInventory.HelperPointer(helperNumber, true);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        playerInventory.HelperPointer(helperNumber, false);
    }
}
