using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TesteBackendEnContact.Core.Domain.Contact;
using TesteBackendEnContact.Core.Interface.Contact;
using TesteBackendEnContact.Repository.Interface;

namespace TesteBackendEnContact.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly ILogger<ContactController> _logger;

        public ContactController(ILogger<ContactController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<IContact> Post(Contact contact, [FromServices] IContactRepository contactRepository)
        {
            return await contactRepository.SaveAsync(contact);
        }

        [HttpDelete]
        public async Task Delete(int id, [FromServices] IContactRepository contactRepository)
        {
            await contactRepository.DeleteAsync(id);
        }

        [HttpGet]
        public async Task<IEnumerable<IContact>> Get([FromServices] IContactRepository contactRepository)
        {
            return await contactRepository.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<IContact> Get(int id, [FromServices] IContactRepository contactRepository)
        {
            return await contactRepository.GetAsync(id);
        }

        [HttpGet("search")]
        public async Task<List<IContact>> GeBySearch([FromQuery] string search, [FromServices] IContactRepository contactRepository)
        {
            return await contactRepository.GetAllSearch(search);
        }

        [HttpGet("search-name")]
        public async Task<List<IContact>> GeBySearchName([FromQuery] string search, [FromServices] IContactRepository contactRepository)
        {
            return await contactRepository.GetAllSearchName(search);
        }

    }
}