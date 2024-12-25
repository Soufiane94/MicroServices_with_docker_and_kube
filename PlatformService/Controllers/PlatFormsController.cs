using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatFormService.AsyncDataServices;
using PlatFormService.Data;
using PlatFormService.Dtos;
using PlatFormService.Models;
using PlatFormService.SyncDataServices.Http;

namespace PlatFormService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatFormsController : ControllerBase
    {
        private readonly IPlatformRepo _repository;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;
        private readonly IMessageBusClient _messageBusClient;

        public PlatFormsController(
            IPlatformRepo repository,
            IMapper mapper,
            ICommandDataClient commandDataClient,
            IMessageBusClient messageBusClient)
        {
            _repository = repository;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
            _messageBusClient = messageBusClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatForms()
        {
            Console.WriteLine("-->Getting Platforms");

            var platforms =_repository.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
        }


        [HttpGet("{id}", Name = nameof(GetPlatFormById))]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatFormById(int id)
        {
            Console.WriteLine($"-->Getting Platform with id : {id}");

            var platform =_repository.GetPlatFormById(id);
            if(platform is not null){
                return Ok(_mapper.Map<PlatformReadDto>(platform));
            }
            return NotFound(id);
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreate)
        {
            Console.WriteLine($"-->Creating Platform : {platformCreate?.Name}");
            Platform platform = _mapper.Map<Platform>(platformCreate);
            _repository.CreatePlatform(platform);
            _repository.SaveChanges();

            PlatformReadDto platformRead = _mapper.Map<PlatformReadDto>(platform);

            // Send sync message
            try
            {
                await _commandDataClient.SendPlatformToCommand(platformRead);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
            }

            // Send Async Message
            try
            {
                var platformPublishedDto = _mapper.Map<PlatformPublishedDto>(platformRead);
                platformPublishedDto.Event = "Platform_Published";
                _messageBusClient.PublishNewPlatform(platformPublishedDto);
            }
            catch (System.Exception)
            {
                
                throw;
            }

            return CreatedAtRoute(nameof(GetPlatFormById), new { Id = platformRead.Id }, platformRead);
        }
    }
}