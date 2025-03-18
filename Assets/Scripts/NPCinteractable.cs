using UnityEngine;

public class NPCinteractable : MonoBehaviour
{
    [SerializeField] private GameObject chatUI;

    public void Interact()
    {
        Debug.Log("Interacted with NPC!");

        if (chatUI != null)
        {
            chatUI.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Chat UI is not assigned in NPCinteractable!");
        }
    }

    public void CloseChat()
    {
        if (chatUI != null)
        {
            chatUI.SetActive(false);
        }
    }
}
