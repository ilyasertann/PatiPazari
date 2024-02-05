using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;

namespace BusinessLayer.Concrete
{
    public class AdvertManager : IAdvertService
    {
        private readonly IAdvertDal _AdvertDal;

        public AdvertManager(IAdvertDal AdvertDal)
        {
            _AdvertDal = AdvertDal;
        }

        public void Delete(Advert entity)
        {
            _AdvertDal.Delete(entity);
            // You may want to handle any additional business logic or error checking here.
        }

        public List<Advert> GetAll()
        {
            return _AdvertDal.GetAll();
        }

        public Advert GetById(int id)
        {
            return _AdvertDal.GetById(id);
        }

        public void Insert(Advert entity)
        {
            _AdvertDal.Insert(entity);
            // You may want to handle any additional business logic or error checking here.
        }

        public void Update(Advert entity)
        {
            _AdvertDal.Update(entity);
            // You may want to handle any additional business logic or error checking here.
        }
    }
}
