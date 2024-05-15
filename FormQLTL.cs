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
    public partial class FormQLTL : Form
    {
        public FormQLTL()
        {
            InitializeComponent();
        }

        string connectionstring = @"Data Source=LAPTOP-D4HEMV8S\MSSQLSERVER02;Initial Catalog=BTL_QLCHS;Integrated Security=True";
        SqlConnection conn = null;
        SqlDataAdapter daTL;
        DataTable dtTL;
        int dong;

        private void bt_Trove_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormQLTL_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(connectionstring);
            conn.Open();
            string sqlTL = "Select * from TheLoai";
            daTL = new SqlDataAdapter(sqlTL, conn);
            dtTL = new DataTable();
            daTL.Fill(dtTL);
            dgv_QLTL.DataSource = dtTL;
        }

        private void bt_Them_Click(object sender, EventArgs e)
        {
            if (tb_MaTL.Text == "" || tb_TenTL.Text == "")
            {
                MessageBox.Show("Chưa nhập đủ thông tin thể loại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    if (tb_Mota.Text == "")
                        tb_Mota.Text = "Không có";
                    string tl_Insert = "Insert into TheLoai values('" + tb_MaTL.Text + "', N'" + tb_TenTL.Text + "', N'" + tb_Mota.Text + "')";
                    SqlCommand cmdTL = new SqlCommand(tl_Insert, conn);
                    cmdTL.ExecuteNonQuery();
                    dtTL.Rows.Clear();
                    daTL.Fill(dtTL);
                    tb_MaTL.Text = "";
                    tb_TenTL.Text = "";
                    tb_Mota.Text = "";
                    tb_MaTL.Focus();
                    MessageBox.Show("Thêm thông tin thể loại thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (SqlException)
                {
                    MessageBox.Show("Mã thể loại đã tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void bt_Sua_Click(object sender, EventArgs e)
        {
            if(tb_MaTL.Text == "" || tb_TenTL.Text == "")
            {
                MessageBox.Show("Chưa nhập đủ thông tin thể loại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                dong = dgv_QLTL.CurrentRow.Index;
                string maTL = dgv_QLTL.Rows[dong].Cells[0].Value.ToString();
                string sql = "Select MaTL from TheLoai";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                bool check = false;
                foreach (DataRow x in dt.Rows)
                    if (x[0].ToString() == tb_MaTL.Text)
                    {
                        check = true;
                        break;
                    }
                if (check)
                {
                    MessageBox.Show("Mã thể loại đã tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    try
                    {
                        if (tb_Mota.Text == "")
                            tb_Mota.Text = "Không có";
                        string tl_Update = "Update TheLoai set MaTL = '" + tb_MaTL.Text + "', TenTL = N'" + tb_TenTL.Text + "', Mota = N'" + tb_Mota.Text + "' where MaTL = '" + maTL + "'";
                        SqlCommand cmdTL = new SqlCommand(tl_Update, conn);
                        cmdTL.ExecuteNonQuery();
                        dtTL.Rows.Clear();
                        daTL.Fill(dtTL);
                        tb_MaTL.Text = "";
                        tb_TenTL.Text = "";
                        tb_Mota.Text = "";
                        tb_MaTL.Focus();
                        MessageBox.Show("Sửa thông tin thể loại thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (SqlException)
                    {
                        MessageBox.Show("Không thể sửa thông tin thể loại vì tồn tại sách có mã thể loại trong kho!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void bt_Xoa_Click(object sender, EventArgs e)
        {
            try
            {
                dong = dgv_QLTL.CurrentRow.Index;
                string maTL = dgv_QLTL.Rows[dong].Cells[0].Value.ToString();
                string tl_Delete = "Delete from TheLoai where MaTL = '" + maTL + "'";
                SqlCommand cmdTL = new SqlCommand(tl_Delete, conn);
                cmdTL.ExecuteNonQuery();
                dtTL.Rows.Clear();
                daTL.Fill(dtTL);
                tb_MaTL.Text = "";
                tb_TenTL.Text = "";
                tb_Mota.Text = "";
                tb_MaTL.Focus();
                MessageBox.Show("Xoá thông tin thể loại thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SqlException)
            {
                MessageBox.Show("Không thể xoá thông tin thể loại vì tồn tại sách có mã thể loại trong kho!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgv_QLTL_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dong = dgv_QLTL.CurrentRow.Index;
            tb_MaTL.Text = dgv_QLTL.Rows[dong].Cells[0].Value.ToString();
            tb_TenTL.Text = dgv_QLTL.Rows[dong].Cells[1].Value.ToString();
            tb_Mota.Text = dgv_QLTL.Rows[dong].Cells[2].Value.ToString();
        }
    }
}
