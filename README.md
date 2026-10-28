# BÁO CÁO THỰC HÀNH LẬP TRÌNH C# - KẾT NỐI MYSQL DATABASE

## THÔNG TIN SINH VIÊN
- **Họ và tên:** Phạm Xuân Tiến
- **Mã số sinh viên:** 24810320273
- **Lớp:** D19QTANM1
- **Tên bài tập:** Quản lý Sinh viên kết nối Cơ sở dữ liệu MySQL

---

## 1. MÔ TẢ HỆ THỐNG & KỸ THUẬT ÁP DỤNG
- **Hệ quản trị CSDL:** MySQL (`ql_sinhvien`)
- **Bảng dữ liệu:** `SinhVien` (MaSV, HoTen, NgaySinh, GioiTinh, Lop, DiemTB)
- **Thư viện kết nối:** `MySql.Data` (Oracle)
- **Các chức năng chính:**
  - Nạp dữ liệu tự động từ MySQL lên `DataGridView`.
  - Thêm sinh viên mới (INSERT), cập nhật thông tin (UPDATE), xóa sinh viên (DELETE).
  - Bắt lỗi dữ liệu nhập với `ErrorProvider` (kiểm tra mã trống, trùng khóa, điểm TB từ 0.00 đến 10.00).
  - Đồng bộ dòng được chọn lên các ô điều khiển nhập liệu.

---

## 2. KẾT QUẢ TRIỂN KHAI GIAO DIỆN
*(Cập nhật hình ảnh minh họa hệ thống)*
