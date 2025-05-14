using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Transformers;

public class TargetLocalScaleLogger: MonoBehaviour {
    private ARTransformer aRTransformer;
    private XRGrabInteractable grabInteractable;
    private Vector3 initialScale;
    private Vector2 minmax = Vector2.zero;

    private Vector3 currentScale;
    private Vector3Int currentScaleInt;
    private bool hasVibratedAt100 = true;

    private void Awake() {
        grabInteractable = GetComponent<XRGrabInteractable>();
        aRTransformer = GetComponent<ARTransformer>();
    }

    void Start() {
        minmax = new Vector2(aRTransformer.minScale,aRTransformer.maxScale);
        initialScale = grabInteractable.GetTargetLocalScale();
        Debug.Log($"initialScale: {initialScale} || {minmax}");
        PrintPercentage();
    }

    void Update() {
        Vector3 newScale = grabInteractable.GetTargetLocalScale();

        // Check if the scale has changed
        if (newScale != currentScale) {
            currentScale = newScale;
            PrintPercentage();
            Debug.Log($"Target scale changed: {currentScale}");
        }
    }

    private void PrintPercentage() {
        //int _percentage = Mathf.RoundToInt((currentScale.x - minmax.x) / (minmax.y - minmax.x) * 100);
        float _percentage = MathUtility.MapValue(currentScale.x,minmax.x,minmax.y,0,200);
        int roundedPercentage = Mathf.RoundToInt(_percentage);
        UIManager.Instance.PrintScalePercentage(roundedPercentage);
        UIManager.Instance.SetPercentageUI(true);

        if (roundedPercentage == 100) {
            if (!hasVibratedAt100) {
#if UNITY_ANDROID && !UNITY_EDITOR
                Handheld.Vibrate();
#endif
                hasVibratedAt100 = true;                
            }
        } else {
            hasVibratedAt100 = false;
        }
    }
}
