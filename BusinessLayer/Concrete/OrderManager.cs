using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;

namespace BusinessLayer.Concrete
{
    public class OrderManager : IOrderService
    {
        private readonly IOrderDal _OrderDal;

        public OrderManager(IOrderDal OrderDal)
        {
            _OrderDal = OrderDal;
        }

        public void Delete(Order entity)
        {
            _OrderDal.Delete(entity);
            // You may want to handle any additional business logic or error checking here.
        }

        public List<Order> GetAll()
        {
            return _OrderDal.GetAll();
        }

        public Order GetById(int id)
        {
            return _OrderDal.GetById(id);
        }

        public void Insert(Order entity)
        {
            _OrderDal.Insert(entity);
            // You may want to handle any additional business logic or error checking here.
        }

        public void Update(Order entity)
        {
            _OrderDal.Update(entity);
            // You may want to handle any additional business logic or error checking here.
        }
    }
}
