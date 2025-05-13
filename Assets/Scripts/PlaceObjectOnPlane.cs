using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class PlaceObjectOnPlane : MonoBehaviour
{
    [SerializeField] private GameObject mObject;
    GameObject spawnedObject;
    private ARRaycastManager aRRaycastManager;
    List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Update() {
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began) {
                if (aRRaycastManager.Raycast(touch.position,hits, TrackableType.PlaneWithinPolygon)) {
                    Pose hitPose = hits[0].pose;
                    if (spawnedObject == null) {
                        spawnedObject = Instantiate(mObject,hitPose.position,hitPose.rotation);
                    } else {
                        spawnedObject.transform.position = hitPose.position;
                        spawnedObject.transform.rotation = hitPose.rotation;
                    }

                    Vector3 lookPos = Camera.main.transform.position - spawnedObject.transform.position;
                    lookPos.y = 0;
                    spawnedObject.transform.rotation = Quaternion.LookRotation(lookPos);
                }
            }
        }
    }
}
