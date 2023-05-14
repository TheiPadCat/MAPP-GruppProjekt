using UnityEngine;
using UnityEngine.UI;

public class GameObjectToggle : MonoBehaviour
{
    public GameObject targetObject;
    public Toggle toggle;

    void Start()
    {
        // Set the initial state of the toggle to match the initial state of the target object
        toggle.isOn = targetObject.activeSelf;
        // Add a listener to the toggle to handle changes in its state
        toggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    void OnToggleValueChanged(bool newValue)
    {
        // Update the state of the target object based on the new value of the toggle
        targetObject.SetActive(newValue);
    }
}