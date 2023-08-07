using Acr.UserDialogs;
using Application.Common.Interfaces;
using Microsoft.JSInterop;



namespace Wasm.Services
{
    public class ToasterService : IToasterService
    {
        private readonly IJSRuntime _jsRuntime;

        public ToasterService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public void ShowError(string message)
        {
            throw new NotImplementedException();
        }

        public void ShowSuccess(string message)
        {
            throw new NotImplementedException();
        }

        //public void ShowSuccess(string message) // Success!
        //{
        //    _jsRuntime.ShowToastAsync(new ToastOptions
        //    {
        //        Text = message,
        //        Position = ToastPosition.TopCenter,
        //        Heading = "Success!UpSchool"
        //    });
        //}

        //public void ShowError(string message) // Error!
        //{
        //    _jsRuntime.ShowToastAsync(new ToastOptions
        //    {
        //        Text = message,
        //        Position = ToastPosition.TopCenter,
        //        Heading = "Error!"
        //    });
        //}
    }
}
