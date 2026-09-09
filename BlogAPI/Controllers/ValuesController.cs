using BlogAPI.models;
using BlogAPI.models.DTIOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace BlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly string ConnectionString = "Server=localhost;Database=blog;uid=root;Password=;";
        [HttpGet]
        public List<blogger> GetAllBloggers() {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            string sql = "SELECT * FROM bloggers";

            var cmd = new MySqlCommand(sql, connector);
            var dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                var blogger = new blogger
                {
                    Id = dataReader.GetInt32(0),
                    Name = dataReader.GetString(1),
                    Email = dataReader.GetString(2),
                    Age = dataReader.GetInt32(3),
                    Password = dataReader.GetString(4),
                    RegistrationTime = dataReader.GetDateTime(5)
                };
            }



            connector.Close();
            return null;
        }
        [HttpPost]
        public  object NewBlogger(AddBloggerDTO Blg)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            var blg = new blogger
            {
                Name = Blg.Name,
                Email = Blg.Email,
                Age = Blg.Age,
                Password = Blg.Password,
                RegistrationTime = DateTime.Now
            };

            var sql = $" INSERT INTO bloggers (Name, Email, Age, Password, RegistrationTime) VALUES (@Name, @Email, @Age, @Password, @RegistrationTime)";

             var cmd = new MySqlCommand(sql, connector);
             cmd.Parameters.AddWithValue("@Name", blg.Name);
             cmd.Parameters.AddWithValue("@Email", blg.Email);
             cmd.Parameters.AddWithValue("@Age", blg.Age);
             cmd.Parameters.AddWithValue("@Password", blg.Password);
             cmd.Parameters.AddWithValue("@RegistrationTime", blg.RegistrationTime);

            cmd.ExecuteNonQuery();

            connector.Close();

            return Blg;
        }
        [HttpPut]
        public object updateBlogger(int id, blogger blogger)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            connector.Close();
            return blogger;
        }
        [HttpDelete]
        public object deleteBlogger(int id) {

            var connector = new MySqlConnection(ConnectionString); connector.Open();

            var sql = $"DELETE FROM bloggers WHERE Id=@Id";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.ExecuteNonQuery();
            connector.Close();
            return new { message = "Blogger deleted successfully" };
        }
    }
}
