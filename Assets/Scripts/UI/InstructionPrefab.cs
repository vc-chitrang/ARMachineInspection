using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InstructionPrefab:MonoBehaviour {
    private Instruction _instruction = new Instruction();
    [SerializeField] private TextMeshProUGUI instructionText;
    [SerializeField] private Image highlighterImage;
    [SerializeField] private Image checkImage;

    private void OnEnable() {
        SetHighlighter(false);
        UpdateTaskStatusOnUI();
    }

    private void UpdateTaskStatusOnUI() {
        checkImage.gameObject.SetActive(_instruction.isTaskPerformed);
        instructionText.fontStyle = _instruction.isTaskPerformed ? FontStyles.Strikethrough : FontStyles.Normal;
    }

    public void SetInstruction(Instruction instruction) {
        _instruction = instruction;
    }

    public void DisplayInstruction() {
        instructionText.text = $"{_instruction.instructionId + 1}. {_instruction.instructionText}";
    }

    public void SetAsCompleted() {
        _instruction.isTaskPerformed = true;
        UpdateTaskStatusOnUI();
    }

    public bool IsTaskPerformed() { 
        return _instruction.isTaskPerformed;
    }

    public void SetHighlighter(bool isHighlighted) {
        highlighterImage.gameObject.SetActive(isHighlighted);
    }

    internal Instruction GetInstruction() {
        return _instruction;
    }
}//InstructionPrefab class end.

public class Instruction {
    public int instructionId;
    public string instructionText;

    public bool isTaskPerformed;
}