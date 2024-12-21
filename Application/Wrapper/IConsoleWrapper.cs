namespace Application.Wrapper;

public interface IConsoleWrapper
{
    void WriteLine(string message);
    string ReadLine();
}