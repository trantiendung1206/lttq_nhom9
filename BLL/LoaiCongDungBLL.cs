using LTTQ_BTL.Dal.Dao;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LTTQ_BTL.BLL
{
    public class LoaiCongDungBLL
    {
        LoaiCongDungDao dao = new LoaiCongDungDao();

        public DataTable DSCongDung()
        {
            try
            {
                DataTable dt = dao.DSCongDung();

                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi lấy danh sách công dụng: " + ex.Message);
            }
        }

        public DataTable DSLoai()
        {
            try
            {
                DataTable dt = dao.DSLoai();

                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi lấy danh sách loại: " + ex.Message);
            }
        }
    }
}
