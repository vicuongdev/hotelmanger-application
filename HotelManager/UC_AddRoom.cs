using System;
using System.Data;
using System.Windows.Forms;
using System.Xml.Linq;
using HotelManager;

namespace HotelManager
{
    public partial class UC_AddRoom : UserControl
    {
        private Connection dbConnection;
        private EditRoomForm editRoomForm;

        public UC_AddRoom()
        {
            InitializeComponent();
            dbConnection = new Connection();
            UC_AddRoom_Load(this, EventArgs.Empty);
        }

        private bool isLoaded = false;

        private void UC_AddRoom_Load(object sender, EventArgs e)
        {
            if (!isLoaded)
            {
                LoadRoomTypes();
                LoadBedTypes();
                LoadRoomData();
                isLoaded = true;

                DataGridView1.CellClick += DataGridView1_CellClick;
            }
        }

        private void LoadRoomTypes()
        {
            try
            {
                string query = "SELECT TypeName FROM RoomType";
                var roomTypeDataSet = dbConnection.GetData(query);

                if (roomTypeDataSet.Tables.Count > 0 && roomTypeDataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in roomTypeDataSet.Tables[0].Rows)
                    {
                        cbxRoomType.Items.Add(row["TypeName"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi tải dữ liệu loại phòng: " + ex.Message);
            }
        }

        private void LoadBedTypes()
        {
            try
            {
                string query = "SELECT TypeName FROM BedType";
                var bedTypeDataSet = dbConnection.GetData(query);

                if (bedTypeDataSet.Tables.Count > 0 && bedTypeDataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in bedTypeDataSet.Tables[0].Rows)
                    {
                        cbxBedType.Items.Add(row["TypeName"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi tải dữ liệu loại giường: " + ex.Message);
            }
        }

        private void LoadRoomData()
        {
            try
            {
                string query = "SELECT R.RoomID, R.RoomNumber, RT.TypeName AS RoomType, BT.TypeName AS BedType, R.Price, R.Booked " +
                               "FROM Rooms R " +
                               "JOIN RoomType RT ON R.RoomTypeID = RT.RoomTypeID " +
                               "JOIN BedType BT ON R.BedTypeID = BT.BedTypeID";

                var roomsDataSet = dbConnection.GetData(query);

                if (roomsDataSet.Tables.Count > 0)
                {
                    DataGridView1.DataSource = roomsDataSet.Tables[0];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi tải dữ liệu phòng: " + ex.Message);
            }
        }

        private void btnAddRoom_Click(object sender, EventArgs e)
        {
            try
            {
                string roomNumber = txtRoomNo.Text;
                if (IsRoomNumberExists(roomNumber))
                {
                    MessageBox.Show("Phòng đã có trong hệ thống vui lòng tạo số phòng mới");
                    return;
                }

                string roomType = cbxRoomType.SelectedItem.ToString();
                string bedType = cbxBedType.SelectedItem.ToString();
                int roomTypeID = dbConnection.GetRoomTypeId(roomType);
                int bedTypeID = dbConnection.GetBedTypeId(bedType);
                decimal price = decimal.Parse(txtPrice.Text);

                string queryAddRoom = "INSERT INTO Rooms (RoomNumber, RoomTypeID, BedTypeID, Price, Booked) " +
                                      "VALUES (@RoomNumber, @RoomTypeID, @BedTypeID, @Price, 0)";
                dbConnection.SetData(queryAddRoom, "Phòng đã được thêm thành công!", ("@RoomNumber", roomNumber),
                                     ("@RoomTypeID", roomTypeID), ("@BedTypeID", bedTypeID), ("@Price", price));

                LoadRoomData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi thêm phòng " + ex.Message);
            }
        }
        private bool IsRoomNumberExists(string roomNumber)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM Rooms WHERE RoomNumber = @RoomNumber";

                using (DataSet result = dbConnection.GetData(query, ("@RoomNumber", roomNumber)))
                {
                    if (result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
                    {
                        int count = Convert.ToInt32(result.Tables[0].Rows[0][0]);
                        return count > 0;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi kiểm tra số phòng: " + ex.Message);
                return false;
            }
        }
        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < DataGridView1.Rows.Count)
            {
                DataGridViewRow selectedRow = DataGridView1.Rows[e.RowIndex];

                string roomNumber = selectedRow.Cells["RoomNumber"].Value.ToString();
                string roomType = selectedRow.Cells["RoomType"].Value.ToString();
                string bedType = selectedRow.Cells["BedType"].Value.ToString();

                if (selectedRow.Cells["Price"].Value != DBNull.Value)
                {
                    decimal price = Convert.ToDecimal(selectedRow.Cells["Price"].Value);
                    txtPrice.Text = price.ToString();
                }
                else
                {
                    txtPrice.Text = string.Empty;
                }

                txtRoomNo.Text = roomNumber;
                cbxRoomType.SelectedItem = roomType;
                cbxBedType.SelectedItem = bedType;
            }
        }

        private void btnDeleteRoom_Click_1(object sender, EventArgs e)
        {
            try
            {
                int selectedRoomID = Convert.ToInt32(DataGridView1.SelectedRows[0].Cells["RoomID"].Value);
                string queryDeleteRoom = "DELETE FROM Rooms WHERE RoomID = @RoomID";
                dbConnection.SetData(queryDeleteRoom, "Phòng đã được xóa thành công!", ("@RoomID", selectedRoomID));

                LoadRoomData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi xóa phòng: " + ex.Message);
            }
        }

        private void btnEditRoom_Click_1(object sender, EventArgs e)
        {
            try
            {
                int selectedRoomID = Convert.ToInt32(DataGridView1.SelectedRows[0].Cells["RoomID"].Value);
                if (editRoomForm != null && !editRoomForm.IsDisposed)
                {
                    editRoomForm.FillData(selectedRoomID);
                }
                else
                {
                    editRoomForm = new EditRoomForm(selectedRoomID);
                    editRoomForm.ShowDialog();
                }
                LoadRoomData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi chỉnh sửa phòng: " + ex.Message);
            }
        }

        private void LoadCustomerData()
        {
            try
            {
                string query = "SELECT * FROM Customer";
                DataSet customerData = dbConnection.GetData(query);

                DataGridView1.DataSource = customerData.Tables[0];

                DataGridView1.DataSource = DataGridView1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefesh_Click(object sender, EventArgs e)
        {
            ResetForm();
            LoadRoomData();
        }
        private void ResetForm()
        {
            txtRoomNo.Text = string.Empty;
            txtPrice.Text = string.Empty;
            cbxRoomType.SelectedIndex = -1;
            cbxBedType.SelectedIndex = -1;
        }
    }
}