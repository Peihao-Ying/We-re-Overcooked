using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SlimeOrder : MonoBehaviour
{
    public KitchenObjectSO wantedFood;
    public GameObject endGamePanel;
    public GameObject star1, star2, star3;
    public Button restartButton;
    public Timer timer;

    private bool isCompleted = false;

    public TextMeshProUGUI resultText;



    public void InteractOperate(Player player)
    {
        if (isCompleted || !player.IsHaveKitchenObject()) return;

        KitchenObject playerObject = player.GetKitchenObject();

        if (playerObject != null && playerObject.GetKitchenObjectSO() == wantedFood)
        {
            ShowWinScreen();
            player.ClearKitchenObject();
        }
        else
        {
            Debug.Log("Wrong food! The slime didn't accept it.");
        }
    }

    private void ShowWinScreen()
    {
        isCompleted = true;
        timer.StopTimer(); // ✅ Stop the timer when the game ends
        endGamePanel.SetActive(true);
        resultText.text = "You Passed! 🎉";

        float remainingTime = timer.GetRemainingTime();
        star3.SetActive(remainingTime > 60);
        star2.SetActive(remainingTime > 30);
        star1.SetActive(remainingTime > 0);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
