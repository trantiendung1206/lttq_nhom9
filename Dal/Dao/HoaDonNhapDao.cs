using LTTQ_BTL.Dal;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LTTQ_BTL.Dal.Dao
{
    public class HoaDonNhapDao
    {
        Database db = new Database();

        public DataTable DSHoaDonNhap()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DSHoaDonNhap", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
            }
            return dt;
        }
        //chi tiet hoa don nhap theo ma
        public DataTable ChiTietHoaDonNhap(string ma)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_ChiTietHoaDonNhap", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Ma", ma);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
            }
            return dt;
        }

        public bool ThemPhieuNhap(
            string maHoaDon,
            DateTime ngayNhap,
            string maNhanVien,
            string maNhaCungCap,
            decimal tongTien,
            string maNguyenLieu,
            decimal soLuong,
            decimal donGia,
            decimal khuyenMai)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_ThemPhieuNhap", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MaHD", maHoaDon);
                cmd.Parameters.AddWithValue("@Date", ngayNhap);
                cmd.Parameters.AddWithValue("@MaNhanVien", maNhanVien);
                cmd.Parameters.AddWithValue("@MaNCC", maNhaCungCap);
                cmd.Parameters.AddWithValue("@TongTien", tongTien);
                cmd.Parameters.AddWithValue("@MaNguyenLieu", maNguyenLieu);
                cmd.Parameters.AddWithValue("@SoLuong", soLuong);
                cmd.Parameters.AddWithValue("@DonGia", donGia);
                cmd.Parameters.AddWithValue("@KhuyenMai", khuyenMai);

                conn.Open();

                return Convert.ToBoolean(cmd.ExecuteScalar());
            }
        }


        public DataTable LayTheoMa(string ma)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_LayHoaDonNhapTheoMa", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Ma", ma);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
            }
            return dt;
        }
    }
}
