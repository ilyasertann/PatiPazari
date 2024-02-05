using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DataAccessLayer.Repositories;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.EntityFramework
{
    public class EfUserRepository : GenericRepository<User>, IUserDal
    {
        private readonly PatiPazariDbContext context;

        public EfUserRepository(PatiPazariDbContext dbContext) : base(dbContext)
        {
            context = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

        }

        public bool GetByUsername(string email)
        {
            return context.Users.Any(x => x.Email == email);
        }

        public int GetByUsernameAndPassword(string email, string password)
        {
            return context.Users.Where(x => x.Email == email && x.Password == password).Count();
        }
    }
}
