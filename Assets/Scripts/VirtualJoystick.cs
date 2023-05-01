using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public Image joystickBackground;
    public Image joystickHandle;
    public float maxSteeringAngle = 30f;
    public float driftForce = 10f;

    public Vector2 inputVector;
    private Rigidbody2D rb;

    [SerializeField] Transform trans;
    private Quaternion lastRotation;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        trans = GameObject.Find("Player").transform;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickBackground.rectTransform, eventData.position, eventData.pressEventCamera, out localPosition))
        {
            localPosition.x /= joystickBackground.rectTransform.sizeDelta.x;
            localPosition.y /= joystickBackground.rectTransform.sizeDelta.y;

            
            float distanceFromCenter = localPosition.magnitude;

            
            float maxDistance = 0.5f;
            float factor = Mathf.Clamp01(distanceFromCenter / maxDistance);
            Debug.Log(factor);
            inputVector = new Vector2(localPosition.x, localPosition.y) * factor;
            lastRotation = trans.transform.rotation;

            UpdateJoystickHandlePosition();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector.Normalize();
        inputVector = Vector2.zero;
        trans.transform.rotation = lastRotation;
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

    public float GetSteeringAngle()
    {
        Vector2 forward = Quaternion.Euler(0, 0, transform.eulerAngles.z) * new Vector2(0.0f, 1.0f);
        Vector2 inputDir = new Vector2(inputVector.x, inputVector.y);
        float angle = Vector2.SignedAngle(inputDir, forward) * -1;
        float maxAngle = Mathf.Clamp(angle, -maxSteeringAngle, maxSteeringAngle);
        return maxAngle / maxSteeringAngle; 
    }

    public float GetDriftForce()
    {
        Vector2 forward = new Vector2(0.0f, 1.0f);
        float steeringRightAngle;
        if (rb.angularVelocity > 0)
        {
            steeringRightAngle = -90;
        }
        else
        {
            steeringRightAngle = 90;
        }
        Vector2 rightAngleFromForward = Quaternion.AngleAxis(steeringRightAngle, Vector3.forward) * forward;
        float driftForce = Vector2.Dot(rb.velocity, rb.GetRelativeVector(rightAngleFromForward.normalized));
        return driftForce * driftForce * Mathf.Sign(driftForce) * driftForce * driftForce * driftForce * driftForce * driftForce * driftForce * driftForce * driftForce * driftForce * driftForce * driftForce * driftForce * driftForce * driftForce * driftForce * driftForce * driftForce * driftForce * driftForce * driftForce * driftForce * driftForce;
    }
}