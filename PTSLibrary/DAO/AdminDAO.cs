using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace PTSLibrary.DAO
{
    /// <summary>   An admin database access object. </summary>
    ///
    /// <remarks>   This is a subclass of <see cref="SuperDAO"/> that enables administrators to access the restricted database records. </remarks>

    class AdminDAO : SuperDAO
    {
        /// <summary>   Authenticates an administrator. </summary>
        ///
        /// <remarks>   The method checks the IsAdministrator flag in Users table. </remarks>
        ///
        /// <exception cref="Exception">    Thrown when an sql exception error condition occurs. </exception>
        ///
        /// <param name="username"> The username. </param>
        /// <param name="password"> The password. </param>
        ///
        /// <returns>   The administrator user id, or '0' if authentication fails. </returns>

        public int Authenticate(string username, string password)
        {
            string sql;
            SqlConnection cn;
            string ConnectionString = @"Data Source = Rosh - PC; Initial Catalog = wm75; Integrated Security = True";
            SqlCommand cmd;
            SqlDataReader dr;

            sql = String.Format("SELECT UserId FROM Person WHERE IsAdministrator = 1 AND Username='{0}' " +
                                    "AND Password='{1}'", username, password);
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand(sql, cn);
            int id = 0;
            try
            {
                cn.Open();
                dr = cmd.ExecuteReader(CommandBehavior.SingleRow);
                if (dr.Read())
                {
                    id = (int)dr["UserId"];
                }
                dr.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error Accessing Database", ex);
            }
            finally
            {
                cn.Close();
            }
            return id;
        }

        /// <summary>   Creates a new project and stores it in the database. </summary>
        ///
        /// <remarks>   This method registers a new project. </remarks>
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <param name="name">             The name. </param>
        /// <param name="startDate">        The expected start date. </param>
        /// <param name="endDate">          The expected end date. </param>
        /// <param name="customerId">       Identifier for the customer. </param>
        /// <param name="administratorId">  Identifier for the project administrator. </param>

        public void CreateProject(string name, DateTime startDate, DateTime endDate, int customerId, int administratorId)
        {
            string sql;
            SqlConnection cn;
            string ConnectionString = @"Data Source = Rosh - PC; Initial Catalog = wm75; Integrated Security = True";
            SqlCommand cmd;
            Guid projectId = Guid.NewGuid();

            sql = "INSERT INTO Project (ProjectId, Name, ExpectedStartDate, ExpectedEndDate, CustomerId, AdministratorId)";
            sql += String.Format("VALUES ( '{0}', '{1}', '{2}', '{3}', {4}, {5})",
                                    projectId, name, startDate, endDate, customerId, administratorId);
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand(sql, cn);

            try
            {
                cn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error Inserting", ex);
            }
            finally
            {
                cn.Close();
            }
        }

        /// <summary>   Creates a new task. </summary>
        ///
        /// <remarks>   Registers a new task or subtask for a project. </remarks>
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <param name="name">         The name of the task. </param>
        /// <param name="startDate">    The expected start date. </param>
        /// <param name="endDate">      The expected end date. </param>
        /// <param name="teamId">       Identifier for the team assigned the task. </param>
        /// <param name="projectId">    Identifier for the project. </param>

        public void CreateTask(string name, DateTime startDate, DateTime endDate, int teamId, Guid projectId)
        {
            string sql;
            SqlConnection cn;
            string ConnectionString = @"Data Source = Rosh - PC; Initial Catalog = wm75; Integrated Security = True";
            SqlCommand cmd;
            Guid taskId = Guid.NewGuid();
            sql = "INSERT INTO Task (TaskId, Name, ExpectedDateStarted, ExpectedDateCompleted, ProjectId, TeamId, StatusId)";
            sql += String.Format("VALUES ( '{0}', '{1}', '{2}', '{3}', '{4}', {5}, {6})", taskId, name,
                                    startDate, endDate, projectId, teamId, 1);
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand(sql, cn);
            try
            {
                cn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error Inserting", ex);
            }
            finally
            {
                cn.Close();
            }
        }

        /// <summary>   Gets list of projects. </summary>
        ///
        /// <remarks>   This method queries the database for all projects that match  a given administrator. </remarks>
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <param name="adminId">  User Id for the admin. </param>
        ///
        /// <returns>   The list of projects for an administrator. </returns>

        public List<Project> GetListOfProjects(int adminId)
        {
            string sql;
            SqlConnection cn;
            string ConnectionString = @"Data Source = Rosh - PC; Initial Catalog = wm75; Integrated Security = True";
            SqlCommand cmd;
            SqlDataReader dr;
            List<Project> projects;
            projects = new List<Project>();
            sql = "SELECT * FROM Project WHERE AdministratorId = " + adminId;
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand(sql, cn);
            try
            {
                cn.Open();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Customer cust = GetCustomer((int)dr["CustomerId"]);
                    Project p = new Project(dr["Name"].ToString(), (DateTime)dr["ExpectedStartDate"],
                   (DateTime)dr["ExpectedEndDate"], (Guid)dr["ProjectId"], cust);
                    projects.Add(p);
                }
                dr.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error Getting list", ex);
            }
            finally
            {
                cn.Close();
            }
            return projects;
        }

        /// <summary>   Gets list of teams. </summary>
        ///
        /// <remarks>   Gets all teams without filtering by any criterion. </remarks>
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <returns>   The list of all teams. </returns>

        public List<Team> GetListOfTeams()
        {
            string sql;
            SqlConnection cn;
            string ConnectionString = @"Data Source = Rosh - PC; Initial Catalog = wm75; Integrated Security = True";
            SqlCommand cmd;
            SqlDataReader dr;
            List<Team> teams;
            teams = new List<Team>();
            sql = "SELECT * FROM Team";
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand(sql, cn);
            try
            {
                cn.Open();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Team t = new Team((int)dr["TeamId"], dr["Location"].ToString(),
                   dr["Name"].ToString(), null);
                    teams.Add(t);
                }
                dr.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error getting team list", ex);
            }
            finally
            {
                cn.Close();
            }
            return teams;
        }

        /// <summary>   Gets list of customers. </summary>
        ///
        /// <remarks>   Gets the list of customers who have commissioned projects. </remarks>
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <returns>   The list of all customers. </returns>

        public List<Customer> GetListOfCustomers()
        {
            string sql;
            SqlConnection cn;
            string ConnectionString = @"Data Source = Rosh - PC; Initial Catalog = wm75; Integrated Security = True";
            SqlCommand cmd;
            SqlDataReader dr;
            List<Customer> customers;
            customers = new List<Customer>();
            sql = "SELECT * FROM Customer";
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand(sql, cn);
            try
            {
                cn.Open();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Customer c = new Customer(dr["Name"].ToString(), (int)dr["CustomerId"]);
                    customers.Add(c);
                }
                dr.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error Getting list", ex);
            }
            finally
            {
                cn.Close();
            }
            return customers;
        }

        public static explicit operator AdminDAO(SuperDAO v)
        {
            throw new NotImplementedException();
        } 
    }
}
