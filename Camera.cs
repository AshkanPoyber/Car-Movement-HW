using UnityEngine;

public class CarFollowCamera : MonoBehaviour
{
    public Transform target; // The car to follow
    public Vector3 offset = new Vector3(0f, 2f, -10f);
    public float followSpeed = 10f;
    public float rotationSpeed = 5f;
    public float zoomSpeed = 2f; //
    public float maxZoomOut = 15f;
    public float minZoomIn = 5f;

    private Rigidbody targetRb;
    private Vector3 velocity = Vector3.zero; // Used for SmoothDamp

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("CarFollowCamera: Target not set for camera to follow.");
            return;
        }

        targetRb = target.GetComponent<Rigidbody>();
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, followSpeed * Time.deltaTime);

        Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        AdjustZoom();
    }

    void AdjustZoom()
    {
        if (targetRb == null) return;

        float carSpeed = targetRb.linearVelocity.magnitude;
        float targetZoom = Mathf.Clamp(Mathf.Lerp(minZoomIn, maxZoomOut, carSpeed / 30f), minZoomIn, maxZoomOut); // Adjust divisor for sensitivity
        offset.z = Mathf.Lerp(offset.z, -targetZoom, zoomSpeed * Time.deltaTime);
    }
}
