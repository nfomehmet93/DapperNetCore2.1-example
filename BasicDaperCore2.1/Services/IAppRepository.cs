using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RestFullServices.Services
{
    public interface IAppRepository
    {
       
        int Add<T>(T entity) where T : class;
        int Delete<T>(T entity) where T : class;
        int Update<T>(T entity) where T : class;
        T GetItem<T>(int Id);
        T GetItemFromField<T>(string column, int value);
        T GetItemFromField<T>(string column, string value);
        IEnumerable<T> GetItemFromFieldList<T>(string column, string value);
        IEnumerable<T> GetItemFromFieldList<T>(string column, int value);
        IEnumerable<T> ExecuteQueryToCollection<T>(string query, object value);
       
    }
}
