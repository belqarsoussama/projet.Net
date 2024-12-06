using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Dictionaire_contacts.Pages.Clients
{
    public class EditModel : PageModel
    {
        public Contact contact = new Contact();
        public String errorMessage = "";
        public String successMessage = "";

        public void OnGet()
        {
            String id = Request.Query["Id"];

            try
            {
                String connectionString = "Data Source=DESKTOP-AS9GTHA\\SQLEXPRESS;Initial Catalog=contacts;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    String sql = "SELECT * FROM contacts WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                contact.Id = "" + reader.GetInt32(0);
                                contact.Name = reader.GetString(1);
                                contact.Telephone = reader.GetString(2);
                                contact.Descr = reader.GetString(3);
                                contact.ImagePath = reader.IsDBNull(4) ? null : reader.GetString(4); 
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }

        public void OnPost(IFormFile uploadedImage)
        {
            contact.Id = Request.Form["Id"];
            contact.Name = Request.Form["Name"];
            contact.Telephone = Request.Form["Telephone"];
            contact.Descr = Request.Form["Description"];

            if (uploadedImage != null)
            {
                
                var fileName = Path.GetFileName(uploadedImage.FileName);
                var filePath = Path.Combine("wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadedImage.CopyTo(stream);
                }

                
                contact.ImagePath = "/images/" + fileName;
            }

            try
            {
                string connectionString = "Data Source=DESKTOP-AS9GTHA\\SQLEXPRESS;Initial Catalog=contacts;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE contacts SET name=@name, telephone=@telephone, descr=@descr, imagePath=@imagePath, creedate=CONVERT(VARCHAR(20), GETDATE(), 120) WHERE id=@id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", contact.Name);
                        command.Parameters.AddWithValue("@telephone", contact.Telephone);
                        command.Parameters.AddWithValue("@descr", contact.Descr);
                        command.Parameters.AddWithValue("@imagePath", contact.ImagePath);
                        command.Parameters.AddWithValue("@id", contact.Id);
                        command.ExecuteNonQuery();
                    }
                }
                successMessage = "Contact Edited Successfully!";
                contact.Name = "";
                contact.Telephone = "";
                contact.Descr = "";
                contact.ImagePath = "";
            }
            catch (Exception ex)
            {
                errorMessage = "An error occurred while updating the contact: " + ex.Message;
            }

            Response.Redirect("/Clients/Index");
        }
    }
}
