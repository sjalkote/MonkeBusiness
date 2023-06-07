public interface IInteractable
{
    public string interactPrompt { get; }

    public bool Interact(Interactor interactor)
    {
        return true;
    }
}