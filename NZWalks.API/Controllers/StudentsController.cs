using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NZWalks.API.Controllers
{
    // https://localhost:7161/api/students
    // https://localhost:7161/scalar/v1
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {

        [HttpGet]
        public IActionResult GetAllStudents() 
        {
            List<string> StudentNames = new List<string> 
            {
                "John",
                "Bill",
                "Shawn",
                "Jane"
            };

            return Ok(StudentNames);
        }

    }
}
