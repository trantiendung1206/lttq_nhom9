using LTTQ_BTL.BLL;
using LTTQ_BTL.Dal;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LTTQ_BTL.Views.NhanSu
{
    public partial class UC_NhanSu : UserControl
    {
        NhanVienBLL bll = new NhanVienBLL();

        public UC_NhanSu()
        {
            InitializeComponent();
        }

        private void DinhDangDGV()
        {
            dgvNhanVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvNhanVien.RowHeadersVisible = false;

            dgvNhanVien.Columns["MaNhanVien"].HeaderText = "Mã NV";
            dgvNhanVien.Columns["TenNhanVien"].HeaderText = "Họ và Tên";
            dgvNhanVien.Columns["GioiTinh"].HeaderText = "Giới tính";
            dgvNhanVien.Columns["NgaySinh"].HeaderText = "Ngày sinh";
            dgvNhanVien.Columns["DiaChi"].HeaderText = "Địa chỉ";
            dgvNhanVien.Columns["TenQue"].HeaderText = "Quê quán";
            dgvNhanVien.Columns["DienThoai"].HeaderText = "Điện thoại";

            dgvNhanVien.Columns["MaNhanVien"].FillWeight = 10;
            dgvNhanVien.Columns["TenNhanVien"].FillWeight = 30;
            dgvNhanVien.Columns["GioiTinh"].FillWeight = 10;
            dgvNhanVien.Columns["NgaySinh"].FillWeight = 15;
            dgvNhanVien.Columns["DiaChi"].FillWeight = 20;
            dgvNhanVien.Columns["TenQue"].FillWeight = 15;
            dgvNhanVien.Columns["DienThoai"].FillWeight = 20;

            dgvNhanVien.Columns["MaQue"].Visible = false;

            dgvNhanVien.ReadOnly = true;
            dgvNhanVien.AllowUserToResizeColumns = false;
            dgvNhanVien.AllowUserToResizeRows = false;
            dgvNhanVien.AllowUserToAddRows = false;
            dgvNhanVien.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }
        bool dangSua = false;
        private void LoadDanhSachNhanVien()
        {
            DataTable dt = bll.DSNhanVien();
            dgvNhanVien.DataSource = dt;
            LoadComboboxQue();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy dữ liệu từ giao diện
                string ma = txtMaNV.Text.Trim();
                string ten = txtTenNV.Text.Trim();
                string gt = cboGioiTinh.Text;
                DateTime ns = dtpNgaySinh.Value;
                string dc = txtDiaChi.Text.Trim();
                string dt = txtDienThoai.Text.Trim();

                // Kiểm tra quê quán được chọn
                if (cboQueQuan.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn quê quán cho nhân viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string maQue = cboQueQuan.SelectedValue.ToString();

                // Gọi BLL
                bll.ThemNhanVien(ma, ten, gt, ns, dc, maQue, dt);

                MessageBox.Show("Thêm nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadDanhSachNhanVien();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvNhanVien.CurrentRow == null ||
        dgvNhanVien.CurrentRow.IsNewRow)
            {
                MessageBox.Show(
                    "Vui lòng chọn nhân viên cần xóa trên bảng!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            string ma = dgvNhanVien.CurrentRow
                .Cells["MaNhanVien"].Value?.ToString();

            if (string.IsNullOrWhiteSpace(ma))
            {
                MessageBox.Show("Không lấy được mã nhân viên!");
                return;
            }

            DialogResult dr = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa nhân viên [{ma}] không?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (dr != DialogResult.Yes)
                return;

            try
            {
                bll.XoaNhanVien(ma);

                MessageBox.Show(
                    "Đã xóa nhân viên thành công!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LoadDanhSachNhanVien();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }

        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvNhanVien.Rows[e.RowIndex];
                txtMaNV.Text = row.Cells["MaNhanVien"].Value.ToString();
                txtTenNV.Text = row.Cells["TenNhanVien"].Value.ToString();
                cboGioiTinh.Text = row.Cells["GioiTinh"].Value.ToString();
                if (row.Cells["NgaySinh"].Value != DBNull.Value)
                    dtpNgaySinh.Value = Convert.ToDateTime(row.Cells["NgaySinh"].Value);
                txtDiaChi.Text = row.Cells["DiaChi"].Value?.ToString();
                cboQueQuan.SelectedValue = row.Cells["MaQue"].Value?.ToString();
                txtDienThoai.Text = row.Cells["DienThoai"].Value?.ToString();
            }
        }

        private void LoadComboboxQue()
        {
            DataTable dt = bll.DSQue();

            cboQueQuan.DataSource = dt;
            cboQueQuan.DisplayMember = "TenQue";
            cboQueQuan.ValueMember = "MaQue";
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaNV.Clear();
            txtTenNV.Clear();
            txtDiaChi.Clear();
            txtDienThoai.Clear();
            txtMaNV.Focus();
        }

        private void UC_NhanSu_Load(object sender, EventArgs e)
        {
            LoadDanhSachNhanVien();
            DinhDangDGV();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!dangSua)
            {
                dangSua = true;
                button1.Text = "Lưu";
                btnLamMoi.Enabled = false;
                btnThem.Enabled = false;
                btnXoa.Enabled = false;
                dgvNhanVien.Enabled = false;

                txtMaNV.Text = dgvNhanVien.CurrentRow.Cells["MaNhanVien"].Value.ToString();
                txtMaNV.Enabled = false;
                txtTenNV.Text = dgvNhanVien.CurrentRow.Cells["TenNhanVien"].Value.ToString();
                cboGioiTinh.SelectedItem = dgvNhanVien.CurrentRow.Cells["GioiTinh"].Value.ToString();
                dtpNgaySinh.Value = Convert.ToDateTime(dgvNhanVien.CurrentRow.Cells["NgaySinh"].Value);
                txtDiaChi.Text = dgvNhanVien.CurrentRow.Cells["DiaChi"].Value.ToString();
                cboQueQuan.SelectedValue = dgvNhanVien.CurrentRow.Cells["MaQue"].Value.ToString();
                txtDienThoai.Text = dgvNhanVien.CurrentRow.Cells["DienThoai"].Value.ToString();
            }
            else
            {
                try
                {
                    string maNhanVien = txtMaNV.Text.Trim();
                    string tenNhanVien = txtTenNV.Text.Trim();
                    string diaChi = txtDiaChi.Text.Trim();
                    string dienThoai = txtDienThoai.Text.Trim();
                    DateTime ngaySinh = dtpNgaySinh.Value;
                    string gioiTinh = cboGioiTinh.Text;
                    string maQue = cboQueQuan.SelectedValue.ToString();

                    bll.SuaNhanVien(maNhanVien, tenNhanVien, gioiTinh, ngaySinh, diaChi, maQue, dienThoai);
                    MessageBox.Show("Cập nhật thông tin nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDanhSachNhanVien();
                    btnLamMoi_Click(sender, e);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                dangSua = false;
                button1.Text = "Sửa";
                btnLamMoi.Enabled = true;
                btnThem.Enabled = true;
                btnXoa.Enabled = true;
                dgvNhanVien.Enabled = true;
                txtMaNV.Enabled = true;
            }
        }
    }
}