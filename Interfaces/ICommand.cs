namespace CourseProject.Interfaces
{
    /// <summary>
    /// Command Pattern interface for undoable operations.
    /// </summary>
    public interface ICommand
    {
        string Description { get; }
        void Execute();
        void Undo();
    }
}
