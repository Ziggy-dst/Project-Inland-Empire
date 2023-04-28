using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindowClickPopUp : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventdata) 
    {
        this.gameObject.transform.SetAsLastSibling();
    }

}
