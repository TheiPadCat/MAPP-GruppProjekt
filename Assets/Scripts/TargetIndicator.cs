using UnityEngine;

public class TargetIndicator : MonoBehaviour
{
    public GameObject target;
    public GameObject arrow; 
    public float offScreenThreshold = 10f;

    private Camera mainCamera;
    private bool isIndicatorActive = true;
    private bool wasTargetVisible = false;

    private void Start()
    {
        mainCamera = Camera.main;

    }

    private void Update()
    {
        if (isIndicatorActive)
        {
            Vector3 targetViewportPosition = mainCamera.WorldToViewportPoint(target.transform.position);
            bool isTargetVisible = targetViewportPosition.z > 0 && targetViewportPosition.x > 0 &&
                                   targetViewportPosition.x < 1 && targetViewportPosition.y > 0 &&
                                   targetViewportPosition.y < 1;
            Debug.Log(isTargetVisible);

            if (isTargetVisible)
            {
                arrow.SetActive(false);
                wasTargetVisible = true;
            }
            else if (!wasTargetVisible)
            {
                arrow.SetActive(true);

                Vector3 screenEdge = mainCamera.ViewportToWorldPoint(new Vector3(
                    Mathf.Clamp(targetViewportPosition.x, 0.1f, 0.9f),
                    Mathf.Clamp(targetViewportPosition.y, 0.1f, 0.9f),
                    mainCamera.nearClipPlane));

                arrow.transform.position = new Vector3(screenEdge.x, screenEdge.y, 0);

                Vector3 direction = target.transform.position - arrow.transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                arrow.transform.rotation = Quaternion.Euler(0, 0, angle);
            }
            else if (wasTargetVisible && !isTargetVisible)
            {
                arrow.gameObject.SetActive(true);
                wasTargetVisible = false;
            }
            else
            {
                arrow.gameObject.SetActive(true);
            }
        }
    }
}