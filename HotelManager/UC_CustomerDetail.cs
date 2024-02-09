using System;
using System.Data;
using System.Windows.Forms;

namespace HotelManager
{
    public partial class UC_CustomerDetail : UserControl
    {
        private Connection dbConnection;
        private BindingSource bindingSource;

        public UC_CustomerDetail()
        {
            InitializeComponent();
            dbConnection = new Connection();
            bindingSource = new BindingSource();
            LoadCustomerData();
        }

        private void LoadCustomerData()
        {
            try
            {
                string query = "SELECT Customer.CustomerID, Customer.CustomerName, Customer.Mobile, Rooms.RoomNumber, " +
                               "Customer.Nationality, Customer.Gender, Customer.DOB, Customer.IDProof, Customer.Address, " +
                               "Customer.Checkin, Customer.Checkout, Customer.CheckedOut " +
                               "FROM Customer " +
                               "JOIN Rooms ON Customer.RoomID = Rooms.RoomID";
                DataSet customerData = dbConnection.GetData(query);

                bindingSource.DataSource = customerData.Tables[0];

                DataGridView2.DataSource = bindingSource;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        public void RefreshData()
        {
            bindingSource.ResetBindings(false);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadCustomerData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (DataGridView2.SelectedRows.Count > 0)
            {
                int customerID = Convert.ToInt32(DataGridView2.SelectedRows[0].Cells["CustomerID"].Value);

                string deleteQuery = "DELETE FROM Customer WHERE CustomerID = @CustomerID";
                dbConnection.SetData(deleteQuery, "Customer deleted successfully", ("@CustomerID", customerID));

                LoadCustomerData();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một khách hàng để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}