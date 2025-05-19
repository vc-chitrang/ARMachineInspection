using UnityEngine;

public class ObjectRotator:MonoBehaviour {
    [Header("Rotation Settings")]
    public float rotationSpeed = 0.2f;
    public float smoothTime = 0.1f;

    private float targetRotationY;
    private float targetRotationX;
    private float currentRotationY;
    private float currentRotationX;
    private float rotationVelocityY;
    private float rotationVelocityX;
    private bool isDragging = false;

    private void Start() {
        currentRotationY = transform.eulerAngles.y;
        currentRotationX = transform.eulerAngles.x;
    }

    private void Update() {
#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouseInput();   // For Editor Testing
#endif

        HandleTouchInput();        // For Mobile Devices
        SmoothRotate();
    }

    void HandleTouchInput() {
        if (Input.touchCount == 1) {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved) {
                float deltaX = touch.deltaPosition.x;
                float deltaY = touch.deltaPosition.y;

                // Apply the deltas to target rotations
                targetRotationY -= deltaX * rotationSpeed;
                targetRotationX -= deltaY * rotationSpeed;
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

            // Apply the deltas to target rotations
            targetRotationY -= deltaX * rotationSpeed * 10f;
            targetRotationX += deltaY * rotationSpeed * 10f;
        }
    }

    void SmoothRotate() {
        currentRotationY = Mathf.SmoothDampAngle(currentRotationY,targetRotationY,ref rotationVelocityY,smoothTime);
        currentRotationX = Mathf.SmoothDampAngle(currentRotationX,targetRotationX,ref rotationVelocityX,smoothTime);

        // Apply the smooth rotation to the object
        transform.eulerAngles = new Vector3(currentRotationX,currentRotationY,transform.eulerAngles.z);
    }
}
