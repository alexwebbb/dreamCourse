using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler {

    public float timeToReturn = .1f;

    public float GetHorizontal { get { return inputVector.x; } }
    public float GetVertical { get { return inputVector.y; } }

    Image bgImg;
    Image joystickImg;
    Vector2 inputVector;

    void Start() {
        bgImg = GetComponent<Image>();
        joystickImg = transform.GetChild(0).GetComponent<Image>();
    }
    
    public virtual void OnDrag(PointerEventData ped) {
        Vector2 pos;
        // checks if you are clicking within the rect transform of the joystick
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImg.rectTransform, ped.position, ped.pressEventCamera, out pos)) {

            // position gives the coordinates within the rectangle, in relation to the pivot
            // size delta is used to convert the raw position into a 0 to 1 value
            pos.x /= bgImg.rectTransform.sizeDelta.x;
            pos.y /= bgImg.rectTransform.sizeDelta.y;

            // the formula used below converts the normalized pos into a value between -1 and 1
            float x = pos.x * 2 - 1;
            float y = pos.y * 2 - 1;

            // input vector is accessed by properties
            inputVector = new Vector2(x, y);
            // if the inputVector magnitude is greater than 1, return the normalized vector
            inputVector = (inputVector.sqrMagnitude > 1.0f) ? inputVector.normalized : inputVector;
            
            // Move Joystick Image
            joystickImg.rectTransform.anchoredPosition = new Vector3(inputVector.x * (bgImg.rectTransform.sizeDelta.x / 3), inputVector.y * (bgImg.rectTransform.sizeDelta.x / 3));
        }

    }

	public virtual void OnPointerDown(PointerEventData ped) {
        StopCoroutine("SlideBack");
        OnDrag(ped);
    }

    public virtual void OnPointerUp(PointerEventData ped) {
        inputVector = Vector3.zero; 
        StartCoroutine("SlideBack");
    }

    IEnumerator SlideBack() {

        float elapsedTime = 0;
        Vector2 startPosition = joystickImg.rectTransform.anchoredPosition;

        while(elapsedTime < timeToReturn) {
            joystickImg.rectTransform.anchoredPosition = Vector2.Lerp(startPosition, Vector2.zero, elapsedTime / timeToReturn);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        joystickImg.rectTransform.anchoredPosition = Vector3.zero;
        yield break;
    }
}
