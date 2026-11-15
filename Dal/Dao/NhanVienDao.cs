using LTTQ_BTL.Dal;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LTTQ_BTL.Dal.Dao
{
    public class NhanVienDao
    {
        Database db = new Database();

        public DataTable DSQue(){
            DataTable dt = new DataTable();
            using (SqlConnection conn = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DSQue", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        public DataTable DSNhaCungCap()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DSNhaCungCap", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }
        public DataTable DSNhanVien()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DSNhanVien", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }
        //them 
        public bool ThemNhanVien(
            string ma,
            string ten,
            string gt,
            DateTime ns,
            string dc,
            string maQue,
            string dt)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_ThemNhanVien", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Ma", ma);
                cmd.Parameters.AddWithValue("@Ten", ten);
                cmd.Parameters.AddWithValue("@Gt", gt);
                cmd.Parameters.AddWithValue("@Ns", ns);
                cmd.Parameters.AddWithValue("@Dc", dc);
                cmd.Parameters.AddWithValue("@MaQue", maQue);
                cmd.Parameters.AddWithValue("@Dt", dt);

                conn.Open();

                return Convert.ToBoolean(cmd.ExecuteScalar());
            }
        }

        //xoa

        public bool XoaNhanVien(string ma)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_XoaNhanVien", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Ma", ma);

                conn.Open();

                return Convert.ToBoolean(cmd.ExecuteScalar());
            }
        }

        //sua
        public bool SuaNhanVien(
                        string ma,
                        string ten,
                        string gioiTinh,
                        DateTime ngaySinh,
                        string diaChi,
                        string maQue,
                        string dienThoai)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("sp_SuaNhanVien", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MaNhanVien", ma);
                cmd.Parameters.AddWithValue("@TenNhanVien", ten);
                cmd.Parameters.AddWithValue("@GioiTinh", gioiTinh);
                cmd.Parameters.AddWithValue("@NgaySinh", ngaySinh);
                cmd.Parameters.AddWithValue("@DiaChi", diaChi);
                cmd.Parameters.AddWithValue("@MaQue", maQue);
                cmd.Parameters.AddWithValue("@DienThoai", dienThoai);

                return Convert.ToBoolean(cmd.ExecuteScalar());
            }
        }
    }
}
