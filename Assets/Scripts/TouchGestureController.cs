using UnityEngine;

public class TouchGestureController:MonoBehaviour {
    [Header("Touch Input Sensitivity")]
    public float moveSpeed = 0.01f;   // Sensitivity for movement
    public float rotateSpeed = 0.2f;  // Sensitivity for rotation
    public float scaleFactor = 0.005f; // Sensitivity for scaling

    [Header("Constraints")]
    public float minScale = 0.5f;     // Minimum scale limit
    public float maxScale = 3f;       // Maximum scale limit

    // Variables to store previous touch positions for gesture calculations
    private Vector2 prevTouchPos1, prevTouchPos2;
    private Vector2 currTouchPos1, currTouchPos2;

    private Camera mainCamera;

    private void Awake() {
        mainCamera = Camera.main;
    }
    
    void Update() {
        HandleTouchGestures();
    }

    void HandleTouchGestures() {
        if (Input.touchCount == 2) {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            // Detect if either finger just started touching the screen
            if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began) {
                prevTouchPos1 = touch1.position;
                prevTouchPos2 = touch2.position;
            }
            // If both touches moved, perform scaling, rotation, and movement
            else if (touch1.phase == TouchPhase.Moved && touch2.phase == TouchPhase.Moved) {
                currTouchPos1 = touch1.position;
                currTouchPos2 = touch2.position;

                // Handle object scaling (pinch gesture)
                HandleScaling(prevTouchPos1,prevTouchPos2,currTouchPos1,currTouchPos2);

                // Handle object rotation based on the angle between two touches
                HandleRotation(prevTouchPos1,prevTouchPos2,currTouchPos1,currTouchPos2);

                // Handle object movement (translation)
                HandleMovement(touch1,touch2);

                // Update previous touch positions
                prevTouchPos1 = currTouchPos1;
                prevTouchPos2 = currTouchPos2;
            }
        }
    }

    /// <summary>
    /// Handles scaling the object using pinch gesture.
    /// </summary>
    void HandleScaling(Vector2 prevPos1,Vector2 prevPos2,Vector2 currPos1,Vector2 currPos2) {
        // Calculate the previous and current distances between the two fingers
        float prevDistance = Vector2.Distance(prevPos1,prevPos2);
        float currDistance = Vector2.Distance(currPos1,currPos2);

        // Calculate the scale factor based on the distance change
        float scaleChange = (currDistance - prevDistance) * scaleFactor;

        // Apply the scale change and clamp the scale between minScale and maxScale
        float newScale = Mathf.Clamp(this.transform.localScale.x + scaleChange,minScale,maxScale);

        // Set uniform scale for the object
        this.transform.localScale = new Vector3(newScale,newScale,newScale);
    }

    /// <summary>
    /// Handles rotating the object based on the angle between two fingers.
    /// </summary>
    void HandleRotation(Vector2 prevPos1,Vector2 prevPos2,Vector2 currPos1,Vector2 currPos2) {
        // Calculate the previous and current angles between the two fingers
        float prevAngle = Mathf.Atan2(prevPos2.y - prevPos1.y,prevPos2.x - prevPos1.x) * Mathf.Rad2Deg;
        float currAngle = Mathf.Atan2(currPos2.y - currPos1.y,currPos2.x - currPos1.x) * Mathf.Rad2Deg;

        // Calculate the difference in angle
        float angleDelta = currAngle - prevAngle;

        // Apply the rotation based on the angle delta and rotation sensitivity
        this.transform.Rotate(Vector3.up,-angleDelta * rotateSpeed,Space.World); // Rotate around the Y axis
    }

    /// <summary>
    /// Handles moving (translating) the object based on touch input.
    /// </summary>
    void HandleMovement(Touch touch1,Touch touch2) {
        // Calculate the average movement of the two touches
        Vector2 avgTouchDelta = (touch1.deltaPosition + touch2.deltaPosition) / 2;

        // Convert the touch delta to movement in world space, relative to the camera's view
        Vector3 right = mainCamera.transform.right;  // Camera's right direction
        Vector3 forward = mainCamera.transform.forward; // Camera's forward direction

        // Zero out the Y-axis to prevent vertical movement
        right.y = 0;
        forward.y = 0;

        // Normalize directions
        right.Normalize();
        forward.Normalize();

        // Translate the object in world space based on touch input
        Vector3 moveDirection = (avgTouchDelta.x * right + avgTouchDelta.y * forward) * moveSpeed;

        this.transform.position += moveDirection;
    }
}//TouchGestureController class end.
