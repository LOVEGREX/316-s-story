namespace Case316.Pixel
{
    public interface IInteractable
    {
        string PromptText { get; }
        void Interact(PixelPlayerController player);
    }
}
