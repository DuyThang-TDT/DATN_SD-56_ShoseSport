# 🐾 LITTLE FRIEND - Hệ Thống Quản Lý Cửa Hàng Thú Cưng

## 📋 Tổng Quan Dự Án

**LITTLE FRIEND** là một hệ thống quản lý cửa hàng thú cưng toàn diện được phát triển bằng ASP.NET Core MVC, cung cấp giải pháp quản lý cho cả khách hàng và nhân viên bán hàng. Hệ thống hỗ trợ đầy đủ các chức năng từ đăng ký, đăng nhập, mua sắm, thanh toán đến quản lý đơn hàng và kho hàng.

## 🏗️ Kiến Trúc Hệ Thống

### Công Nghệ Sử Dụng
- **Backend**: ASP.NET Core 8.0 MVC
- **Database**: SQL Server với Entity Framework Core
- **Frontend**: Razor Views, Bootstrap, JavaScript
- **Authentication**: Cookie-based Authentication
- **Testing**: xUnit, Moq, FluentAssertions

### Cấu Trúc Project
```
LITTLE FRIEND/
├── LITTLE FRIEND.API/          # API Controllers & Models
├── LITTLE FRIEND.Web/          # MVC Web Application
└── UnitTest/                  # Unit Tests
```

## 👥 Đối Tượng Người Dùng

### 1. **Admin** 
- Quản lý toàn bộ hệ thống
- Quản lý sản phẩm, danh mục, thương hiệu
- Quản lý nhân viên và khách hàng
- Xem báo cáo doanh thu và thống kê

### 2. **Nhân Viên Bán Hàng**
- Xử lý đơn hàng offline
- Quản lý kho hàng
- Hỗ trợ khách hàng
- Xử lý phiếu hoàn trả

### 3. **Khách Hàng**
- Đăng ký tài khoản
- Mua sắm online
- Quản lý đơn hàng
- Áp dụng voucher và khuyến mãi

### 4. **Khách Vãng Lai**
- Xem sản phẩm
- Thêm vào giỏ hàng
- Đặt hàng không cần đăng ký

## 🔄 Luồng Hoạt Động Chính

### 1. **Luồng Đăng Ký - Đăng Nhập**
```
Khách hàng → Đăng ký → Xác thực thông tin → Tạo tài khoản → Đăng nhập tự động → Chuyển hướng trang chủ
```

**Tính năng bảo mật:**
- Kiểm tra trùng lặp username, email, số điện thoại
- Mã hóa mật khẩu
- Session management
- Cookie authentication

### 2. **Luồng Mua Hàng - Thanh Toán**
```
Chọn sản phẩm → Thêm vào giỏ → Áp dụng voucher → Kiểm tra tồn kho → Thanh toán → Tạo đơn hàng
```

**Xử lý đồng thời:**
- Database Lock (UPDLOCK) để tránh double order
- Transaction management
- Rollback khi giao dịch thất bại
- Cập nhật số lượng tồn kho real-time

### 3. **Luồng Quản Lý Đơn Hàng**
```
Tạo đơn → Xác nhận → Xử lý → Giao hàng → Hoàn thành
```

**Trạng thái đơn hàng:**
- Chờ xác nhận
- Đã xác nhận
- Đang xử lý
- Đang giao hàng
- Hoàn thành
- Đã hủy

## 🛡️ Xử Lý Tình Huống Đặc Biệt

### 1. **Double Order Prevention**
- Sử dụng Database Lock (UPDLOCK) trong quá trình thanh toán
- Kiểm tra số lượng tồn kho trước khi tạo đơn hàng
- Transaction rollback khi không đủ hàng

### 2. **Hết Hàng Trong Giỏ**
- Kiểm tra real-time số lượng tồn kho
- Cập nhật giỏ hàng tự động
- Thông báo cho khách hàng

### 3. **Hủy Đơn Hàng**
- **Online**: Khách hàng có thể hủy trong thời gian cho phép
- **Offline**: Nhân viên xử lý hủy đơn
- Hoàn trả voucher và cập nhật tồn kho

### 4. **Phiếu Hoàn Trả**
- Khách hàng tạo yêu cầu hoàn trả
- Admin/Nhân viên xử lý và phê duyệt
- Cập nhật trạng thái và hoàn tiền

## 💳 Hệ Thống Thanh Toán

### Quy Trình Thanh Toán
1. **Kiểm tra voucher** - Tính toán giảm giá
2. **Kiểm tra tồn kho** - Xác nhận số lượng có sẵn
3. **Tạo đơn hàng** - Lưu thông tin đơn hàng
4. **Cập nhật tồn kho** - Giảm số lượng sản phẩm
5. **Xử lý voucher** - Cập nhật trạng thái voucher
6. **Commit transaction** - Hoàn tất giao dịch

### Rollback Khi Thất Bại
- Hoàn trả số lượng tồn kho
- Khôi phục trạng thái voucher
- Xóa đơn hàng đã tạo
- Thông báo lỗi cho khách hàng

## 🔍 Tối Ưu Hóa Hiệu Suất

### 1. **Database Optimization**
- Sử dụng `AsNoTracking()` cho read-only queries
- `Include()` để eager loading
- `Where()` và `Select()` để giảm dữ liệu truyền tải
- Pagination cho danh sách sản phẩm

### 2. **Caching Strategy**
- Session-based caching cho thông tin người dùng
- Query optimization với Entity Framework
- InMemoryDatabase cho unit testing

### 3. **Frontend Updates**
- **Hiện tại**: Page reload để cập nhật dữ liệu
- **Tương lai**: Có thể implement SignalR cho real-time updates

## 🧪 Testing Strategy

### Unit Tests
- **Coverage**: Controllers, Services, Repositories
- **Frameworks**: xUnit, Moq, FluentAssertions
- **Database**: InMemoryDatabase cho testing

### Test Cases
📋 **Test Cases chi tiết**: [Google Sheets](https://docs.google.com/spreadsheets/d/1Uq-htkwqIAW3ISPuMBqQsw7gCBJiRj0LciWAc5UklSs/edit?gid=0#gid=0)

### Test Categories
- **Authentication Tests**: Đăng ký, đăng nhập, phân quyền
- **Cart Tests**: Thêm, sửa, xóa giỏ hàng
- **Order Tests**: Tạo đơn, thanh toán, hủy đơn
- **Product Tests**: CRUD sản phẩm, tìm kiếm
- **Voucher Tests**: Áp dụng, validation voucher

## 📊 Database Design

### ERD & Use Cases
📐 **ERD và Use Case Diagrams**: [Draw.io](https://app.diagrams.net/#G1qJkKNC5QWYDyvoLMXdB8fZk4A3ATzufa#%7B%22pageId%22%3A%22bjAfSX4491UcBPhlg_gX%22%7D)

### Key Entities
- **TaiKhoan**: Quản lý tài khoản người dùng
- **KhachHang**: Thông tin khách hàng
- **NhanVien**: Thông tin nhân viên
- **SanPham**: Sản phẩm trong kho
- **HoaDon**: Đơn hàng
- **GioHang**: Giỏ hàng
- **Voucher**: Mã giảm giá
- **PhieuHoanTra**: Phiếu hoàn trả

## 🚀 Cài Đặt Và Chạy Dự Án

### Yêu Cầu Hệ Thống
- .NET 8.0 SDK
- SQL Server 2019+
- Visual Studio 2022 hoặc VS Code

### Cài Đặt
```bash
# Clone repository
git clone [repository-url]

# Restore packages
dotnet restore

# Update database
dotnet ef database update

# Run application
dotnet run --project LITTLE FRIEND.Web
```

### Cấu Hình Database
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=LITTLE FRIENDDB;Trusted_Connection=true;"
  }
}
```

## 📈 Tính Năng Nổi Bật

### ✅ Đã Hoàn Thành
- [x] Hệ thống đăng ký/đăng nhập đa vai trò
- [x] Quản lý sản phẩm và kho hàng
- [x] Giỏ hàng và thanh toán
- [x] Hệ thống voucher và khuyến mãi
- [x] Quản lý đơn hàng online/offline
- [x] Phiếu hoàn trả
- [x] Báo cáo và thống kê
- [x] Unit testing đầy đủ

### 🔄 Đang Phát Triển
- [ ] Real-time notifications với SignalR
- [ ] Payment gateway integration
- [ ] Mobile app
- [ ] Advanced analytics dashboard

## 🤝 Đóng Góp

1. Fork dự án
2. Tạo feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to branch (`git push origin feature/AmazingFeature`)
5. Tạo Pull Request

## 📄 License

Dự án này được phát triển cho mục đích học tập và nghiên cứu.

## 👨‍💻 Tác Giả

**Nhóm Phát Triển**: DATN-SD-07-LITTLE FRIEND

---

*Hệ thống LITTLE FRIEND - Nơi thú cưng được chăm sóc tốt nhất! 🐕🐱*
