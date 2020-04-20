using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class NationalParksController : Controller
    {
        private readonly INationalParkRepository _npRepo;
        private readonly IMapper _mapper;

        public NationalParksController(INationalParkRepository npRepo, IMapper mapper)
        {
            this._npRepo = npRepo;
            this._mapper = mapper;
        }

        /// <summary>
        /// Get list of National Parks.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<NationalParkDto>))]
        public IActionResult GetNationalParks()
        {
            var objList = this._npRepo.GetNationalParks();
            var objDto = new List<NationalParkDto>();

            foreach( var obj in objList)
            {
                objDto.Add(this._mapper.Map<NationalParkDto>(obj));
            }

            return Ok(objDto);
        }

        /// <summary>
        /// Get individual national park.
        /// </summary>
        /// <param name="Id">The Id of National Park</param>
        /// <returns></returns>
        [HttpGet("{Id:int}", Name = "GetNationalPark")]
        [ProducesResponseType(200, Type = typeof(NationalParkDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetNationalPark(int Id)
        {
            var obj = this._npRepo.GetNationalPark(Id);
            if ( obj == null)
            {
                return NotFound();
            }
            var objDto = _mapper.Map<NationalParkDto>(obj);

            return Ok(objDto);
        }

        /// <summary>
        /// Create a National Park
        /// </summary>
        /// <param name="nationalParkDto">Fields for create a National Park</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(NationalParkDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateNationalPark([FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null)
            {
                return BadRequest(ModelState);
            }

            if (this._npRepo.NationalParkExists(nationalParkDto.Name))
            {
                ModelState.AddModelError("","National Park Exists!");
                return StatusCode(404, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var nationalParkObj = this._mapper.Map<NationalPark>(nationalParkDto);

            if (!this._npRepo.CreateNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {nationalParkObj.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetNationalPark", new { Id = nationalParkObj.Id }, nationalParkObj);
        }

        /// <summary>
        /// Update a National Park.
        /// </summary>
        /// <param name="Id">The Id of National Park</param>
        /// <param name="nationalParkDto">Fields to update</param>
        /// <returns></returns>
        [HttpPatch("{Id:int}", Name = "UpdateNationalPark")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateNationalPark(int Id, [FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null || Id != nationalParkDto.Id)
            {
                return BadRequest(ModelState);
            }

            var nationalParkObj = this._mapper.Map<NationalPark>(nationalParkDto);
            if (!this._npRepo.UpdateNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {nationalParkObj.Name}");

                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        /// <summary>
        /// Delete a National Park.
        /// </summary>
        /// <param name="Id">The Id of National Park</param>
        /// <returns></returns>
        [HttpDelete("{Id:int}", Name = "DeleteNationalPark")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteNationalPark(int Id)
        {
            if (!this._npRepo.NationalParkExists(Id))
            {
                return NotFound();
            }

            var nationalParkObj = this._npRepo.GetNationalPark(Id);
            if (!this._npRepo.DeleteNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {nationalParkObj.Name}");

                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}