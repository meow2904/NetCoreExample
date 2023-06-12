using NetCoreEx.Model;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace NetCoreEx.Library.Helper
{
    public interface ISqlHelper
    {
        void SetConnection(IConfiguration configuration);
        bool BeginTransaction();
        bool ComitTransaction();
        bool RollBackTransaction();

    }

    public class SqlHelper : ISqlHelper
    {
        public IDbConnection _dbConnection;
        public SqlHelper(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        #region "# Transaction #"
        private SqlTransaction _objTransaction = null;
        private SqlConnection _objSqlConnWithTrans = null;

        public void SetConnection(IConfiguration connection)
        {
            _dbConnection.ConnectionString = connection.GetConnectionString("DefaultConnection");
        }

        public bool BeginTransaction()
        {
            _objSqlConnWithTrans = new SqlConnection(_dbConnection.ConnectionString);
            try
            {
                _objSqlConnWithTrans.Open();
                _objTransaction = _objSqlConnWithTrans.BeginTransaction();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool ComitTransaction()
        {
            try
            {
                if (_objSqlConnWithTrans == null)
                    return false;
                _objTransaction.Commit();
                if (_objSqlConnWithTrans.State != ConnectionState.Closed)
                {
                    _objSqlConnWithTrans.Close();
                    _objTransaction = null;
                    _objSqlConnWithTrans = null;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RollBackTransaction()
        {
            try
            {
                if (_objSqlConnWithTrans == null)
                    return false;
                _objTransaction.Rollback();
                if (_objSqlConnWithTrans.State != ConnectionState.Closed)
                {
                    _objSqlConnWithTrans.Close();
                    _objTransaction = null;
                    _objSqlConnWithTrans = null;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
    }
}
