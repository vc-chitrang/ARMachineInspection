using UnityEngine;

public class OrbitCamera:MonoBehaviour {
    [Header("Target & Settings")]
    [SerializeField] public Transform target;           // The object to orbit around
    public float distance = 3.0f;      // Distance from the object
    public float orbitSpeed = 0.5f;    // Sensitivity for orbiting
    public float smoothTime = 0.1f;    // Smooth transition time
    public float verticalClampMin = -20f;
    public float verticalClampMax = 60f;

    private float currentX;
    private float currentY;
    private float targetX;
    private float targetY;
    private float velocityX;
    private float velocityY;
    private bool isDragging = false;

    private void Start() {
        // Initialize the camera position
        currentX = transform.eulerAngles.y;
        currentY = transform.eulerAngles.x;
    }

    private void Update() {
#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouseInput();
#endif

        HandleTouchInput();
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

    void SmoothOrbit() {
        // Smooth damp for smooth transition
        currentX = Mathf.SmoothDampAngle(currentX,targetX,ref velocityX,smoothTime);
        currentY = Mathf.SmoothDampAngle(currentY,targetY,ref velocityY,smoothTime);

        // Compute the orbit position
        Quaternion rotation = Quaternion.Euler(currentY,currentX,0);
        Vector3 offset = rotation * new Vector3(0,0,-distance);

        // Set the camera position and look at the target
        transform.position = target.position + offset;
        transform.LookAt(target);
    }
}
