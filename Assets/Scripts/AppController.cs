using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class AppController:MonoBehaviour {
    [SerializeField] private ObjectSpawner objectSpawner;
    [SerializeField] private TextMeshProUGUI appVersion;
    [SerializeField] private GameObject _percentageDisplayUI;
    [SerializeField] private TextMeshProUGUI _percentageText;
    public static AppController Instance { get; private set; } = null;
    private void Awake() {
        appVersion.text = Application.version;
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
        SetPercentageUI(false);
    }

    private void OnEnable() {
        objectSpawner.objectSpawned += ObjectSpawner_objectSpawned;
    }

    private void OnDisable() {
        objectSpawner.objectSpawned -= ObjectSpawner_objectSpawned;
    }

    private void ObjectSpawner_objectSpawned(GameObject spawnedObject) {
        Debug.Log($"Spawned object: {spawnedObject.name}");
    }

    public void SetPercentageUI(bool enable) {
        if (!_percentageDisplayUI.activeInHierarchy) {
            _percentageDisplayUI.SetActive(enable);
        }
    }

    public void PrintScalePercentage(int percentage) {
        _percentageText.text = $"{percentage}%";
    }
}//AppController class end.
