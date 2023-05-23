using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchPosJoyStick : MonoBehaviour
{
    public GameObject touchIndicator;
    private bool newTouch = false;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                newTouch = true;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                touchIndicator.SetActive(false);
                newTouch = false;
            }

            if (newTouch)
            {
                Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                touchPos.z = 0;
                touchIndicator.transform.position = touchPos;
                touchIndicator.SetActive(true);
                newTouch = false;
            }
        }
        else
        {
            touchIndicator.SetActive(false);
            newTouch = false;
        }
    }
}

