using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace PTSLibrary.DAO
{
    class SuperDAO
    {
        protected Customer GetCustomer(int custId)
        {
            string sql;
            string ConnectionString = @"Data Source = Rosh - PC; Initial Catalog = wm75; Integrated Security = True";
            SqlConnection cn;
            SqlCommand cmd;
            SqlDataReader dr;
            Customer cust;

            sql = "SELECT * FROM Customer Where CustomerId = " + custId;
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand(sql, cn);

            try
            {
                cn.Open();
                dr = cmd.ExecuteReader(CommandBehavior.SingleRow);
                dr.Read();
                cust = new Customer(dr["Name"].ToString(), (int)dr["CustomerId"]);
                dr.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error Getting Customer", ex);
            }
            finally
            {
                cn.Close();
            }

            return cust;
        }

        public List<Task> GetListOfTasks(Guid projectId)
        {
            string sql;
            SqlConnection cn;
            string ConnectionString = @"Data Source = Rosh - PC; Initial Catalog = wm75; Integrated Security = True";
            SqlCommand cmd;
            SqlDataReader dr;
            List<Task> tasks;
            tasks = new List<Task>();

            sql = "SELECT * FROM Task Where ProjectId = ' " + projectId + "'";
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand(sql, cn);

            try
            {
                cn.Open();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Task t = new Task((Guid)dr["TaskId"], dr["Name"].ToString(), (Status) ((int)dr["StatusId"]));
                    tasks.Add(t);
                }
                dr.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error Getting Task List", ex);
            }
            finally
            {
                cn.Close();
            }

            return tasks;
        }
    }
}
