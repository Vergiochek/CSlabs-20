using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using DatabaseModels.Models;

namespace DataAccessLayer
{
    public class Repository:IFiller<HumanData>
    {
        private DALayercfg props;

        public Repository(string connectionStringg,int num)
        {
            props=new DALayercfg(connectionStringg,num);
        }
        public Search<HumanData> GetPersons() 
        {
            using (var connection = new SqlConnection(props.connectionString))
            {
                var findres = new SearchRes<PersonalInfo>();
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                SqlCommand command = new SqlCommand();
                command.Transaction = transaction;
                command.Connection = connection;
                command.Parameters.AddWithValue("@Start", (int) props.criteria.start);
                command.Parameters.AddWithValue("@Count", (int) props.criteria.count);
                  
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = props.StoredFunction;
                command.CommandTimeout = 45;

                var outputparam = new SqlParameter("@Total", SqlDbType.Int);
                outputparam.Direction = ParameterDirection.Output;
                command.Parameters.Add(outputparam);
                var entities = command.ReadAll<HumanData>();
                    
                transaction.Commit();

                findres.Entities = entities;
                findres.Total = Convert.ToInt32(outputparam.Value);

                finally
                {
                    connection.Close();
                }

                return findres;
            }
        }
    }
}
