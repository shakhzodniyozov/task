namespace Application;

public class ValidationException : Exception
{
    public ValidationException() : base("One or more validation errors occurred.")
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(Dictionary<string, string[]> failures) : this()
    {
        Errors = failures;
    }

    public Dictionary<string, string[]> Errors { get; set; }
}
