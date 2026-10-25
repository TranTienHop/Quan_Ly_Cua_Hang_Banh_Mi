using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.SqlClient;
using QuanLyCuaHangBanhMi.Diagnostics;

namespace QuanLyCuaHangBanhMi.Data
{
    public static class DatabaseHelper
    {
        // Dùng chung một database và một SQL Server instance cho toàn ứng dụng.
        public const string DefaultConnectionString =
            "Server=(localdb)\\MSSQLLocalDB;Database=QuanLyBanhMi;Trusted_Connection=True;TrustServerCertificate=True;";

        private static string _connectionString = DefaultConnectionString;

        public static string ConnectionString
        {
            get => _connectionString;
            private set => _connectionString = value;
        }

        public static void ConfigureConnectionString(string? connectionString)
        {
            ConnectionString = string.IsNullOrWhiteSpace(connectionString)
                ? DefaultConnectionString
                : connectionString.Trim();
        }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public static bool TestConnection(out string error)
        {
            error = string.Empty;
            try
            {
                using var conn = GetConnection();
                conn.Open();
                return true;
            }
            catch (Exception ex)
            {
                ApplicationLogger.Error("Database connection test failed.", ex);
                error = ex.Message;
                return false;
            }
        }

        public static DataTable ExecuteQuery(string query, SqlParameter[]? parameters = null)
        {
            using var conn = GetConnection();
            using var cmd = new SqlCommand(query, conn);
            if (parameters != null && parameters.Length > 0)
            {
                cmd.Parameters.AddRange(parameters);
            }

            try
            {
                using var adapter = new SqlDataAdapter(cmd);
                var dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                ApplicationLogger.Error("Database query failed.", ex);
                throw;
            }
        }

        public static int ExecuteNonQuery(string query, SqlParameter[]? parameters = null, CommandType commandType = CommandType.Text)
        {
            using var conn = GetConnection();
            using var cmd = new SqlCommand(query, conn)
            {
                CommandType = commandType
            };
            if (parameters != null && parameters.Length > 0)
            {
                cmd.Parameters.AddRange(parameters);
            }

            try
            {
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ApplicationLogger.Error("Database non-query command failed.", ex);
                throw;
            }
        }

        public static object? ExecuteScalar(string query, SqlParameter[]? parameters = null, CommandType commandType = CommandType.Text)
        {
            using var conn = GetConnection();
            using var cmd = new SqlCommand(query, conn)
            {
                CommandType = commandType
            };
            if (parameters != null && parameters.Length > 0)
            {
                cmd.Parameters.AddRange(parameters);
            }

            try
            {
                conn.Open();
                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                ApplicationLogger.Error("Database scalar query failed.", ex);
                throw;
            }
        }

        // ===== Các overload chạy trong transaction (nhiều lệnh cùng 1 kết nối, commit/rollback chung) =====

        public static DataTable ExecuteQuery(SqlConnection conn, SqlTransaction tran, string query, SqlParameter[]? parameters = null)
        {
            using var cmd = new SqlCommand(query, conn, tran);
            if (parameters != null && parameters.Length > 0)
            {
                cmd.Parameters.AddRange(parameters);
            }

            try
            {
                using var adapter = new SqlDataAdapter(cmd);
                var dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                ApplicationLogger.Error("Transactional database query failed.", ex);
                throw;
            }
        }

        public static int ExecuteNonQuery(SqlConnection conn, SqlTransaction tran, string query, SqlParameter[]? parameters = null)
        {
            using var cmd = new SqlCommand(query, conn, tran);
            if (parameters != null && parameters.Length > 0)
            {
                cmd.Parameters.AddRange(parameters);
            }
            try
            {
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ApplicationLogger.Error("Transactional database non-query command failed.", ex);
                throw;
            }
        }

        public static object? ExecuteScalar(SqlConnection conn, SqlTransaction tran, string query, SqlParameter[]? parameters = null)
        {
            using var cmd = new SqlCommand(query, conn, tran);
            if (parameters != null && parameters.Length > 0)
            {
                cmd.Parameters.AddRange(parameters);
            }
            try
            {
                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                ApplicationLogger.Error("Transactional database scalar query failed.", ex);
                throw;
            }
        }

        /// <summary>
        /// Tạo tham số có kiểu rõ ràng; null → DBNull. Chuỗi luôn truyền NVarChar để giữ tiếng Việt.
        /// </summary>
        public static SqlParameter Param(string name, SqlDbType type, object? value, int size = 0)
        {
            var p = new SqlParameter(name, type) { Value = value ?? DBNull.Value };
            if (size != 0) p.Size = size;
            if (type == SqlDbType.Decimal)
            {
                p.Precision = 18;
                p.Scale = 3;
            }
            return p;
        }

        /// <summary>
        /// Băm mật khẩu bằng SHA-256, trả về chuỗi hex in hoa (khớp với cột TaiKhoan.MatKhauBam,
        /// tức CONVERT(NVARCHAR(255), HASHBYTES('SHA2_256', ...), 2) trong SQL Server)
        /// </summary>
        public static string HashPasswordSHA256(string password)
        {
            return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(password)));
        }
    }
}
