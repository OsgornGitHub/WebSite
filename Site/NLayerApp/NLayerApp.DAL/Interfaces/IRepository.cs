using System;
using System.Collections.Generic;
using System.Text;

namespace NLayerApp.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        IEnumerable<T> Find(Guid id);
        void Create(T item);
        void Delete(int id);
    }
}
