using CourseProject.Interfaces;
using CourseProject.Models;
using CourseProject.States;
using CourseProject.Observers;

namespace CourseProject.Commands
{
    /// <summary>
    /// Command Pattern: Encapsulates borrow operation with undo support.
    /// </summary>
    public class BorrowCommand : ICommand
    {
        private readonly IMaterial _material;
        private readonly IUser _user;
        private readonly IRepository<BorrowRecord> _borrowRepo;
        private BorrowRecord? _record;

        public string Description => $"Borrow '{_material.Title}' by {_user.Name}";

        public BorrowCommand(IMaterial material, IUser user, IRepository<BorrowRecord> borrowRepo)
        {
            _material = material;
            _user = user;
            _borrowRepo = borrowRepo;
        }

        public void Execute()
        {
            _material.State = new BorrowedState();
            _material.BorrowCount++;
            _user.BorrowedMaterialIds.Add(_material.Id);
            _record = new BorrowRecord
            {
                UserId = _user.Id,
                MaterialId = _material.Id,
                BorrowDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(14)
            };
            _borrowRepo.Add(_record);
        }

        public void Undo()
        {
            _material.State = new AvailableState();
            _material.BorrowCount--;
            _user.BorrowedMaterialIds.Remove(_material.Id);
            if (_record != null)
            _borrowRepo.Remove(_record.Id);
        }
    }

    /// <summary>
    /// Command Pattern: Encapsulates return operation with undo support.
    /// </summary>
    public class ReturnCommand : ICommand
    {
        private readonly IMaterial _material;
        private readonly IUser _user;
        private readonly IRepository<BorrowRecord> _borrowRepo;
        private readonly MaterialNotificationService _notificationService;
        private BorrowRecord? _record;

        public string Description => $"Return '{_material.Title}' by {_user.Name}";

        public ReturnCommand(IMaterial material, IUser user, IRepository<BorrowRecord> borrowRepo, MaterialNotificationService notificationService)
        {
            _material = material;
            _user = user;
            _borrowRepo = borrowRepo;
            _notificationService = notificationService;
        }

        public void Execute()
        {
            _material.State = new AvailableState();
            _user.BorrowedMaterialIds.Remove(_material.Id);
            _record = _borrowRepo.GetAll()
             .FirstOrDefault(r => r.MaterialId == _material.Id && r.UserId == _user.Id && !r.IsReturned);
            if (_record != null)
            {
                _record.IsReturned = true;
                _record.ReturnDate = DateTime.Now;
            }
            // Notify observers that material is available
            _notificationService.Notify($"Material '{_material.Title}' is now available!");
            _notificationService.ClearObservers();
     }

        public void Undo()
        {
            _material.State = new BorrowedState();
            _user.BorrowedMaterialIds.Add(_material.Id);
            if (_record != null)
            {
                _record.IsReturned = false;
                _record.ReturnDate = null;
            }
        }
    }

    /// <summary>
    /// Command Pattern: Encapsulates add material operation.
    /// </summary>
    public class AddMaterialCommand : ICommand
    {
        private readonly IMaterial _material;
        private readonly IRepository<IMaterial> _repo;

        public string Description => $"Add material '{_material.Title}'";

        public AddMaterialCommand(IMaterial material, IRepository<IMaterial> repo)
        {
            _material = material;
            _repo = repo;
        }

        public void Execute() => _repo.Add(_material);
        public void Undo() => _repo.Remove(_material.Id);
    }

    /// <summary>
    /// Command Pattern: Encapsulates remove material operation.
    /// </summary>
    public class RemoveMaterialCommand : ICommand
    {
        private readonly IMaterial _material;
        private readonly IRepository<IMaterial> _repo;

        public string Description => $"Remove material '{_material.Title}'";

        public RemoveMaterialCommand(IMaterial material, IRepository<IMaterial> repo)
        {
            _material = material;
            _repo = repo;
        }

        public void Execute() => _repo.Remove(_material.Id);
        public void Undo() => _repo.Add(_material);
    }

    /// <summary>
    /// Command history manager for undo functionality.
    /// </summary>
    public class CommandHistory
    {
        private readonly Stack<ICommand> _history = new();

        public void ExecuteCommand(ICommand command)
        {
            command.Execute();
            _history.Push(command);
            Console.WriteLine($"  Executed: {command.Description}");
        }

        public bool Undo()
        {
            if (_history.Count == 0) return false;
            var command = _history.Pop();
            command.Undo();
            Console.WriteLine($"  Undone: {command.Description}");
            return true;
        }

        public int Count => _history.Count;
    }
}
