using BlogAPI.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public List<blogger> GetAllBloggers() {
            return null;
        }
        [HttpPost]
        public void AddNewBlogger(blogger blogger)
        {
            return null;
        }
        [HttpPut]
        public object updateBlogger(int id, blogger blogger)
        {
            return null;
        }
        [HttpDelete]
        public object deleteBlogger(int id) {

            return null;
        }
    }
}
