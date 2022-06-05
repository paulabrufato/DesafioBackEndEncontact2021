using System.ComponentModel.DataAnnotations;
using TesteBackendEnContact.Core.Interface.ContactBook.Company;
using TesteBackendEnContact.Core.Domain.Company;

namespace TesteBackendEnContact.Controllers.Dtos
{
    public class SaveCompanyRequest
    {
        public int Id { get; set; }
        [Required]
        public int ContactBookId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public ICompany ToCompany() => new Company(Id, ContactBookId, Name);
    }
}
