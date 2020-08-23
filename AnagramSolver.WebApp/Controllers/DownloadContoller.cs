using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnagramSolver.Interfaces;
using AnagramSolver.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using Grpc.Core;
using System.Text;

namespace AnagramSolver.WebApp.Controllers
{
    [Route("api/download_dictionary")]
    [ApiController]
    public class DownloadController : ControllerBase
    {

        public DownloadController()
        {
        }

        [HttpGet]
        [Route("zodynas")]
        public FileResult DownloadDictionary()
        {
            var file = Configuration.GetFileNameFromConfiguration();
            //String mimeType = MimeMapping.GetMimeMapping(path);
            //var filepath = System.IO.Path.Combine(Server.MapPath("./zodynas.txt"), file);
            byte[] fileBytes = System.IO.File.ReadAllBytes(@"./zodynas.txt");
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, file);
        }
    }
}

