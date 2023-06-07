using UnityEngine;

public class InteractDoor : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactionPrompt;

    public string interactPrompt => interactionPrompt;

    public bool Interact(Interactor interactor)
    {
        Debug.Log("Interacting with door!");
        return true;
    }
}