using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;

namespace BusinessLayer.Concrete
{
    public class BasketManager : IBasketService
    {
        private readonly IBasketDal _basketDal;

        public BasketManager(IBasketDal BasketDal)
        {
            _basketDal = BasketDal;
        }

        public void Delete(Basket entity)
        {
            _basketDal.Delete(entity);
            // You may want to handle any additional business logic or error checking here.
        }

        public List<Basket> GetAll()
        {
            return _basketDal.GetAll();
        }

        public Basket GetById(int id)
        {
            return _basketDal.GetById(id);
        }

        public void Insert(Basket entity)
        {
            _basketDal.Insert(entity);
            // You may want to handle any additional business logic or error checking here.
        }

        public void Update(Basket entity)
        {
            _basketDal.Update(entity);
            // You may want to handle any additional business logic or error checking here.
        }
    }
}
