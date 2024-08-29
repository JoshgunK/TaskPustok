namespace Pustok.Business.Exceptions.CommonExceptions;

public class FilevalidationException: Exception
{
    public string PropertyName { get; set; }
    public FilevalidationException()
    {
    }

    public FilevalidationException(string? message) : base(message)
    {
    }

    public FilevalidationException(string propertyName, string? message) : base(message)
    {
        PropertyName = propertyName;
    }
}
