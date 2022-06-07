using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;



namespace Quanly_VLXD
{
    public partial class FrmDMCongviec : Form
    {
        public FrmDMCongviec()
        {
            InitializeComponent();
        }
        private void Reset()
        {
            txtMacv.Text = "";
            txtTencv.Text = "";
           

        }

        private void dgvCV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            FrmDMCongviec f = new FrmDMCongviec();
            f.ShowDialog();
        }
        DataTable tblcv;

        private void LoadDatagridview()
        {
            string sql = "Select*from congviec";
            DataTable tblkh = new DataTable();
            tblkh = DAO.LoadDataToTable(sql);
            dgvCV.DataSource = tblcv;

        }
        private void FrmDMCongviec_Load(object sender, EventArgs e)
        {
            string sql = "Select*from congviec";
            DataTable mytable = new DataTable();
            tblcv = DAO.LoadDataToTable(sql);
            dgvCV.DataSource = tblcv;
            LoadDatagridview();
        }

        private void dgvCV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMacv.Text = dgvCV.CurrentRow.Cells[0].Value.ToString();
            txtTencv.Text = dgvCV.CurrentRow.Cells[1].Value.ToString();


        }

        private void btnThoat_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Bạn có muốn thoát không?", "Thông báo",
           MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DAO.Close();
                this.Close();
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            /* txtMacv.Text = "";
            txtTenChatLieu.Text = "";
            btnLuu.Enabled = true;
            btnSua .Enabled = false;
            btnXoa.Enabled = false;
            btnThem.Enabled = false;
            txtMaChatLieu.Enabled = true;
            txtMaChatLieu.Focus();*/
            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            //Hien thi nut Luu de luu ban ghi moi la 2 dong duoc go tren 2 textbox
            btnLuu.Enabled = true;
            //Hien thi bo qua neu nguoi dung khong muon them moi ban ghi do nua
            btnHuy.Enabled = true;
            //Sau khi Luu hoac Bo qua thi xoa trang du lieu de nguoi dung nhap lai
            Reset();
            txtMacv.Enabled = true;
            txtMacv.Focus();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (txtMacv.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập mã công việc", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMacv.Focus();
                return;
            }
            if (txtTencv.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tên công việc", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTencv.Focus();
                return;
            }

            string sql = "SELECT MaCV FROM Congviec WHERE MaCV =N'" + txtMacv.Text.Trim() + "'";
            if (DAO.Checkey(sql))
            {
                MessageBox.Show("Mã công việc này đã có, bạn phải nhập mã khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMacv.Focus();
                txtMacv.Text = "";
                return;
            }
            sql = "INSERT INTO Congviec(MaCV,TenCV) VALUES(N'" + txtMacv.Text + "',N'" + txtTencv.Text + "')";
            DAO.RunSql(sql);
            LoadDatagridview();
            Reset();

            btnXoa.Enabled = true;
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnHuy.Enabled = false;
            btnLuu.Enabled = false;
            txtMacv.Enabled = false;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Bạn có chắc muốn xóa không", "Cảnh cáo",
                MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                string sql = "delete from congviec where MaCV='" + txtMacv.Text
                    + "'";
                try
                {

                    SqlCommand mycommand = new SqlCommand(sql, DAO.con);
                    mycommand.ExecuteNonQuery();
                    LoadDatagridview();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("xoá không thành công vì:" + ex.ToString());
                }

                SqlCommand myconmmand = new SqlCommand(sql, DAO.con);
                myconmmand.ExecuteNonQuery();
                LoadDatagridview();
            }
        }

        private void txtMacv_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
        

