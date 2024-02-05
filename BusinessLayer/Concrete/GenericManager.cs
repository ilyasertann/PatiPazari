using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class GenericManager<T> : IGenericService<T> where T : class
    {
        private readonly IGenericDal<T> genericDal;

        public GenericManager(IGenericDal<T> genericDal)
        {
            this.genericDal = genericDal ?? throw new ArgumentNullException(nameof(genericDal));
        }

        public void Insert(T entity)
        {
            genericDal.Insert(entity);
        }

        public void Update(T entity)
        {
            genericDal.Update(entity);
        }

        public void Delete(T entity)
        {
            genericDal.Delete(entity);
        }

        public List<T> GetAll()
        {
            return genericDal.GetAll();
        }

        public T GetById(int id)
        {
            return genericDal.GetById(id);
        }
    }
}
