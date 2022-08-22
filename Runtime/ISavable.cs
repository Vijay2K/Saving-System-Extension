namespace Extension.SavingSystem
{
    public interface ISavable
    {
        object CaptureState();
        void RestoreState(object stateToRestore);
    }

}