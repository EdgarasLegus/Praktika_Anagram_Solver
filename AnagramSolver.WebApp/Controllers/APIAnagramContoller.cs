using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AnagramSolver.Interfaces;

namespace AnagramSolver.WebApp.Controllers
{
    [Route("api")]
    [ApiController]
    public class APIAnagramController : ControllerBase
    {
        private readonly IAnagramSolver _anagramSolver;

        public APIAnagramController(IAnagramSolver anagramSolver)
        {
            _anagramSolver = anagramSolver;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {

            try
            {
                if (string.IsNullOrEmpty(id))
                    throw new Exception("Error! At least one word must be entered.");

                var anagrams = await _anagramSolver.GetAnagrams(id);

                return Ok(anagrams);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

