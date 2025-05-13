using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARRaycastManager))]
public class PlaceObjectOnPlaneNewInput : PressInputBase
{
    [SerializeField] private int index = 0;
    [SerializeField] private List<GameObject> mObjectList;

    GameObject spawnedObject;
    bool isPressed;
    private ARRaycastManager aRRaycastManager;
    List<ARRaycastHit> hits = new List<ARRaycastHit>();

    protected override void Awake() {
        base.Awake();
        aRRaycastManager = GetComponent<ARRaycastManager>();
    }

    private void Update() {
        if (Pointer.current == null || isPressed == false) {
            return;
        }

        var touchPosition = Pointer.current.position.ReadValue();
        if (aRRaycastManager.Raycast(touchPosition,hits,UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon)) {
            var hitPose = hits[0].pose;
            if (spawnedObject == null) {
                spawnedObject = Instantiate(mObjectList[index],hitPose.position,hitPose.rotation);

                //Look at the Camera On First Time Spawn
                //Vector3 lookPos = Camera.main.transform.position - spawnedObject.transform.position;
                //lookPos.y = 0;
                //spawnedObject.transform.rotation = Quaternion.LookRotation(lookPos);
            } else { 
                //spawnedObject.transform.position = hitPose.position;
                //spawnedObject.transform.rotation = hitPose.rotation;
            }
        }
    }

    protected override void OnPress(Vector3 position) => isPressed = true;

    protected override void OnPressCancel() => isPressed = false;
}
