using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace CulinaryBlogCore.Services.Repository.Contracts
{
    public interface IRepository
    {
        IQueryable<T> Set<T>() where T : class;

        void Execute(string sql, params object[] parameters);

        void Add<T>(T obj) where T : class;

        void Update<T>(T obj) where T : class;

        void Delete<T>(T obj) where T : class;

        void RemoveRange<T>(IEnumerable<T> objRange) where T : class;

        IQueryable<T> Include<T>(Expression<Func<T, object>> include) where T : class;

        T GetById<T>(object id) where T : class;

        IQueryable<T> SetNoTracking<T>(params string[] includes) where T : class;

        void SaveChanges();
    }
}
