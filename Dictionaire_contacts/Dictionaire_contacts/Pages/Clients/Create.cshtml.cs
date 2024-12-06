using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Dictionaire_contacts.Pages.Clients
{
    public class CreateModel : PageModel
    {
        public Contact contact = new Contact();
        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
        }

        public void OnPost(IFormFile Image)
        {
            contact.Name = Request.Form["Name"];
            contact.Telephone = Request.Form["Telephone"];
            contact.Descr = Request.Form["Description"];

            if (string.IsNullOrEmpty(contact.Name) || string.IsNullOrEmpty(contact.Telephone))
            {
                errorMessage = "Tel and name are required";
                return;
            }

            if (Image != null && Image.Length > 0)
            {
                var imagePath = Path.Combine("wwwroot/images", Image.FileName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    Image.CopyTo(stream);
                }
                contact.ImagePath = "/images/" + Image.FileName;
            }
            else
            {
                contact.ImagePath = "/images/inconu.jpg";
            }

            try
            {
                string connectionString = "Data Source=DESKTOP-AS9GTHA\\SQLEXPRESS;Initial Catalog=contacts;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "INSERT INTO contacts (name, telephone, descr, creedate, ImagePath)" +
                        " VALUES (@name, @telephone, @descr, GETDATE(), @ImagePath);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", contact.Name);
                        command.Parameters.AddWithValue("@telephone", contact.Telephone);
                        command.Parameters.AddWithValue("@descr", contact.Descr);
                        command.Parameters.AddWithValue("@ImagePath", contact.ImagePath);
                        command.ExecuteNonQuery();
                    }
                }
                successMessage = "New Contact Added Successfully!";
                contact.Name = "";
                contact.Telephone = "";
                contact.Descr = "";
            }
            catch (Exception ex)
            {
                errorMessage = "An error occurred while adding the contact: " + ex.Message;
            }
           
        }
    }
}
