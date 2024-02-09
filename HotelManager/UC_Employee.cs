using System;
using System.Data;
using System.Windows.Forms;

namespace HotelManager
{
    public partial class UC_Employee : UserControl
    {
        private Connection dbConnection;
        private int selectedEmployeeID;
        private bool isResettingForm = false;

        public UC_Employee()
        {
            InitializeComponent();
            dbConnection = new Connection();
            UC_Employee_Load(this, EventArgs.Empty);
        }

        private bool isLoaded = false;

        private void UC_Employee_Load(object sender, EventArgs e)
        {
            if (!isLoaded)
            {
                LoadPositions();
                LoadRoomNumbers();
                isLoaded = true;

                DataGridView1.CellClick += DataGridView1_CellClick;
                btnAddEmployee.Click += btnAddEmployee_Click;
                btnEditEmployee.Click += btnEditEmployee_Click;
                btnDeleteEmployee.Click += btnDeleteEmployee_Click;
                btnRefesh.Click += btnRefesh_Click;
                LoadEmployeeData();
            }
        }

        private void LoadPositions()
        {
            try
            {
                string query = "SELECT DISTINCT Position FROM Employee";
                var positionDataSet = dbConnection.GetData(query);

                if (positionDataSet.Tables.Count > 0 && positionDataSet.Tables[0].Rows.Count > 0)
                {
                    cbxPosition.Items.Clear(); // Clear existing items
                    foreach (DataRow row in positionDataSet.Tables[0].Rows)
                    {
                        cbxPosition.Items.Add(row["Position"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi tải vị trí nhân viên: " + ex.Message);
            }
        }

        private void LoadRoomNumbers()
        {
            try
            {
                cbxRoomNo.Items.Clear();

                // Lấy tất cả các phòng từ cơ sở dữ liệu
                string queryAllRooms = "SELECT RoomNumber FROM Rooms";
                var allRoomDataSet = dbConnection.GetData(queryAllRooms);

                if (allRoomDataSet.Tables.Count > 0 && allRoomDataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in allRoomDataSet.Tables[0].Rows)
                    {
                        // Thêm phòng vào ComboBox
                        cbxRoomNo.Items.Add(row["RoomNumber"].ToString());
                    }
                }

                // Hiển thị phòng đã được booked và phân công nhân viên
                string queryBookedRooms = "SELECT DISTINCT RoomNumber FROM Employee";
                var bookedRoomDataSet = dbConnection.GetData(queryBookedRooms);

                if (bookedRoomDataSet.Tables.Count > 0 && bookedRoomDataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in bookedRoomDataSet.Tables[0].Rows)
                    {
                        // Kiểm tra xem phòng đã được booked và phân công nhân viên chưa
                        string roomNumber = row["RoomNumber"].ToString();
                        if (cbxRoomNo.Items.Contains(roomNumber))
                        {
                            cbxRoomNo.Items.Remove(roomNumber);
                            cbxRoomNo.Items.Add(roomNumber + " (Booked)");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi tải danh sách phòng: " + ex.Message);
            }
        }

        private void btnAddEmployee_Click(object sender, EventArgs e)
        {
            try
            {
                isResettingForm = true;

                string employeeName = txtEmployeeName.Text;
                string position = cbxPosition.SelectedItem?.ToString();
                string address = txtAddress.Text;
                long mobile = long.Parse(txtMobile.Text);
                string roomNumber = GetSelectedRoomNumber();

                if (string.IsNullOrEmpty(employeeName) || string.IsNullOrEmpty(position) ||
                    string.IsNullOrEmpty(address) || mobile == 0 || string.IsNullOrEmpty(roomNumber))
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin nhân viên.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (IsRoomOccupied(roomNumber))
                {
                    MessageBox.Show("Phòng đã có nhân viên, vui lòng chọn phòng khác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string queryAddEmployee = "INSERT INTO Employee (EmployeeName, Position, Address, Mobile, RoomNumber) " +
                                          "VALUES (@EmployeeName, @Position, @Address, @Mobile, @RoomNumber)";
                dbConnection.SetData(queryAddEmployee, "Nhân viên đã được thêm thành công!",
                                     ("@EmployeeName", employeeName), ("@Position", position),
                                     ("@Address", address), ("@Mobile", mobile), ("@RoomNumber", roomNumber));

                string updateRoomQuery = "UPDATE Rooms SET EmployeeID = (SELECT EmployeeID FROM Employee WHERE RoomNumber = @RoomNumber) " +
                                         "WHERE RoomNumber = @RoomNumber";
                dbConnection.SetData(updateRoomQuery, "", ("@RoomNumber", roomNumber));

                isResettingForm = false;

                UpdateRoomList(); // Cập nhật danh sách phòng

                RefreshDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi thêm nhân viên: " + ex.Message);
            }
        }

        private void btnEditEmployee_Click(object sender, EventArgs e)
        {
            try
            {
                isResettingForm = true;

                string employeeName = txtEmployeeName.Text;
                string position = cbxPosition.SelectedItem?.ToString();
                string address = txtAddress.Text;
                long mobile = long.Parse(txtMobile.Text);
                string roomNumber = GetSelectedRoomNumber();

                if (string.IsNullOrEmpty(employeeName) || string.IsNullOrEmpty(position) ||
                    string.IsNullOrEmpty(address) || mobile == 0 || string.IsNullOrEmpty(roomNumber))
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin nhân viên.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (IsRoomOccupied(roomNumber))
                {
                    MessageBox.Show("Phòng đã có nhân viên, vui lòng chọn phòng khác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string queryGetOldRoomNumber = "SELECT RoomNumber FROM Employee WHERE EmployeeID = @EmployeeID";
                string oldRoomNumber = dbConnection.GetData(queryGetOldRoomNumber, ("@EmployeeID", selectedEmployeeID)).Tables[0].Rows[0]["RoomNumber"].ToString();

                string updateOldRoomQuery = "UPDATE Rooms SET EmployeeID = NULL WHERE RoomNumber = @OldRoomNumber";
                dbConnection.SetData(updateOldRoomQuery, "", ("@OldRoomNumber", oldRoomNumber));

                string queryUpdateEmployee = "UPDATE Employee " +
                                            "SET EmployeeName = @EmployeeName, Position = @Position, Address = @Address, Mobile = @Mobile, RoomNumber = @RoomNumber " +
                                            "WHERE EmployeeID = @EmployeeID";
                dbConnection.SetData(queryUpdateEmployee, "Nhân viên đã được cập nhật thành công!",
                                     ("@EmployeeID", selectedEmployeeID), ("@EmployeeName", employeeName),
                                     ("@Position", position), ("@Address", address), ("@Mobile", mobile),
                                     ("@RoomNumber", roomNumber));

                string updateRoomQuery = "UPDATE Rooms SET EmployeeID = @EmployeeID WHERE RoomNumber = @RoomNumber";
                dbConnection.SetData(updateRoomQuery, "", ("@EmployeeID", selectedEmployeeID), ("@RoomNumber", roomNumber));

                isResettingForm = false;

                UpdateRoomList();

                RefreshDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi cập nhật nhân viên: " + ex.Message);
            }
        }

        private void ResetEmployeeDetails()
        {
            txtEmployeeName.Text = string.Empty;
            cbxPosition.SelectedIndex = -1;
            txtAddress.Text = string.Empty;
            txtMobile.Text = string.Empty;
            cbxRoomNo.SelectedIndex = -1;
        }

        private void btnDeleteEmployee_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Bạn có chắc chắn muốn xóa nhân viên này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    isResettingForm = true;

                    string queryGetRoomNumber = "SELECT RoomNumber FROM Employee WHERE EmployeeID = @EmployeeID";
                    string roomNumber = dbConnection.GetData(queryGetRoomNumber, ("@EmployeeID", selectedEmployeeID)).Tables[0].Rows[0]["RoomNumber"].ToString();

                    string updateRoomQuery = "UPDATE Rooms SET EmployeeID = NULL WHERE RoomNumber = @RoomNumber";
                    dbConnection.SetData(updateRoomQuery, "", ("@RoomNumber", roomNumber));

                    string queryDeleteEmployee = "DELETE FROM Employee WHERE EmployeeID = @EmployeeID";
                    dbConnection.SetData(queryDeleteEmployee, "Nhân viên đã được xóa thành công!", ("@EmployeeID", selectedEmployeeID));

                    isResettingForm = false;

                    UpdateRoomList(); // Cập nhật danh sách phòng

                    RefreshDataGridView();

                    ResetEmployeeDetails(); // Làm mới thông tin nhân viên trên giao diện
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi xóa nhân viên: " + ex.Message);
            }
        }

        private void LoadEmployeeData()
        {
            try
            {
                string query = "SELECT EmployeeID, EmployeeName, Position, Address, Mobile, RoomNumber FROM Employee";
                DataSet employeeData = dbConnection.GetData(query);

                DataGridView1.DataSource = employeeData.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi tải dữ liệu nhân viên: " + ex.Message);
            }
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < DataGridView1.Rows.Count)
            {
                DataGridViewRow selectedRow = DataGridView1.Rows[e.RowIndex];

                selectedEmployeeID = Convert.ToInt32(selectedRow.Cells["EmployeeID"].Value);

                string employeeName = selectedRow.Cells["EmployeeName"].Value.ToString();
                string position = selectedRow.Cells["Position"].Value.ToString();
                string address = selectedRow.Cells["Address"].Value.ToString();
                long mobile = Convert.ToInt64(selectedRow.Cells["Mobile"].Value);
                string roomNumber = selectedRow.Cells["RoomNumber"].Value.ToString();


                txtEmployeeName.Text = employeeName;
                cbxPosition.SelectedItem = position;
                txtAddress.Text = address;
                txtMobile.Text = mobile.ToString();
                cbxRoomNo.SelectedItem = roomNumber;
                if (roomNumber.Contains(" (Booked)"))
                {
                    cbxRoomNo.SelectedItem = roomNumber.Replace(" (Booked)", "");
                }
                else
                {
                    cbxRoomNo.SelectedItem = roomNumber;
                }
            }
        }

        private bool IsRoomOccupied(string roomNumber)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM Employee WHERE RoomNumber = @RoomNumber AND EmployeeID != @EmployeeID";
                int count = Convert.ToInt32(dbConnection.GetData(query, ("@RoomNumber", roomNumber), ("@EmployeeID", selectedEmployeeID)).Tables[0].Rows[0][0]);
                return count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi kiểm tra trạng thái phòng: " + ex.Message);
                return true;
            }
        }

        private void ResetForm()
        {
            txtEmployeeName.Text = string.Empty;
            cbxPosition.SelectedIndex = -1;
            txtAddress.Text = string.Empty;
            txtMobile.Text = string.Empty;
            cbxRoomNo.SelectedIndex = -1;
            
        }

        private void RefreshDataGridView()
        {
            DataGridView1.DataSource = null;
            LoadEmployeeData();
            DataGridView1.ClearSelection();
        }

        private void btnRefesh_Click(object sender, EventArgs e)
        {
            LoadRoomNumbers();
            LoadEmployeeData();
            ResetForm();
        }

        private string GetSelectedRoomNumber()
        {
            if (cbxRoomNo.SelectedItem != null)
            {
                string selectedRoom = cbxRoomNo.SelectedItem.ToString();
                if (selectedRoom.Contains(" (Booked)"))
                {
                    return selectedRoom.Replace(" (Booked)", "");
                }
                else
                {
                    return selectedRoom;
                }
            }
            return string.Empty;
        }

        private void UpdateRoomList()
        {
            try
            {
                LoadRoomNumbers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi cập nhật danh sách phòng: " + ex.Message);
            }
        }
    }
}
