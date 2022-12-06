using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickHandle : MonoBehaviour,IPointerDownHandler,IDragHandler,IPointerUpHandler
{
    public Transform HandleTransform;
    
    private Vector2 PointDownPos;
    public float MaxRadius;

    private Vector2 moveInput;
    public Vector2 MoveInput=>(moveInput);

    public void OnPointerDown(PointerEventData eventData)
    {
        PointDownPos = eventData.position;
    }
    public void OnDrag(PointerEventData eventData)
    {
        var dis = eventData.position - PointDownPos;
        var clamp  = Mathf.Clamp(dis.magnitude,0f,MaxRadius);
        var normalized  = clamp * dis.normalized;
        HandleTransform.localPosition = normalized;
        moveInput = normalized.normalized;
    }
        
    public void OnPointerUp(PointerEventData eventData)
    {
        HandleTransform.localPosition = Vector3.zero;
        moveInput = Vector2.zero;
    }
}