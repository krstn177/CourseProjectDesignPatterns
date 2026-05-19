namespace CourseProject.Interfaces
{
    /// <summary>
    /// Adapter Pattern: Common interface for exporting data to different file formats.
    /// Allows different export implementations (CSV, JSON, XML, Excel) without changing client code.
    /// </summary>
    public interface IDataExporter
    {
        string FileExtension { get; }
        string FormatName { get; }
        void ExportToFile(string filePath, object data);
    }
}
