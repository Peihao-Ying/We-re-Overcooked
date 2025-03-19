using UnityEngine;
using UnityEngine.UI;

public class InstructionUI : MonoBehaviour
{
    public GameObject instructionPanel;
    public Button instructionButton;
    public Button exitButton;

    private void Start()
    {
        instructionPanel.SetActive(false);

        instructionButton.onClick.AddListener(ShowInstructionPanel);
        exitButton.onClick.AddListener(HideInstructionPanel);
    }

    private void ShowInstructionPanel()
    {
        instructionPanel.SetActive(true);
    }

    private void HideInstructionPanel()
    {
        instructionPanel.SetActive(false);
    }
}
