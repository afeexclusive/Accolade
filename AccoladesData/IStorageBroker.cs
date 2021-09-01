using System;
using System.Collections.Generic;
using System.Text;

namespace AccoladesData
{
    public interface IStorageBroker<T> where T : class
    {
        string UserId { get; set; }

        IEnumerable<T> GetAll();
        void Update(T entity, T entityUpdate);
        T Post(T entity);
        void Delete(T entity);
        void PostRange(List<T> entities);
    }
}
