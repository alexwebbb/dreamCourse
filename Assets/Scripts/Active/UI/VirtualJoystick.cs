using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler {

    public float timeToReturn = .1f;

    public float GetHorizontal { get { return inputVector.x; } }
    public float GetVertical { get { return inputVector.z; } }

    Image bgImg;
    Image joystickImg;
    Vector3 inputVector;

    void Start() {
        bgImg = GetComponent<Image>();
        joystickImg = transform.GetChild(0).GetComponent<Image>();
    }
    
    public virtual void OnDrag(PointerEventData ped) {
        Vector2 pos;
        // checks if you are clicking within the rect transform of the joystick
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImg.rectTransform, ped.position, ped.pressEventCamera, out pos)) {

            Debug.Log("pos x: " + pos.x + ", size delta x: " + bgImg.rectTransform.sizeDelta.x);
            // position gives the coordinates within the rectangle, in relation to the pivot
            // size delta is used to convert the raw position into a 0 to 1 value
            pos.x /= bgImg.rectTransform.sizeDelta.x;
            pos.y /= bgImg.rectTransform.sizeDelta.y;

            Debug.Log("x pivot: " + bgImg.rectTransform.pivot.x + ", pos x after div: " + pos.x);
            // the formula used below converts the normalized pos into a value between -1 and 1
            // the ternary operator is being used in case you for some reason you needed to change the pivot to 0
            // however, the pivot is totally arbitrary as long as the math is correct, so the extra case is
            // totally unnecessary, and confusing to have in there
            //
            // going to save this in git and then refactor this
            float x = (bgImg.rectTransform.pivot.x == 1) ? pos.x * 2 + 1 : pos.x * 2 - 1;
            float y = (bgImg.rectTransform.pivot.y == 1) ? pos.y * 2 + 1 : pos.y * 2 - 1;

            inputVector = new Vector3(x, 0f, y);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;
            Debug.Log("input vector : " + inputVector);

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
        StartCoroutine("SlideBack");
    }

    IEnumerator SlideBack() {

        float elapsedTime = 0;
        Vector2 startPosition = joystickImg.rectTransform.anchoredPosition;

        while(elapsedTime < timeToReturn) {
            joystickImg.rectTransform.anchoredPosition = Vector2.Lerp(startPosition, Vector2.zero, elapsedTime / timeToReturn);
            elapsedTime += Time.deltaTime;
            // Debug.Log(elapsedTime);
            yield return new WaitForEndOfFrame();
        }

        joystickImg.rectTransform.anchoredPosition = Vector3.zero;
        yield break;
    }
}
