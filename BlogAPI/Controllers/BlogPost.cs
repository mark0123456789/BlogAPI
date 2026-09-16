using BlogAPI.models;
using BlogAPI.models.DTIOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace BlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPost : ControllerBase
    {
        private readonly string ConnectionString = "Server=localhost;Database=blog;uid=root;Password=;";
        [HttpGet]
        public List<BlogPost> GetAllBloggersPosts() 
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            string sql = "SELECT * FROM blogpost";
            var cmd = new MySqlCommand(sql, connector);
            var dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                var bloggerpost = new blogger
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
        public object NewBloggersPosts(AddBloggerPostDTO BlgPST) 
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            var blgPST = new BlogPost
            {
                Title= BlgPST.title,
                Content = BlgPST.content,
                PostTime = DateTime.Now,
                BlogId = BlgPST.BlogId
            };

            var sql = $" INSERT INTO `blogpost`(`title`, `content`, `PostTime`,`BlogId`) VALUES (@title, @content, @PostTime, @BlogId)";

            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@title", blgPST.Title);
            cmd.Parameters.AddWithValue("@content", blgPST.Content);
            cmd.Parameters.AddWithValue("@PostTime", blgPST.PostTime);
            cmd.Parameters.AddWithValue("@BlogId", blgPST.BlogId);

            cmd.ExecuteNonQuery();

            connector.Close();

            return BlgPST;
        }
        [HttpPut]
        public object updateBloggersPosts(int id, UpdateBloggerDTO updateBloggerDTO) 
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            connector.Close();
            return null;
        }
        [HttpDelete]
        public object deleteBloggersPosts(int id)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            connector.Close();
            return null;
        }
    }
}
