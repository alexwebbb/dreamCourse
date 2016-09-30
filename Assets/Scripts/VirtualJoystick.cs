using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler {

    public float time = .1f;

    private Image bgImg;
    private Image joystickImg;
    private Vector3 inputVector;

    void Start() {
        bgImg = GetComponent<Image>();
        joystickImg = transform.GetChild(0).GetComponent<Image>();
    }
    

    public virtual void OnDrag(PointerEventData ped) {
        Vector2 pos;
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImg.rectTransform, ped.position, ped.pressEventCamera, out pos)) {

            pos.x /= bgImg.rectTransform.sizeDelta.x;
            pos.y /= bgImg.rectTransform.sizeDelta.y;

            inputVector = new Vector3(pos.x * 2 + 1, 0f, pos.y * 2 - 1);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            // Move Joystick Image

            joystickImg.rectTransform.anchoredPosition = new Vector3(inputVector.x * (bgImg.rectTransform.sizeDelta.x / 3), inputVector.z * (bgImg.rectTransform.sizeDelta.x / 3));

            // Debug.Log(inputVector);
        }

    }

	public virtual void OnPointerDown(PointerEventData ped) {
        StopCoroutine("SlideBack");
        OnDrag(ped);
    }

    public virtual void OnPointerUp(PointerEventData ped) {
        inputVector = Vector3.zero;
        // 
        StartCoroutine("SlideBack");
    }

    IEnumerator SlideBack() {

        float elapsedTime = 0;
        Vector2 startPosition = joystickImg.rectTransform.anchoredPosition;

        while(elapsedTime < time) {
            joystickImg.rectTransform.anchoredPosition = Vector2.Lerp(startPosition, Vector2.zero, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            Debug.Log(elapsedTime);
            yield return new WaitForEndOfFrame();
        }

        joystickImg.rectTransform.anchoredPosition = Vector3.zero;
        yield break;
    }

    public float Horizontal() {
        if(inputVector.x != 0) {
            return inputVector.x;
        } else {
            return Input.GetAxis("Horizontal");
        }
    }

    public float Vertical() {
        if (inputVector.z != 0) {
            return inputVector.z;
        } else {
            return Input.GetAxis("Vertical");
        }
    }
}
