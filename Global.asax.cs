using System;
using System.Web;

namespace TaskQuest
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            // Set default encoding for the application
            // System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            // Ensure UTF-8 encoding for all requests
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Charset = "UTF-8";
            
            // Set proper headers for Persian content
            Response.Headers.Remove("Content-Type");
            Response.Headers.Add("Content-Type", "text/html; charset=utf-8");
            
            // Set culture to Persian for all requests
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("fa-IR");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fa-IR");
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            // Ensure session encoding is set properly
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Charset = "UTF-8";
        }
    }
}