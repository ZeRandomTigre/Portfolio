using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Port_Yetti.Models;
using Port_Yetti.Repository;
using Microsoft.Extensions.Options;

namespace Port_Yetti.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class  SettingNameController : Controller
    {
        private readonly SettingNameRepository settingNameRepository;

        private readonly PortYettiOptions _options;

        public SettingNameController(IOptions<PortYettiOptions> optionsAccessor)
        {
            _options = optionsAccessor.Value;

            settingNameRepository = new SettingNameRepository(_options);
        }

        /// <summary>
        /// Get list of SettingNames
        /// </summary>
        /// <returns>A list of SettingNames</returns>
        /// <response code="200">List of SettingNames found</response>
        /// <response code="404">List of SettingNames not found</response>>
        // GET api/settingName/getall
        [ProducesResponseType(typeof(IEnumerable<SettingName>), 200)]
        [ProducesResponseType(typeof(IEnumerable<SettingName>), 404)]
        [HttpGet("", Name = "SettingName/GetAll")]
        public IActionResult Get()
        {
            IEnumerable<SettingName> returnedSettingNames = settingNameRepository.GetAll();
                        
            if (returnedSettingNames != null)
                return new OkObjectResult(returnedSettingNames);
            else
                return new NotFoundResult();
        }

        /// <summary>
        /// Get a SettingName by ID
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">SettingName found</response>
        /// <response code="404">SettingName not found</response>
        // GET api/settingName/getbyid
        [ProducesResponseType(typeof(SettingName), 200)]
        [ProducesResponseType(typeof(SettingName), 404)]
        [HttpGet("GetById/{id}", Name = "SettingName/GetById")]
        public IActionResult GetById(int id)
        {
            SettingName returnedSettingName = settingNameRepository.GetByID(id);

            if (returnedSettingName != null)
                return new OkObjectResult(returnedSettingName);
            else
                return new NotFoundResult();
        }

        /// <summary>
        /// Get a SettingName by Name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <response code="200">SettingName found</response>
        /// <response code="404">SettingName not found</response>
        // GET api/settingName/getbyname
        [ProducesResponseType(typeof(SettingName), 200)]
        [ProducesResponseType(typeof(EmptyResult), 400)]
        [ProducesResponseType(typeof(EmptyResult), 404)]
        [HttpGet("GetByName/{name}", Name = "SettingName/GetByName")]
        public IActionResult GetByName(string name)
        {
            if (name == null)
                return new BadRequestResult();

            SettingName returnedSettingName = settingNameRepository.GetByName(name);

            if (returnedSettingName != null)
                return new OkObjectResult(returnedSettingName);
            else
                return new NotFoundResult();
        }

        /// <summary>
        /// Add new SettingName
        /// </summary>
        /// <param name="settingName"></param>
        /// <response code="201">SettingName created</response>
        /// <response code="400">Invalid entry</response>
        // POST api/settingName/post
        [ProducesResponseType(typeof(int), 201)]
        [ProducesResponseType(typeof(EmptyResult), 400)]
        [HttpPost("", Name = "SettingName/Post")]
        public IActionResult Post([FromBody]SettingNamePost settingName)
        {
            if (settingName == null)
                return new BadRequestResult();

            int insertedId = settingNameRepository.AddNew(settingName);

            if (insertedId > 0)
                return new OkObjectResult(insertedId);
            else
                return new BadRequestResult();
        }

        /// <summary>
        /// Update an existing SettingName
        /// </summary>
        /// <param name="settingName"></param>
        /// <response code="201">SettingName updated</response>
        /// <response code="400">Invalid entry</response>
        // PUT api/settingNames/put
        [ProducesResponseType(typeof(EmptyResult), 201)]
        [ProducesResponseType(typeof(EmptyResult), 400)]
        [HttpPut("", Name = "SettingName/Put")]
        public IActionResult Put([FromBody]SettingName settingName)
        {
            if (settingName == null)
                return new BadRequestResult();

            bool isUpdated = settingNameRepository.Update(settingName);

            if (isUpdated)
                return new OkResult();
            else
                return new BadRequestResult();
        }
    }
}