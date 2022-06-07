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
    public partial class Nhacungcap : Form
    {
        public Nhacungcap()
        {
            InitializeComponent();
        }
        DataTable tblncc;
        private void LoadDatagridview()
        {
            string sql = "Select*from nhacungcap";
            DataTable tblncc = new DataTable();
            tblncc = DAO.LoadDataToTable(sql);
            dgvNCC.DataSource = tblncc;
        }
        private void Nhacungcap_Load(object sender, EventArgs e)
        {
            string sql = "Select*from nhacungcap";
            DataTable mytable = new DataTable();
            tblncc = DAO.LoadDataToTable(sql);
            dgvNCC.DataSource = tblncc;
            LoadDatagridview();
        }
        
        private void dgvNCC_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Nhacungcap f = new Nhacungcap();
            f.ShowDialog();
        }
        private void Reset()
        {
            txtMancc.Text = "";
            txtTenncc.Text = "";
            txtDiachi.Text = "";
            txtsdt.Text = "";

        }

        private void dgvNCC_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMancc.Text = dgvNCC.CurrentRow.Cells[0].Value.ToString();
            txtTenncc.Text = dgvNCC.CurrentRow.Cells[1].Value.ToString();
            txtDiachi.Text = dgvNCC.CurrentRow.Cells[2].Value.ToString();
            txtsdt.Text = dgvNCC.CurrentRow.Cells[3].Value.ToString();

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
            Reset();
            int tongds = tblncc.Rows.Count;
            string ma = "";
            if (tongds <= 0)                                 //tạo mã tự động 
            {
                ma = "NCC001";
            }
            else
            {
                int so;
                ma = "NCC";
                so = Convert.ToInt32(tblncc.Rows[tongds - 1][0].ToString().Substring(3, 5));
                so = so + 1;
                if (so < 10)
                {
                    ma = ma + "00";
                }
                else if (so < 100)
                {
                    ma = ma + "0";
                }
                else if (so < 1000)
                {
                    ma = ma + "";
                }
                ma = ma + so.ToString();
            }
           
            
                btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            //Hien thi nut Luu de luu ban ghi moi la 2 dong duoc go tren 2 textbox
            btnLuu.Enabled = true;
            //Hien thi bo qua neu nguoi dung khong muon them moi ban ghi do nua
            btnHuy.Enabled = true;
            //Sau khi Luu hoac Bo qua thi xoa trang du lieu de nguoi dung nhap lai
            Reset();
            txtMancc.Enabled = true;
            txtMancc.Focus();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (txtMancc.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập mã nhà cung cấp", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMancc.Focus();
                return;
            }
            if (txtTenncc.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tên nhà cung cấp", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenncc.Focus();
                return;
            }
            if (txtDiachi.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập địa chỉ nhà cung cấp", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDiachi.Focus();
                return;
            }
            if (txtsdt.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập sdt nhà cung cấp", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtsdt.Focus();
                return;
            }

            string sql = "SELECT Mancc FROM nhacungcap WHERE Mancc =N'" + txtMancc.Text.Trim() + "'";
            if (DAO.Checkey(sql))
            {
                MessageBox.Show("Mã nhà cung cấp này đã có, bạn phải nhập mã khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMancc.Focus();
                txtMancc.Text = "";
                return;
            }
            sql = "INSERT INTO Nhacungcap(MaNCC,TenNCC,Diachi, Dienthoai) VALUES(N'" + txtMancc.Text +
                "',N'" + txtTenncc.Text + "',N'" + txtDiachi.Text + "',N'" + txtsdt.Text + "')";
            DAO.RunSql(sql);
            LoadDatagridview();
            Reset();

            btnXoa.Enabled = true;
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnHuy.Enabled = false;
            btnLuu.Enabled = false;
            txtMancc.Enabled = false;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (tblncc.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu!", "Thông báo",
               MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtMancc.Text.Trim() == "")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào để xóa!", "Thông báo",
               MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa không", "Cảnh cáo",
                MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                string sql = "delete from nhacungcap where Mancc='" + txtMancc.Text
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

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (tblncc.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu", "Thông báo",
               MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtMancc.Text.Trim() == "")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào để sửa!", "Thông báo",
               MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtTenncc.Text.Trim() == "")
            {
                MessageBox.Show("Bạn chưa nhập tên nhà cung cấp!", "Thông báo",
               MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTenncc.Focus();
                return;
            }
            if (txtsdt.Text.Trim() == "")
            {
                MessageBox.Show("Bạn chưa số điện thoại nhà cung cấp!", "Thông báo",
               MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtsdt.Focus();
                return;
            }
            if (txtDiachi.Text.Trim() == "")
            {
                MessageBox.Show("Bạn chưa nhập địa chỉ nhà cung cấp!", "Thông báo",
               MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtDiachi.Focus();
                return;
            }
            string sql = "UPDATE nhacungcap SET tenncc=N'" + txtTenncc.Text.Trim() + "',diachi=N'"
                                                            + txtDiachi.Text.Trim() + "',dienthoai='"
                                                            + txtsdt.Text.Trim()
                                                             + "' where mancc=  N'"
                                                             + txtMancc.Text.Trim() + "'";
                                                            //+ txtMancc.SelectedText + "'";
            DAO.RunSql(sql);
            LoadDatagridview();
            Reset();
            btnHuy.Enabled = false;


        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            Reset();
            btnHuy.Enabled = false;
            btnThem.Enabled = true;
            btnXoa.Enabled = true;
            btnSua.Enabled = true;
            btnLuu.Enabled = false;
            txtMancc.Enabled = false;

        }

        private void btnTimkiem_Click(object sender, EventArgs e)
        {
            string sql;
            if ((txtMancc.Text == "") && (txtTenncc.Text == "") && (txtDiachi.Text == "")&&(txtDiachi.Text ==""))
                {
                MessageBox.Show("Hãy nhập một điều kiện tìm kiếm!!!", "Yêu cầu ...",
MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            sql = "SELECT * FROM nhacungcap WHERE 1=1";
            if (txtMancc.Text != "")
                sql = sql + " AND Mancc Like N'%" + txtMancc.Text + "%'";
            if (txtTenncc.Text != "")
                sql = sql + " AND Tenhang Like N'%" + txtTenncc.Text + "%'";
            if (txtDiachi.Text != "")
                sql = sql + " AND diachi Like N'%" + txtDiachi + "%'";
            if (txtsdt.Text != "")
                sql = sql + " AND sdt Like N'%" + txtDiachi + "%'";
            tblncc = DAO.LoadDataToTable(sql);
            if (tblncc.Rows.Count == 0)
                MessageBox.Show("Không có bản ghi thỏa mãn điều kiện!!!", "Thông báo",
MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
                MessageBox.Show("Có " + tblncc.Rows.Count + " bản ghi thỏa mãn điều kiện!!!",
"Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            dgvNCC.DataSource = tblncc; Reset();

        }

        private void txtsdt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar >= '0') && (e.KeyChar <= '9')) || (Convert.ToInt32(e.KeyChar) == 8))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }

        }
    }
}
