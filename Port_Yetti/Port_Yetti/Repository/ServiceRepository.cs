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
    public class ServiceRepository
    {
        private string connectionString;

        public ServiceRepository(PortYettiOptions options)
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
        /// Get a list of services
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Service> GetAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                // select all records
                string sqlQueryGetAll = "SELECT ServiceId as Id, Name "
                       + "FROM Service";

                dbConnection.Open();

                // return all records
                return dbConnection.Query<Service>(sqlQueryGetAll);
            }
        }

        /// <summary>
        /// Get a service by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Service GetByID(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                // get record by id
                string sqlQueryGetById = "SELECT ServiceId as Id, Name "
                    + "FROM Service "
                    + "WHERE ServiceId = @Id";

                dbConnection.Open();

                var getByIdParams = new { Id = id };

                try
                {
                    // return record
                    return dbConnection.Query<Service>(sqlQueryGetById, getByIdParams).SingleOrDefault();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        /// <summary>
        /// Get a service by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Service GetByName(string name)
        {
            using (IDbConnection dbConnection = Connection)
            {
                // get record by name
                string sqlQueryGetByName = "SELECT ServiceId as Id, Name "
                   + "FROM Service "
                   + "WHERE Name = @Name";

                dbConnection.Open();

                var getByNameParams = new { Name = name };

                try
                {
                    // return record
                    return dbConnection.Query<Service>(sqlQueryGetByName, getByNameParams).FirstOrDefault();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        /// <summary>
        /// Add a service
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public int AddNew(ServicePost service)
        {
            // First Check to make sure input values are valid
            // rules:            
            // service.name != null
            if (service.Name == null)
                return -1;

            // name not > 50 or empty
            if (service.Name.Length > 50 || service.Name == "")
                return -1;

            // no duplicate values
            if (GetByName(service.Name) != null)
                return -1;

            using (IDbConnection dbConnection = Connection)
            {
                // insert new record into database with params & return inserted id
                string sqlQueryAddNew = "INSERT INTO Service(Name) "
                    + "OUTPUT INSERTED.ServiceId "
                    + "VALUES(@Name);";

                dbConnection.Open();

                try
                {                    
                    // insert record and return inserted id
                    return dbConnection.Query<int>(sqlQueryAddNew, service).SingleOrDefault();                    
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        /// <summary>
        /// Update an existing service
        /// </summary>
        /// <param name="service"></param>
        public bool Update(Service service)
        {
            // First Check to make sure input values are valid
            // rules:            
            // service.name != null
            if (service.Name == null)
                return false;

            // name not > 50 or empty
            if (service.Name.Length > 50 || service.Name == "")
                return false;

            // no duplicate values
            if (GetByName(service.Name) != null)
                return false;

            using (IDbConnection dbConnection = Connection)
            {
                // update database record with params at id location
                string sqlQueryUpdate = "Update Service " +
                    "SET " +
                    "Name = @Name " +
                    "WHERE ServiceId = @Id";

                dbConnection.Open();                

                try
                {
                    // update database record
                    int rowsAffected = dbConnection.Execute(sqlQueryUpdate, service);
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
