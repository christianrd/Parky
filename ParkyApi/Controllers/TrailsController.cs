using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyApi.Models;
using ParkyApi.Models.Dtos;
using ParkyApi.Repository.IRepository;

namespace ParkyApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class TrailsController : Controller
    {
        private readonly ITrailRepository _trailRepo;
        private readonly IMapper _mapper;

        public TrailsController(ITrailRepository trailRepo, IMapper mapper)
        {
            this._trailRepo = trailRepo;
            this._mapper = mapper;
        }

        /// <summary>
        /// Get list of Trails.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<TrailDto>))]
        public IActionResult GetTrails()
        {
            var objList = this._trailRepo.GetTrails();
            var objDto = new List<TrailDto>();

            foreach( var obj in objList)
            {
                objDto.Add(this._mapper.Map<TrailDto>(obj));
            }

            return Ok(objDto);
        }

        /// <summary>
        /// Get individual trail.
        /// </summary>
        /// <param name="Id">The Id of Trail</param>
        /// <returns></returns>
        [HttpGet("{Id:int}", Name = "GetTrail")]
        [ProducesResponseType(200, Type = typeof(TrailDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrail(int Id)
        {
            var obj = this._trailRepo.GetTrail(Id);
            if ( obj == null)
            {
                return NotFound();
            }
            var objDto = _mapper.Map<TrailDto>(obj);

            return Ok(objDto);
        }

        /// <summary>
        /// Create a Trail
        /// </summary>
        /// <param name="trailDto">Fields for create a Trail</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(TrailDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateTrail([FromBody] TrailCreateDto trailDto)
        {
            if (trailDto == null)
            {
                return BadRequest(ModelState);
            }

            if (this._trailRepo.TrailExists(trailDto.Name))
            {
                ModelState.AddModelError("","Trail Exists!");
                return StatusCode(404, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var trailObj = this._mapper.Map<Trail>(trailDto);

            if (!this._trailRepo.CreateTrail(trailObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {trailObj.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetTrail", new { Id = trailObj.Id }, trailObj);
        }

        /// <summary>
        /// Update a Trail.
        /// </summary>
        /// <param name="Id">The Id of Trail</param>
        /// <param name="trailDto">Fields to update</param>
        /// <returns></returns>
        [HttpPatch("{Id:int}", Name = "UpdateTrail")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateTrail(int Id, [FromBody] TrailUpdateDto trailDto)
        {
            if (trailDto == null || Id != trailDto.Id)
            {
                return BadRequest(ModelState);
            }

            var trailObj = this._mapper.Map<Trail>(trailDto);
            if (!this._trailRepo.UpdateTrail(trailObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {trailObj.Name}");

                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        /// <summary>
        /// Delete a Trail.
        /// </summary>
        /// <param name="Id">The Id of Trail</param>
        /// <returns></returns>
        [HttpDelete("{Id:int}", Name = "DeleteTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteTrail(int Id)
        {
            if (!this._trailRepo.TrailExists(Id))
            {
                return NotFound();
            }

            var trailObj = this._trailRepo.GetTrail(Id);
            if (!this._trailRepo.DeleteTrail(trailObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {trailObj.Name}");

                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}