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
    public partial class FormQLTG : Form
    {
        public FormQLTG()
        {
            InitializeComponent();
        }

        string connectionstring = @"Data Source=LAPTOP-D4HEMV8S\MSSQLSERVER02;Initial Catalog=BTL_QLCHS;Integrated Security=True";
        SqlConnection conn = null;
        SqlDataAdapter daTG;
        DataTable dtTG;
        int dong;

        private void bt_Trove_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormQLTG_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(connectionstring);
            conn.Open();
            string sqlTG = "Select * from TacGia";
            daTG = new SqlDataAdapter(sqlTG, conn);
            dtTG = new DataTable();
            daTG.Fill(dtTG);
            dgv_QLTG.DataSource = dtTG;
        }

        private void bt_Them_Click(object sender, EventArgs e)
        {
            if (tb_MaTG.Text == "" || tb_TenTG.Text == "")
            {
                MessageBox.Show("Chưa nhập đủ thông tin tác giả!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    string tg_Insert = "Insert into TacGia values('" + tb_MaTG.Text + "', N'" + tb_TenTG.Text + "')";
                    SqlCommand cmdTG = new SqlCommand(tg_Insert, conn);
                    cmdTG.ExecuteNonQuery();
                    dtTG.Rows.Clear();
                    daTG.Fill(dtTG);
                    tb_MaTG.Text = "";
                    tb_TenTG.Text = "";
                    tb_MaTG.Focus();
                    MessageBox.Show("Thêm thông tin tác giả thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (SqlException)
                {
                    MessageBox.Show("Mã tác giả đã tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void bt_Sua_Click(object sender, EventArgs e)
        {
            if (tb_MaTG.Text == "" || tb_TenTG.Text == "")
            {
                MessageBox.Show("Chưa nhập đủ thông tin tác giả!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                dong = dgv_QLTG.CurrentRow.Index;
                string maTG = dgv_QLTG.Rows[dong].Cells[0].Value.ToString();
                string sql = "Select MaTG from TacGia";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                bool check = false;
                foreach (DataRow x in dt.Rows)
                    if (x[0].ToString() == tb_MaTG.Text)
                    {
                        check = true;
                        break;
                    }
                if (check)
                {
                    MessageBox.Show("Mã tác giả đã tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    try
                    {
                        string tg_Update = "Update TacGia set MaTG = '" + tb_MaTG.Text + "', TenTG = N'" + tb_TenTG.Text + "' where MaTG = '" + maTG + "'";
                        SqlCommand cmdTG = new SqlCommand(tg_Update, conn);
                        cmdTG.ExecuteNonQuery();
                        dtTG.Rows.Clear();
                        daTG.Fill(dtTG);
                        tb_MaTG.Text = "";
                        tb_TenTG.Text = "";
                        tb_MaTG.Focus();
                        MessageBox.Show("Sửa thông tin tác giả thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (SqlException)
                    {
                        MessageBox.Show("Không thể sửa thông tin tác giả vì tồn tại sách có mã tác giả trong kho!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void bt_Xoa_Click(object sender, EventArgs e)
        {
            try
            {
                dong = dgv_QLTG.CurrentRow.Index;
                string maTG = dgv_QLTG.Rows[dong].Cells[0].Value.ToString();
                string tg_Delete = "Delete from TacGia where MaTG = '" + maTG + "'";
                SqlCommand cmdTG = new SqlCommand(tg_Delete, conn);
                cmdTG.ExecuteNonQuery();
                dtTG.Rows.Clear();
                daTG.Fill(dtTG);
                tb_MaTG.Text = "";
                tb_TenTG.Text = "";
                tb_MaTG.Focus();
                MessageBox.Show("Xoá thông tin tác giả thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SqlException)
            {
                MessageBox.Show("Không thể xoá thông tin tác giả vì tồn tại sách có mã tác giả trong kho!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgv_QLTG_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dong = dgv_QLTG.CurrentRow.Index;
            tb_MaTG.Text = dgv_QLTG.Rows[dong].Cells[0].Value.ToString();
            tb_TenTG.Text = dgv_QLTG.Rows[dong].Cells[1].Value.ToString();
        }
    }
}
