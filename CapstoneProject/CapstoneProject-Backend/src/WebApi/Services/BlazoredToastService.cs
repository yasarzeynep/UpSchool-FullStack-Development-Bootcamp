
using Application.Common.Interfaces;

namespace Wasm.Services
{
    public class BlazoredToastService:IToasterService
    {
        private readonly IToasterService _toastService; //IToastService

        public BlazoredToastService(IToasterService toastService) //IToastService
        {
            _toastService = toastService;
        }

        public void ShowSuccess(string message)
        {
            _toastService.ShowSuccess(message);
        }

        public void ShowError(string message)
        {
            _toastService.ShowError(message);
        }
    }
}
