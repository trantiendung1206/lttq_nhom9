using LTTQ_BTL.Dal.Dao;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LTTQ_BTL.BLL
{
    public class KhachHangBLL
    {
        KhachHangDao dao = new KhachHangDao();
        public DataTable DSKhachHang()
        {
            return dao.DSKhachHang();
        }

        public void ThemKhach(
            string maKhach,
            string tenKhach,
            string diaChi,
            string dienThoai)
        {
            if (string.IsNullOrWhiteSpace(maKhach) ||
                string.IsNullOrWhiteSpace(tenKhach))
            {
                throw new Exception("Vui lòng nhập đầy đủ mã khách và tên khách!");
            }

            bool ketQua = dao.ThemKhach(
                maKhach.Trim(),
                tenKhach.Trim(),
                diaChi.Trim(),
                dienThoai.Trim()
            );

            if (!ketQua)
                throw new Exception("Thêm khách hàng thất bại!");
        }


        public void XoaKhachHang(string maKhach)
        {
            if (string.IsNullOrWhiteSpace(maKhach))
                throw new Exception("Vui lòng chọn khách hàng cần xóa.");

            try
            {
                bool ketQua = dao.XoaKhachHang(maKhach.Trim());

                if (!ketQua)
                    throw new Exception("Không tìm thấy khách hàng trong cơ sở dữ liệu.");
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                throw new Exception(
                    "Không thể xóa khách hàng vì khách hàng đang được liên kết với dữ liệu khác.",
                    ex
                );
            }
        }

        //sưa 
        public bool SuaKhach(
                        string maKhach,
                        string tenKhach,
                        string diaChi,
                        string dienThoai)
        {
            if (string.IsNullOrWhiteSpace(maKhach))
                throw new Exception("Vui lòng chọn khách hàng cần sửa!");

            if (string.IsNullOrWhiteSpace(tenKhach))
                throw new Exception("Tên khách hàng không được để trống!");

            if (string.IsNullOrWhiteSpace(diaChi))
                throw new Exception("Địa chỉ không được để trống!");

            if (string.IsNullOrWhiteSpace(dienThoai))
                throw new Exception("Số điện thoại không được để trống!");

            return dao.SuaKhach(maKhach, tenKhach, diaChi, dienThoai);
        }
    }
}
