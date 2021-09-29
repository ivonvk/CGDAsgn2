using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BtnEventTrigger : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IDragHandler
{
    public PlayerBtnDown BtnDownMethodToCall;
    public PlayerBtnUp BtnUpMethodToCall;
    public virtual void OnPointerUp(PointerEventData ped)
    {
        BtnUp(BtnUpMethodToCall);
    }
    public virtual void OnDrag(PointerEventData ped)
    {

    }
    public virtual void OnPointerDown(PointerEventData ped)
    {
        BtnDown(BtnDownMethodToCall);
    }

    public virtual void BtnDown(PlayerBtnDown method)
    {
        method();
    }
    public virtual void BtnUp(PlayerBtnUp method)
    {
        method();
    }
}
