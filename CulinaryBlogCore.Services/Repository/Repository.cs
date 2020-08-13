using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using CulinaryBlogCore.DataAccess;
using CulinaryBlogCore.Services.Repository.Contracts;

using Microsoft.EntityFrameworkCore;

namespace CulinaryBlogCore.Services.Repository
{
    public class Repository : IRepository
    {
        private CulinaryBlogDbContext _context;

        public Repository(CulinaryBlogDbContext dbContext)
        {
            _context = dbContext ?? throw new ArgumentException("An instance of DbContext is required to use this repository.", "context");
        }

        public IQueryable<T> Set<T>() where T : class
        {
            return _context.Set<T>().AsQueryable<T>();
        }

        public void Execute(string sql, params object[] parameters)
        {
            _context.Database.ExecuteSqlCommand(sql, parameters);
        }

        public void Add<T>(T obj) where T : class
        {
            _context.Set<T>().Add(obj);
            _context.SaveChanges();
        }

        public void Update<T>(T obj) where T : class
        {
            _context.Entry<T>(obj).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete<T>(T obj) where T : class
        {
            _context.Entry<T>(obj).State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public void RemoveRange<T>(IEnumerable<T> objRange) where T : class
        {
            _context.Set<T>().RemoveRange(objRange);
            _context.SaveChanges();
        }

        public virtual IQueryable<T> Include<T>(Expression<Func<T, object>> include) where T : class
        {
            return _context.Set<T>().Include(include).AsQueryable();
        }

        public virtual T GetById<T>(object id) where T : class
        {
            return _context.Set<T>().Find(id);
        }

        public IQueryable<T> Query<T>(string sql, params object[] parameters) where T : class
        {
            //TODO Check if tehre is support for EFCore query in EF
            return null;
            // _context.Query<T>()
            //               .FromSql(sql, parameters)
            //               .AsQueryable<T>();
        }

        public IQueryable<T> SetNoTracking<T>(params string[] includes) where T : class
        {
            var query = _context.Set<T>().AsNoTracking();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return query;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        protected void Dispose()
        {
            _context.Dispose();
        }
    }
}
