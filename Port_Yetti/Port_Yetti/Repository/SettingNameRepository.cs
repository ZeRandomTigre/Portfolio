using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

using Dapper;
using Port_Yetti.Models;

namespace Port_Yetti.Repository
{
    public class  SettingNameRepository
    {
        private string connectionString;

        public  SettingNameRepository(PortYettiOptions options)
        {
            connectionString = String.Format("Server={0};Database={1};User Id={2};Password={3};", options.server, options.database, options.userId, options.password);
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(connectionString);
            }
        }

        /// <summary>
        /// Get a list of  SettingNames
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SettingName> GetAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                // get all records
                string sqlQueryGetAll = "SELECT SettingNameId as Id, Name "
                       + "FROM SettingName";

                dbConnection.Open();

                // return all records
                return dbConnection.Query<SettingName>(sqlQueryGetAll);
            }
        }

        /// <summary>
        /// Get a  SettingName by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SettingName GetByID(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                // get record by id
                string sqlQueryGetById = "SELECT SettingNameId as Id, Name "
                    + "FROM SettingName "
                    + "WHERE SettingNameId = @Id";

                dbConnection.Open();

                var getByIdParams = new { Id = id };

                try
                {
                    // return record
                    return dbConnection.Query<SettingName>(sqlQueryGetById, getByIdParams).FirstOrDefault();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        /// <summary>
        /// Get a settingName by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public SettingName GetByName(string name)
        {
            // get record by name
            using (IDbConnection dbConnection = Connection)
            {
                string sqlQueryGetByName = "SELECT SettingNameId as Id, Name "
                   + "FROM SettingName "
                   + "WHERE Name = @Name";

                dbConnection.Open();

                var getByNameParams = new { Name = name };

                try
                {
                    // return record
                    return dbConnection.Query<SettingName>(sqlQueryGetByName, getByNameParams).FirstOrDefault();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        /// <summary>
        /// Add a settingName
        /// </summary>
        /// <param name="settingName"></param>
        /// <returns></returns>
        public int AddNew(SettingNamePost settingName)
        {
            // First Check to make sure input values are valid
            // rules:
            // no duplicate values
            // SettingName.name less than 50 nchar
            // SettingName.name != null
            if (settingName.Name == null)
                return -1;

            if (GetByName(settingName.Name) != null || settingName.Name.Length > 50 || settingName.Name == "")
                return -1;

            using (IDbConnection dbConnection = Connection)
            {
                // inserted new record and return inserted id
                string sqlQueryAddNew = "INSERT INTO  SettingName(Name) "
                    + "OUTPUT INSERTED. SettingNameId "
                    + "VALUES(@Name);";

                dbConnection.Open();

                try
                {
                    // insert record and return inputed id
                    return dbConnection.Query<int>(sqlQueryAddNew, settingName).SingleOrDefault();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        /// <summary>
        /// Update an existing settingName
        /// </summary>
        /// <param name="settingName"></param>
        public bool Update(SettingName settingName)
        {
            // First Check to make sure input values are valid
            // rules:
            // no duplicate values
            // SettingName.name less than 50 nchar
            // SettingName.name != null
            if (settingName.Name == null)
                return false;

            if (GetByName(settingName.Name) != null || settingName.Name.Length > 50 || settingName.Name == "")
                return false;

            using (IDbConnection dbConnection = Connection)
            {
                // update record with params at id location
                string sqlQueryUpdate = "Update SettingName " +
                    "SET " +
                    "Name = @Name " +
                    "WHERE  SettingNameId = @Id";

                dbConnection.Open();

                try
                {
                    // update record
                    int rowsAffected = dbConnection.Execute(sqlQueryUpdate, settingName);
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
    }
}
