using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Dictionaire_contacts.Pages.Clients
{
    public class IndexModel : PageModel
    {
        public List<Contact> ListContacts { get; private set; }

        public void OnGet()
        {
            ListContacts = new List<Contact>();

            try
            {
                string connectionString = "Data Source=DESKTOP-AS9GTHA\\SQLEXPRESS;Initial Catalog=contacts;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM contacts";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Contact contact = new Contact
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("id")).ToString(),
                                    Name = reader.GetString(reader.GetOrdinal("name")),
                                    Telephone = reader.GetString(reader.GetOrdinal("telephone")),
                                    Descr = reader.GetString(reader.GetOrdinal("descr")),
                                    Creedate = reader.GetDateTime(reader.GetOrdinal("creedate")).ToString(),
                                    ImagePath = reader["ImagePath"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("ImagePath")) : null
                                };
                                ListContacts.Add(contact);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception or display an error message to the user
                Console.WriteLine(ex.Message);
            }
        }
    }
    public class Contact
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Telephone { get; set; }
        public string Descr { get; set; }
        public string Creedate { get; set; }
        public string ImagePath { get; set; } 
    }


}


