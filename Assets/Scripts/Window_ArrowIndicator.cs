using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Window_ : MonoBehaviour
{
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
    }
}
