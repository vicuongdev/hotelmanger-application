using System;
using System.Data;
using System.Windows.Forms;

namespace HotelManager
{
    public partial class UC_CheckOut : UserControl
    {
        private Connection dbConnection;

        public UC_CheckOut()
        {
            InitializeComponent();
            dbConnection = new Connection();

            btnRefesh.Click += BtnRefesh_Click;

            InitializeDataGridView();

            LoadCustomerData();
        }

        private void InitializeDataGridView()
        {
            DataGridView3.Columns.Add("CustomerID", "CustomerID");
            DataGridView3.Columns.Add("CustomerName", "CustomerName");
            DataGridView3.Columns.Add("Mobile", "Mobile");
            DataGridView3.Columns.Add("RoomNumber", "RoomNumber");
            DataGridView3.Columns.Add("Price", "Price");
            DataGridView3.Columns.Add("RoomID", "RoomID");
            DataGridView3.Columns["RoomID"].Visible = false;

            DataGridView3.Columns["Price"].DefaultCellStyle.Format = "N2";
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            if (DataGridView3.SelectedRows.Count > 0)
            {
                int customerID = Convert.ToInt32(DataGridView3.SelectedRows[0].Cells["CustomerID"].Value);
                int roomID = Convert.ToInt32(DataGridView3.SelectedRows[0].Cells["RoomID"].Value);

                string updateRoomQuery = "UPDATE Rooms SET Booked = 0 WHERE RoomID = @RoomID AND Booked = 1";
                dbConnection.SetData(updateRoomQuery, "Cập nhật trạng thái phòng thành công", ("@RoomID", roomID));

                string deleteCustomerQuery = "DELETE FROM Customer WHERE CustomerID = @CustomerID";
                dbConnection.SetData(deleteCustomerQuery, "Checkout thành công", ("@CustomerID", customerID));

                LoadCustomerData();

            }
            else
            {
                MessageBox.Show("Vui lòng chọn ít nhất một khách hàng để checkout.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void BtnRefesh_Click(object sender, EventArgs e)
        {
            // Load lại dữ liệu khi nhấn nút Refresh
            LoadCustomerData();
        }

        private void LoadCustomerData()
        {
            try
            {
                // Làm mới dữ liệu trên DataGridView3
                string query = "SELECT c.CustomerID, c.CustomerName, c.Mobile, r.RoomNumber, r.RoomID, r.Price " +
                               "FROM Customer c " +
                               "JOIN Rooms r ON c.RoomID = r.RoomID " +
                               "WHERE c.CheckedOut = 'NO'";
                DataSet customerData = dbConnection.GetData(query);
                DataGridView3.Rows.Clear(); // Xóa dữ liệu cũ

                // Thêm dữ liệu mới
                foreach (DataRow row in customerData.Tables[0].Rows)
                {
                    DataGridView3.Rows.Add(row["CustomerID"], row["CustomerName"], row["Mobile"], row["RoomNumber"], row["Price"], row["RoomID"]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
