using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float totalTime = 90f;
    private float remainingTime;
    public TextMeshProUGUI timerText;
    private bool isRunning = true;
    public GameObject failurePanel;

    private void Start()
    {
        remainingTime = totalTime;
        UpdateUI();
    }

    private void Update()
    {
        if (isRunning && remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            UpdateUI();

            if (remainingTime <= 0)
            {
                remainingTime = 0;
                ShowFailureScreen();
            }
        }
    }

    private void UpdateUI()
    {
        timerText.text = "Time: " + Mathf.Max(0, Mathf.FloorToInt(remainingTime)) + "s";
    }

    private void ShowFailureScreen()
    {
        isRunning = false;
        failurePanel.SetActive(true);
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public float GetRemainingTime()
    {
        return remainingTime;
    }
}
