using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InstructionPanel:MonoBehaviour {
    [SerializeField] private Button _openPanelButton;
    [SerializeField] private Button _closePanelButton;

    [SerializeField] private RectTransform instructionPanel;
    bool isOpen = false;
    private float _animationDuration = 0.5f;
    private Ease easeType = Ease.Linear;

    [SerializeField] private InstructionPrefab instructionPrefab;
    [SerializeField] private RectTransform content;
    private List<InstructionPrefab> _instructionList = new List<InstructionPrefab>();

    [SerializeField] private Button nextButton;    
    [SerializeField] private Button previousButton;
    [SerializeField] private TextMeshProUGUI counterText;

    private int _selectedTaskIndex = 0;
    private int selectedTaskIndex {
        get => _selectedTaskIndex;
        set {
            _selectedTaskIndex = value;
            UpdateNextPreviousButtonUI();
            UpdateCounterText();
            HighlightSelectedTask();
            //Debug.Log($"{_selectedTaskIndex}/{_instructionList.Count}");

            PerformOprationOnMachine();

            UpdateCurrentInstructionOnUI();
        }
    }

    [Header("TitlePanel")]
    [SerializeField] private GameObject _titlePanel;
    [SerializeField] private TextMeshProUGUI _currentInstruction;
    [SerializeField] private Button _nextInstructionButton;
    [SerializeField] private Button _previousInstructionButton;
    [SerializeField] private Image _checkBoxInstructionImage;

    private void Start() {
        _openPanelButton.onClick.AddListener(Open);
        _closePanelButton.onClick.AddListener(Close);

        nextButton.onClick.AddListener(OnNextButtonClick);
        previousButton.onClick.AddListener(OnPreviousButtonClick);

        _nextInstructionButton.onClick.AddListener(OnNextButtonClick);
        _previousInstructionButton.onClick.AddListener(OnPreviousButtonClick);

        _closePanelButton.gameObject.SetActive(false);

        _titlePanel.SetActive(false);
    }

    #region PANEL_OPEN_CLOSE

    void Open() {
        // Kill any running tweens on the panel to avoid conflicts
        instructionPanel.DOKill();

        // Animate the panel to the open position
        instructionPanel.DOAnchorPosY(0,_animationDuration)
            .SetEase(easeType)
            .OnComplete(() => {
                isOpen = true;
                _closePanelButton.gameObject.SetActive(true);
            });
    }

    void Close() {
        float targetPos = Mathf.Abs(instructionPanel.rect.height);        

        instructionPanel.DOKill();
        _closePanelButton.gameObject.SetActive(false);
        // Animate the panel to the closed position
        instructionPanel.DOAnchorPosY(targetPos,_animationDuration)
            .SetEase(easeType)
            .OnComplete(() => {
                isOpen = false;
            });
    }
    #endregion PANEL_OPEN_CLOSE

    internal void DisplayMachineInformation(MachineData machineData) {
        CleareInstructionPrefabList();

        for (int i = 0;i < machineData.components.Count;i++) {
            ComponentData componentData = machineData.components[i];
            InstructionPrefab instructionPrefabInstance = Instantiate(instructionPrefab,content);

            Instruction instruction = new Instruction {
                instructionId = i,
                instructionText = componentData.instructions
            };

            instructionPrefabInstance.SetInstruction(instruction);
            instructionPrefabInstance.DisplayInstruction();

            _instructionList.Add(instructionPrefabInstance);
        }
        selectedTaskIndex = 0;
    }

    private void CleareInstructionPrefabList() {
        _instructionList.ForEach(i => {
            Destroy(i.gameObject);
        });
        _instructionList.Clear();
    }

    public void UpdateNextPreviousButtonUI() {
        nextButton.gameObject.SetActive(selectedTaskIndex <= _instructionList.Count - 1);
        previousButton.gameObject.SetActive(selectedTaskIndex > 0);

        _nextInstructionButton.gameObject.SetActive(selectedTaskIndex <= _instructionList.Count - 1);
        _previousInstructionButton.gameObject.SetActive(selectedTaskIndex > 0);
    }

    private bool IsAllTaskPerformed() {
        int completedTask = _instructionList.Count(i => i.IsTaskPerformed());
        return completedTask == _instructionList.Count;
    }

    public void UpdateCounterText() {

        int completedTask = _instructionList.Count(i => i.IsTaskPerformed());
        if (IsAllTaskPerformed()) {
            counterText.text = $"All Tasks Performed";
            return;
        }
        string taskText = completedTask > 1 ? "Tasks" : "Task";
        counterText.text = $"{completedTask} {taskText} Performed out of {_instructionList.Count}";
    }

    private void OnNextButtonClick() {
        if (!_instructionList[selectedTaskIndex].IsTaskPerformed()) {
            _instructionList[selectedTaskIndex].SetAsCompleted();
        }
        selectedTaskIndex++;
    }

    private void OnPreviousButtonClick() {
        selectedTaskIndex--;
    }

    private void HighlightSelectedTask() {
        _instructionList.ForEach(t => t.SetHighlighter(false));

        if (selectedTaskIndex >= _instructionList.Count)
            _selectedTaskIndex = _instructionList.Count - 1;

        _instructionList[selectedTaskIndex].SetHighlighter(true);
    }

    private Machine _machine;
    public void SetMachine(Machine machine) {
        _machine = machine;
    }

    private void PerformOprationOnMachine() {
        if (_machine == null)
            return;

        _machine.ApplyGlassMaterial();
        _machine.HighlightComponent(selectedTaskIndex);
    }

    private void UpdateCurrentInstructionOnUI() {
        if (selectedTaskIndex >= _instructionList.Count)
            return;

        Instruction instruction = _instructionList[selectedTaskIndex].GetInstruction();
        _currentInstruction.text = $"{instruction.instructionId + 1}. {instruction.instructionText}";

        _checkBoxInstructionImage.gameObject.SetActive(instruction.isTaskPerformed);
        _currentInstruction.fontStyle = instruction.isTaskPerformed ? FontStyles.Strikethrough : FontStyles.Normal;
    }

    internal void EnableInstructionPanel() {
        _titlePanel.SetActive(true);
    }
}//InstructionPanel class end.
