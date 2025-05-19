using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARRaycastManager))]
public class PlaceObjectOnPlaneNewInput:PressInputBase {
    [SerializeField] private int index = 0;
    [SerializeField] private List<GameObject> mObjectList;

    bool isPressed;
    private ARRaycastManager aRRaycastManager;
    List<ARRaycastHit> hits = new List<ARRaycastHit>();
    GameObject _spawnedObject = null;
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
            if (_spawnedObject == null) {
                var hitPose = hits[0].pose;
                _spawnedObject = Instantiate(mObjectList[index],hitPose.position,hitPose.rotation);
                Machine _machine = _spawnedObject.GetComponent<Machine>();
                UIManager.Instance.EnableInstructionPanel();
                UIManager.Instance.EnableViewSwitchButtons();

                AppController.Instance.SetMachie(_machine);
            }
        }
    }

    protected override void OnPress(Vector3 position) => isPressed = true;

    protected override void OnPressCancel() => isPressed = false;
}
