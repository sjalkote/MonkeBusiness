using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractWeapon : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactionPrompt;
    
    public string interactPrompt => interactionPrompt;
    
    public bool Interact(Interactor interactor)
    {
        Debug.Log("Interacting with weapon!");
        return true;
    }
}
