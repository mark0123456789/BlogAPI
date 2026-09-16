using BlogAPI.models;
using BlogAPI.models.DTIOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace BlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase       
    {
        private readonly string ConnectionString = "Server=localhost;Database=blog;uid=root;Password=;";

        [HttpGet]
        public List<models.BlogPost> GetAllBloggersPosts()
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
           
            string sql = "SELECT * FROM blogpost";
            var cmd = new MySqlCommand(sql, connector);
            var dataReader = cmd.ExecuteReader();

            var results = new List<models.BlogPost>();
            while (dataReader.Read())
            {
                var post = new models.BlogPost
                {
                    Id = dataReader.GetInt32(0),
                    Title = dataReader.GetString(1),
                    Content = dataReader.GetString(2),
                    PostTime = dataReader.GetDateTime(3),
                    UpdateTime = dataReader.GetDateTime(4),
                    BlogId = dataReader.GetInt32(5)
                };
                results.Add(post);
            }

            connector.Close();
            return results;
        }
        [HttpPost]
        public object NewBloggersPosts(AddBloggerPostDTO BlgPST) 
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            var blgPST = new models.BlogPost
            {
                Title= BlgPST.title,
                Content = BlgPST.content,
                PostTime = DateTime.Now,
                BlogId = BlgPST.BlogId 
            };

            var sql = $" INSERT INTO `blogpost`(`title`, `content`, `PostTime`,`UpdateTime`,`BlogId`) VALUES (@title, @content, @PostTime, @UpdateTime, @BlogId)";

            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@title", blgPST.Title);
            cmd.Parameters.AddWithValue("@content", blgPST.Content);
            cmd.Parameters.AddWithValue("@PostTime", blgPST.PostTime);
            cmd.Parameters.AddWithValue("@BlogId", blgPST.BlogId);
            cmd.Parameters.AddWithValue("@UpdateTime", blgPST.PostTime ?? DateTime.Now);

            cmd.ExecuteNonQuery();

            connector.Close();

            return BlgPST;
        }
        [HttpPut]
        public object UpdateBlogPost(int id, UpdateBloggerPostsDTO dto)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            var sql = "UPDATE blogpost SET title=@title, content=@content, BlogId=@BlogId, UpdateTime=@UpdateTime WHERE id=@Id";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@title", dto.title);
            cmd.Parameters.AddWithValue("@content",dto.content);
            cmd.Parameters.AddWithValue("@BlogId", dto.BlogId ?? 0);
            var updatedTime = DateTime.Now;
            cmd.Parameters.AddWithValue("@UpdateTime", updatedTime);

            cmd.ExecuteNonQuery();

            connector.Close();

            return new models.BlogPost
            {
                Id = id,
                Title = dto.title,
                Content = dto.content,
                PostTime = DateTime.Now,
                UpdateTime = updatedTime,
                BlogId = dto.BlogId ?? 0
            };
        }

        [HttpDelete]
        public object DeleteBlogPost(int id)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            var sql = "DELETE FROM blogpost WHERE id=@Id";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.ExecuteNonQuery();

            connector.Close();
            return new { message = "Blog post deleted successfully" };
        }
        
    }
}
