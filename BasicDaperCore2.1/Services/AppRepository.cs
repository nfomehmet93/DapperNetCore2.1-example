using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace RestFullServices.Services
{
    public class AppRepository : IAppRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public AppRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;

        }
        public int Add<T>(T entity) where T : class
        {
            using (var con = new SqlConnection(_connectionFactory.GetConnection.ConnectionString))
            {
                con.Open();
                using (var transaction = con.BeginTransaction())
                {
                    int count = 0;
                    try
                    {
                        var query = GetInsertQuery(entity);
                        count = con.Execute(query, entity, transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                    }
                    return count;
                }
            }
        }
        public int Delete<T>(T entity) where T : class
        {
            using (var con = new SqlConnection(_connectionFactory.GetConnection.ConnectionString))
            {
                con.Open();
                using (var transaction = con.BeginTransaction())
                {
                    int count = 0;
                    try
                    {
                        var query = GetDeleteQuery(entity);
                        count = con.Execute(query, entity, transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                    }
                    return count;
                }
            }
        }
        public int Update<T>(T entity) where T : class
        {
            using (var con = new SqlConnection(_connectionFactory.GetConnection.ConnectionString))
            {
                con.Open();
                using (var transaction = con.BeginTransaction())
                {
                    int count = 0;
                    try
                    {
                        var query = GetUpdateQuery(entity);
                        count = con.Execute(query, entity, transaction);
                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                    }
                    return count;
                }
            }
        }
        public T GetItem<T>(int Id)
        {
            using (var con = new SqlConnection(_connectionFactory.GetConnection.ConnectionString))
            {
                try
                {
                    con.Open();
                    var tablename = typeof(T).Name;
                    T res = con.Query<T>("select Top 1 * from " + tablename + " with(nolock) where Id=@prm0", new { prm0 = Id }).FirstOrDefault<T>();
                    return res;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }

            }
        }
        public T GetItemFromField<T>(string column, int value)
        {
            using (var con = new SqlConnection(_connectionFactory.GetConnection.ConnectionString))
            {
                try
                {
                    con.Open();
                    var tablename = typeof(T).Name;
                    string query = string.Format("select  * from  {0} with(nolock) where  {1}  = @prm0", tablename, column);
                    T res = con.Query<T>(query, new{ prm0=value }).FirstOrDefault();
                    return res;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }

            }
        }
        public T GetItemFromField<T>(string column, string value)
        {
            using (var con = new SqlConnection(_connectionFactory.GetConnection.ConnectionString))
            {
                try
                {
                    con.Open();
                    var tablename = typeof(T).Name;
                    string query = string.Format("select  * from  {0} with(nolock) where  {1}  = @prm0", tablename, column);
                    T res = con.Query<T>(query, new { prm0 = value }).FirstOrDefault();
                    return res;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }

            }
        }
        public IEnumerable<T> GetItemFromFieldList<T>(string column, string value)
        {
            using (var con = new SqlConnection(_connectionFactory.GetConnection.ConnectionString))
            {
                try
                {
                    con.Open();
                    var tablename = typeof(T).Name;
                    IEnumerable<T> res = con.Query<T>("select  * from " + tablename + " with(nolock) where " + column + "=@prm0", new { prm0 = value }).ToList();
                    return res;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }

            }
        }
        public IEnumerable<T> GetItemFromFieldList<T>(string column, int value)
        {
            using (var con = new SqlConnection(_connectionFactory.GetConnection.ConnectionString))
            {
                try
                {
                    con.Open();
                    var tablename = typeof(T).Name;
                    IEnumerable<T> res = con.Query<T>("select  * from " + tablename + " with(nolock) where " + column + "=@prm0", new { prm0 = value }).ToList();
                    return res;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }

            }
        }
        public IEnumerable<T> ExecuteQueryToCollection<T>(string query, object value)
        {
            using (var con = new SqlConnection(_connectionFactory.GetConnection.ConnectionString))
            {
                try
                {
                    con.Open();
                    IEnumerable<T> res = con.Query<T>(query, value);
                    return res;
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
        }
        private string GetInsertQuery<T>(T entity) where T : class
        {
            var map = (typeof(T));
            List<PropertyInfo> props = map.GetProperties().ToList();
            string[] columns = props.Select(p => p.Name).Where(s => s != "Id").ToArray();
            return string.Format("INSERT INTO {0} ({1})  VALUES (@{2}) select SCOPE_IDENTITY()",
                            map.Name,
                            string.Join(",", columns),
                            string.Join(",@", columns));

        }
        private string GetDeleteQuery<T>(T entity) where T : class
        {
            var map = (typeof(T));
            List<PropertyInfo> props = map.GetProperties().ToList();
            string[] columns = props.Select(p => p.Name).Where(s => s == "Id").ToArray();
            return string.Format("DELETE {0} where {1}=(@{2})  ",
                           map.Name,
                           string.Join(",", columns),
                           string.Join(",@", columns));


        }
        private string GetUpdateQuery<T>(T entity) where T : class
        {
            var map = (typeof(T));
            List<PropertyInfo> props = map.GetProperties().ToList();
            string[] columns = props.Select(p => p.Name).Where(s => s != "Id").ToArray();
            var parameters = columns.Select(name => name + "=@" + name).ToList();
            return string.Format("UPDATE {0} SET {1} WHERE Id=@Id", map.Name, string.Join(",", parameters));


        }
       

    }
}
