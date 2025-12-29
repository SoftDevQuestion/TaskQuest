using System;
using System.Web;
using System.Web.UI;
using System.Text;

namespace TaskQuest
{
    public class BasePage : Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            
            // Set UTF-8 encoding for all pages
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Charset = "UTF-8";
            
            // Set culture to Persian
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("fa-IR");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fa-IR");
        }
        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            // Ensure proper encoding for all responses
            Response.ContentType = "text/html; charset=utf-8";
            Response.Headers.Remove("Content-Type");
            Response.Headers.Add("Content-Type", "text/html; charset=utf-8");
        }
        
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            
            // Final encoding check before rendering
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Charset = "UTF-8";
        }
        
        /// <summary>
        /// Safely encode Persian text to prevent encoding issues
        /// </summary>
        /// <param name="text">Persian text to encode</param>
        /// <returns>Properly encoded text</returns>
        protected string SafeEncodePersianText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;
            
            // Ensure the text is properly UTF-8 encoded
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            return Encoding.UTF8.GetString(bytes);
        }
        
        /// <summary>
        /// Set control text with proper encoding
        /// </summary>
        /// <param name="control">The control to set text for</param>
        /// <param name="text">The Persian text</param>
        protected void SetControlText(System.Web.UI.WebControls.Label control, string text)
        {
            if (control != null && !string.IsNullOrEmpty(text))
            {
                control.Text = SafeEncodePersianText(text);
                control.Attributes.Add("dir", "rtl");
            }
        }
    }
}