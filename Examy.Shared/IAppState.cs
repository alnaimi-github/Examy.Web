namespace Examy.Shared
{
    public interface IAppState
    {
        string? LoadingText { get; }
        void ShowLoader(string loadingText);
    
        void HideLoader();

        event Action? OnToggleLoader;
    }
}
