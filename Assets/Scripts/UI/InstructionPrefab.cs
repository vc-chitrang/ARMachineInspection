using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InstructionPrefab:MonoBehaviour {
    private Instruction _instruction;
    [SerializeField] private TextMeshProUGUI instructionText;
    [SerializeField] private Image highlighterImage;
    [SerializeField] private Image checkImage;

    private void OnEnable() {
        highlighterImage.gameObject.SetActive(false);
        IsChecked(false);
    }

    private void IsChecked(bool isChecked) {
        checkImage.gameObject.SetActive(isChecked);
        instructionText.fontStyle = isChecked ? FontStyles.Strikethrough : FontStyles.Normal;
    }

    public void SetInstruction(Instruction instruction) {
        _instruction = instruction;
    }

    public void DisplayInstruction() {
        instructionText.text = $"{_instruction.instructionId + 1}. {_instruction.instructionText}";
    }
}//InstructionPrefab class end.

public class Instruction {
    public int instructionId;
    public string instructionText;
}