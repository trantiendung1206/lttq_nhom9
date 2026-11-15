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

namespace LTTQ_BTL.Views.Khach
{
    public partial class UC_Khach : UserControl
    {
        KhachHangBLL bll = new KhachHangBLL();

        public UC_Khach()
        {
            InitializeComponent();
        }

        private void DinhDangDGV()
        {
            dgvKhach.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvKhach.RowHeadersVisible = false;

            dgvKhach.Columns["MaKhach"].HeaderText = "Mã Khách";
            dgvKhach.Columns["TenKhach"].HeaderText = "Tên Khách Hàng";
            dgvKhach.Columns["DiaChi"].HeaderText = "Địa chỉ";
            dgvKhach.Columns["DienThoai"].HeaderText = "Điện thoại";

            dgvKhach.Columns["MaKhach"].FillWeight = 10;
            dgvKhach.Columns["TenKhach"].FillWeight = 40;
            dgvKhach.Columns["DiaChi"].FillWeight = 20;
            dgvKhach.Columns["DienThoai"].FillWeight = 20;

            dgvKhach.ReadOnly = true;
            dgvKhach.AllowUserToResizeColumns = false;
            dgvKhach.AllowUserToResizeRows = false;
            dgvKhach.AllowUserToAddRows = false;
            dgvKhach.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        bool dangSua = false;
        private void LoadDanhSachKhach()
        {
            DataTable dt = bll.DSKhachHang();
            dgvKhach.DataSource = dt;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                string maKhach = txtMaKhach.Text.Trim();
                string tenKhach = txtTenKhach.Text.Trim();
                string diaChi = txtDiaChi.Text.Trim();
                string dienThoai = txtDienThoai.Text.Trim();

                bll.ThemKhach(
                    maKhach,
                    tenKhach,
                    diaChi,
                    dienThoai
                );

                MessageBox.Show("Thêm khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadDanhSachKhach();
                btnLamMoi_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaKhach.Clear();
            txtTenKhach.Clear();
            txtDiaChi.Clear();
            txtDienThoai.Clear();
            txtMaKhach.Focus();
        }

        private void dgvKhach_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvKhach.SelectedRows[0];
                txtMaKhach.Text = row.Cells["MaKhach"].Value.ToString();
                txtTenKhach.Text = row.Cells["TenKhach"].Value.ToString();
                txtDiaChi.Text = row.Cells["DiaChi"].Value.ToString() == "" ? "" : row.Cells["DiaChi"].Value.ToString();
                txtDienThoai.Text = row.Cells["DienThoai"].Value.ToString() == "" ? "" : row.Cells["DienThoai"].Value.ToString();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string maKhach = "";

            if (dgvKhach.SelectedRows.Count > 0)
            {
                maKhach = dgvKhach.SelectedRows[0].Cells["MaKhach"].Value?.ToString() ?? "";
            }
            else if (dgvKhach.CurrentRow != null)
            {
                maKhach = dgvKhach.CurrentRow.Cells["MaKhach"].Value?.ToString() ?? "";
            }

            if (string.IsNullOrWhiteSpace(maKhach))
            {
                maKhach = txtMaKhach.Text.Trim();
            }

            if (string.IsNullOrWhiteSpace(maKhach))
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần xóa trên bảng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dr = MessageBox.Show($"Bạn có chắc chắn muốn xóa khách hàng có mã '{maKhach}' không?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dr != DialogResult.Yes)
                return;

            try
            {
                bll.XoaKhachHang(maKhach);

                MessageBox.Show("Xóa khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadDanhSachKhach();
                btnLamMoi_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void UC_Khach_Load(object sender, EventArgs e)
        {
            LoadDanhSachKhach();
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
                dgvKhach.Enabled = false;

                txtMaKhach.Text = dgvKhach.CurrentRow.Cells[0].Value.ToString();
                txtMaKhach.Enabled = false;
                txtTenKhach.Text = dgvKhach.CurrentRow.Cells[1].Value.ToString();
                txtDiaChi.Text = dgvKhach.CurrentRow.Cells[2].Value.ToString();
                txtDienThoai.Text = dgvKhach.CurrentRow.Cells[3].Value.ToString();
            }
            else
            {
                try
                {
                    string maKhach = txtMaKhach.Text.Trim();
                    string tenKhach = txtTenKhach.Text.Trim();
                    string diaChi = txtDiaChi.Text.Trim();
                    string dienThoai = txtDienThoai.Text.Trim();
                    bll.SuaKhach(maKhach, tenKhach, diaChi, dienThoai);
                    MessageBox.Show("Cập nhật thông tin khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDanhSachKhach();
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
                dgvKhach.Enabled = true;
                txtMaKhach.Enabled = true;
            }
        }
    }
}