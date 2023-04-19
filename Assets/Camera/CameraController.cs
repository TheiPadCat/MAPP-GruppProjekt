using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // The target to follow (in this case, the ship)
    public float followSpeed = 5f; // The speed at which the camera follows the target
    public float zoomSpeed = 1f; // The speed at which the camera adjusts its zoom level
    public float minZoom = 5f; // The minimum zoom level
    public float maxZoom = 15f; // The maximum zoom level
    public float zoomFactor = 0.5f; // The factor that determines how much the zoom level changes based on the ship's velocity

    private Camera cam; // The camera component
    private float currentZoom; // The current zoom level

    private void Start()
    {
        cam = GetComponent<Camera>(); // Get the camera component
        currentZoom = cam.orthographicSize; // Set the initial zoom level
    }

    private void Update()
    {
        // Calculate the new zoom level based on the ship's velocity
           float targetZoom = Mathf.Lerp(maxZoom, minZoom, target.GetComponent<Rigidbody2D>().velocity.magnitude * zoomFactor);
        // Smoothly adjust the camera's zoom level towards the target zoom level
          currentZoom = Mathf.Lerp(currentZoom, targetZoom, Time.deltaTime * zoomSpeed);
        //  cam.orthographicSize = currentZoom; // Apply the new zoom level to the camera


        Vector3 test = new Vector3(target.position.x, target.position.y, -10);
        // Smoothly move the camera towards the target's position
        transform.position = Vector3.Lerp(transform.position, test, Time.deltaTime * followSpeed);
    }

    private void LateUpdate()
    {
       
    }
}