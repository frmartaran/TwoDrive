
using Microsoft.AspNetCore.Mvc;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.Domain;
using TwoDrive.WebApi.Models;

namespace TwoDrive.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class WriterController : ControllerBase
    {
        private ILogic<Writer> Logic { get; set; }

        public WriterController(ILogic<Writer> logic) : base()
        {
            Logic = logic;
        }

        [HttpPost]
        public IActionResult Create([FromBody] WriterModel model)
        {
            var writer = WriterModel.ToDomain(model);
            Logic.Create(writer);
            return Ok("Writer Created");
        }
    }
}