﻿using CustomerData.Context;
using CustomerData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerData.Repositories
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        IEnumerable<T> FindAll();
        T FindByPrimaryKey(int id);
        T Insert(T entity);
        T Update(T entity);
        T Delete(int id);
    }
    public class GenericRepository<T> where T : BaseEntity
    {
        public GenericRepository(CustomerDbContext context)
        {
            this.Context = context;
        }
        public CustomerDbContext Context { get; set; }
        public IEnumerable<T> FindAll()
        {
            IQueryable<T> query = this.Context.Set<T>();
            return query.Select(c => c).ToList();
        }
        public T FindByPrimaryKey(int id)
        {
            var entity = this.Context.Find<T>(id);
            if (entity is object)
            {
                this.Context.Entry<T>(entity).State = EntityState.Detached;
                return entity;
            }
            throw new Exception($"Entity with ID {id} was not found.");
        }
        public T Insert(T entity)
        {
            Context.Add<T>(entity);
            Context.SaveChanges();
            return entity;
        }
        public T Update(T entity)
        {
            this.Context.Attach<T>(entity);
            this.Context.Entry(entity).State = EntityState.Modified;
            this.Context.SaveChanges();
            return entity;
        }
        public T Delete(int id)
        {
            var entity = this.FindByPrimaryKey(id);
            this.Context.Remove<T>(entity);
            this.Context.SaveChanges();
            return entity;
        }
    }
}
