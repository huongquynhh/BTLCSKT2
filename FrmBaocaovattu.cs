using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using COMExcel = Microsoft.Office.Interop.Excel;

namespace Quanly_VLXD
{
    public partial class FrmBaocaovattu : Form
    {
        public FrmBaocaovattu()
        {
            InitializeComponent();
        }
        DataTable tblkho;
        private void FrmBaocaovattu_Load(object sender, EventArgs e)
        {
            DAO.Connect();
            btnKiemtra.Enabled = true;
            btnXuat.Enabled = false;
            DAO.Filldatocombo("SELECT makho, tenkho FROM tblkhohang", cboKhohang, "makho","tenkho");
            cboKhohang.SelectedIndex = -1;
            btnXuat.Enabled = false;
            btnTimlai.Enabled = false;


        }
        private void ResetValues()
        {

            cboKhohang.Text = "";

        }

        private void btnKiemtra_Click(object sender, EventArgs e)
        {
            string sql;
            sql = " SELECT chitietkhohang.mavattu, tenvattu, tendonvitinh, soluong ,gianhap, giaxuat, mancc " +
                "FROM(chitietkhohang join vattu on chitietkhohang.mavattu = vattu.mavattu) " +
                "join donvitinh on vattu.madonvitinh = donvitinh.madonvitinh Where 1 = 1 AND makho =  '" + cboKhohang.SelectedValue + "' ";


            tblkho = DAO.LoadDataToTable(sql);
            if (cboKhohang.Text == "")
            {
                MessageBox.Show("Hãy Chọn Kho Hàng", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboKhohang.Focus();
                return;
            }
            if (cboKhohang.Text != "")
            {
                dgchitietkho.DataSource = tblkho;
                btnXuat.Enabled = true;

            }
            else if (tblkho.Rows.Count == 0)
            {
                MessageBox.Show("Không có bản ghi thỏa mãn điều kiện!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            btnXuat.Enabled = true;
            btnTimlai.Enabled = true;
            btnThoat.Enabled = true;


        }

        private void btnTimlai_Click(object sender, EventArgs e)
        {
            ResetValues();
            dgchitietkho.DataSource = null;
            btnXuat.Enabled = false;
        }

        private void btnXuat_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("\tVui lòng chờ...\n Đang cập nhật dữ liệu");
            // Khởi động chương trình Excel
            COMExcel.Application exApp = new COMExcel.Application();
            COMExcel.Workbook exBook; //Trong 1 chương trình Excel có nhiều Workbook
            COMExcel.Worksheet exSheet; //Trong 1 Workbook có nhiều Worksheet
            COMExcel.Range exRange;
            string sql;
            int hang = 0, cot = 0;
            DataTable tblkho;
            exBook = exApp.Workbooks.Add(COMExcel.XlWBATemplate.xlWBATWorksheet);
            exSheet = exBook.Worksheets[1];
            // Định dạng chung
            exRange = exSheet.Cells[1, 1];
            exRange.Range["A1:B3"].Font.Size = 13;
            exRange.Range["A1:B3"].Font.Name = "Times new roman";
            exRange.Range["A1:B3"].Font.Bold = true;
            exRange.Range["A1:B3"].Font.ColorIndex = 5; //Màu xanh da trời
            exRange.Range["A1:A1"].ColumnWidth = 15;
            exRange.Range["B1:B1"].ColumnWidth = 20;

            exRange.Range["A1:B1"].MergeCells = true;
            exRange.Range["A1:B1"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["A1:B1"].Value = "Công Ty Vật Liệu Xây Dựng";

            exRange.Range["A2:B2"].MergeCells = true;
            exRange.Range["A2:B2"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["A2:B2"].Value = "Đống Đa - Hà Nội";

            exRange.Range["A3:B3"].MergeCells = true;
            exRange.Range["A3:B3"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["A3:B3"].Value = "Điện thoại: (08)9999-9999";

            exRange.Range["D6:G6"].Font.Size = 15;
            exRange.Range["D6:G6"].Font.Name = "Times new roman";
            exRange.Range["D6:G6"].Font.Bold = true;
            exRange.Range["D6:G6"].Font.ColorIndex = 3; //Màu đỏ
            exRange.Range["D6:G6"].MergeCells = true;
            exRange.Range["D6:G6"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            string tenkho = DAO.laydulieucombo("Select tenkho from tblkhohang where makho = '" + cboKhohang.SelectedValue + "'");
            exRange.Range["D6:F6"].Value = "Bảng báo cáo vật tư " + tenkho;

            sql = " SELECT tblchitietkhohang.mavattu, tenvattu, tendonvitinh, soluong ,gianhap, giaxuat, mancc " +
                           "FROM(tblchitietkhohang join tblvattu on tblchitietkhohang.mavattu = tblvattu.mavattu) " +
                           "join tbldonvitinh on tblvattu.madonvitinh = tbldonvitinh.madonvitinh Where 1 = 1 AND makho =  '" + cboKhohang.SelectedValue + "' ";
            tblkho = DAO.LoadDataToTable(sql);
            //Tạo dòng tiêu đề bảng
            exRange.Range["C9:J9"].Font.Bold = true;
            exRange.Range["C9:J9"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["C9:J9"].ColumnWidth = 14;
            exRange.Range["C8:C8"].ColumnWidth = 7;
            exRange.Range["C9:C9"].Value = "STT";
            exRange.Range["D9:D9"].Value = "Mã Vật Tư";
            exRange.Range["E9:E9"].Value = "Tên Vật Tư";
            exRange.Range["F9:F9"].Value = "Đơn Vị Tính";
            exRange.Range["G9:G9"].Value = "Số lượng";
            exRange.Range["H9:H9"].Value = "Giá nhập";
            exRange.Range["I9:I9"].Value = "Giá xuất";
            exRange.Range["J9:J9"].Value = "Mã NCC";
            for (hang = 0; hang < tblkho.Rows.Count; hang++)
            {
                exSheet.Cells[3][hang + 10] = hang + 1;//điền số thứ tự vào cột 2 bắt đầu từ hàng 10 (mở excel ra hình dung)
                for (cot = 0; cot < tblkho.Columns.Count; cot++)
                {
                    exSheet.Cells[cot + 4][hang + 10] = tblkho.Rows[hang][cot].ToString();
                }

            }
            //int cott, hangg;
            //cott = cot + 3;
            //hangg = hang + 10;
            //exRange.Range["C"+ cott +":J" + hangg].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            //exRange.Range["C9:J9"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            //exRange.Range["C10:J"+hang].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;

            exRange = exSheet.Cells[1][hang + 12];//chỗ này là đánh dấu vị trí viết cái dòng "Hà Nội, ngày..."
            exRange.Range["G1:I1"].MergeCells = true;
            exRange.Range["G1:I1"].Font.Italic = true;
            exRange.Range["G1:I1"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["G1:I1"].Value = "Hà Nội, Ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year;
            exRange.Range["G2:I2"].MergeCells = true;
            exRange.Range["G2:I2"].Font.Italic = true;
            exRange.Range["G2:I2"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["G2:I2"].Value = "Người tạo báo cáo";
            exRange.Range["G2:I2"].Font.Bold = true;
            exSheet.Name = "Báo cáo nhập hàng";
            exApp.Visible = true;
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
    }
}
