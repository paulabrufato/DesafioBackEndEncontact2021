using System.Collections.Generic;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Interface.Contact;

namespace TesteBackendEnContact.Repository.Interface
{
    public interface IContactRepository
    {
        Task<IContact> SaveAsync(IContact contact);
        Task DeleteAsync(int id);
        Task<IEnumerable<IContact>> GetAllAsync();
        Task<IContact> GetAsync(int id); 
        Task<List<IContact>> GetAllSearch(string search);
        Task<List<IContact>> GetAllSearchName(string search);
    }
}