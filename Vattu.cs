using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quanly_VLXD
{
    public partial class FrmVattu : Form
    {
        public FrmVattu()
        {
            InitializeComponent();
        }
        DataTable tblvt;
        private void Reset()
        {
            txtMavt.Text = "";
            txtTenvt.Text = "";
            txtGĩauat.Text = "";
            txtGianhap.Text = "";
            cmbdvt.Text = "";
            cmbncc.Text = "";

        }
        private void LoadDatagridview()
        {
            string sql = "Select*from vattu";
            DataTable tblvt = new DataTable();
            tblvt = DAO.LoadDataToTable(sql);
            dgvvt.DataSource = tblvt;
        }
        private void FrmVattu_Load(object sender, EventArgs e)
        {

            txtMavt.Enabled = false;
            btnLuu.Enabled = false;
            btnHuy.Enabled = false;
            LoadDatagridview();
            string sql = "SELECT MaNCC, TenNCC FROM nhacungcap" + "";
            DAO.Filldatocombo(sql, cmbncc, "MaNCC","TenNCC");

            sql = "SELECT Madonvitinh, Tendonvitinh FROM donvitinh" + "";
             DAO.Filldatocombo(sql, cmbdvt, "Madonvitinh","Tendonvitinh");



        }
        private void dgvvt_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMavt.Text = dgvvt.CurrentRow.Cells[0].Value.ToString();
            txtTenvt.Text = dgvvt.CurrentRow.Cells[1].Value.ToString();

            cmbdvt.Text = dgvvt.CurrentRow.Cells[2].Value.ToString();
            cmbncc.Text = dgvvt.CurrentRow.Cells[3].Value.ToString();

            txtGĩauat.Text = dgvvt.CurrentRow.Cells[4].Value.ToString();
            txtGianhap.Text = dgvvt.CurrentRow.Cells[5].Value.ToString();

        }
        private void dgvvt_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            FrmVattu f = new FrmVattu();
            f.ShowDialog();

        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            //Hien thi nut Luu de luu ban ghi moi la 2 dong duoc go tren 2 textbox
            btnLuu.Enabled = true;
            //Hien thi bo qua neu nguoi dung khong muon them moi ban ghi do nua
            btnHuy.Enabled = true;
            //Sau khi Luu hoac Bo qua thi xoa trang du lieu de nguoi dung nhap lai
            Reset();
            txtMavt.Enabled = true;
            txtMavt.Focus();
        }
        //Khi nhập giá nhập thì tự động hiện ra giá xuất 

        private void txtGianhap_TextChanged(object sender, EventArgs e)
        {
            double dgn, dgb;
            if (txtGianhap.Text == "")
                dgn = 0;
            else
                dgn = Convert.ToDouble(txtGianhap.Text);
            dgb = dgn * 1.1;
            txtGĩauat.Text = dgb.ToString();
        }
    }
}
