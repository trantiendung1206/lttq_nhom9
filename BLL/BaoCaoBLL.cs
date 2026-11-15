using LTTQ_BTL.Dal.Dao;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LTTQ_BTL.BLL
{
    public class BaoCaoBLL
    {
        BaoCaoDao dao = new BaoCaoDao();

        public int LayTongSoNhanVien()
        {
            return dao.LayTongSoNhanVien();
        }

        public int LayTongSoKhachHang()
        {
            return dao.LayTongSoKhachHang();
        }

        public decimal LayChiTieuThangHienTai()
        {
            return dao.LayChiTieuThangHienTai();
        }

        public decimal LayDoanhThuThangHienTai()
        {
            return dao.LayDoanhThuThangHienTai();
        }

        public DataTable LayDoanhThu6ThangGanNhat()
        {
            return dao.LayDoanhThu6ThangGanNhat();
        }

        public DataTable LayTyleGioiTinhNhanVien()
        {
            return dao.LayTyleGioiTinhNhanVien();
        }
    }
}
