using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Window_ArrowIndicator : MonoBehaviour
{
    [SerializeField] Camera uiCamera;
    private Vector3 targetPosition;
    private RectTransform poinerRectTransform;

    private void Awake()
    {
        targetPosition = GameObject.Find("LootDude").transform.position;
        poinerRectTransform = transform.Find("Pointer").GetComponent<RectTransform>();
    }

    private void Update()
    {
        Vector3 toPos = targetPosition;
        Vector3 fromPos = Camera.main.transform.position;
        fromPos.z = 0f;
        Vector3 direction = (toPos - fromPos).normalized;
        float angle = UtilsClass.GetAngleFromVector(direction);
        poinerRectTransform.localEulerAngles = new Vector3(0, 0, angle);

        float borderSize = 100f;
        Vector3 targetPosScreenPoint = Camera.main.WorldToScreenPoint(targetPosition);
        bool isOffScreen = targetPosScreenPoint.x <= borderSize || targetPosScreenPoint.x >= Screen.width - borderSize || targetPosScreenPoint.y <= borderSize || targetPosScreenPoint.y >= Screen.height - borderSize;
        Debug.Log(isOffScreen + " " + targetPosScreenPoint);

        if (isOffScreen)
        {
            Vector3 cappedTargetScreenPos = targetPosScreenPoint;
            if(cappedTargetScreenPos.x <= borderSize)
            {
                cappedTargetScreenPos.x = borderSize;
            }
            if(cappedTargetScreenPos.x >= Screen.width - borderSize)
            {
                cappedTargetScreenPos.x = Screen.width - borderSize;
            }
            if(cappedTargetScreenPos.y <= borderSize)
            {
                cappedTargetScreenPos.y = borderSize;
            }
            if(cappedTargetScreenPos.y >= Screen.height - borderSize)
            {
                cappedTargetScreenPos.y = Screen.height - borderSize;
            }

            Vector3 pointerWorldPosition = uiCamera.ScreenToWorldPoint(cappedTargetScreenPos);
            poinerRectTransform.position = pointerWorldPosition;
            poinerRectTransform.localPosition = new Vector3(poinerRectTransform.localPosition.x, poinerRectTransform.localPosition.y, 0f);

        }
        else
        {
            Vector3 pointerWorldPos = uiCamera.ScreenToWorldPoint(targetPosScreenPoint);
            poinerRectTransform.position = pointerWorldPos;
            poinerRectTransform.localPosition = new Vector3(poinerRectTransform.localPosition.x, poinerRectTransform.localPosition.y, 0f);
        }
    }
}
