using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Port_Yetti.Models;
using Port_Yetti.Repository;
using Microsoft.Extensions.Options;

namespace Port_Yetti.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ServiceController : Controller
    {
        private readonly ServiceRepository serviceRepository;

        private readonly PortYettiOptions _options;

        public ServiceController(IOptions<PortYettiOptions> optionsAccessor)
        {
            _options = optionsAccessor.Value;

            serviceRepository = new ServiceRepository(_options);
        }

        /// <summary>
        /// Get list of Services
        /// </summary>
        /// <returns>A list of Services</returns>
        /// <response code="200">List of Services found</response>
        /// <response code="404">List of Services not found</response>
        // GET api/service/getall
        [ProducesResponseType(typeof(IEnumerable<Service>), 200)]
        [ProducesResponseType(typeof(IEnumerable<Service>), 404)]
        [HttpGet("", Name = "Service/GetAll")]
        public IActionResult Get()
        {
            IEnumerable<Service> returnedServices = serviceRepository.GetAll();
            
            if (returnedServices != null)
                return new OkObjectResult(returnedServices);
            else
                return new NotFoundResult();
        }

        /// <summary>
        /// Get a Service by ID
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Service found</response>
        /// <response code="400">Service found</response>
        /// <response code="404">Service not found</response>
        // GET api/service/getbyid
        [ProducesResponseType(typeof(Service), 200)]
        [ProducesResponseType(typeof(Service), 404)]
        [HttpGet("GetById/{id}", Name = "Service/GetById")]
        public IActionResult GetById(int id)
        {
            Service returnedService = serviceRepository.GetByID(id);

            if (returnedService != null)
                return new OkObjectResult(returnedService);
            else
                return new NotFoundResult();
        }

        /// <summary>
        /// Get a Service by Name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <response code="200">Service found</response>
        /// <response code="404">Service not found</response>
        // GET api/service/getbyname
        [ProducesResponseType(typeof(Service), 200)]
        [ProducesResponseType(typeof(EmptyResult), 400)]
        [ProducesResponseType(typeof(EmptyResult), 404)]
        [HttpGet("GetByName/{name}", Name = "Service/GetByName")]
        public IActionResult GetByName(string name)
        {
            if (name == null)
                return new BadRequestResult();

            Service returnedService = serviceRepository.GetByName(name);

            if (returnedService != null)
                return new OkObjectResult(returnedService);
            else
                return new NotFoundResult();
        }

        /// <summary>
        /// Add new Service
        /// </summary>
        /// <param name="service"></param>
        /// <response code="201">Service created</response>
        /// <response code="400">Invalid entry</response>
        // POST api/service/post
        [ProducesResponseType(typeof(int?), 201)]
        [ProducesResponseType(typeof(EmptyResult), 400)]
        [HttpPost("", Name = "Service/Post")]
        public IActionResult Post([FromBody]ServicePost service)
        {
            if (service == null)
                return new BadRequestObjectResult(service);

            int insertedId = serviceRepository.AddNew(service);

            if (insertedId >= 0)
                return new OkObjectResult(insertedId);
            else
                return new BadRequestResult();
        }

        /// <summary>
        /// Update an existing Service
        /// </summary>
        /// <param name="service"></param>
        /// <response code="201">Service updated</response>
        /// <response code="400">Invalid entry</response>
        // PUT api/service/put
        [ProducesResponseType(typeof(EmptyResult), 201)]
        [ProducesResponseType(typeof(EmptyResult), 400)]
        [HttpPut("", Name = "Service/Put")]
        public IActionResult Put([FromBody]Service service)
        {
            if (service == null)
                return new BadRequestResult();

            bool isUpdated = serviceRepository.Update(service);

            if (isUpdated)
                return new OkResult();
            else
                return new BadRequestResult();
        }
    }
}
