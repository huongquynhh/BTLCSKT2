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
    public partial class FrmDonvitinh : Form
    {
        public FrmDonvitinh()
        {
            InitializeComponent();
        }

        private void dgvdvt_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            FrmDonvitinh f = new FrmDonvitinh();
            f.ShowDialog();

        }
        private void LoadDatagridview()
        {
            string sql = "Select*from donvitinh";
            DataTable tblkh = new DataTable();
            tblkh = DAO.LoadDataToTable(sql);
            dgvdvt.DataSource = tbldvt;
        }
        DataTable tbldvt;
        private void FrmDonvitinh_Load(object sender, EventArgs e)
        {
            string sql = "Select*from donvitinh";
            DataTable mytable = new DataTable();
            tbldvt = DAO.LoadDataToTable(sql);
            dgvdvt.DataSource = tbldvt;
            LoadDatagridview();
        }
        private void Reset()
        {
            txtMadvt.Text = "";
            txtTendvt.Text = "";


        }

        private void dgvdvt_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMadvt.Text = dgvdvt.CurrentRow.Cells[0].Value.ToString();
            txtTendvt.Text = dgvdvt.CurrentRow.Cells[1].Value.ToString();
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
            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            //Hien thi nut Luu de luu ban ghi moi la 2 dong duoc go tren 2 textbox
            btnLuu.Enabled = true;
            //Hien thi bo qua neu nguoi dung khong muon them moi ban ghi do nua
            btnHuy.Enabled = true;
            //Sau khi Luu hoac Bo qua thi xoa trang du lieu de nguoi dung nhap lai
            Reset();
            txtMadvt.Enabled = true;
            txtMadvt.Focus();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (txtMadvt.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập mã đơn vị tính", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMadvt.Focus();
                return;
            }
            if (txtTendvt.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tên đơn vị tính", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTendvt.Focus();
                return;
            }

            string sql = "SELECT Madonvitinh FROM donvitinh WHERE Madonvitinh =N'" + txtMadvt.Text.Trim() + "'";
            if (DAO.Checkey(sql))
            {
                MessageBox.Show("Mã đơn vị tính này đã có, bạn phải nhập mã khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMadvt.Focus();
                txtMadvt.Text = "";
                return;
            }
            sql = "INSERT INTO donvitinh(Madonvitinh,Tendonvitinh) VALUES(N'" + txtMadvt.Text + "',N'" + txtTendvt.Text + "')";
            DAO.RunSql(sql);
            LoadDatagridview();
            Reset();

            btnXoa.Enabled = true;
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnHuy.Enabled = false;
            btnLuu.Enabled = false;
            txtMadvt.Enabled = false;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (tbldvt.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu!", "Thông báo",
               MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtMadvt.Text.Trim() == "")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào để xóa!", "Thông báo",
               MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa không", "Cảnh cáo",
                MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                string sql = "delete from donvitinh where Madvt='" + txtMadvt.Text
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
    }
}
