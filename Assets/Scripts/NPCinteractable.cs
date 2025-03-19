using UnityEngine;

public class NPCinteractable : MonoBehaviour
{
    [SerializeField] private GameObject chatUI;
    [SerializeField] private float interactionRange = 4f;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        
        if (distance <= interactionRange)
        {
            KeyPromptUI.Instance.ShowPrompt("E or F", "Talk to submit Food");
        }
        else
        {
            // KeyPromptUI.Instance.HidePrompt();
        }
    }

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

