using LTTQ_BTL.Dal;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LTTQ_BTL.Dal.Dao
{
    public class KhachHangDao
    {
        Database db = new Database();
        public DataTable DSKhachHang()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DSKhachHang", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }


        public bool ThemKhach(
            string maKhach,
            string tenKhach,
            string diaChi,
            string dienThoai)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_ThemKhach", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MaKhach", maKhach);
                cmd.Parameters.AddWithValue("@TenKhach", tenKhach);
                cmd.Parameters.AddWithValue("@DiaChi", diaChi);
                cmd.Parameters.AddWithValue("@DienThoai", dienThoai ?? "");

                return Convert.ToBoolean(cmd.ExecuteScalar());
            }
        }


        public bool XoaKhachHang(string maKhach)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_XoaKhachHang", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MaKhach", maKhach);

                return Convert.ToBoolean(cmd.ExecuteScalar());
            }
        }

        //sua 
        public bool SuaKhach(
                        string maKhach,
                        string tenKhach,
                        string diaChi,
                        string dienThoai)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("sp_SuaKhach", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MaKhach", maKhach);
                cmd.Parameters.AddWithValue("@TenKhach", tenKhach);
                cmd.Parameters.AddWithValue("@DiaChi", diaChi);
                cmd.Parameters.AddWithValue("@DienThoai", dienThoai ?? "");

                return Convert.ToBoolean(cmd.ExecuteScalar());
            }
        }
    }
}
