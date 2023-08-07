using Application.Common.Helpers;
using Domain.Entities;
using System.Collections.Generic;
using System.IO;

namespace Application.Common.Models.Email
{
    public class EmailTemplateProvider
    {
        private readonly string _wwwrootPath;

        public EmailTemplateProvider(string wwwrootPath)
        {
            _wwwrootPath = wwwrootPath;
        }

        public string GetEmailConfirmationTemplate(string name, List<Product> products)
        {
            var emailTemplatePath = $"{_wwwrootPath}/email_templates/email_confirmation.html";
            var htmlContent = File.ReadAllText(emailTemplatePath);

            htmlContent = htmlContent.Replace("{{subject}}", MessagesHelper.Email.Confirmation.Subject);
            htmlContent = htmlContent.Replace("{{name}}", MessagesHelper.Email.Confirmation.Name(name));
            htmlContent = htmlContent.Replace("{{excelProductFile}}", MessagesHelper.Email.Confirmation.ExcelProductFile(products));
            htmlContent = htmlContent.Replace("{{activationMessage}}", MessagesHelper.Email.Confirmation.Message);

            return htmlContent;
        }
    }
}
