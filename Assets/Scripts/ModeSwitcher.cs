using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ModeSwitcher:MonoBehaviour {
    public ARSession arSession;
    public GameObject arCamera;
    public GameObject mainCamera;
    public GameObject model3D;
    public Button arModeButton;
    public Image arModeHighlighter;
    public Button view3DButton;
    public Image view3DHighlighter;

    private Machine _machine;
    private Cord _previouslyStoredCordinates;

    void Start() {
        // Initialize states
        arModeButton.onClick.AddListener(SwitchToARMode);
        view3DButton.onClick.AddListener(SwitchTo3DView);

        // Start in AR Mode
        SwitchToARMode();

        SetArModeHighlight(true);
    }

    private void SetArModeHighlight(bool isEnable) { 
        arModeHighlighter.gameObject.SetActive(isEnable);
        view3DHighlighter.gameObject.SetActive(!isEnable);
    }
    void SwitchToARMode() {
        // Enable AR components
        arSession.enabled = true;
        arCamera.SetActive(true);

        // Disable 3D View components
        mainCamera.SetActive(false);
        model3D.SetActive(false);

        //My Custom Code Logic
        if (_machine == null) {
            _machine = AppController.Instance.GetMachine();
        }

        if (_previouslyStoredCordinates != null) {
            _machine.transform.SetParent(null,false);
            _machine.SetCurrentCordinate(_previouslyStoredCordinates);
        }

        SetArModeHighlight(true);
    }

    void SwitchTo3DView() {
        // Disable AR components
        arSession.enabled = false;
        arCamera.SetActive(false);

        // Enable 3D View components
        mainCamera.SetActive(true);
        model3D.SetActive(true);

        //My Custom Code Logic
        if (_machine == null) {
            _machine = AppController.Instance.GetMachine();
        }

        _previouslyStoredCordinates = _machine.GetCurrentCordinate();

        _machine.transform.SetParent(model3D.transform,false);
        _machine.ResetPosition();

        SetArModeHighlight(false);
    }
}//ModeSwitcher class end.
