using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace PTSLibrary.DAO
{
    class CustomerDAO
    {
        public int Authenticate(string username, string password)
        {
            string sql;
            string ConnectionString= @"Data Source = Rosh - PC; Initial Catalog = wm75; Integrated Security = True";
            SqlConnection cn;
            SqlCommand cmd;
            SqlDataReader dr;

            sql = String.Format("SELECT CustomerId FROM Customer WHERE Username = '{0}' AND Password = '{1}' ", username, password);

            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand(sql, cn);
            int id = 0;

            try
            {
                cn.Open();
                dr = cmd.ExecuteReader(CommandBehavior.SingleRow);
                if (dr.Read())
                {
                    id = (int)dr["CustomerId"];
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

        /// <summary>   Gets list of projects. </summary>
        ///
        /// <remarks>   This method queries the database for projects that belong to a customer. </remarks>
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <param name="customerId">   Identifier for the customer. </param>
        ///
        /// <returns>   The list of projects. </returns>

        public List<Project> GetListOfProjects(int customerId)
        {

            string sql;
            SqlConnection cn;
            string ConnectionString = @"Data Source = Rosh - PC; Initial Catalog = wm75; Integrated Security = True";
            SqlCommand cmd;
            SqlDataReader dr;
            List<Project> projects;
            projects = new List<Project>();

            sql = "SELECT * FROM Project WHERE CustomerId = " + customerId.ToString();
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand(sql, cn);

            try
            {
                cn.Open();
                dr = cmd.ExecuteReader();
                SqlConnection cn2; SqlCommand cmd2; SqlDataReader dr2;// string sql2;    //custom
                while (dr.Read())
                {
                    List<Task> tasks = new List<Task>();
                    sql = "SELECT * FROM Task WHERE ProjectId = '" + dr["ProjectId"].ToString() + "'";
                    //sql2 = "SELECT * FROM Task WHERE ProjectId = '" + dr["ProjectId"].ToString() + "'";
                    cn2 = new SqlConnection(ConnectionString);
                    cmd2 = new SqlCommand(sql, cn2);
                    cn2.Open();
                    dr2 = cmd2.ExecuteReader();
                    while (dr2.Read())
                    {
                        Task t = new Task((Guid)dr2["TaskId"], dr2["Name"].ToString(),
                                                (Status)dr2["StatusId"]);
                        tasks.Add(t);
                    }
                    dr2.Close();
                    Project p = new Project(dr["Name"].ToString(), (DateTime)dr["ExpectedStartDate"],
                                        (DateTime)dr["ExpectedEndDate"], (Guid)dr["ProjectId"], tasks);
                    projects.Add(p);
                    cn2.Close();
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
    }
}


/* public List<Task> GetListOfProjects(Guid projectId)
{
    string sql;
    SqlConnection cn2;
    string ConnectionString = @"Data Source = Rosh - PC; Initial Catalog = wm75; Integrated Security = True";
    SqlCommand cmd2;
    SqlDataReader dr2;
    List<Task> tasks;
    tasks = new List<Task>();

    sql = "SELECT * FROM Task Where ProjectId = ' " + projectId + "'";
    cn2 = new SqlConnection(ConnectionString);
    cmd2 = new SqlCommand(sql, cn2);

    try
    {
        cn2.Open();
        dr2 = cmd2.ExecuteReader();
        while (dr2.Read())
        {
            List<Task> tasks = new List<Task>();
            sql = "SELECT * FROM Task WHERE ProjectId = ' " + dr2["ProjectId"].ToString() + "'";
            cn2 = new SqlConnection(ConnectionString);
            cmd2 = new SqlCommand(sql, cn2);
            cn2.Open();
            dr2 = cmd2.ExecuteReader();
            while
            {
                Task t = new Task((Guid)dr2["TaskId"], dr2["Name"].ToString(), (Status)dr2["StatusId"]);
                tasks.Add(t);
            }
            dr2.Close();
            Project p = new Project(dr2["Name"].ToString(), (DateTime)dr2["ExpectedStartDate"], (DateTime)dr2["ExpectedEndDate"], (Guid)dr2["ProjectId"], tasks);
            projects.Add(p);
        }
        dr2.Close();
    }
    catch (SqlException ex)
    {
        throw new Exception("Error Getting List", ex);
    }
    finally
    {
        cn2.Close();
    }
}
}
} */
