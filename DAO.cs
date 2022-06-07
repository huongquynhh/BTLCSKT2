using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;

namespace Quanly_VLXD
{
    internal class DAO
    {

        private static string ConnectionString = " Data Source = DESKTOP-9PGQD6K\\SQLEXPRESS;" +
                                   "Initial Catalog=QlyVLXD;" +
                                   "Integrated Security=True";

        static string[] mNumText = "không;một;hai;ba;bốn;năm;sáu;bảy;tám;chín".Split(';');


        public static SqlConnection con = new SqlConnection(ConnectionString);
        public static void Connect()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                    MessageBox.Show("Ket noi csdl thanh cong");
                }
            }
            catch (Exception ex)
            {
                // throw ex;
                MessageBox.Show(ex.ToString());
            }
        }
        public static void Close()
        {
            try
            {
                if (con.State == ConnectionState.Open)
                    con.Close();

            }
            catch (Exception ex)
            {
                throw ex;

            }

        }
        public static DataTable LoadDataToTable(string sql) //GetDataToTable
        {
            SqlDataAdapter mydata = new SqlDataAdapter(sql, con);
            //khai báo
            mydata.SelectCommand = new SqlCommand();
            mydata.SelectCommand.Connection = con; 	// Kết nối CSDL
            mydata.SelectCommand.CommandText = sql;
            DataTable table = new DataTable();//khai báo data table nhận dữ liệu trả về

            mydata.Fill(table);// select và đổ dữ liệu vào bảng
            return table;
        }
        public static bool Checkey(string sql) /// check khóa chính
        {
            /* SqlCommand myconmmand = new SqlCommand(sql, con);
             SqlDataReader myreader =  myconmmand.ExecuteReader();
             if (myreader.HasRows)
                 return true;
             else return false;*/
            SqlDataAdapter Mydata = new SqlDataAdapter(sql, con);
            DataTable table = new DataTable();
            Mydata.Fill(table);
            if (table.Rows.Count > 0)
                return true;
            else
                return false;


        }
        //RunSql có tác dụng thực thi các câu lệnh SQL.

        public static void RunSql(string sql)   // chạy lệnh
        {
            SqlCommand cmd;		                // Khai báo đối tượng SqlCommand
            cmd = new SqlCommand();	         // Khởi tạo đối tượng
            cmd.Connection = con;	  // Gán kết nối
            cmd.CommandText = sql;			  // Gán câu lệnh SQL
            try
            {
                cmd.ExecuteNonQuery();		  // Thực hiện câu lệnh SQL update
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            cmd.Dispose(); //Giai phong
            cmd = null;
        }
        // đổ dữ liệu vào combotext


        public static void Filldatocombo(string sql, ComboBox cmb, string ma,string ten)// dodulieuvaocombo
        {
            SqlDataAdapter myadapter = new SqlDataAdapter(sql, con);
            DataTable dataTable = new DataTable();
            myadapter.Fill(dataTable);
            cmb.DataSource = dataTable;
            cmb.ValueMember = ma;// trường giá trị
            cmb.DisplayMember = ten;// trường hiển thị

        }
        static public string laydulieucombo(string sql)
        {
            string ma = "";
            SqlCommand cmd = new SqlCommand(sql, DAO.con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ma = reader.GetValue(0).ToString();
            }
            reader.Close();
            return ma;
        }
        private static string DocHangTrieu(double so, bool daydu)
        {
            string chuoi = "";
            //Lấy số hàng triệu
            Int64 trieu = Convert.ToInt64(Math.Floor((double)so / 1000000));
            //Lấy phần dư sau số hàng triệu ví dụ 2,123,000 => so = 123,000
            so = so % 1000000;
            if (trieu > 0)
            {
                chuoi = DocHangTram(trieu, daydu) + " triệu";
                daydu = true;
            }
            //Lấy số hàng nghìn
            Int64 nghin = Convert.ToInt64(Math.Floor((double)so / 1000));
            //Lấy phần dư sau số hàng nghin 
            so = so % 1000;
            if (nghin > 0)
            {
                chuoi += DocHangTram(nghin, daydu) + " nghìn";
                daydu = true;
            }
            if (so > 0)
            {
                chuoi += DocHangTram(so, daydu);
            }
            return chuoi;
        }
        private static string DocHangTram(double so, bool daydu)
        {
            string chuoi = "";
            //Lấy số hàng trăm ví du 434 / 100 = 4 (hàm Floor sẽ làm tròn số nguyên bé nhất)
            Int64 tram = Convert.ToInt64(Math.Floor((double)so / 100));
            //Lấy phần còn lại của hàng trăm 434 % 100 = 34 (dư 34)
            so = so % 100;
            if (daydu || tram > 0)
            {
                chuoi = " " + mNumText[tram] + " trăm";
                chuoi += DocHangChuc(so, true);
            }
            else
            {
                chuoi = DocHangChuc(so, false);
            }
            return chuoi;
        }

        private static string DocHangChuc(double so, bool daydu)
        {
            string chuoi = "";
            //Hàm để lấy số hàng chục ví dụ 21/10 = 2
            Int64 chuc = Convert.ToInt64(Math.Floor((double)(so / 10)));
            //Lấy số hàng đơn vị bằng phép chia 21 % 10 = 1
            Int64 donvi = (Int64)so % 10;
            //Nếu số hàng chục tồn tại tức >=20
            if (chuc > 1)
            {
                chuoi = " " + mNumText[chuc] + " mươi";
                if (donvi == 1)
                {
                    chuoi += " mốt";
                }
            }
            else if (chuc == 1)
            {//Số hàng chục từ 10-19
                chuoi = " mười";
                if (donvi == 1)
                {
                    chuoi += " một";
                }
            }
            else if (daydu && donvi > 0)
            {//Nếu hàng đơn vị khác 0 và có các số hàng trăm ví dụ 101 => thì biến daydu = true => và sẽ đọc một trăm lẻ một
                chuoi = " lẻ";
            }
            if (donvi == 5 && chuc >= 1)
            {//Nếu đơn vị là số 5 và có hàng chục thì chuỗi sẽ là " lăm" chứ không phải là " năm"
                chuoi += " lăm";
            }
            else if (donvi > 1 || (donvi == 1 && chuc == 0))
            {
                chuoi += " " + mNumText[donvi];
            }
            return chuoi;
        }

            public static string ChuyenSoSangChuoi(double so)
        {
            if (so == 0)
                return mNumText[0];
            string chuoi = "", hauto = "";
            Int64 ty;
            do
            {
                //Lấy số hàng tỷ
                ty = Convert.ToInt64(Math.Floor((double)so / 1000000000));
                //Lấy phần dư sau số hàng tỷ
                so = so % 1000000000;
                if (ty > 0)
                {
                    chuoi = DocHangTrieu(so, true) + hauto + chuoi;
                }
                else
                {
                    chuoi = DocHangTrieu(so, false) + hauto + chuoi;
                }
                hauto = " tỷ";
            } while (ty > 0);
            return chuoi + " đồng";
        }

        static public void chaylenhdelete(string sql)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.ExecuteNonQuery();
                // MessageBox.Show("Thao tác thành công!", "Thông báo", MessageBoxButtons.OK);
            }
            catch (Exception)
            {
                MessageBox.Show("Dữ liệu đang được dùng không thể xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

    }
}


