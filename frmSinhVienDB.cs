using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace QuanLySinhVienDB
{
    public class frmSinhVienDB : Form
    {
        // CHÚ Ý: Đổi 'Pwd=' thành mật khẩu MySQL của bạn nếu có (nếu dùng XAMPP thì để trống như dưới)
        private string connectionString = "Server=localhost;Database=ql_sinhvien;Uid=root;Pwd=;Charset=utf8mb4;";

        private TextBox txtMaSV = null!;
        private TextBox txtHoTen = null!;
        private DateTimePicker dtpNgaySinh = null!;
        private ComboBox cboGioiTinh = null!;
        private TextBox txtLop = null!;
        private TextBox txtDiemTB = null!;
        private Button btnThem = null!, btnSua = null!, btnXoa = null!, btnLamMoi = null!;
        private DataGridView dgvSinhVien = null!;
        private ErrorProvider errorProvider = null!;

        public frmSinhVienDB()
        {
            SetupForm();
            SetupControls();
            LoadData();
        }

        private void SetupForm()
        {
            this.Text = "Hệ thống Quản lý Sinh viên - Kết nối MySQL Database";
            this.Size = new Size(950, 620);
            this.MinimumSize = new Size(850, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9.5F);
            errorProvider = new ErrorProvider(this) { BlinkStyle = ErrorBlinkStyle.NeverBlink };
        }

        private void SetupControls()
        {
            // Panel Nhập liệu phía trên
            Panel topPanel = new Panel { Dock = DockStyle.Top, Height = 220, Padding = new Padding(10) };

            GroupBox grpInput = new GroupBox
            {
                Text = "Thông tin chi tiết sinh viên",
                Dock = DockStyle.Fill
            };

            Label lblMa = new Label { Text = "Mã SV:", Location = new Point(25, 30), AutoSize = true };
            txtMaSV = new TextBox { Location = new Point(100, 26), Width = 160 };

            Label lblTen = new Label { Text = "Họ và tên:", Location = new Point(290, 30), AutoSize = true };
            txtHoTen = new TextBox { Location = new Point(370, 26), Width = 230 };

            Label lblNgaySinh = new Label { Text = "Ngày sinh:", Location = new Point(630, 30), AutoSize = true };
            dtpNgaySinh = new DateTimePicker { Location = new Point(710, 26), Width = 180, Format = DateTimePickerFormat.Short };

            Label lblGioiTinh = new Label { Text = "Giới tính:", Location = new Point(25, 75), AutoSize = true };
            cboGioiTinh = new ComboBox { Location = new Point(100, 71), Width = 160, DropDownStyle = ComboBoxStyle.DropDownList };
            cboGioiTinh.Items.AddRange(new object[] { "Nam", "Nữ" });
            cboGioiTinh.SelectedIndex = 0;

            Label lblLop = new Label { Text = "Lớp học:", Location = new Point(290, 75), AutoSize = true };
            txtLop = new TextBox { Location = new Point(370, 71), Width = 230 };

            Label lblDiem = new Label { Text = "Điểm TB:", Location = new Point(630, 75), AutoSize = true };
            txtDiemTB = new TextBox { Location = new Point(710, 71), Width = 180, Text = "0.00" };

            btnThem = new Button { Text = "Thêm mới", Location = new Point(100, 140), Size = new Size(110, 35), BackColor = Color.FromArgb(0, 120, 215), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnSua = new Button { Text = "Cập nhật", Location = new Point(230, 140), Size = new Size(110, 35) };
            btnXoa = new Button { Text = "Xóa SV", Location = new Point(360, 140), Size = new Size(110, 35), BackColor = Color.FromArgb(217, 83, 79), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnLamMoi = new Button { Text = "Làm mới", Location = new Point(490, 140), Size = new Size(110, 35) };

            btnThem.Click += (s, e) => AddStudent();
            btnSua.Click += (s, e) => UpdateStudent();
            btnXoa.Click += (s, e) => DeleteStudent();
            btnLamMoi.Click += (s, e) => ResetFields();

            grpInput.Controls.AddRange(new Control[] {
                lblMa, txtMaSV, lblTen, txtHoTen, lblNgaySinh, dtpNgaySinh,
                lblGioiTinh, cboGioiTinh, lblLop, txtLop, lblDiem, txtDiemTB,
                btnThem, btnSua, btnXoa, btnLamMoi
            });

            topPanel.Controls.Add(grpInput);

            // DataGridView hiển thị danh sách từ CSDL
            dgvSinhVien = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvSinhVien.SelectionChanged += DgvSinhVien_SelectionChanged;

            this.Controls.Add(dgvSinhVien);
            this.Controls.Add(topPanel);
        }

        // Tải danh sách từ MySQL lên DataGridView
        private void LoadData()
        {
            try
            {
                using MySqlConnection conn = new MySqlConnection(connectionString);
                conn.Open();
                string query = "SELECT MaSV, HoTen, NgaySinh, GioiTinh, Lop, DiemTB FROM SinhVien";
                using MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvSinhVien.DataSource = dt;

                if (dgvSinhVien.Columns["MaSV"] != null) dgvSinhVien.Columns["MaSV"].HeaderText = "Mã Sinh Viên";
                if (dgvSinhVien.Columns["HoTen"] != null) dgvSinhVien.Columns["HoTen"].HeaderText = "Họ và Tên";
                if (dgvSinhVien.Columns["NgaySinh"] != null)
                {
                    dgvSinhVien.Columns["NgaySinh"].HeaderText = "Ngày Sinh";
                    dgvSinhVien.Columns["NgaySinh"].DefaultCellStyle.Format = "dd/MM/yyyy";
                }
                if (dgvSinhVien.Columns["GioiTinh"] != null) dgvSinhVien.Columns["GioiTinh"].HeaderText = "Giới Tính";
                if (dgvSinhVien.Columns["Lop"] != null) dgvSinhVien.Columns["Lop"].HeaderText = "Lớp";
                if (dgvSinhVien.Columns["DiemTB"] != null) dgvSinhVien.Columns["DiemTB"].HeaderText = "Điểm TB";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối MySQL: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Đổ dữ liệu dòng đang chọn lên các ô nhập
        private void DgvSinhVien_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvSinhVien.CurrentRow != null && dgvSinhVien.CurrentRow.Index >= 0)
            {
                var row = dgvSinhVien.CurrentRow;
                txtMaSV.Text = row.Cells["MaSV"].Value?.ToString();
                txtMaSV.ReadOnly = true;
                txtHoTen.Text = row.Cells["HoTen"].Value?.ToString();
                if (DateTime.TryParse(row.Cells["NgaySinh"].Value?.ToString(), out DateTime ns))
                    dtpNgaySinh.Value = ns;
                cboGioiTinh.SelectedItem = row.Cells["GioiTinh"].Value?.ToString();
                txtLop.Text = row.Cells["Lop"].Value?.ToString();
                txtDiemTB.Text = row.Cells["DiemTB"].Value?.ToString();
                errorProvider.Clear();
            }
        }

        private bool ValidateData(bool isNew)
        {
            errorProvider.Clear();
            bool valid = true;

            if (string.IsNullOrWhiteSpace(txtMaSV.Text))
            {
                errorProvider.SetError(txtMaSV, "Mã sinh viên không được để trống!");
                valid = false;
            }

            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                errorProvider.SetError(txtHoTen, "Họ tên không được để trống!");
                valid = false;
            }

            if (!decimal.TryParse(txtDiemTB.Text.Trim(), out decimal diem) || diem < 0 || diem > 10)
            {
                errorProvider.SetError(txtDiemTB, "Điểm trung bình phải từ 0.00 đến 10.00!");
                valid = false;
            }

            return valid;
        }

        // Thêm mới Sinh Viên
        private void AddStudent()
        {
            if (!ValidateData(isNew: true)) return;

            try
            {
                using MySqlConnection conn = new MySqlConnection(connectionString);
                conn.Open();
                string sql = "INSERT INTO SinhVien (MaSV, HoTen, NgaySinh, GioiTinh, Lop, DiemTB) " +
                             "VALUES (@MaSV, @HoTen, @NgaySinh, @GioiTinh, @Lop, @DiemTB)";

                using MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaSV", txtMaSV.Text.Trim());
                cmd.Parameters.AddWithValue("@HoTen", txtHoTen.Text.Trim());
                cmd.Parameters.AddWithValue("@NgaySinh", dtpNgaySinh.Value.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@GioiTinh", cboGioiTinh.SelectedItem?.ToString());
                cmd.Parameters.AddWithValue("@Lop", txtLop.Text.Trim());
                cmd.Parameters.AddWithValue("@DiemTB", decimal.Parse(txtDiemTB.Text.Trim()));

                cmd.ExecuteNonQuery();
                MessageBox.Show("Thêm mới sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ResetFields();
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062)
                    MessageBox.Show("Mã sinh viên này đã tồn tại!", "Trùng khóa chính", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                    MessageBox.Show("Lỗi MySQL: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Cập nhật thông tin Sinh Viên
        private void UpdateStudent()
        {
            if (!ValidateData(isNew: false)) return;

            try
            {
                using MySqlConnection conn = new MySqlConnection(connectionString);
                conn.Open();
                string sql = "UPDATE SinhVien SET HoTen=@HoTen, NgaySinh=@NgaySinh, GioiTinh=@GioiTinh, Lop=@Lop, DiemTB=@DiemTB WHERE MaSV=@MaSV";

                using MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaSV", txtMaSV.Text.Trim());
                cmd.Parameters.AddWithValue("@HoTen", txtHoTen.Text.Trim());
                cmd.Parameters.AddWithValue("@NgaySinh", dtpNgaySinh.Value.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@GioiTinh", cboGioiTinh.SelectedItem?.ToString());
                cmd.Parameters.AddWithValue("@Lop", txtLop.Text.Trim());
                cmd.Parameters.AddWithValue("@DiemTB", decimal.Parse(txtDiemTB.Text.Trim()));

                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    MessageBox.Show("Cập nhật thông tin sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sinh viên cần sửa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xóa Sinh Viên
        private void DeleteStudent()
        {
            string maSV = txtMaSV.Text.Trim();
            if (string.IsNullOrWhiteSpace(maSV))
            {
                MessageBox.Show("Vui lòng chọn sinh viên cần xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var dr = MessageBox.Show($"Bạn có chắc chắn muốn xóa sinh viên [{maSV}]?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    using MySqlConnection conn = new MySqlConnection(connectionString);
                    conn.Open();
                    string sql = "DELETE FROM SinhVien WHERE MaSV=@MaSV";
                    using MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@MaSV", maSV);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Đã xóa sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ResetFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ResetFields()
        {
            txtMaSV.ReadOnly = false;
            txtMaSV.Clear();
            txtHoTen.Clear();
            dtpNgaySinh.Value = DateTime.Today;
            cboGioiTinh.SelectedIndex = 0;
            txtLop.Clear();
            txtDiemTB.Text = "0.00";
            errorProvider.Clear();
            txtMaSV.Focus();
        }
    }
}