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
    public class SettingRepository
    {
        private string connectionString;

        public SettingRepository(PortYettiOptions options)
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
        /// Get a list of settings
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Setting> GetAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                // get all records
                string sqlGetAll = "SELECT SettingID as Id, SettingNameID, SettingNameID, ServiceID, Value FROM Setting";

                dbConnection.Open();

                // return all records
                return dbConnection.Query<Setting>(sqlGetAll);
            }
        }

        /// <summary>
        /// Get a setting by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Setting GetByID(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                // select database record matching id in params
                string sqlGetById = "SELECT SettingID, SettingNameID, SettingNameID, ServiceID, Value FROM Setting "
                    + "WHERE SettingId = @Id";

                dbConnection.Open();

                var getByIdParams = new { Id = id };

                try
                {
                    // return record
                    return dbConnection.Query<Setting>(sqlGetById, getByIdParams).SingleOrDefault();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        /// <summary>
        /// Get a setting by value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Setting GetByValue(string value)
        {
            using (IDbConnection dbConnection = Connection)
            {
                // select database record by matching value in params
                string sqlGetByValue = "SELECT SettingID, SettingNameID, SettingNameID, ServiceID, Value FROM Setting "
                    + "WHERE Value = @value";

                dbConnection.Open();

                var getByNameParams = new { Value = value };

                try
                {
                    // return record
                    return dbConnection.Query<Setting>(sqlGetByValue, getByNameParams).FirstOrDefault();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        /// <summary>
        /// Get a setting by SettingNameId and ServiceId
        /// </summary>
        /// <param name="settingNameId"></param>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        public Setting GetBySettingServiceId(int settingNameId, int serviceId)
        {
            using (IDbConnection dbConnection = Connection)
            {
                // get database record that matches params
                string sqlGetByIds = "SELECT * FROM Setting " +
                   "WHERE SettingNameID = @SettingNameId AND " +
                   "ServiceID = @ServiceId";

                dbConnection.Open();

                var getByIds = new { SettingNameId = settingNameId, ServiceId = serviceId };

                try
                {
                    // return record
                    return dbConnection.Query<Setting>(sqlGetByIds, getByIds).FirstOrDefault();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        /// <summary>
        /// Add a setting
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        public int AddNew(SettingPost setting)
        {
            // first validate input
            // rules
            // no null values          
            if (setting.Value == null)
                return -1;

            // string not > 50 or empty
            if (setting.Value.Length > 50 || setting.Value == "")
                return -1;

            // serviceId and settingNameId must exist in relevant databases            
            if (!validateIds(setting.SettingNameID, setting.ServiceID))
                return -1;

            // serviceId, settingNameId and value must not be duplicated 
            if (GetBySettingServiceId(setting.SettingNameID, setting.ServiceID) != null || GetByValue(setting.Value) != null)
                return -1;

            using (IDbConnection dbConnection = Connection)
            {
                // insert new record into database with params & return inserted id
                string sqlAddNew = "INSERT INTO Setting(SettingNameID, ServiceID, Value) "
                    + "OUTPUT INSERTED.SettingId "
                    + "VALUES(@SettingNameId, @ServiceId, @Value);";

                dbConnection.Open();

                try
                {
                    // insert record and return inserted id
                    return dbConnection.Query<int>(sqlAddNew, setting).SingleOrDefault();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        /// <summary>
        /// Update an existing setting
        /// </summary>
        /// <param name="setting"></param>
        public bool Update(Setting setting)
        {
            // first validate input
            // rules
            // no null values            
            if (setting.Value == null)
                return false;

            // string not > 50 or empty
            if (setting.Value.Length > 50 || setting.Value == "")
                return false;

            // serviceId and settingNameId must exist in relevant databases
            if (!validateIds(setting.SettingNameID, setting.ServiceID))
                return false;

            // serviceId, settingNameId and value must not be duplicated 
            if (GetBySettingServiceId(setting.SettingNameID, setting.ServiceID) != null || GetByValue(setting.Value) != null)
                return false;

            using (IDbConnection dbConnection = Connection)
            {
                // update database record with params at location id
                string sqlUpdate = "Update Setting " +
                    "SET " +
                    "SettingNameID = @SettingNameId, " +
                    "ServiceID = @ServiceId, " +
                    "Value = @Value " +
                    "WHERE SettingID = @Id";

                dbConnection.Open();                

                try
                {
                    // update database
                    int rowsAffected = dbConnection.Execute(sqlUpdate, setting);
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public bool validateIds(int settingNameId, int serviceId)
        {
            using (IDbConnection dbConnection = Connection)
            {
                // return number of records containing relevant parameters from SettingName and Service databases
                string sqlSettingNameId = "SELECT COUNT(*) from SettingName WHERE SettingNameId = @Id";
                var settingNameIdParams = new { Id = settingNameId };

                string sqlServiceId = "SELECT COUNT(*) from Service WHERE ServiceId = @Id";
                var serviceIdParams = new { Id = serviceId };

                dbConnection.Open();

                try
                {
                    // return number of occurances of params
                    int settingNameIdCount = dbConnection.Query<int>(sqlSettingNameId, settingNameIdParams).FirstOrDefault();
                    int serviceIdCount = dbConnection.Query<int>(sqlServiceId, serviceIdParams).FirstOrDefault();

                    // return if params exist
                    if (settingNameIdCount == 1 && serviceIdCount == 1)
                        return true;
                    else
                        return false;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
    }
}
