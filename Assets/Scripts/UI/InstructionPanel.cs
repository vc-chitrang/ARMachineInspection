using DG.Tweening;
using System;
using System.Collections.Generic;
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
    private List<InstructionPrefab> _instructionPrefabList = new List<InstructionPrefab>();

    private void Start() {
        toggleButton.onClick.AddListener(TogglePanel);
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

            _instructionPrefabList.Add(instructionPrefabInstance);
        }
    }

    private void CleareInstructionPrefabList() {
        _instructionPrefabList.ForEach(i => {
            Destroy(i.gameObject);
        });
        _instructionPrefabList.Clear();
    }
    #endregion PANEL_OPEN_CLOSE


}
