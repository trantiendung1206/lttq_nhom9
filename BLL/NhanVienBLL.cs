using LTTQ_BTL.Dal.Dao;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LTTQ_BTL.BLL
{
    public class NhanVienBLL
    {
        NhanVienDao dao = new NhanVienDao();

        public DataTable DSNhanVien()
        {
            return dao.DSNhanVien();
        }

        public DataTable DSQue()
        {
            return dao.DSQue();
        }

        public DataTable DSNhaCungCap()
        {
            return dao.DSNhaCungCap();
        }

        public void ThemNhanVien(
            string ma,
            string ten,
            string gt,
            DateTime ns,
            string dc,
            string maQue,
            string dt)
        {
            // Kiểm tra mã và tên
            if (string.IsNullOrWhiteSpace(ma) ||
                string.IsNullOrWhiteSpace(ten))
            {
                throw new Exception("Vui lòng nhập mã và tên nhân viên!");
            }

            // Kiểm tra quê quán
            if (string.IsNullOrWhiteSpace(maQue))
            {
                throw new Exception("Vui lòng chọn quê quán cho nhân viên!");
            }

            // Kiểm tra ngày sinh
            if (ns.Date > DateTime.Today)
            {
                throw new Exception("Ngày sinh không hợp lệ!");
            }

            bool ketQua = dao.ThemNhanVien(ma.Trim(),ten.Trim(),gt,ns,dc.Trim(),maQue,dt.Trim());

            if (!ketQua)
            {
                throw new Exception("Thêm nhân viên thất bại!");
            }
        }


        public void XoaNhanVien(string ma)
        {
            if (string.IsNullOrWhiteSpace(ma))
                throw new Exception("Vui lòng chọn nhân viên cần xóa.");

            try
            {
                bool ketQua = dao.XoaNhanVien(ma.Trim());

                if (!ketQua)
                    throw new Exception("Nhân viên không tồn tại.");
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                throw new Exception(
                    "Không thể xóa nhân viên vì nhân viên này đang được liên kết với hóa đơn nhập hoặc phiếu đặt bàn.",
                    ex
                );
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
            if (string.IsNullOrWhiteSpace(ma))
                throw new Exception("Vui lòng chọn nhân viên cần sửa!");

            if (string.IsNullOrWhiteSpace(ten))
                throw new Exception("Tên nhân viên không được để trống!");

            if (string.IsNullOrWhiteSpace(gioiTinh))
                throw new Exception("Vui lòng chọn giới tính!");

            if (string.IsNullOrWhiteSpace(diaChi))
                throw new Exception("Địa chỉ không được để trống!");

            if (string.IsNullOrWhiteSpace(maQue))
                throw new Exception("Vui lòng chọn quê quán!");

            if (string.IsNullOrWhiteSpace(dienThoai))
                throw new Exception("Số điện thoại không được để trống!");

            if (ngaySinh.Date >= DateTime.Today)
                throw new Exception("Ngày sinh không hợp lệ!");

            return dao.SuaNhanVien(
                ma, ten, gioiTinh, ngaySinh, diaChi, maQue, dienThoai);
        }
    }
}