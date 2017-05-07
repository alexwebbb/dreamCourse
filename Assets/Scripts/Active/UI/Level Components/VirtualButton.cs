using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    
    public bool Pressed { get; protected set; }

    public virtual void OnPointerDown(PointerEventData ped) {
        Pressed = true;
    }

    public virtual void OnPointerUp(PointerEventData ped) {
        Pressed = false;
    }

}
