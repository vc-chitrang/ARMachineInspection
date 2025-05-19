using UnityEngine;

public class OrbitCamera:MonoBehaviour {
    [Header("Target & Settings")]
    [SerializeField] public Transform target;           // The object to orbit around
    public float distance = 3.0f;      // Distance from the object
    public float orbitSpeed = 0.5f;    // Sensitivity for orbiting
    public float smoothTime = 0.1f;    // Smooth transition time
    public float verticalClampMin = -20f;
    public float verticalClampMax = 60f;

    [Header("Zoom Settings")]
    public float minZoom = 1.0f;
    public float maxZoom = 10.0f;
    public float zoomSpeed = 0.5f;

    private float currentX;
    private float currentY;
    private float targetX;
    private float targetY;
    private float velocityX;
    private float velocityY;
    private float targetDistance;
    private float distanceVelocity;
    private bool isDragging = false;

    private void Start() {
        // Initialize the camera position
        currentX = transform.eulerAngles.y;
        currentY = transform.eulerAngles.x;
        targetDistance = distance;
    }

    private void Update() {
#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouseInput();
        HandleMouseZoom();
#endif
        HandleTouchInput();
        HandleTouchZoom();
        SmoothOrbit();
    }

    void HandleTouchInput() {
        if (Input.touchCount == 1) {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved) {
                float deltaX = touch.deltaPosition.x;
                float deltaY = touch.deltaPosition.y;

                targetX += deltaX * orbitSpeed;
                targetY -= deltaY * orbitSpeed;
                targetY = Mathf.Clamp(targetY,verticalClampMin,verticalClampMax);
            }
        }
    }

    void HandleTouchZoom() {
        if (Input.touchCount == 2) {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            Vector2 prevTouch0 = touch0.position - touch0.deltaPosition;
            Vector2 prevTouch1 = touch1.position - touch1.deltaPosition;

            float prevMagnitude = (prevTouch0 - prevTouch1).magnitude;
            float currentMagnitude = (touch0.position - touch1.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            targetDistance -= difference * zoomSpeed * 0.01f;
            targetDistance = Mathf.Clamp(targetDistance,minZoom,maxZoom);
        }
    }

    void HandleMouseInput() {
        if (Input.GetMouseButtonDown(0)) {
            isDragging = true;
        }
        if (Input.GetMouseButtonUp(0)) {
            isDragging = false;
        }

        if (isDragging) {
            float deltaX = Input.GetAxis("Mouse X");
            float deltaY = Input.GetAxis("Mouse Y");

            targetX += deltaX * orbitSpeed * 10f;
            targetY -= deltaY * orbitSpeed * 10f;
            targetY = Mathf.Clamp(targetY,verticalClampMin,verticalClampMax);
        }
    }

    void HandleMouseZoom() {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f) {
            targetDistance -= scroll * zoomSpeed * 5f;
            targetDistance = Mathf.Clamp(targetDistance,minZoom,maxZoom);
        }
    }

    void SmoothOrbit() {
        // Smooth damp for smooth transition
        currentX = Mathf.SmoothDampAngle(currentX,targetX,ref velocityX,smoothTime);
        currentY = Mathf.SmoothDampAngle(currentY,targetY,ref velocityY,smoothTime);
        distance = Mathf.SmoothDamp(distance,targetDistance,ref distanceVelocity,smoothTime);

        // Compute the orbit position
        Quaternion rotation = Quaternion.Euler(currentY,currentX,0);
        Vector3 offset = rotation * new Vector3(0,0,-distance);

        // Set the camera position and look at the target
        transform.position = target.position + offset;
        transform.LookAt(target);
    }
}
