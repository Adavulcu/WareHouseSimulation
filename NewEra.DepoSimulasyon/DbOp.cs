using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewEra.DepoSimulasyon
{
   public class DbOp
    {
        public readonly SqlConnection _connection;

        public DbOp()
        {

            _connection = new SqlConnection("Data Source=.;Initial Catalog=NewEra_DizaynII;Integrated Security=True");

        }

        /// <summary>
        /// Parametre olarak aldıgı string degerini ve List<SqlParameter> degerini kullanarak
        /// bir SqlCommand nesnesi oluşturup geriye döndürür.
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="parametre"></param>
        /// <returns></returns>
        public SqlCommand DBCmd(string cmdText, List<SqlParameter> parametre = null)
        {
            SqlCommand result = new SqlCommand(cmdText, _connection);
            result.Parameters.Clear();
            if (parametre != null)
                foreach (var item in parametre)
                {
                    result.Parameters.AddWithValue(item.ParameterName, item.Value);
                }
            return result;
        }

        /// <summary>
        /// Parametre olarak aldıgı string degerini ve List<SqlParameter> degerini kullanarak
        /// bir DataTable nesnesi oluşturarak geriye döndürür.
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="parametre"></param>
        /// <returns></returns>
        public DataTable DBDataTable(string cmdText, List<SqlParameter> parametre = null)
        {
            try
            {
                DataTable result = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(DBCmd(cmdText, parametre));
                _connection.Open();
                da.Fill(result);

                return result;
            }
            catch (Exception ex)
            {

                throw new Exception("\nDBDataTable\n" + ex.Message);
            }
            finally
            {
                _connection.Close();
            }
        }
    }
}
