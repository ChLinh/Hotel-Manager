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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class QuanLyDatPhong : Form
    {
        public QuanLyDatPhong()
        {
            InitializeComponent();
            layDatPhong();
        }
        private void layDatPhong()
        {
            Connection.Connect();
            SqlCommand cmd = new SqlCommand("SELECT * FROM DatPhong", Connection.Con);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable table = new DataTable();
            adapter.Fill(table);

            listView1.Items.Clear();
            foreach (DataRow row in table.Rows)
            {
                ListViewItem item = new ListViewItem(row["CMND"].ToString());
                item.SubItems.Add(row["MaPhong"].ToString());
                item.SubItems.Add(row["SoLuongNguoi"].ToString());
                item.SubItems.Add(row["SoDemLuuTru"].ToString());
                item.SubItems.Add(row["NgayDP"].ToString());
                item.SubItems.Add(row["TienCoc"].ToString());
                listView1.Items.Add(item);
            }
            listView1.FullRowSelect = true;
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            QuanLyPhong phong = new QuanLyPhong();
            if (phong.ShowDialog() == DialogResult.OK)
            {
                // Update the selected user's data in the DataGridView
                //selectedRow.Cells["UserName"].Value = userEditForm.UserName;
                // ...
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                string selectedID = listView1.SelectedItems[0].SubItems[0].Text;
                string query = "DELETE FROM DatPhong WHERE CMND = @cmnd";
                SqlCommand cmd = new SqlCommand(query, Connection.Con);
                cmd.Parameters.AddWithValue("@cmnd", selectedID);

                try
                {
                    Connection.Con.Open();
                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Đã xóa đặt phòng.");
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

        private void listView1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

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
}
