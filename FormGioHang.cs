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
    public partial class FormGioHang : Form
    {
        public FormGioHang()
        {
            InitializeComponent();
        }
        string connectionstr = @"Data Source=LAPTOP-D4HEMV8S\MSSQLSERVER02;Initial Catalog=BTL_QLCHS;Integrated Security=True";
        SqlConnection conn = null;
        SqlDataAdapter daGioHang, daKH;
        DataTable dtGioHang, dtKH;
        int dong;

        private void FormGioHang_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(connectionstr);
            conn.Open();
            string sqlGioHang = "Select Sach.MaSach, TenSach, TenTL as TheLoai, TenTG as TacGia, TenNXB as NhaXuatBan, Sach.DonGia, SLMua as SoLuongMua, ThanhTien  from Sach, TheLoai, TacGia, NXB, ChiTietHD where Sach.MaTL = TheLoai.MaTL and Sach.MaTG = TacGia.MaTG and Sach.MaNXB = NXB.MaNXB and Sach.MaSach = ChiTietHD.MaSach and MaHD = '1'";
            daGioHang = new SqlDataAdapter(sqlGioHang, conn);
            dtGioHang = new DataTable();
            daGioHang.Fill(dtGioHang);
            dgv_Giohang.DataSource = dtGioHang;
            //Combobox tên khách hàng:
            string sqlKH = "Select * from KhachHang";
            daKH = new SqlDataAdapter(sqlKH, conn);
            dtKH = new DataTable();
            daKH.Fill(dtKH);
            cb_TenKH.DataSource = dtKH;
            cb_TenKH.DisplayMember = "TenKH";
            cb_TenKH.ValueMember = "MaKH";
        }

        private void bt_Trove_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bt_Xuathoadon_Click_1(object sender, EventArgs e)
        {
            if(tb_MaHD.Text == "")
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin hoá đơn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    string cthd_Delete = "Delete from ChiTietHD where MaHD = '1'";
                    SqlCommand cmdDelCTHD = new SqlCommand(cthd_Delete, conn);
                    cmdDelCTHD.ExecuteNonQuery();
                    string hd_Delete = "Delete from HoaDon where MaHD = '1'";
                    SqlCommand cmdDelHD = new SqlCommand(hd_Delete, conn);
                    cmdDelCTHD.ExecuteNonQuery();
                    string hd_Insert = "Insert into HoaDon values ('" + tb_MaHD.Text + "', '" + datetime_Ngayxuat.Text + "', '" + tb_Tongtien.Text + "', '" + cb_TenKH.SelectedValue + "')";
                    SqlCommand cmdHoaDon = new SqlCommand(hd_Insert, conn);
                    cmdHoaDon.ExecuteNonQuery();
                    for (int i = 0; i < dgv_Giohang.Rows.Count; i++)
                    {
                        string cthd_Insert = "Insert into ChiTietHD values ('" + tb_MaHD.Text + "', '" + dgv_Giohang.Rows[i].Cells[0].Value + "', '" + dgv_Giohang.Rows[i].Cells[6].Value + "', '" + dgv_Giohang.Rows[i].Cells[5].Value + "', '" + dgv_Giohang.Rows[i].Cells[7].Value + "')";
                        SqlCommand cmdCTHD = new SqlCommand(cthd_Insert, conn);
                        cmdCTHD.ExecuteNonQuery();
                        string sqlSach = "Select SoLuong from Sach where MaSach = '"+ dgv_Giohang.Rows[i].Cells[0].Value + "'";
                        SqlDataAdapter daSach = new SqlDataAdapter(sqlSach, conn);
                        DataTable dtSach = new DataTable();
                        daSach.Fill(dtSach);
                        int soLuong = Convert.ToInt32(dtSach.Rows[0][0]);
                        string sach_Update = "Update Sach set SoLuong = '" + (soLuong - Convert.ToInt32(dgv_Giohang.Rows[i].Cells[6].Value)) + "' where MaSach = '" + dgv_Giohang.Rows[i].Cells[0].Value + "'";
                        SqlCommand cmdSach = new SqlCommand(sach_Update, conn);
                        cmdSach.ExecuteNonQuery();
                    }
                    dtGioHang.Rows.Clear();
                    daGioHang.Fill(dtGioHang);
                    MessageBox.Show("Xuất hoá đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Close();
                }
                catch (SqlException)
                {
                    MessageBox.Show("Mã hoá đơn đã trùng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void bt_Taohoadon_Click(object sender, EventArgs e)
        {
            string sql = "Select sum(ThanhTien) from ChiTietHD where MaHD = '1'";
            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            tb_Tongtien.Text = dt.Rows[0][0].ToString();
            if(tb_Tongtien.Text != "")
            {
                tlp_Giohang.Visible = false;
                tlp_Taohoadon.Visible = false;
                tlp_Nhaphd.Visible = true;
                tlp_Xuathd.Visible = true;
            }
            else
            {
                MessageBox.Show("Chưa có sách trong giỏ hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bt_Trovegiohang_Click(object sender, EventArgs e)
        {
            tlp_Giohang.Visible = true;
            tlp_Taohoadon.Visible = true;
            tlp_Nhaphd.Visible = false;
            tlp_Xuathd.Visible = false;
        }

        private void bt_Xoahoadon_Click(object sender, EventArgs e)
        {
            try
            {
                dong = dgv_Giohang.CurrentRow.Index;
                string maSach = dgv_Giohang.Rows[dong].Cells[0].Value.ToString();
                string giohang_Delete = "Delete from ChiTietHD where MaHD = '1' and MaSach = '" + maSach + "'";
                SqlCommand cmdGioHang = new SqlCommand(giohang_Delete, conn);
                cmdGioHang.ExecuteNonQuery();
                dtGioHang.Rows.Clear();
                daGioHang.Fill(dtGioHang);
            }
            catch (Exception)
            {
                MessageBox.Show("Vui lòng chọn sách để xoá!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
