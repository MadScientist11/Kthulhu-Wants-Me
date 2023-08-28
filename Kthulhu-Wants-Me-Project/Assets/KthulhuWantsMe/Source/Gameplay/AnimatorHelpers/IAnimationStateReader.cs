namespace KthulhuWantsMe.Source.Gameplay.AnimatorHelpers
{
    public interface IAnimationStateReader
    {
        void EnteredState(int stateHash);
        void ExitedState(int stateHash);

    }
}