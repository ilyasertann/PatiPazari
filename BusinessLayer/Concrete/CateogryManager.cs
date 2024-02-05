using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;

namespace BusinessLayer.Concrete
{
    public class CategoryManager : ICategoryService
    {
        private readonly ICategoryDal _categoryDal;

        public CategoryManager(ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
        }

        public void Delete(Category entity)
        {
            _categoryDal.Delete(entity);
            // You may want to handle any additional business logic or error checking here.
        }

        public List<Category> GetAll()
        {
            return _categoryDal.GetAll();
        }

        public Category GetById(int id)
        {
            return _categoryDal.GetById(id);
        }

        public void Insert(Category entity)
        {
            _categoryDal.Insert(entity);
            // You may want to handle any additional business logic or error checking here.
        }

        public void Update(Category entity)
        {
            _categoryDal.Update(entity);
            // You may want to handle any additional business logic or error checking here.
        }
    }
}
