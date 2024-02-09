using System;
using System.Data;
using System.Windows.Forms;
using HotelManager;

namespace HotelManager
{
    public partial class EditRoomForm : Form
    {
        private Connection dbConnection;
        private int roomID;
        private void EditRoomForm_Load(object sender, EventArgs e)
        {
            LoadRoomTypes();
            LoadBedTypes();
        }
        public EditRoomForm(int selectedRoomID)
        {
            InitializeComponent();
            dbConnection = new Connection();
            roomID = selectedRoomID;

            FillData(roomID);
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
                MessageBox.Show("Đã xảy ra lỗi khi tải Room Types: " + ex.Message);
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
                MessageBox.Show("Đã xảy ra lỗi khi tải Bed Types: " + ex.Message);
            }
        }

        public void FillData(int roomID)
        {
            try
            {
                string query = "SELECT R.RoomNumber, RT.TypeName AS RoomType, BT.TypeName AS BedType, R.Price " +
                               "FROM Rooms R " +
                               "JOIN RoomType RT ON R.RoomTypeID = RT.RoomTypeID " +
                               "JOIN BedType BT ON R.BedTypeID = BT.BedTypeID " +
                               "WHERE R.RoomID = @RoomID";

                var roomData = dbConnection.GetData(query, ("@RoomID", roomID));

                if (roomData.Tables.Count > 0 && roomData.Tables[0].Rows.Count > 0)
                {
                    txtRoomNo.Text = roomData.Tables[0].Rows[0]["RoomNumber"].ToString();

                    string roomType = roomData.Tables[0].Rows[0]["RoomType"].ToString();
                    if (cbxRoomType.Items.Contains(roomType))
                    {
                        cbxRoomType.SelectedItem = roomType;
                    }

                    string bedType = roomData.Tables[0].Rows[0]["BedType"].ToString();
                    if (cbxBedType.Items.Contains(bedType))
                    {
                        cbxBedType.SelectedItem = bedType;
                    }

                    txtPrice.Text = roomData.Tables[0].Rows[0]["Price"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi lấy dữ liệu phòng: " + ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string roomNumber = txtRoomNo.Text;
                string roomType = cbxRoomType.SelectedItem.ToString();
                string bedType = cbxBedType.SelectedItem.ToString();
                int roomTypeID = dbConnection.GetRoomTypeId(roomType);
                int bedTypeID = dbConnection.GetBedTypeId(bedType);
                decimal price = decimal.Parse(txtPrice.Text);

                string queryUpdateRoom = "UPDATE Rooms " +
                                         "SET RoomNumber = @RoomNumber, RoomTypeID = @RoomTypeID, BedTypeID = @BedTypeID, Price = @Price " +
                                         "WHERE RoomID = @RoomID";

                dbConnection.SetData(queryUpdateRoom, "Phòng đã được cập nhật thành công!",
                                     ("@RoomNumber", roomNumber), ("@RoomTypeID", roomTypeID),
                                     ("@BedTypeID", bedTypeID), ("@Price", price), ("@RoomID", roomID));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi cập nhật thông tin phòng: " + ex.Message);
            }
        }
    }
}
