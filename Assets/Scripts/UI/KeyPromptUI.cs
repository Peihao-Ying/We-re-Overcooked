using UnityEngine;
using TMPro;

public class KeyPromptUI : MonoBehaviour
{
    public static KeyPromptUI Instance;

    [SerializeField] private TextMeshProUGUI keyPromptText;

    private void Awake()
    {
        Instance = this;
        
        if (keyPromptText == null)
        {
            Debug.LogError("KeyPromptText is NOT assigned in the Inspector!");
            return;
        }

        keyPromptText.gameObject.SetActive(false);
    }

    public void ShowPrompt(string key, string action)
    {
        if (keyPromptText == null)
        {
            Debug.LogError("keyPromptText is NULL! Make sure it is assigned in the Inspector.");
            return;
        }
        Debug.Log("KeyPrompt UI script is called");
        keyPromptText.text = $"Press [{key}] to {action}";
        keyPromptText.gameObject.SetActive(true);
    }

    public void HidePrompt()
    {
        if (keyPromptText != null)
            keyPromptText.gameObject.SetActive(false);
    }
}

