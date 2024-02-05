using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;

namespace BusinessLayer.Concrete
{
    public class UserManager : IUserService
    {
        private readonly IUserDal _UserDal;

        public UserManager(IUserDal UserDal)
        {
            _UserDal = UserDal;
        }

        public void Delete(User entity)
        {
            _UserDal.Delete(entity);
         }

        public List<User> GetAll()
        {
            return _UserDal.GetAll();
        }

        public User GetById(int id)
        {
            return _UserDal.GetById(id);
        }

        public bool GetByUsername(string email)
        {
           return _UserDal.GetByUsername(email);
        }

        public int GetByUsernameAndPassword(string email, string password)
        {
            return _UserDal.GetByUsernameAndPassword(email, password);
        }

        public void Insert(User entity)
        {
            _UserDal.Insert(entity);
         }

        public void Update(User entity)
        {
            _UserDal.Update(entity);
         }
    }
}
