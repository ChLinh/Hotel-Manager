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
    public partial class QuanLyKH : Form
    {
        public QuanLyKH()
        {
            InitializeComponent();
        }
        internal class Connection
        {
            private static string exactly_server_name = "Server=DESKTOP-SCBOCHA;Database=QLKhachSan;Integrated Security=True;";
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
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void HienThiDSKH()
        {
            dataGridView1.ClearSelection();
            Connection.Connect();
            string sql = "Select CMND,HoTenKH,DiaChiKH,EmailKH,NgaySinhKH,SdtKH,SoFAX,MaDoan from KhachHang";
            dataGridView2.DataSource = Connection.GetDataToTable(sql);

            dataGridView2.EditMode = DataGridViewEditMode.EditProgrammatically;
            Connection.Disconnect();
        }


        private void button4_Click(object sender, EventArgs e)
        {
            HienThiDSKH();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Connection.Connect();
            string sql = "Select CMND,HoTenKH,DiaChiKH,EmailKH,NgaySinhKH,SdtKH,SoFAX,MaDoan from KhachHang where CMND = '" + textBox1.Text + "'";
            dataGridView2.DataSource = Connection.GetDataToTable(sql);
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Connection.Connect();
            string Doan = "null";

            string MaDoan = "Select MaDoan from Doan where MaDoan = '" + textBox10.Text + "'";
            string bufferDoan = Connection.GetFieldValues(MaDoan).Trim();
            string CMND = "Select CMND from KhachHang where CMND = '" + textBox2.Text + "'";
            string bufferCMND = Connection.GetFieldValues(CMND).Trim();
            if (textBox2.Text == "")
            {
                MessageBox.Show("Vui lòng nhập CMND");
            }
            else if (textBox2.Text.Length != 10)
            {
                MessageBox.Show("Mã CMND không hợp lệ !!!");
            }
            else if (bufferCMND != "")
            {
                MessageBox.Show("Mã CMND đã tồn tại !!!");
            }
            else if (bufferDoan == "" && textBox10.Text != "")
            {
                MessageBox.Show("Mã Đoàn không tồn tại !!!");
            }
            else
            {
                Doan = bufferDoan;
                if (Doan == "")
                {
                    string sql = "insert into KhachHang(CMND,HoTenKH,DiaChiKH,EmailKH,NgaySinhKH,SdtKH,SoFAX) values('" + textBox2.Text + "', '" + textBox4.Text + "', '" + textBox5.Text + "', '" + textBox6.Text + "', '" + textBox7.Text + "', '" + textBox8.Text + "', '" + textBox9.Text + "')";
                    Connection.RunSQL(sql);
                    MessageBox.Show("Thêm khách hàng thành công");
                }
                else
                {
                    string sql = "insert into KhachHang values('" + textBox2.Text + "', '" + textBox4.Text + "', '" + textBox5.Text + "', '" + textBox6.Text + "', '" + textBox7.Text + "', '" + textBox8.Text + "', '" + textBox9.Text + "','" + Doan + "')";
                    Connection.RunSQL(sql);
                    MessageBox.Show("Thêm khách hàng thành công");
                }
            }
        }
        private object Length(string buffer)
        {
            throw new NotImplementedException();
        }
        private void NgaySinh_TextChanged(object sender, EventArgs e)
        {
        }
        private void button3_Click(object sender, EventArgs e)
        {
            ChinhSuaThongTin cs = new ChinhSuaThongTin();
            cs.Show();
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
        private void listView1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }
    }
}
