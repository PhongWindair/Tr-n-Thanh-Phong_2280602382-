using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KT
{
    public partial class Form1 : Form
    {
        private bool changesPending = false; 

        public Form1()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                var context = new Model1();
                var listsinhvien = context.Sinhviens
                    .Select(s => new
                    {
                        s.MaSV,
                        s.HotenSV,
                        s.Ngaysinh,
                        Tenlop = s.Lop.Tenlop,
                    }).ToList();

                dataGridView1.DataSource = listsinhvien;
                var listlop = context.Lops.ToList();
                Fillkhoa(listlop);

                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                btnSave.Enabled = false;
                btnCancel.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Fillkhoa(List<Lop> listlop)
        {
            cblophoc.DataSource = listlop;
            cblophoc.DisplayMember = "Tenlop";
            cblophoc.ValueMember = "Malop";
        }

        private void add_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new Model1())
                {
                    var sinhvien = new Sinhvien
                    {
                        MaSV = txtms.Text,
                        HotenSV = txtten.Text,
                        Ngaysinh = dateNgaySinh.Value,
                        Malop = cblophoc.SelectedValue.ToString()
                    };

                    context.Sinhviens.Add(sinhvien);
                    context.SaveChanges();

                    MessageBox.Show("Thêm sinh viên thành công!");
                    LoadData();
                    ClearInputFields();

                    ShowSaveCancelButtons();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }
        private void LoadData()
        {
            try
            {
                using (var context = new Model1())
                {
                    var listsinhvien = context.Sinhviens
                        .Select(s => new
                        {
                            s.MaSV,
                            s.HotenSV,
                            s.Ngaysinh,
                            Tenlop = s.Lop.Tenlop,
                        }).ToList();

                    dataGridView1.DataSource = listsinhvien;

                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    changesPending = false; 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ClearInputFields()
        {
            txtms.Text = string.Empty;
            txtten.Text = string.Empty;
            dateNgaySinh.Value = DateTime.Now;
            cblophoc.SelectedIndex = -1;
        }
        private void ShowSaveCancelButtons()
        {
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            changesPending = true; 
        }

        private void edit_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentRow != null)
                {
                    string maSV = dataGridView1.CurrentRow.Cells["MaSV"].Value.ToString();

                    using (var context = new Model1())
                    {
                        var sinhvien = context.Sinhviens.FirstOrDefault(s => s.MaSV == maSV);

                        if (sinhvien != null)
                        {
                            sinhvien.HotenSV = txtten.Text;
                            sinhvien.Ngaysinh = dateNgaySinh.Value;
                            sinhvien.Malop = cblophoc.SelectedValue.ToString();

                            context.SaveChanges();

                            MessageBox.Show("Cập nhật thông tin sinh viên thành công!");
                            LoadData();
                            ClearInputFields();

                            ShowSaveCancelButtons();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy sinh viên để sửa.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn một sinh viên để sửa.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        private void del_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentRow != null)
                {
                    var result = MessageBox.Show("Bạn có chắc chắn muốn xóa sinh viên này không?",
                                                 "Xác nhận xóa",
                                                 MessageBoxButtons.YesNo,
                                                 MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        string maSV = dataGridView1.CurrentRow.Cells["MaSV"].Value.ToString();

                        using (var context = new Model1())
                        {
                            var sinhvien = context.Sinhviens.FirstOrDefault(s => s.MaSV == maSV);

                            if (sinhvien != null)
                            {
                                context.Sinhviens.Remove(sinhvien);
                                context.SaveChanges();

                                MessageBox.Show("Xóa sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadData();
                                ClearInputFields();

                                ShowSaveCancelButtons(); 
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy sinh viên để xóa.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn một sinh viên để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Tất cả thay đổi đã được lưu.");
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            changesPending = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadData();
            MessageBox.Show("Các thay đổi đã bị hủy.");
        }

        private void exit_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn thoát không?",
                             "Xác nhận thoát",
                             MessageBoxButtons.YesNo,
                             MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void find_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new Model1())
                {
                    if (!string.IsNullOrWhiteSpace(txtfind.Text))
                    {
                        var searchName = txtfind.Text.ToLower();

                        var result = context.Sinhviens
                            .Where(s => s.HotenSV.ToLower().Contains(searchName))
                            .Select(s => new
                            {
                                s.MaSV,
                                s.HotenSV,
                                s.Ngaysinh,
                                Tenlop = s.Lop.Tenlop
                            })
                            .ToList();

                        dataGridView1.DataSource = result;

                        if (result.Count == 0)
                        {
                            MessageBox.Show("Không tìm thấy sinh viên phù hợp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Vui lòng nhập tên sinh viên để tìm kiếm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0) 
                {
                    DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                    txtms.Text = row.Cells["MaSV"].Value?.ToString();
                    txtten.Text = row.Cells["HotenSV"].Value?.ToString();
                    dateNgaySinh.Value = row.Cells["Ngaysinh"].Value != null
                        ? Convert.ToDateTime(row.Cells["Ngaysinh"].Value)
                        : DateTime.Now;
                    cblophoc.Text = row.Cells["Tenlop"].Value?.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
