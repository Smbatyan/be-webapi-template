namespace Core.Exceptions;

public class ApplicationExceptionBase : ApplicationException
{
    public int StatusCode => HResult;

    public ApplicationExceptionBase(string message, int code) : base(message)
    {
        HResult = code;
    }
}