using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace QuanLyCuaHangBanhMi.Data
{
    /// <summary>Lớp mỏng bọc DatabaseHelper, dùng cho module Kho.</summary>
    public static class Db_QLKho
    {
        public static SqlParameter P(string name, object? value)
            => new(name, value ?? DBNull.Value);

        public static SqlConnection Open()
        {
            var c = DatabaseHelper.GetConnection();
            c.Open();
            return c;
        }

        // ---------- Không transaction ----------
        public static DataTable Query(string sql, params SqlParameter[] ps)
            => DatabaseHelper.ExecuteQuery(sql, ps);

        public static int Exec(string sql, params SqlParameter[] ps)
            => DatabaseHelper.ExecuteNonQuery(sql, ps);

        public static object? Scalar(string sql, params SqlParameter[] ps)
        {
            var r = DatabaseHelper.ExecuteScalar(sql, ps);
            return r == DBNull.Value ? null : r;
        }

        // ---------- Dùng chung connection + transaction ----------
        public static DataTable Query(SqlConnection c, SqlTransaction? t, string sql, params SqlParameter[] ps)
            => DatabaseHelper.ExecuteQuery(c, t!, sql, ps);

        public static int Exec(SqlConnection c, SqlTransaction? t, string sql, params SqlParameter[] ps)
            => DatabaseHelper.ExecuteNonQuery(c, t!, sql, ps);

        public static object? Scalar(SqlConnection c, SqlTransaction? t, string sql, params SqlParameter[] ps)
        {
            var r = DatabaseHelper.ExecuteScalar(c, t!, sql, ps);
            return r == DBNull.Value ? null : r;
        }
    }
}