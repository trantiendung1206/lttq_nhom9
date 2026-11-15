using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using LTTQ_BTL.Dal;
using Microsoft.Data.SqlClient;

namespace LTTQ_BTL.Dal.Dao
{
    public class NguyenLieuDAO
    {
        Database db = new Database();

        //DS nguyen lieu
        public DataTable DSNguyenLieu(string ten, string congdung,string ccd)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DSNguyenLieu", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Ten", ten ?? "");
                cmd.Parameters.AddWithValue("@CongDung", congdung ?? "");
                cmd.Parameters.AddWithValue("@CCD", ccd ?? "");

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
            }
            return dt;
        }
        //lay theo ma
        public DataTable LayTheoMa(string ma)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_LayNguyenLieuTheoMa", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Ma", ma);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
            }
            return dt;
        }
        //update nguyen lieu
        public bool SuaNguyenLieu(string ma, string ten, string donvi, decimal soluong,
            decimal nhap, decimal ban, string congdung, string yeucau, string chidinh)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_SuaNguyenLieu", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Ma", ma);
                cmd.Parameters.AddWithValue("@Ten", ten);
                cmd.Parameters.AddWithValue("@DonVi", donvi);
                cmd.Parameters.AddWithValue("@SL", soluong);
                cmd.Parameters.AddWithValue("@Nhap", nhap);
                cmd.Parameters.AddWithValue("@Ban", ban);
                cmd.Parameters.AddWithValue("@CD", congdung);
                cmd.Parameters.AddWithValue("@YC", yeucau);
                cmd.Parameters.AddWithValue("@CCD", chidinh);

                conn.Open();

                return Convert.ToBoolean(cmd.ExecuteScalar());
            }
        }

        //create nguyen lieu
        public bool ThemNguyenLieu(
                                string ma,
                                string ten,
                                string donViTinh,
                                decimal donGiaNhap,
                                string congDung,
                                string yeuCau,
                                string CCD)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_ThemNguyenLieu", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Ma", ma);
                cmd.Parameters.AddWithValue("@Ten", ten);
                cmd.Parameters.AddWithValue("@DonViTinh", donViTinh);
                cmd.Parameters.AddWithValue("@DonGiaNhap", donGiaNhap);
                cmd.Parameters.AddWithValue("@CongDung", congDung);
                cmd.Parameters.AddWithValue("@YeuCau", yeuCau);
                cmd.Parameters.AddWithValue("@CCD", CCD);

                conn.Open();

                return Convert.ToBoolean(cmd.ExecuteScalar());
            }
        }


        public bool XoaNguyenLieu(string maNguyenLieu)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_XoaNguyenLieu", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MaNguyenLieu", maNguyenLieu);

                conn.Open();

                return Convert.ToBoolean(cmd.ExecuteScalar());
            }
        }
    }
}
