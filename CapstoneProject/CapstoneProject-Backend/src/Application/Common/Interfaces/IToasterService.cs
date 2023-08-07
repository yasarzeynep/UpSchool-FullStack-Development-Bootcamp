namespace Application.Common.Interfaces;

public interface IToasterService
{
    void ShowSuccess(string message);

    void ShowError(string message);
}
