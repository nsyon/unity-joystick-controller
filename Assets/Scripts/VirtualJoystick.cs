using System.Collections;
using UnityEngine;
using UnityEngine.UI;   // Image
using UnityEngine.EventSystems; // PointerEventData

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private Image bgImg;
    private Image joystickImg;

    public Vector3 InputDirection { set; get; }

    // Start is called before the first frame update
    void Start()
    {
        bgImg = GetComponent<Image>();
        joystickImg = transform.GetChild(0).GetComponent<Image>();
        InputDirection = Vector3.zero;
    }

    // Update is called once per frame

    public virtual void OnDrag(PointerEventData ped)
    {
        Debug.Log("OnDrag");
        Vector2 pos = Vector2.zero;
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(
                bgImg.rectTransform,
                ped.position, // current pointer position
                ped.pressEventCamera,
                out pos
             )
           )
        {
            Debug.Log("pos.x:" + pos.x + ", pos.y:" + pos.y + ", pivot.x:" + bgImg.rectTransform.pivot.x + ", pivot.y:" + bgImg.rectTransform.pivot.y+", sd.x:"+bgImg.rectTransform.sizeDelta.x + ", sd.y:"+bgImg.rectTransform.sizeDelta.y);
            pos.x = (pos.x / bgImg.rectTransform.sizeDelta.x); // relative position inside bgImg
            pos.y = (pos.y / bgImg.rectTransform.sizeDelta.y); // (0,300) -> (0,1)

            float x = (bgImg.rectTransform.pivot.x == 1) ? pos.x * 2 + 1 : pos.x * 2 - 1; // change scale (0,1) -> (-1,-1)
            float y = (bgImg.rectTransform.pivot.y == 1) ? pos.y * 2 + 1 : pos.y * 2 - 1; // *2 -> -1

            InputDirection = new Vector3(x, 0, y);
            InputDirection = (InputDirection.magnitude > 1) ? InputDirection.normalized : InputDirection;

            joystickImg.rectTransform.anchoredPosition = new Vector3(
                InputDirection.x * (bgImg.rectTransform.sizeDelta.x / 3),
                InputDirection.z * (bgImg.rectTransform.sizeDelta.y / 3));

            Debug.Log("pos.x: "+ pos.x + ", pos.y:"+ pos.y + ", x:" + x + ", y:" + y);
        }

    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
        Debug.Log("OnPointerDown");
        OnDrag(ped);
    }

    public virtual void OnPointerUp(PointerEventData ped)
    {
        Debug.Log("OnPointerUp");
        InputDirection = Vector3.zero;
        joystickImg.rectTransform.anchoredPosition = Vector3.zero;
    }
}
