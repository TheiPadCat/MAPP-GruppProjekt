using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public Image joystickBackground;
    public Image joystickHandle;
    public Vector2 inputVector;

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickBackground.rectTransform, eventData.position, eventData.pressEventCamera, out localPosition))
        {
            localPosition.x /= joystickBackground.rectTransform.sizeDelta.x;
            localPosition.y /= joystickBackground.rectTransform.sizeDelta.y;

            inputVector = new Vector2(localPosition.x, localPosition.y);

            if (inputVector.magnitude > 1)
            {
                inputVector.Normalize();
            }

            UpdateJoystickHandlePosition();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        UpdateJoystickHandlePosition();
    }

    private void UpdateJoystickHandlePosition()
    {
        joystickHandle.rectTransform.anchoredPosition = inputVector * (joystickBackground.rectTransform.sizeDelta.x * 0.5f);
    }

    public float GetHorizontalValue()
    {
        return inputVector.x;
    }

    public float GetVerticalValue()
    {
        return inputVector.y;
    }
}
