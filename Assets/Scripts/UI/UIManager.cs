using TMPro;
using UnityEngine;

public class UIManager:MonoBehaviour {
    public static UIManager Instance { get; private set; } = null;
    [SerializeField] private TextMeshProUGUI appVersion;
    [SerializeField] private GameObject _percentageDisplayUI;
    [SerializeField] private TextMeshProUGUI _percentageText;
    [Header("------- Instruction Panel -------")]
    [SerializeField] private InstructionPanel _instructionPanel;
    [SerializeField] private GameObject _viewSwitchButtons;

    private void Awake() {
        appVersion.text = Application.version;
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
        SetPercentageUI(false);
        _viewSwitchButtons.SetActive(false);
    }

    public void SetPercentageUI(bool enable) {
        if (!_percentageDisplayUI.activeInHierarchy) {
            _percentageDisplayUI.SetActive(enable);
        }
    }

    public void PrintScalePercentage(int percentage) {
        _percentageText.text = $"{percentage}%";
    }

    public void DisplayMachineInformation(MachineData machineData) {
        _instructionPanel.DisplayMachineInformation(machineData);
    }

    public void SetMachine(Machine machine) { 
        _instructionPanel.SetMachine(machine);
    }

    public void EnableInstructionPanel() {
        _instructionPanel.EnableInstructionPanel();
    }

    public void EnableViewSwitchButtons() {
        _viewSwitchButtons.SetActive(true);
    }
}
