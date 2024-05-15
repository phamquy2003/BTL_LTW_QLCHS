using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace BTL_LTW_QLCHS_G6
{
    public partial class FormQLKH : Form
    {
        public FormQLKH()
        {
            InitializeComponent();
        }

        string connectionstr = @"Data Source=LAPTOP-D4HEMV8S\MSSQLSERVER02;Initial Catalog=BTL_QLCHS;Integrated Security=True";
        SqlConnection conn = null;
        SqlDataAdapter daKH;
        DataTable dtKH;
        int dong;

        private void bt_Trove_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormQLKH_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(connectionstr);
            conn.Open();
            string sqlKH = "Select * from KhachHang";
            daKH = new SqlDataAdapter(sqlKH, conn);
            dtKH = new DataTable();
            daKH.Fill(dtKH);
            dgv_QLKH.DataSource = dtKH;
        }

        private void bt_Them_Click(object sender, EventArgs e)
        {
            if(tb_MaKH.Text == "" || tb_TenKH.Text == "" || tb_SDT.Text == "")
            {
                MessageBox.Show("Chưa nhập đủ thông tin khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    string kh_Insert = "Insert into KhachHang values('" + tb_MaKH.Text + "', N'" + tb_TenKH.Text + "', '" + tb_SDT.Text + "')";
                    SqlCommand cmdKH = new SqlCommand(kh_Insert, conn);
                    cmdKH.ExecuteNonQuery();
                    dtKH.Rows.Clear();
                    daKH.Fill(dtKH);
                    tb_MaKH.Text = "";
                    tb_TenKH.Text = "";
                    tb_SDT.Text = "";
                    tb_MaKH.Focus();
                    MessageBox.Show("Thêm thông tin khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (SqlException)
                {
                    MessageBox.Show("Mã khách hàng đã tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void bt_Sua_Click(object sender, EventArgs e)
        {
            if (tb_MaKH.Text == "" || tb_TenKH.Text == "" || tb_SDT.Text == "")
            {
                MessageBox.Show("Chưa nhập đủ thông tin khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                dong = dgv_QLKH.CurrentRow.Index;
                string maKH = dgv_QLKH.Rows[dong].Cells[0].Value.ToString();
                string sql = "Select MaKH from KhachHang";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                bool check = false;
                foreach (DataRow x in dt.Rows)
                    if (x[0].ToString() == tb_MaKH.Text)
                    {
                        check = true;
                        break;
                    }
                if (check)
                {
                    MessageBox.Show("Mã khách hàng đã tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    try
                    {
                        string kh_Update = "Update KhachHang set MaKH = '" + tb_MaKH.Text + "', TenKH = N'" + tb_TenKH.Text + "', SDT = '" + tb_SDT.Text + "' where MaKH = '" + maKH + "'";
                        SqlCommand cmdKH = new SqlCommand(kh_Update, conn);
                        cmdKH.ExecuteNonQuery();
                        dtKH.Rows.Clear();
                        daKH.Fill(dtKH);
                        tb_MaKH.Text = "";
                        tb_TenKH.Text = "";
                        tb_SDT.Text = "";
                        tb_MaKH.Focus();
                        MessageBox.Show("Sửa thông tin khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (SqlException)
                    {
                        MessageBox.Show("Không thể sửa mã khách hàng vì tồn tại hoá đơn có mã khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void bt_Xoa_Click(object sender, EventArgs e)
        {
            try
            {
                dong = dgv_QLKH.CurrentRow.Index;
                string maKH = dgv_QLKH.Rows[dong].Cells[0].Value.ToString();
                string kh_Delete = "Delete from KhachHang where MaKH = '" + maKH + "'";
                SqlCommand cmdKH = new SqlCommand(kh_Delete, conn);
                cmdKH.ExecuteNonQuery();
                dtKH.Rows.Clear();
                daKH.Fill(dtKH);
                tb_MaKH.Text = "";
                tb_TenKH.Text = "";
                tb_SDT.Text = "";
                tb_MaKH.Focus();
                MessageBox.Show("Xoá thông tin khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SqlException)
            {
                MessageBox.Show("Không thể xoá thông tin khách hàng vì tồn tại hoá đơn có mã khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgv_QLKH_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dong = dgv_QLKH.CurrentRow.Index;
            tb_MaKH.Text = dgv_QLKH.Rows[dong].Cells[0].Value.ToString();
            tb_TenKH.Text = dgv_QLKH.Rows[dong].Cells[1].Value.ToString();
            tb_SDT.Text = dgv_QLKH.Rows[dong].Cells[2].Value.ToString();
        }
    }
}
