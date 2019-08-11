using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Common.model;
using Dapper;

namespace Common.dao
{
    public class DownDao
    {

        public Down GetDown()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 * from T_Down where status=0");

            using (IDbConnection conn = new SqlConnection(DapperHelper.GetConStr()))
            {
                return conn.Query<Down>(strSql.ToString())?.FirstOrDefault();

            }

        }

        public bool UpdateDown(Down model)
        {
            string sql = "update T_Down set filename=@filename,status=1 where infoid=@infoid";
            using (IDbConnection conn = new SqlConnection(DapperHelper.GetConStr()))
            {
                int count = conn.Execute(sql, model);
                if (count > 0)//如果更新失败
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }

        }
    }
}

