using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace WindowsFormsApp1
{
    public partial class QuanLyPhong : Form
    {
        public QuanLyPhong()
        {
            InitializeComponent();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView2_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Connection.Connect();

            String cmnd = textBox1.Text;
            String cmnd_check = "SELECT COUNT(*) FROM KhachHang WHERE CMND = '" + cmnd +"'";
            SqlCommand c1 = new SqlCommand(cmnd_check, Connection.Con);
            int count = (int)c1.ExecuteScalar();
            if (count == 0)
            {
                MessageBox.Show("CMND của khách hàng không tồn tại.");
                textBox1.Text = "";
                textBox2.Text = "";
                return;
            }

            String phong = textBox2.Text;
            String phong_check = "SELECT COUNT(*) FROM Phong WHERE MaPhong = '" + phong + "'";
            SqlCommand c2 = new SqlCommand(cmnd_check, Connection.Con);
            count = (int)c2.ExecuteScalar();
            if (count == 0)
            {
                MessageBox.Show("Phòng không tồn tại.");
                textBox1.Text = "";
                textBox2.Text = "";
                return;
            }

            DateTime selectedDateTime = dateTimePicker1.Value;
            String sqlFormattedTime = selectedDateTime.ToString("yyyy-MM-dd");
            String query = "INSERT INTO KhachDat(CMND,MaPhongDat,KhungGio) VALUES (@cmnd, @phong, @time);";
            // Create a SqlCommand object with the SQL statement and connection
            SqlCommand cmd = new SqlCommand(query, Connection.Con);

            // Add the parameters and their values
            cmd.Parameters.AddWithValue("@cmnd", cmnd);
            cmd.Parameters.AddWithValue("@phong", phong);
            cmd.Parameters.AddWithValue("@time", sqlFormattedTime);

            // Execute the command
            cmd.ExecuteNonQuery();
            textBox1.Text = "";
            textBox2.Text = "";
            Connection.Con.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Connection.Connect();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Phong", Connection.Con);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable table = new DataTable();
            adapter.Fill(table);

            listView2.Items.Clear();
            foreach (DataRow row in table.Rows)
            {
                ListViewItem item = new ListViewItem(row["MaPhong"].ToString());
                item.SubItems.Add(row["LoaiPhong"].ToString());
                item.SubItems.Add(row["SoLuongNguoi"].ToString());
                item.SubItems.Add(row["GiaPhong"].ToString());
                item.SubItems.Add(row["TrangThaiVS"].ToString());
                listView2.Items.Add(item);
            }
            listView2.FullRowSelect = true;
            Connection.Con.Close();
        }

        class ListViewItemComparer : IComparer
        {
            private int column;

            public ListViewItemComparer(int column)
            {
                this.column = column;
            }

            public int Compare(object x, object y)
            {
                DateTime dateX, dateY;
                ListViewItem itemX = (ListViewItem)x;
                ListViewItem itemY = (ListViewItem)y;

                if (DateTime.TryParse(itemX.SubItems[column].Text, out dateX) && DateTime.TryParse(itemY.SubItems[column].Text, out dateY))
                {
                    return DateTime.Compare(dateX, dateY);
                }
                else
                {
                    return String.Compare(itemX.SubItems[column].Text, itemY.SubItems[column].Text);
                }
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            Connection.Con.Open();
            if (listView2.SelectedItems.Count > 0)
            {
                string selectedData = listView2.SelectedItems[0].SubItems[0].Text;
                String query = "SELECT * FROM KhachDat Where MaPhongDat = '" + selectedData + "'";
                SqlCommand cmd = new SqlCommand(query, Connection.Con);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable table = new DataTable();
                adapter.Fill(table);
                listView1.Items.Clear();
                foreach (DataRow row in table.Rows)
                {
                    String KH_cmnd = row["CMND"].ToString();
                    String time = row["KhungGio"].ToString();
                    String query1 = "SELECT * FROM KhachHang Where CMND = '" + KH_cmnd + "'";
                    SqlCommand cmd1 = new SqlCommand(query1, Connection.Con);
                    SqlDataAdapter adapter1 = new SqlDataAdapter(cmd1);
                    DataTable table1 = new DataTable();
                    adapter1.Fill(table1);
                    
                    foreach (DataRow innerrow in table1.Rows)
                    {
                        ListViewItem item = new ListViewItem(innerrow["HoTenKH"].ToString());
                        item.SubItems.Add(innerrow["SdtKH"].ToString());
                        item.SubItems.Add(innerrow["EmailKH"].ToString());
                        item.SubItems.Add(time);
                        item.SubItems.Add(KH_cmnd);
                        listView1.Items.Add(item);
                    }
                }
                ListViewItemComparer comparer = new ListViewItemComparer(3);
                listView1.ListViewItemSorter = comparer;
                listView1.Sort();
                listView1.FullRowSelect = true;
            }
            Connection.Con.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                string selectedID = listView1.SelectedItems[0].SubItems[4].Text; 
                string query = "DELETE FROM KhachDat WHERE CMND = @cmnd";
                SqlCommand cmd = new SqlCommand(query, Connection.Con);
                cmd.Parameters.AddWithValue("@cmnd", selectedID);

                try
                {
                    Connection.Con.Open();
                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Đã xóa khách hàng đặt phòng.");
                        // remove the selected item from the listView
                        listView1.SelectedItems[0].Remove();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    Connection.Con.Close();
                }
            }
        }
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

            //Kiểm tra kết nối
            if (Con.State == ConnectionState.Open)
            {

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
                //MessageBox.Show("Đóng Kết nối DB thành công");
            }
        }

    }
   
}
