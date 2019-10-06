using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;
using TwoDrive.WebApi.Interfaces;

namespace TwoDrive.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private ICurrent inSession;

        private IFolderLogic folderLogic;

        private ILogic<File> fileLogic;

        private ILogic<Writer> writerLogic;

        [HttpPost("{id}")]
        public IActionResult Create(int id)
        {
            return null;
        }


    }
}