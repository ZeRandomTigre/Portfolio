using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Port_Yetti.Models;
using Port_Yetti.Repository;
using Microsoft.Extensions.Options;

namespace Port_Yetti.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class  SettingController : Controller
    {
        private readonly SettingRepository settingRepository;

        private readonly PortYettiOptions _options;

        public SettingController(IOptions<PortYettiOptions> optionsAccessor)
        {
            _options = optionsAccessor.Value;

            settingRepository = new SettingRepository(_options);
        }

        /// <summary>
        /// Get a list of Settings
        /// </summary>
        /// <returns>A list of Settings</returns>
        /// <response code="200">List of Settings found</response>
        /// <response code="404">List of Settings not found</response>>
        // GET api/setting/getall
        [ProducesResponseType(typeof(IEnumerable<Setting>), 200)]
        [ProducesResponseType(typeof(IEnumerable<EmptyResult>), 404)]
        [HttpGet("", Name = "Setting/GetAll")]
        public IActionResult Get()
        {
            IEnumerable<Setting> returnedSettings = settingRepository.GetAll();
                        
            if (returnedSettings != null)
                return new OkObjectResult(returnedSettings);
            else
                return new NotFoundResult();
        }

        /// <summary>
        /// Get a Setting by ID
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Setting found</response>
        /// <response code="404">Setting not found</response>
        // GET api/setting/getbyid
        [ProducesResponseType(typeof(Setting), 200)]
        [ProducesResponseType(typeof(EmptyResult), 404)]
        [HttpGet("GetById/{id}", Name = "Setting/GetById")]
        public IActionResult GetById(int id)
        {
            Setting returnedSetting = settingRepository.GetByID(id);

            if (returnedSetting != null)
                return new OkObjectResult(returnedSetting);
            else
                return new NotFoundResult();
        }

        /// <summary>
        /// Get a Setting by Value
        /// </summary>
        /// <param name="value"></param>
        /// <response code="200">Setting found</response>
        /// <response code="400">Invalid entry</response>
        /// <response code="404">Setting not found</response>
        // GET api/setting/getbyvalue
        [ProducesResponseType(typeof(Setting), 200)]
        [ProducesResponseType(typeof(EmptyResult), 400)]
        [ProducesResponseType(typeof(EmptyResult), 404)]
        [HttpGet("GetByValue/{value}", Name = "Setting/GetByValue")]
        public IActionResult GetByValue(string value)
        {
            if (value == null)
                return new BadRequestResult();

            Setting returnedSetting = settingRepository.GetByValue(value);

            if (returnedSetting != null)
                return new OkObjectResult(returnedSetting);
            else
                return new NotFoundResult();
        }

        /// <summary>
        /// Get a Setting by SettingID and ServiceID
        /// </summary>
        /// <param name="settingId"></param>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        /// <response code="200">Setting found</response>
        /// <response code="404">Setting not found</response>
        // api/setting/getbysettingserviceid
        [ProducesResponseType(typeof(Setting), 200)]
        [ProducesResponseType(typeof(EmptyResult), 404)]
        [HttpGet("GetBySettingIdServiceId/{settingId}/{serviceId}", Name = "Setting/GetBySettingIdServiceId")]
        public IActionResult GetBySettingServiceId(int settingId, int serviceId)
        {
            Setting returnedSetting = settingRepository.GetBySettingServiceId(settingId, serviceId);

            if (returnedSetting != null)
                return new OkObjectResult(returnedSetting);
            else
                return new NotFoundResult();
        }

        /// <summary>
        /// Add a new Setting
        /// </summary>
        /// <param name="setting"></param>
        /// <response code="201">Setting created</response>
        /// <response code="400">Invalid entry</response>
        // POST api/setting/post
        [ProducesResponseType(typeof(int?), 201)]
        [ProducesResponseType(typeof(EmptyResult), 400)]
        [HttpPost("", Name = "Setting/Post")]
        public IActionResult Post([FromBody]SettingPost setting)
        {
            if (setting == null)
                return new BadRequestResult();

            int insertedId = settingRepository.AddNew(setting);

            if (insertedId > 0)
                return new OkObjectResult(insertedId);
            else
                return new BadRequestResult();
        }

        /// <summary>
        /// Update an existing Setting
        /// </summary>
        /// <param name="setting"></param>
        /// <response code="201">Setting updated</response>
        /// <response code="400">Invalid entry</response>
        // PUT api/setting/put
        [ProducesResponseType(typeof(EmptyResult), 201)]
        [ProducesResponseType(typeof(EmptyResult), 400)]
        [HttpPut("", Name = "Setting/Put")]
        public IActionResult Put([FromBody]Setting setting)
        {
            if (setting == null)
                return new BadRequestResult();

            bool isUpdated = settingRepository.Update(setting);

            if (isUpdated)
                return new OkResult();
            else
                return new BadRequestResult();
        }
    }
}
