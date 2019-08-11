using Common.model;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;

namespace Common.dao
{
    public class UserDao
    {
        /// <summary>
        /// 获取需要登陆的用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public User GetUserExpired()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 * from T_User where status=1 and retrycount<=3");

            using (IDbConnection conn = new SqlConnection(DapperHelper.GetConStr()))
            {
                return conn.Query<User>(strSql.ToString())?.FirstOrDefault();

            }

        }
        /// <summary>
        /// 获取可用的账号
        /// </summary>
        /// <returns></returns>
        public User GetUser()
        {

            string sql = "select top 1 * from T_User where status=0";

            using (IDbConnection conn = new SqlConnection(DapperHelper.GetConStr()))
            {
                return conn.Query<User>(sql)?.FirstOrDefault();

            }

        }

        public User GetById(int id)
        {

            string sql = "select * from T_User where   id=" + id;
            using (IDbConnection conn = new SqlConnection(DapperHelper.GetConStr()))
            {
                return conn.Query<User>(sql)?.FirstOrDefault();

            }

        }
        public bool AddCookie(User cookie)
        {
            string sql = "INSERT INTO T_User (username,cookie) VALUES (@username,@cookie)";
            using (IDbConnection conn = new SqlConnection(DapperHelper.GetConStr()))
            {
                int count = conn.Execute(sql, cookie);
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
        public bool UpdateCookie(User user)
        {
            string sql = "update T_User set cookie=@cookie,status=@status,retrycount=@retrycount where username=@username";
            using (IDbConnection conn = new SqlConnection(DapperHelper.GetConStr()))
            {
                int count = conn.Execute(sql, user);
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
        public bool UpdateRetryCount(int id)
        {
            string sql = "update T_User set retrycount=retrycount+1 where id="+id;
            using (IDbConnection conn = new SqlConnection(DapperHelper.GetConStr()))
            {
                int count = conn.Execute(sql);
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
