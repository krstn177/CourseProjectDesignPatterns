using CourseProject.Interfaces;
using CourseProject.Models;

namespace CourseProject.Factories
{
    /// <summary>
    /// Factory Method Pattern: Creates Book instances.
    /// Demonstrates: Open/Closed Principle - add new factories without modifying existing code.
    /// </summary>
    public class BookFactory : IMaterialFactory
    {
        public IMaterial Create(string title, string author, string category, int year) =>
        new Book { Title = title, Author = author, Category = category, Year = year };
    }

    public class MagazineFactory : IMaterialFactory
    {
       public IMaterial Create(string title, string author, string category, int year) =>
       new Magazine { Title = title, Author = author, Category = category, Year = year };
    }

    public class EbookFactory : IMaterialFactory
    {
        public IMaterial Create(string title, string author, string category, int year) =>
        new Ebook { Title = title, Author = author, Category = category, Year = year };
    }

    public class AudiobookFactory : IMaterialFactory
    {
        public IMaterial Create(string title, string author, string category, int year) =>
       new Audiobook { Title = title, Author = author, Category = category, Year = year };
    }

    /// <summary>
    /// Factory provider to get the correct factory by material type name.
    /// </summary>
    public static class MaterialFactoryProvider
    {
        private static readonly Dictionary<string, IMaterialFactory> _factories = new(StringComparer.OrdinalIgnoreCase)
        {
            ["Book"] = new BookFactory(),
            ["Magazine"] = new MagazineFactory(),
            ["Ebook"] = new EbookFactory(),
            ["Audiobook"] = new AudiobookFactory()
        };

        public static IMaterialFactory GetFactory(string type)
        {
          if (_factories.TryGetValue(type, out var factory))
            return factory;
            throw new ArgumentException($"Unknown material type: {type}");
        }

        public static IEnumerable<string> AvailableTypes => _factories.Keys;
    }
}
