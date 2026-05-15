using System.Text.Json;
using CourseProject.Interfaces;
using CourseProject.Models;
using CourseProject.States;

namespace CourseProject.Repositories
{
    /// <summary>
    /// Generic JSON file-based repository.
    /// Demonstrates: Dependency Inversion Principle - services depend on IRepository abstraction.
    /// </summary>
    public class JsonRepository<T> : IRepository<T> where T : class
    {
        private List<T> _items = new();
        private readonly string _filePath;
        private readonly Func<T, Guid> _getId;

        public JsonRepository(string filePath, Func<T, Guid> getId)
        {
            _filePath = filePath;
            _getId = getId;
        }

        public List<T> GetAll() => _items;

        public T? GetById(Guid id) => _items.FirstOrDefault(x => _getId(x) == id);

        public void Add(T entity)
        {
            _items.Add(entity);
            SaveToFile(); // Auto-save after add
        }

        public void Update(T entity)
        {
            var index = _items.FindIndex(x => _getId(x) == _getId(entity));
            if (index >= 0)
            {
                _items[index] = entity;
                SaveToFile(); // Auto-save after update
            }
        }

        public void Remove(Guid id)
        {
            _items.RemoveAll(x => _getId(x) == id);
            SaveToFile(); // Auto-save after remove
        }

        public void SaveToFile()
        {
            var dir = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);
            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(_filePath, JsonSerializer.Serialize(_items, options));
        }

        public void LoadFromFile()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                _items = JsonSerializer.Deserialize<List<T>>(json) ?? new();
            }
        }
    }

    /// <summary>
    /// Specialized repository for materials that handles polymorphic deserialization.
    /// </summary>
    public class MaterialRepository : IRepository<IMaterial>
    {
        private List<IMaterial> _items = new();
        private readonly string _filePath;

        public MaterialRepository(string filePath)
        {
            _filePath = filePath;
        }

        public List<IMaterial> GetAll() => _items;
        public IMaterial? GetById(Guid id) => _items.FirstOrDefault(m => m.Id == id);
        public void Add(IMaterial entity)
        {
            _items.Add(entity);
            SaveToFile(); // Auto-save after add
        }

        public void Update(IMaterial entity)
        {
            var index = _items.FindIndex(m => m.Id == entity.Id);
            if (index >= 0)
            {
                _items[index] = entity;
                SaveToFile(); // Auto-save after update
            }
        }

        public void Remove(Guid id)
        {
            _items.RemoveAll(m => m.Id == id);
            SaveToFile(); // Auto-save after remove
        }

        public void SaveToFile()
        {
            var dir = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);

            var dtos = _items.Select(m => new MaterialDto
            {
                Id = m.Id,
                Title = m.Title,
                Author = m.Author,
                Category = m.Category,
                Year = m.Year,
                BorrowCount = m.BorrowCount,
                MaterialType = m.MaterialType,
                StateName = m.State.StateName
            }).ToList();

            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(_filePath, JsonSerializer.Serialize(dtos, options));
        }

        public void LoadFromFile()
        {
            if (!File.Exists(_filePath)) return;

            var json = File.ReadAllText(_filePath);
            var dtos = JsonSerializer.Deserialize<List<MaterialDto>>(json) ?? new();

            _items = dtos.Select(dto =>
            {
                IMaterial material = dto.MaterialType switch
                {
                    "Book" => new Book(),
                    "Magazine" => new Magazine(),
                    "Ebook" => new Ebook(),
                    "Audiobook" => new Audiobook(),
                    _ => new Book()
                };
                material.Id = dto.Id;
                material.Title = dto.Title;
                material.Author = dto.Author;
                material.Category = dto.Category;
                material.Year = dto.Year;
                material.BorrowCount = dto.BorrowCount;
                material.State = dto.StateName switch
                {
                    "Borrowed" => new BorrowedState(),
                    "Reserved" => new ReservedState(),
                    "Lost" => new LostState(),
                    _ => new AvailableState()
                };
                return material;
            }).ToList();
        }

        private class MaterialDto
        {
            public Guid Id { get; set; }
            public string Title { get; set; } = "";
            public string Author { get; set; } = "";
            public string Category { get; set; } = "";
            public int Year { get; set; }
            public int BorrowCount { get; set; }
            public string MaterialType { get; set; } = "";
            public string StateName { get; set; } = "";
        }
    }
}
