using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class Hello : MonoBehaviour
{
    RectTransform rect { get { if (_rect == null) _rect = GetComponent<RectTransform>(); return _rect; } }
    RectTransform _rect;
    [SerializeField] bool edit;
    [SerializeField] bool _symmetricalXMode;
    [SerializeField] bool _symmetricalYMode;
    public bool symmetricalXMode { get { return _symmetricalXMode; } set { _symmetricalXMode = value; CheckAndSet(); } }

    public bool symmetricalYMode { get { return _symmetricalYMode; } set { _symmetricalYMode = value; CheckAndSet(); } }

    [Range(0, 1)]
    [SerializeField] float _xAnchorMin;
    [Range(0, 1)]
    [SerializeField] float _xAnchorMax = 1;
    [Range(0, 1)]
    [SerializeField] float _yAnchorMin;
    [Range(0, 1)]
    [SerializeField] float _yAnchorMax = 1;
    public float xAnchorMin { get { return _xAnchorMin; } set { _xAnchorMin = value; CheckAndSet(); } }

    public float xAnchorMax { get { return _xAnchorMax; } set { _xAnchorMax = value; CheckAndSet(); } }

    public float yAnchorMin { get { return _yAnchorMin; } set { _yAnchorMin = value; CheckAndSet(); } }

    public float yAnchorMax { get { return _yAnchorMax; } set { _yAnchorMax = value; CheckAndSet(); } }

    // [SerializeField] [HideInInspector] Vector2 offsetMin;
    // [SerializeField] [HideInInspector] Vector2 offsetMax;
    public void SetMargin(float f) { margin = f; }

    [Range(-1, 15)]
    [SerializeField] float _margin = -1;
    public float margin { get { return _margin; } set { _margin = value; CheckAndSet(); } }
    void CheckAndSet()
    {
        if (symmetricalXMode) _xAnchorMax = 1 - _xAnchorMin;
        if (symmetricalYMode) _yAnchorMax = 1 - _yAnchorMin;
        SetValues();

    }
    void Reset()
    {
        PrepareRect();
        GetValues();
    }
    void OnValidate()
    {
        if (symmetricalXMode) xAnchorMax = 1 - xAnchorMin;
        if (symmetricalYMode) yAnchorMax = 1 - yAnchorMin;
        //      if (Application.isPlaying) return;
#if UNITY_EDITOR
        bool isSelected = false;
        foreach (var s in Selection.gameObjects)
        {
            if (s == gameObject) isSelected = true;
        }
        if (!isSelected) { edit = false; return; }
        Undo.RecordObject(rect, "RectAnchor");
#endif
        if (edit)
        {
            SetValues();
        }
        else
            GetValues();
    }

    void SetValues()
    {
        rect.anchorMin = new Vector2(xAnchorMin, yAnchorMin);
        rect.anchorMax = new Vector2(xAnchorMax, yAnchorMax);
        if (margin != -1)
        {
            rect.offsetMin = new Vector2(margin * margin, margin * margin);
            rect.offsetMax = new Vector2(-(margin * margin), -(margin * margin));
        }
    }
    void GetValues()
    {
        _xAnchorMin = rect.anchorMin.x;
        _xAnchorMax = rect.anchorMax.x;
        _yAnchorMin = rect.anchorMin.y;
        _yAnchorMax = rect.anchorMax.y;
    }

    void PrepareRect()
    {
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMax = Vector2.zero;
        rect.offsetMin = Vector2.zero;
        rect.localScale = Vector3.one;
    }
}