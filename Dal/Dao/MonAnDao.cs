using LTTQ_BTL.Dal;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LTTQ_BTL.Dal.Dao
{
    public class MonAnDao
    {
        Database db = new Database();

        // danh sach mon an
        public DataTable DSMonAn(string ten, string loai, string congdung)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DSMonAn", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Ten", ten);
                cmd.Parameters.AddWithValue("@Loai", loai);
                cmd.Parameters.AddWithValue("@CongDung", congdung);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
            }
            return dt;
        }

        //lai theo ma
        public DataTable LayTheoMa(string ma)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_LayMonAnTheoMa", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Ma", ma);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
            }
            return dt;
        }

        //create mon an (stored procedure)
        public bool ThemMonAn(
                        string maMon,
                        string tenMon,
                        string maCongDung,
                        string maLoai,
                        string cachLam,
                        string yeuCau,
                        decimal donGia,
                        string hinhAnh)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_ThemMonAn", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MaMon", maMon);
                cmd.Parameters.AddWithValue("@TenMon", tenMon);
                cmd.Parameters.AddWithValue("@MaCongDung", maCongDung);
                cmd.Parameters.AddWithValue("@MaLoai", maLoai);
                cmd.Parameters.AddWithValue("@CachLam", cachLam);
                cmd.Parameters.AddWithValue("@YeuCau", yeuCau);
                cmd.Parameters.AddWithValue("@DonGia", donGia);

                cmd.Parameters.AddWithValue("@HinhAnh",string.IsNullOrWhiteSpace(hinhAnh) ? (object)DBNull.Value : hinhAnh);

                conn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool ThemNguyenLieuMonAn(
                                    string maMon,
                                    string maNguyenLieu,
                                    decimal soLuong)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_ThemNguyenLieuMonAn", conn);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MaMonAn", maMon);
                cmd.Parameters.AddWithValue("@MaNguyenLieu", maNguyenLieu);
                cmd.Parameters.AddWithValue("@SoLuong", soLuong);

                conn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        //lấy nguyên liệu cho vào danh sách trong lúc tạo món ăn
        public DataTable DSNguyenLieu_MonAn(string ma)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DSNguyenLieu_MonAn", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Ma", ma);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
            }
            return dt;
        }

        //update mon an
        public bool SuaMonAn(
                        string maMon,
                        string tenMon,
                        string maCongDung,
                        string maLoai,
                        string cachLam,
                        string yeuCau,
                        decimal donGia,
                        string hinhAnh,
                        DataTable dtNguyenLieu)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        // 1. Cập nhật thông tin món ăn
                        string sql = @"UPDATE MonAn SET TenMonAn = @TenMon, MaCongDung = @MaCongDung,
                        MaLoai = @MaLoai, CachLam = @CachLam, YeuCau = @YeuCau, DonGia = @DonGia, HinhAnh = @HinhAnh WHERE MaMonAn = @MaMon";

                        using (SqlCommand cmd = new SqlCommand(sql, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@MaMon", maMon);
                            cmd.Parameters.AddWithValue("@TenMon", tenMon);
                            cmd.Parameters.AddWithValue("@MaCongDung", maCongDung);
                            cmd.Parameters.AddWithValue("@MaLoai", maLoai);
                            cmd.Parameters.AddWithValue("@CachLam", cachLam);
                            cmd.Parameters.AddWithValue("@YeuCau", yeuCau);
                            cmd.Parameters.AddWithValue("@DonGia", donGia);

                            cmd.Parameters.AddWithValue("@HinhAnh",
                                string.IsNullOrWhiteSpace(hinhAnh)
                                ? (object)DBNull.Value : hinhAnh);

                            if (cmd.ExecuteNonQuery() == 0)
                                throw new Exception("Không tìm thấy món ăn cần sửa!");
                        }

                        // 2. Xóa danh sách nguyên liệu cũ
                        string sqlXoa = @"DELETE FROM NguyenLieu_MonAn WHERE MaMonAn = @MaMon";

                        using (SqlCommand cmd = new SqlCommand(sqlXoa, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@MaMon", maMon);
                            cmd.ExecuteNonQuery();
                        }

                        // 3. Thêm lại danh sách nguyên liệu mới
                        foreach (DataRow row in dtNguyenLieu.Rows)
                        {
                            string sqlThem = @"INSERT INTO NguyenLieu_MonAn (MaMonAn, MaNguyenLieu, SoLuong)
                        VALUES (@MaMon, @MaNguyenLieu, @SoLuong)";

                            using (SqlCommand cmd = new SqlCommand(sqlThem, conn, tran))
                            {
                                cmd.Parameters.AddWithValue("@MaMon", maMon);
                                cmd.Parameters.AddWithValue("@MaNguyenLieu", row["MaNguyenLieu"]);
                                cmd.Parameters.AddWithValue("@SoLuong", row["SoLuong"]);

                                cmd.ExecuteNonQuery();
                            }
                        }

                        tran.Commit();
                        return true;
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
        }
        //Xóa món ăn
        public bool XoaMonAn(string maMon)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_XoaMonAn", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MaMon", maMon);

                conn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}
