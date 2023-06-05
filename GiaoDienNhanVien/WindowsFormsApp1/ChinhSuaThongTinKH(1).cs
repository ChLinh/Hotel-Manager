using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QuanLyKhachHang
{

    public partial class ChinhSuaThongTin : Form
    {
        public string Doan = "null";
        public ChinhSuaThongTin()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
        internal class Connection
        {
            private static string exactly_server_name = @"Data Source=LAPTOP-CGEPSB65;Initial Catalog=QLKhachSan;Integrated Security=True";
            public static SqlConnection Con;
            public static void Connect()
            {
                Con = new SqlConnection();
                Con.ConnectionString = exactly_server_name;
                Con.Open();

                ////Kiểm tra kết nối
                if (Con.State == ConnectionState.Open)
                {
                    MessageBox.Show("Kết nối DB thành công");
                }
                else MessageBox.Show("Không thể kết nối với DB");
            }
            public static void Disconnect()
            {
                if (Con.State == ConnectionState.Open)
                {
                    //Đóng kết nối
                    Con.Close();

                    //Giải phóng tài nguyên
                    Con.Dispose();
                    Con = null;

                    //Kiểm tra kết nối
                    MessageBox.Show("Đóng Kết nối DB thành công");
                }
            }
            public static DataTable GetDataToTable(string sql) //Lấy dữ liệu đổ vào bảng
            {
                SqlDataAdapter dap = new SqlDataAdapter();
                dap.SelectCommand = new SqlCommand();

                //Kết nối cơ sở dữ liệu
                dap.SelectCommand.Connection = Connection.Con;
                dap.SelectCommand.CommandText = sql;

                DataTable table = new DataTable();
                dap.Fill(table);
                return table;
            }
            public static void RunSQL(string sql) // chạy câu lệnh sql
            {
                SqlCommand cmd = new SqlCommand();

                //Gán kết nối
                cmd.Connection = Con;

                //Gán lệnh SQL
                cmd.CommandText = sql;

                //Thực hiện câu lệnh SQL
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                //Giải phóng bộ nhớ
                cmd.Dispose();
                cmd = null;
            }
            public static string GetFieldValues(string sql) // lấy dữ liệu từ câu lệnh sql
            {
                string ma = "";
                SqlCommand cmd = new SqlCommand(sql, Con);
                SqlDataReader reader;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                    ma = reader.GetValue(0).ToString();
                reader.Close();
                return ma;
            }

        }
        private void button2_Click(object sender, EventArgs e)
        {
            Connection.Connect();
            string CMND = "Select CMND from KhachHang where CMND = '" + textBox2.Text + "'";
            string bufferCMND = Connection.GetFieldValues(CMND).Trim();
            if (bufferCMND != "")
            {
                MessageBox.Show("Mã CMND đã tồn tại !!!");
            }
            else
            {
                string sql1 = "Select CMND from KhachHang where CMND = '" + textBox9.Text + "'";
                string sql2 = "Select HoTenKH from KhachHang where CMND = '" + textBox9.Text + "'";
                string sql3 = "Select DiaChiKH from KhachHang where CMND = '" + textBox9.Text + "'";
                string sql4 = "Select EmailKH from KhachHang where CMND = '" + textBox9.Text + "'";
                string sql5 = "Select NgaySinhKH from KhachHang where CMND = '" + textBox9.Text + "'";
                string sql7 = "Select SdtKH from KhachHang where CMND = '" + textBox9.Text + "'";
                string sql6 = "Select SoFAX from KhachHang where CMND = '" + textBox9.Text + "'";
                string sql8 = "Select MaDoan from KhachHang where CMND = '" + textBox9.Text + "'";
                textBox1.Text = Connection.GetFieldValues(sql1).Trim();
                textBox2.Text = Connection.GetFieldValues(sql2).Trim();
                textBox3.Text = Connection.GetFieldValues(sql3).Trim();
                textBox4.Text = Connection.GetFieldValues(sql4).Trim();
                textBox5.Text = Connection.GetFieldValues(sql5).Trim();
                textBox6.Text = Connection.GetFieldValues(sql6).Trim();
                textBox7.Text = Connection.GetFieldValues(sql7).Trim();
                textBox8.Text = Connection.GetFieldValues(sql8).Trim();
                Doan = Connection.GetFieldValues(sql8).Trim();
                Connection.Disconnect();
            }

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Connection.Connect();
            MessageBox.Show(Doan);
            string MaDoan = "Select MaDoan from Doan where MaDoan = '" + textBox8.Text + "'";
            string bufferDoan = Connection.GetFieldValues(MaDoan).Trim();
            if (textBox1.Text != textBox9.Text)
            {
                MessageBox.Show("Không được thay đổi CMND");
                textBox1.Text = textBox9.Text;
            }
            else if (bufferDoan == "" && textBox8.Text != "")
            {
                MessageBox.Show("Mã Đoàn không tồn tại !!!");
                textBox8.Text = Doan;
            }
            else
            {
                string sql = "update KhachHang set HoTenKH='" + textBox2.Text + "', DiaChiKH='" + textBox3.Text + "', EmailKH='" + textBox4.Text + "', NgaySinhKH= '" + textBox5.Text + "', SdtKH='" + textBox7.Text + "',SoFAX= '" + textBox6.Text + "', MaDoan='" + textBox8.Text + "' where CMND = '" + textBox9.Text + "'";
                Connection.RunSQL(sql);
            }
        }
    }
}
