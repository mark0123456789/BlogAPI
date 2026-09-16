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

            string sql = "SELECT * FROM blogger";

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

            var sql = $" INSERT INTO blogger (Name, Email, Age, Password, RegistrationTime) VALUES (@Name, @Email, @Age, @Password, @RegistrationTime)";

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
        public object updateBlogger(int id, UpdateBloggerDTO updateBloggerDTO)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            string sql = $"UPDATE blogger SET Name=@Name, Email=@Email, Age=@Age, Password=@Password WHERE Id=@Id";

            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Name", updateBloggerDTO.Name);
            cmd.Parameters.AddWithValue("@Email", updateBloggerDTO.Email);
            cmd.Parameters.AddWithValue("@Age", updateBloggerDTO.Age);
            cmd.Parameters.AddWithValue("@Password", updateBloggerDTO.Password);

            cmd.ExecuteNonQuery();

            var updatedBlogger = new blogger
            {
                Id = id,
                Name = updateBloggerDTO.Name,
                Email = updateBloggerDTO.Email,
                Age = updateBloggerDTO.Age,
                Password = updateBloggerDTO.Password
            };
            connector.Close();
            return updatedBlogger;
        }
        [HttpDelete]
        public object deleteBlogger(int id) {

            var connector = new MySqlConnection(ConnectionString); connector.Open();

            var sql = $"DELETE FROM blogger WHERE Id=@Id";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.ExecuteNonQuery();
            connector.Close();
            return new { message = "Blogger deleted successfully" };
        }
        [HttpGet("ById")]
        public object GetBloggerById(int id)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            string sql = $"SELECT Name, Email FROM blogger WHERE Id=@Id";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@Id", id);
            var dataReader = cmd.ExecuteReader();
            dataReader.Read();
            var blogger = new 
                {
                    Name = dataReader.GetString(0),
                    Email = dataReader.GetString(1),
                };

            connector.Close();
            return blogger;
        }
        [HttpGet("Bybloggerownpost")]
        public object GetBloggerOwnPosts(int id)
        {
            var OwnPosts = new List<object>();
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            string sql = $"SELECT blogger.Name, blogpost.title,blogpost.content\r\nFROM `blogger`\r\nINNER JOIN blogpost ON blogger.Id = blogpost.BlogId\r\nWHERE blogger.Id = @Id";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@Id", id);
            var dataReader = cmd.ExecuteReader();
            while (dataReader.Read()) { 
            var BloggerOwnPosts = new
            {
                Name = dataReader.GetString(0),
                title = dataReader.GetString(1),
                Content = dataReader.GetString(2)
            };
                OwnPosts.Add(BloggerOwnPosts);
            }
            connector.Close();
            return OwnPosts;
        }

        [HttpGet("NumberOfposts")]
        public object GetNumberOfPosts(int id)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            string sql = $"SELECT COUNT(blogpost.Id) FROM `blogger` INNER JOIN blogpost ON blogger.Id = blogpost.BlogId WHERE blogger.Id = @Id";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@Id", id);
            var dataReader = cmd.ExecuteReader();
            dataReader.Read();
            var NumberOfPosts = new
            {
                Count = dataReader.GetInt32(0)
            };
            connector.Close();
            return NumberOfPosts;
        }

    }
}
