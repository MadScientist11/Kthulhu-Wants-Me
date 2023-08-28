namespace KthulhuWantsMe.Source.Gameplay.AnimatorHelpers
{
    public interface IAnimationStateReader
    {
        void EnteredState(int stateInfoShortNameHash);
        void ExitedState(int stateInfoShortNameHash);
    }
}