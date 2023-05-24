using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    [SerializeField] private Transform interactionPoint;
    /** Controls the radius of the interaction sphere, therefore determining how close the interactable must be to use. */
    [SerializeField] private float interactionPointRadius = 0.5f;
    [SerializeField] private LayerMask interactionMask;
    [SerializeField] private InteractionPromptUI _interactionPromptUI;

    /** The amount of things we search for, once it's full we don't look for more. */
    private readonly Collider[] _colliders = new Collider[3];
    [SerializeField] private int numFound;
    
    private IInteractable _interactable;

    // Update is called once per frame
    void Update()
    {
        numFound = Physics.OverlapSphereNonAlloc(interactionPoint.position, interactionPointRadius, _colliders, interactionMask);
        
        if (numFound > 0)
        {
            _interactable = _colliders[0].GetComponent<IInteractable>();
            if (_interactable != null)
            {
                if (!_interactionPromptUI.IsDisplayed) 
                    _interactionPromptUI.SetUp(_interactable.interactPrompt);
                
                // TODO: Convert this check to an action from the InputSystem GameObject. (set to E and west button)
                if (Keyboard.current.eKey.wasPressedThisFrame) 
                    _interactable.Interact(this);
            }
        }
        else
        {
            // if (_interactable != null) _interactable = null;
            if (_interactionPromptUI.IsDisplayed) _interactionPromptUI.Close();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(interactionPoint.position, interactionPointRadius);
    }
}
