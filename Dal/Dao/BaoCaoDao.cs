using LTTQ_BTL.Dal;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LTTQ_BTL.Dal.Dao
{
    public class BaoCaoDao
    {
        Database db = new Database();

        public int LayTongSoNhanVien()
        {
            string query = "SELECT COUNT(*) FROM NhanVien";
            DataTable dt = db.ExecuteQuery(query);
            return dt.Rows.Count > 0 ? Convert.ToInt32(dt.Rows[0][0]) : 0;
        }

        public int LayTongSoKhachHang()
        {
            string query = "SELECT COUNT(*) FROM Khach";
            DataTable dt = db.ExecuteQuery(query);
            return dt.Rows.Count > 0 ? Convert.ToInt32(dt.Rows[0][0]) : 0;
        }

        public decimal LayChiTieuThangHienTai()
        {
            string query = "SELECT ISNULL(SUM(TongTien), 0) FROM HoaDonNhap WHERE MONTH(NgayNhap) = MONTH(GETDATE()) AND YEAR(NgayNhap) = YEAR(GETDATE())";
            DataTable dt = db.ExecuteQuery(query);
            return dt.Rows.Count > 0 ? Convert.ToDecimal(dt.Rows[0][0]) : 0;
        }

        public decimal LayDoanhThuThangHienTai()
        {
            string query = "SELECT ISNULL(SUM(TongTien), 0) FROM PhieuDatBan WHERE MONTH(NgayDung) = MONTH(GETDATE()) AND NgayDung < GETDATE() AND YEAR(NgayDung) = YEAR(GETDATE())";
            DataTable dt = db.ExecuteQuery(query);
            return dt.Rows.Count > 0 ? Convert.ToDecimal(dt.Rows[0][0]) : 0;
        }


        public DataTable LayDoanhThu6ThangGanNhat()
        {
            string query = "SELECT FORMAT(T.Thang, 'MM/yyyy') AS Thang, ISNULL(D.TongThu, 0) AS TongTienDatBan, ISNULL(C.TongChi, 0) AS TongTienNhap, ISNULL(D.TongThu, 0) - ISNULL(C.TongChi, 0) AS DoanhThu " +
                "FROM (SELECT DATEADD(MONTH, V.n, DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1)) AS Thang FROM (VALUES (-5), (-4), (-3), (-2), (-1), (0)) V(n)) T " +
                "OUTER APPLY " +
                "(SELECT SUM(TongTien) AS TongThu  FROM PhieuDatBan WHERE NgayDung >= T.Thang AND NgayDung < DATEADD(MONTH, 1, T.Thang) AND NgayDung < CAST(GETDATE() AS DATE)) D " +
                "OUTER APPLY " +
                "(SELECT SUM(TongTien) AS TongChi FROM HoaDonNhap WHERE NgayNhap >= T.Thang AND NgayNhap < DATEADD(MONTH, 1, T.Thang)) C " +
                "ORDER BY T.Thang;";

            DataTable dt = db.ExecuteQuery(query);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataTable dtReversed = dt.Clone();
                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    dtReversed.ImportRow(dt.Rows[i]);
                }
                return dtReversed;
            }
            return dt;
        }


        public DataTable LayTyleGioiTinhNhanVien()
        {
            string query = "SELECT GioiTinh, COUNT(*) AS SoLuong FROM NhanVien GROUP BY GioiTinh;";
            return db.ExecuteQuery(query);
        }
    }
}
