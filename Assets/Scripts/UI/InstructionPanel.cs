using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InstructionPanel:MonoBehaviour {
    [SerializeField] private Button toggleButton;
    [SerializeField] private RectTransform instructionPanel;
    [SerializeField] private RectTransform arrowIcon;
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
        }
    }

    private void Start() {
        toggleButton.onClick.AddListener(TogglePanel);
        nextButton.onClick.AddListener(OnNextButtonClick);
        previousButton.onClick.AddListener(OnPreviousButtonClick);
    }    

    #region PANEL_OPEN_CLOSE
    public void TogglePanel() {
        if (isOpen) {
            Close();
        } else {
            Open();
        }
    }
    void Open() {
        float targetPos = instructionPanel.rect.height * 0.5f;
        instructionPanel.DOLocalMoveY(targetPos,_animationDuration)
            .SetEase(easeType)
            .OnComplete(() => {
                isOpen = true;
                arrowIcon.localScale = new Vector3(1,-1,1);
            });
    }

    void Close() {
        float targetPos = (instructionPanel.rect.height * 0.5f) * -1;
        instructionPanel.DOLocalMoveY(targetPos,_animationDuration)
            .SetEase(easeType)
            .OnComplete(() => {
                isOpen = false;
                arrowIcon.localScale = Vector3.one;
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
    }

    private bool IsAllTaskPerformed() { 
        int completedTask = _instructionList.Count(i => i.IsTaskPerformed());
        return completedTask == _instructionList.Count;
    }

    public void UpdateCounterText() {
        Debug.Log($"{selectedTaskIndex}/{_instructionList.Count}");       

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
        _instructionList.ForEach(t=> t.SetHighlighter(false));
        
        if(selectedTaskIndex >= _instructionList.Count)
            _selectedTaskIndex = _instructionList.Count - 1;

        _instructionList[selectedTaskIndex].SetHighlighter(true);
    }
}//InstructionPanel class end.
