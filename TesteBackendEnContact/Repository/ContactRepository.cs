using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.Sqlite;
using TesteBackendEnContact.Core.Domain.Contact;
using TesteBackendEnContact.Core.Interface.Contact;
using TesteBackendEnContact.Database;
using TesteBackendEnContact.Repository.Interface;

namespace TesteBackendEnContact.Repository
{
    public class ContactRepository : IContactRepository
    {
        private readonly DatabaseConfig databaseConfig;

        public ContactRepository(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public async Task<IContact> SaveAsync(IContact contact)
        {
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);
            var dao = new ContactDao(contact);

            if (dao.Id == 0)
                dao.Id = await connection.InsertAsync(dao);
            else
                await connection.UpdateAsync(dao);

            return dao.Export();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);

            var sql = "DELETE FROM Contact WHERE Id = @id;";

            await connection.ExecuteAsync(sql.ToString(), new { id });
        }

        public async Task<IEnumerable<IContact>> GetAllAsync()
        {
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);

            var query = "SELECT * FROM Contact";
            var result = await connection.QueryAsync<ContactDao>(query);

            var returnList = new List<IContact>();

            foreach (var contact in result.ToList())
            {
                IContact contactInterface = new Contact(contact.Id, contact.ContactBookId, contact.CompanyId, contact.Name, contact.Phone, contact.Email, contact.Address );
                returnList.Add(contactInterface);
            }

            return returnList.ToList();
        }

        public async Task<IContact> GetAsync(int id)
        {
            var list = await GetAllAsync();

            return list.ToList().Where(item => item.Id == id).FirstOrDefault();
        }

        public async Task<List<IContact>> GetAllSearch(string search)
        {
            var list = await GetAllAsync();
            if (search == null){
                List<IContact> result1 =(from item in list select item).ToList();
                return result1;
            }
                
            List<IContact> result2 =    
                (from item in list
                where 
                    item.Name.Contains(search) ||
                    item.Email.Contains(search) ||
                    item.Address.Contains(search)
                select item).ToList();

            return result2;
        }

        public async Task<List<IContact>> GetAllSearchName(string search)
        {
            var list = await GetAllAsync();
            if (search == null){
                List<IContact> result1 =
                    (from item in list
                    orderby item.ContactBookId ascending
                    select item).ToList();
                return result1;
            }
                
            List<IContact> result2 =    
                (from item in list
                where 
                    item.Name.Contains(search)
                orderby item.ContactBookId ascending
                select item).ToList();

            return result2;
        }

        [Table("Contact")]
        public class ContactDao : IContact
        {
            [Key]
            public int Id { get; set; }
            public int ContactBookId { get; set; }
            public int CompanyId { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public string Address { get; set; }

            public ContactDao()
            {
            }

            public ContactDao(IContact contact)
            {
                Id = contact.Id;
                ContactBookId = contact.ContactBookId;
                CompanyId = contact.CompanyId;
                Name = contact.Name;
                Phone = contact.Phone;
                Email = contact.Email;
                Address = contact.Address;
            }

            public IContact Export() => new Contact(Id, ContactBookId, CompanyId, Name, Phone, Email, Address);
        }

    }
}