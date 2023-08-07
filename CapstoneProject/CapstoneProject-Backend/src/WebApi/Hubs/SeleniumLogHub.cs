
using Application.Common.Dtos;
using ExcelDataReader.Log;
using Microsoft.AspNetCore.SignalR;

namespace WebApi.Hubs
{
    public class SeleniumLogHub : Hub
    {
        //Buraya yazılan methodlar clientlerı calıstırır;
        //server dısındaki her sey client olabilir
        //blazor, react uygulaması, selenium gibi
        //crawlerden buraya istek yaparak calıstırabiliriz

            
        public async Task SendLogNotificationAsync(SeleniumLogDto log)
        {
        //   try
        //   {
          await Clients.AllExcept(Context.ConnectionId).SendAsync("NewSeleniumLogAdded", log);
        //    }
        //  catch (Exception ex)
        //    {

        //   Console.WriteLine("Error: " + ex.Message);
        //    Console.WriteLine("Details: " + ex.StackTrace);
        //
        
        }


        public async Task SendProductLogNotificationAsync(SeleniumLogDto productLog)
        {
        //    try
        //    {
             await Clients.AllExcept(Context.ConnectionId).SendAsync("NewProductLogAdded", productLog);
        //    }
        //    catch (Exception ex)
        //    {

        //    Console.WriteLine("Error: " + ex.Message);
        //     Console.WriteLine("Details: " + ex.StackTrace);
        //   }
         }
    }
}