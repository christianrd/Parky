using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

            return Ok(objList);
        }

        [HttpGet(":Id")]
        public IActionResult GetNationalPark(int Id)
        {
            var obj = this._npRepo.GetNationalPark(Id);

            return Ok(obj);
        }
    }
}