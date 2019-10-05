
using Microsoft.AspNetCore.Mvc;
using TwoDrive.BusinessLogic.Exceptions;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.Domain;
using TwoDrive.WebApi.Filters;
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
        [AuthorizeFilter(Role.Administrator)]
        public IActionResult Create([FromBody] WriterModel model)
        {
            try
            {
                var writer = WriterModel.ToDomain(model);
                Logic.Create(writer);
                return Ok("Writer Created");
            }
            catch (ValidationException validationError)
            {
                return BadRequest(validationError.Message);
            }
        }

        [HttpDelete("{id}")]
        [AuthorizeFilter(Role.Administrator)]
        public IActionResult Delete(int id)
        {
            try
            {
                var writer = Logic.Get(id);
                Logic.Delete(id);
                return Ok($"Writer: {writer.UserName} has been deleted");
            }
            catch (LogicException exception)
            {
                return BadRequest(exception.Message);
            }
        }
    }
}