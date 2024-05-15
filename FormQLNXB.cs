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
    public partial class FormQLNXB : Form
    {
        public FormQLNXB()
        {
            InitializeComponent();
        }

        string connectionstring = @"Data Source=LAPTOP-D4HEMV8S\MSSQLSERVER02;Initial Catalog=BTL_QLCHS;Integrated Security=True";
        SqlConnection conn = null;
        SqlDataAdapter daNXB;
        DataTable dtNXB;
        int dong;

        private void bt_Trove_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormQLNXB_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(connectionstring);
            conn.Open();
            string sqlNXB = "Select * from NXB";
            daNXB = new SqlDataAdapter(sqlNXB, conn);
            dtNXB = new DataTable();
            daNXB.Fill(dtNXB);
            dgv_QLNXB.DataSource = dtNXB;
        }

        private void bt_Them_Click(object sender, EventArgs e)
        {
            if (tb_MaNXB.Text == "" || tb_TenNXB.Text == "" || tb_SDTNXB.Text == "" || tb_DiachiNXB.Text == "")
            {
                MessageBox.Show("Chưa nhập đủ thông tin nhà xuất bản!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    string nxb_Insert = "Insert into NXB values('" + tb_MaNXB.Text + "', N'" + tb_TenNXB.Text + "', N'" + tb_DiachiNXB.Text + "', '" + tb_SDTNXB.Text + "')";
                    SqlCommand cmdNXB = new SqlCommand(nxb_Insert, conn);
                    cmdNXB.ExecuteNonQuery();
                    dtNXB.Rows.Clear();
                    daNXB.Fill(dtNXB);
                    tb_MaNXB.Text = "";
                    tb_TenNXB.Text = "";
                    tb_DiachiNXB.Text = "";
                    tb_SDTNXB.Text = "";
                    tb_MaNXB.Focus();
                    MessageBox.Show("Thêm thông tin NXB thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (SqlException)
                {
                    MessageBox.Show("Mã NXB đã tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void bt_Sua_Click(object sender, EventArgs e)
        {
            if (tb_MaNXB.Text == "" || tb_TenNXB.Text == "" || tb_SDTNXB.Text == "" || tb_DiachiNXB.Text == "")
            {
                MessageBox.Show("Chưa nhập đủ thông tin NXB!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                dong = dgv_QLNXB.CurrentRow.Index;
                string maNXB = dgv_QLNXB.Rows[dong].Cells[0].Value.ToString();
                string sql = "Select MaNXB from NXB";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                bool check = false;
                foreach (DataRow x in dt.Rows)
                    if (x[0].ToString() == tb_MaNXB.Text)
                    {
                        check = true;
                        break;
                    }
                if (check)
                {
                    MessageBox.Show("Mã NXB đã tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    try
                    {
                        string nxb_Update = "Update NXB set MaNXB = '" + tb_MaNXB.Text + "', TenNXB = N'" + tb_TenNXB.Text + "', DiaChi = N'" + tb_DiachiNXB.Text + "', SDT = '" + tb_SDTNXB.Text + "' where MaNXB = '" + maNXB + "'";
                        SqlCommand cmdNXB = new SqlCommand(nxb_Update, conn);
                        cmdNXB.ExecuteNonQuery();
                        dtNXB.Rows.Clear();
                        daNXB.Fill(dtNXB);
                        tb_MaNXB.Text = "";
                        tb_TenNXB.Text = "";
                        tb_DiachiNXB.Text = "";
                        tb_SDTNXB.Text = "";
                        tb_MaNXB.Focus();
                        MessageBox.Show("Sửa thông tin NXB thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                    catch (SqlException)
                    {
                        MessageBox.Show("Không thể thay đổi mã NXB vì tồn tại sách trong kho có mã NXB này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void bt_Xoa_Click(object sender, EventArgs e)
        {
            try
            {
                dong = dgv_QLNXB.CurrentRow.Index;
                string maNXB = dgv_QLNXB.Rows[dong].Cells[0].Value.ToString();
                string nxb_Delete = "Delete from NXB where MaNXB = '" + maNXB + "'";
                SqlCommand cmdNXB = new SqlCommand(nxb_Delete, conn);
                cmdNXB.ExecuteNonQuery();
                dtNXB.Rows.Clear();
                daNXB.Fill(dtNXB);
                tb_MaNXB.Text = "";
                tb_TenNXB.Text = "";
                tb_DiachiNXB.Text = "";
                tb_SDTNXB.Text = "";
                tb_MaNXB.Focus();
                MessageBox.Show("Xoá thông tin NXB thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SqlException)
            {
                MessageBox.Show("Không thể xoá NXB vì tồn tại sách của NXB trong kho!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgv_QLNXB_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dong = dgv_QLNXB.CurrentRow.Index;
            tb_MaNXB.Text = dgv_QLNXB.Rows[dong].Cells[0].Value.ToString();
            tb_TenNXB.Text = dgv_QLNXB.Rows[dong].Cells[1].Value.ToString();
            tb_DiachiNXB.Text = dgv_QLNXB.Rows[dong].Cells[2].Value.ToString();
            tb_SDTNXB.Text = dgv_QLNXB.Rows[dong].Cells[3].Value.ToString();
        }
    }
}
