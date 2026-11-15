using LTTQ_BTL.Dal.Dao;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LTTQ_BTL.BLL
{
    public class HoaDonNhapBLL
    {
        HoaDonNhapDao dao = new HoaDonNhapDao();

        public DataTable DSHoaDonNhap()
        {
            try
            {
                DataTable dt = dao.DSHoaDonNhap();

                return dt.DefaultView.ToTable(false, "MaHoaDonNhap", "NgayNhap", "TenNhanVien", "TenNhaCungCap", "TongTien");
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi lấy danh sách hóa đơn nhập: " + ex.Message);
            }
        }
        //dùng trong báo cáo
        public DataTable DSHoaDonNhap_BaoCao()
        {
            try
            {
                DataTable dt = dao.DSHoaDonNhap();

                return dt.DefaultView.ToTable(false, "MaHoaDonNhap", "NgayNhap", "MaNhanVien", "TenNhanVien", "TenNhaCungCap", "TongTien");
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi lấy danh sách hóa đơn nhập: " + ex.Message);
            }
        }

        //dùng trong báo cáo
        public DataTable LocDSHoaDonNhap_BaoCao(DateTime tuNgay, DateTime denNgay, string maNhanVien)
        {
            try
            {
                DataTable dt = dao.DSHoaDonNhap();

                dt = dt.DefaultView.ToTable(false, "MaHoaDonNhap", "NgayNhap", "MaNhanVien", "TenNhanVien", "TenNhaCungCap", "TongTien");

                DataTable ketQua = dt.Clone();
                ketQua.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    DateTime ngay = Convert.ToDateTime(row[1]);

                    if (ngay.Date >= tuNgay.Date &&
                        ngay.Date <= denNgay.Date && (string.IsNullOrEmpty(maNhanVien) || row[2].ToString() == maNhanVien))
                    {
                        ketQua.ImportRow(row);
                    }
                }

                return ketQua;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi lấy danh sách hóa đơn nhập: " + ex.Message);
            }
        }

        public DataTable ChiTietHoaDon(string ma)
        {
            if (string.IsNullOrWhiteSpace(ma))
                throw new Exception("Mã hóa đơn không được để trống.");

            try
            {
                DataTable dt = dao.ChiTietHoaDonNhap(ma.Trim());

                return dt.DefaultView.ToTable(false, "MaHoaDonNhap", "MaNguyenLieu", "TenNguyenLieu", "SoLuong", "DonGia", "KhuyenMai", "ThanhTien");
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi lấy chi tiết hóa đơn: " + ex.Message);
            }
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
            if (string.IsNullOrWhiteSpace(maHoaDon))
                throw new Exception("Mã hóa đơn không được để trống.");

            if (string.IsNullOrWhiteSpace(maNhanVien))
                throw new Exception("Nhân viên không được để trống.");

            if (string.IsNullOrWhiteSpace(maNhaCungCap))
                throw new Exception("Nhà cung cấp không được để trống.");

            if (string.IsNullOrWhiteSpace(maNguyenLieu))
                throw new Exception("Nguyên liệu không được để trống.");

            if (soLuong <= 0)
                throw new Exception("Số lượng phải lớn hơn 0.");

            if (donGia <= 0)
                throw new Exception("Đơn giá phải lớn hơn 0.");

            if (khuyenMai < 0)
                throw new Exception("Khuyến mại không được âm.");

            if (khuyenMai > 100)
                throw new Exception("Khuyến mại không được vượt quá 100%.");

            if (tongTien <= 0)
                throw new Exception("Tổng tiền phải lớn hơn 0.");

            try
            {
                return dao.ThemPhieuNhap(
                    maHoaDon.Trim(), ngayNhap, maNhanVien.Trim(), maNhaCungCap.Trim(), 
                    tongTien, maNguyenLieu.Trim(), soLuong, donGia, khuyenMai);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi thêm phiếu nhập: " + ex.Message);
            }
        }

        public DataTable LayTheoMa(string ma)
        {
            if (string.IsNullOrWhiteSpace(ma))
                throw new Exception("Mã hóa đơn không được để trống.");

            try
            {
                DataTable dt = dao.LayTheoMa(ma.Trim());
                if (dt.Rows.Count == 0)
                    throw new Exception("Không tìm thấy hóa đơn này.");
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi lấy thông tin hóa đơn: " + ex.Message);
            }
        }
    }
}
