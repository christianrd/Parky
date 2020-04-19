using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyApi.Models;
using ParkyApi.Models.Dto;
using ParkyApi.Repository.IRepository;

namespace ParkyApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class NationalParksController : Controller
    {
        private INationalParkRepository _npRepo;
        private readonly IMapper _mapper;

        public NationalParksController(INationalParkRepository npRepo, IMapper mapper)
        {
            this._npRepo = npRepo;
            this._mapper = mapper;
        }

        [HttpGet]
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

        [HttpGet("{Id:int}", Name = "GetNationalPark")]
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

        [HttpPost]
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

        [HttpPatch("{Id:int}", Name = "UpdateNationalPark")]
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

        [HttpDelete("{Id:int}", Name = "DeleteNationalPark")]
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