using LTTQ_BTL.Dal;
using OfficeOpenXml;
using OfficeOpenXml;
using System.IO;
using System;
using System.Data;
using System.Drawing;
using System.Security.Policy;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using LTTQ_BTL.BLL;

namespace LTTQ_BTL.Reports.BaoCao
{
    public partial class UC_BaoCao : UserControl
    {
        BaoCaoBLL bll1 = new BaoCaoBLL();
        HoaDonNhapBLL bll2 = new HoaDonNhapBLL();
        PhieuDatBanBLL bll3 = new PhieuDatBanBLL();
        NhanVienBLL bll4 = new NhanVienBLL();
        public UC_BaoCao()
        {
            InitializeComponent();
        }

        private void DinhDangDGV()
        {
            dgvBaoCao.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBaoCao.RowHeadersVisible = false;

            dgvBaoCao.Columns[0].HeaderText = "Mã";
            dgvBaoCao.Columns[1].HeaderText = "Ngày thực hiên";
            dgvBaoCao.Columns[2].HeaderText = "Mã nhân viên";
            dgvBaoCao.Columns[3].HeaderText = "Tên nhân viên";
            dgvBaoCao.Columns[4].HeaderText = "Tên khách/NCC";
            dgvBaoCao.Columns[5].HeaderText = "Tổng tiền";

            dgvBaoCao.Columns[0].FillWeight = 10;
            dgvBaoCao.Columns[1].FillWeight = 10;
            dgvBaoCao.Columns[2].FillWeight = 10;
            dgvBaoCao.Columns[3].FillWeight = 10;
            dgvBaoCao.Columns[4].FillWeight = 10;
            dgvBaoCao.Columns[5].FillWeight = 10;

            dgvBaoCao.ReadOnly = true;
            dgvBaoCao.AllowUserToResizeColumns = false;
            dgvBaoCao.AllowUserToResizeRows = false;
            dgvBaoCao.AllowUserToAddRows = false;
            dgvBaoCao.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        public DataTable DTGop(DataTable hoadon, DataTable datban)
        {
            DataTable dtTongHop = new DataTable();

            dtTongHop.Columns.Add("Ma", typeof(string));
            dtTongHop.Columns.Add("NgayThucHien", typeof(DateTime));
            dtTongHop.Columns.Add("MaNhanVien", typeof(string));
            dtTongHop.Columns.Add("TenNhanVien", typeof(string));
            dtTongHop.Columns.Add("KhachHang", typeof(string));
            dtTongHop.Columns.Add("TongTien", typeof(decimal));

            foreach (DataRow row in hoadon.Rows)
            {
                dtTongHop.Rows.Add(row[0], row[1], row[2], row[3], row[4], row[5]);
            }

            foreach (DataRow row in datban.Rows)
            {
                dtTongHop.Rows.Add(row[0], row[1], row[2], row[3], row[4], row[5]);
            }

            dtTongHop.DefaultView.Sort = "NgayThucHien DESC";

            return dtTongHop;
        }

        private void UC_BaoCao_Load(object sender, EventArgs e)
        {
            //dashboard 
            try
            {
                lblTongSoNV.Text = bll1.LayTongSoNhanVien().ToString();
                lblTongSoKH.Text = bll1.LayTongSoKhachHang().ToString();

                decimal doanhThu = bll1.LayDoanhThuThangHienTai();
                doanhThu = doanhThu - bll1.LayChiTieuThangHienTai();
                lblDoanhThuThang.Text = doanhThu.ToString("N0") + " VNĐ";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load Dashboard: " + ex.Message);
            }
            //bieu do 
            try
            {
                LoadChartDoanhThu6Thang();
                LoadChartTyLeGioiTinh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load Biểu đồ: " + ex.Message);
            }

            DataTable dt = bll4.DSNhanVien();
            cboNhanVien.DataSource = dt;
            cboNhanVien.DisplayMember = "TenNhanVien";
            cboNhanVien.ValueMember = "MaNhanVien";

            DataTable dt2 =  bll2.DSHoaDonNhap_BaoCao();
            DataTable dt3 = bll3.DSPhieuDatBan_BaoCao();
            DataTable dt4 = DTGop(dt2, dt3);
            dgvBaoCao.DataSource = dt4;
            DinhDangDGV();
        }

        /*private void LoadDashboardOverview()
        {
            try
            {
                lblTongSoNV.Text = bll1.LayTongSoNhanVien().ToString();
                lblTongSoKH.Text = bll1.LayTongSoKhachHang().ToString();

                decimal doanhThuThang = bll1.LayDoanhThuThangHienTai();
                doanhThuThang = doanhThuThang - bll1.LayChiTieuThangHienTai();
                lblDoanhThuThang.Text = doanhThuThang.ToString("N0") + " VNĐ";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải số liệu Dashboard: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }*/
        //xong
        private void LoadChartDoanhThu6Thang()
        {
            chartDoanhThu6Thang.Series.Clear();
            chartDoanhThu6Thang.ChartAreas.Clear();
            chartDoanhThu6Thang.Titles.Clear();
            chartDoanhThu6Thang.Legends.Clear();

            var area = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            chartDoanhThu6Thang.ChartAreas.Add(area);

            chartDoanhThu6Thang.Titles.Add("DOANH THU 6 THÁNG GẦN NHẤT");

            var series = chartDoanhThu6Thang.Series.Add("Doanh thu");

            series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;

            series.IsXValueIndexed = true;
            series.IsValueShownAsLabel = true;
            series.LabelFormat = "#,##0";

            area.AxisX.Interval = 1;
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisX.Title = "Tháng";

            area.AxisY.Title = "Doanh thu (VNĐ)";
            area.AxisY.LabelStyle.Format = "#,##0";
            area.AxisY.Minimum = 0;

            DataTable dt = bll1.LayDoanhThu6ThangGanNhat();

            foreach (DataRow row in dt.Rows)
            {
                decimal doanhThu = Convert.ToDecimal(row["DoanhThu"]);

                int index = series.Points.AddY((double)doanhThu);

                series.Points[index].AxisLabel = $"Tháng {row[0].ToString()}";
            }

            chartDoanhThu6Thang.Invalidate();
        }
        //xong
        private void LoadChartTyLeGioiTinh()
        {
            chartTyLeGioiTinh.Series.Clear();
            var seriesTron = chartTyLeGioiTinh.Series.Add("TyLe");
            seriesTron.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;

            DataTable dt = bll1.LayTyleGioiTinhNhanVien();

            decimal tilenam = decimal.Parse(dt.Rows[0][1].ToString()) / (decimal.Parse(dt.Rows[0][1].ToString()) + decimal.Parse(dt.Rows[1][1].ToString())) * 100;
            decimal tilenu = 100 - tilenam;
            tilenam = Math.Round(tilenam, 2);
            tilenu = Math.Round(tilenu, 2);
            string txtNam = "Nam (" + tilenam.ToString() + "%)";
            string txtNu = "Nữ (" + tilenu.ToString() + "%)";

            int pNam = seriesTron.Points.AddXY(txtNam, tilenam);
            seriesTron.Points[pNam].Color = Color.Blue;

            int pNu = seriesTron.Points.AddXY(txtNu, tilenu);
            seriesTron.Points[pNu].Color = Color.Orange;

            seriesTron["PieRadiusPercentage"] = "78";

            if (chartTyLeGioiTinh.Legends.Count > 0)
            {
                chartTyLeGioiTinh.Legends[0].Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
                chartTyLeGioiTinh.Legends[0].Alignment = StringAlignment.Center;
            }

            seriesTron["PieLabelStyle"] = "Outside";
        }
        //xong
        private void LoadDanhSachChiTiet() {
            string maNhanVien = cboNhanVien.SelectedValue?.ToString() ?? "";

            DataTable dt = bll2.LocDSHoaDonNhap_BaoCao(dtpTuNgay.Value, dtpDenNgay.Value, maNhanVien);
            DataTable dt2 = bll3.LocDSPhieuDatBan_BaoCao(dtpTuNgay.Value, dtpDenNgay.Value, maNhanVien);
            if(cboLoaiBaoCao.SelectedIndex == 0)
            {
                dt.Rows.Clear();
            }
            if(cboLoaiBaoCao.SelectedIndex == 1) dt2.Rows.Clear();

            DataTable dt3 = DTGop(dt, dt2);
            dgvBaoCao.DataSource = dt3;

            DinhDangDGV();
        }

        private void btnTaoBaoCao_Click(object sender, EventArgs e)
        {
            LoadDanhSachChiTiet();

            MessageBox.Show("Đã tạo báo cáo mới dựa trên điều kiện lọc!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            dtpTuNgay.Value = DateTime.Now.AddMonths(-1);
            dtpDenNgay.Value = DateTime.Now;
            cboNhanVien.SelectedIndex = -1;

            DataTable dt2 = bll2.DSHoaDonNhap_BaoCao();
            DataTable dt3 = bll3.DSPhieuDatBan_BaoCao();
            DataTable dt4 = DTGop(dt2, dt3);
            dgvBaoCao.DataSource = dt4;
            DinhDangDGV();

            MessageBox.Show("Đã làm mới dữ liệu báo cáo!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void XuatExcel(DataTable dt, string fileName)
        {
            ExcelPackage.License.SetNonCommercialPersonal("Hoang Trong Giap");

            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet ws = package.Workbook.Worksheets.Add("BaoCaoDoanhThu");
                ws.Cells["A1"].LoadFromDataTable(dt, true);

                using (var range = ws.Cells[1, 1, 1, dt.Columns.Count])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                ws.Cells.AutoFitColumns();

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel Files|*.xlsx";
                saveFileDialog.FileName = fileName + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    FileInfo file = new FileInfo(saveFileDialog.FileName);
                    package.SaveAs(file);
                    MessageBox.Show("Xuất file Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void lblTongSoNV_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cboNhanVien_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = dgvBaoCao.DataSource as DataTable;
                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để xuất!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                XuatExcel(dt, "BaoCaoQuanLyNhaHang");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xuất file Excel: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void chartDoanhThu6Thang_Click(object sender, EventArgs e)
        {

        }

        private void cboLoaiBaoCao_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}