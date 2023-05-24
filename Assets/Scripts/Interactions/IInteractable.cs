using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public string interactPrompt { get; }

    public bool Interact(Interactor interactor)
    {
        return true;
    }
}
