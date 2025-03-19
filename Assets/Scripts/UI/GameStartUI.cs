using UnityEngine;
using UnityEngine.UI;

public class GameStartUI : MonoBehaviour
{
    [SerializeField] private GameObject startMenuPanel;
    [SerializeField] private Button startButton;

    private void Start()
    {
        Time.timeScale = 0;
        startMenuPanel.SetActive(true);
        startButton.onClick.AddListener(StartGame);
    }

    public void StartGame()
    {
        startMenuPanel.SetActive(false);
        Time.timeScale = 1;
    }
}
