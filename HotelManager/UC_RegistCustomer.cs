using System;
using System.Data;
using System.Windows.Forms;

namespace HotelManager
{
    public partial class UC_RegistCustomer : UserControl
    {
        private Connection dbConnection;

        public UC_RegistCustomer()
        {
            dbConnection = new Connection();
            InitializeComponent();
            LoadRoomTypes();
            LoadBedTypes();
            cbxRoomType.SelectedIndexChanged += CbxRoomType_SelectedIndexChanged;
            cbxBedType.SelectedIndexChanged += CbxBedType_SelectedIndexChanged;
            cbxRoomNo.SelectedIndexChanged += CbxRoomNo_SelectedIndexChanged;
        }

        private void LoadRoomTypes()
        {
            try
            {
                DataSet roomTypesData = dbConnection.GetData("SELECT TypeName FROM RoomType");

                foreach (DataRow row in roomTypesData.Tables[0].Rows)
                {
                    cbxRoomType.Items.Add(row["TypeName"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading Room Types: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadBedTypes()
        {
            try
            {
                DataSet bedTypesData = dbConnection.GetData("SELECT TypeName FROM BedType");

                foreach (DataRow row in bedTypesData.Tables[0].Rows)
                {
                    cbxBedType.Items.Add(row["TypeName"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading Bed Types: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CbxRoomType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadRoomNumbers();
            LoadPrice();
        }

        private void CbxBedType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadRoomNumbers();
            LoadPrice();
        }

        private void CbxRoomNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPrice();
        }

        private void LoadRoomNumbers()
        {
            try
            {
                string selectedRoomType = cbxRoomType.SelectedItem?.ToString();
                string selectedBedType = cbxBedType.SelectedItem?.ToString();

                if (selectedRoomType != null && selectedBedType != null)
                {
                    int roomTypeId = dbConnection.GetRoomTypeId(selectedRoomType);
                    int bedTypeId = dbConnection.GetBedTypeId(selectedBedType);

                    string query = "SELECT RoomNumber FROM Rooms WHERE RoomTypeID = @RoomTypeID AND BedTypeID = @BedTypeID AND Booked = 0";
                    DataSet roomNumbersData = dbConnection.GetData(query, ("@RoomTypeID", roomTypeId), ("@BedTypeID", bedTypeId));

                    cbxRoomNo.Items.Clear();
                    foreach (DataRow row in roomNumbersData.Tables[0].Rows)
                    {
                        cbxRoomNo.Items.Add(row["RoomNumber"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading Room Numbers: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadPrice()
        {
            try
            {
                string selectedRoomType = cbxRoomType.SelectedItem?.ToString();
                string selectedBedType = cbxBedType.SelectedItem?.ToString();
                string selectedRoomNumber = cbxRoomNo.SelectedItem?.ToString();

                if (selectedRoomType != null && selectedBedType != null && selectedRoomNumber != null)
                {
                    int roomTypeId = dbConnection.GetRoomTypeId(selectedRoomType);
                    int bedTypeId = dbConnection.GetBedTypeId(selectedBedType);

                    string query = "SELECT Price FROM Rooms WHERE RoomTypeID = @RoomTypeID AND BedTypeID = @BedTypeID AND RoomNumber = @RoomNumber";
                    object price = dbConnection.ExecuteScalar(query, ("@RoomTypeID", roomTypeId), ("@BedTypeID", bedTypeId), ("@RoomNumber", selectedRoomNumber));

                    if (price != null && price != DBNull.Value)
                    {
                        txtPrice.Text = price.ToString();
                    }
                    else
                    {
                        txtPrice.Text = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading Price: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            try
            {
                string customerName = txtName.Text;
                string mobile = txtPhoneNumber.Text;
                string nationality = txtNationality.Text;
                string gender = cbxGender.SelectedItem?.ToString();
                DateTime dob = DateDOB.Value;
                string idProof = txtIDProof.Text;
                string address = txtAddress.Text;
                DateTime checkIn = DateCheckIn.Value;
                DateTime? checkOut = DateCheckOut.Value;
                string roomNumber = cbxRoomNo.SelectedItem?.ToString();

                if (checkIn != null && checkOut != null && checkOut > checkIn)
                {
                    int roomId = dbConnection.GetRoomId(roomNumber);
                    if (roomId != -1)
                    {
                        string query = "INSERT INTO Customer (CustomerName, Mobile, Nationality, Gender, DOB, IDProof, Address, Checkin, Checkout, RoomID) " +
                                       "VALUES (@CustomerName, @Mobile, @Nationality, @Gender, @DOB, @IDProof, @Address, @Checkin, @Checkout, @RoomID)";

                        dbConnection.SetData(query, "Đã thêm khách hàng thành công",
                            ("@CustomerName", customerName),
                            ("@Mobile", mobile),
                            ("@Nationality", nationality),
                            ("@Gender", gender),
                            ("@DOB", dob),
                            ("@IDProof", idProof),
                            ("@Address", address),
                            ("@Checkin", checkIn),
                            ("@Checkout", checkOut),
                            ("@RoomID", roomId));

                        string updateQuery = "UPDATE Rooms SET Booked = 1 WHERE RoomNumber = @RoomNumber";
                        dbConnection.SetData(updateQuery, "Đã cập nhật trạng thái phòng", ("@RoomNumber", roomNumber));
                    }
                    else
                    {
                        MessageBox.Show("Phòng đã được đặt hoặc không tồn tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn đủ giá trị cho Check-In và Check-Out, và ngày Check-Out phải sau ngày Check-In.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding customer: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefesh_Click(object sender, EventArgs e)
        {
            ResetForm();
            LoadRoomNumbers();
            LoadPrice();
        }


        private void ResetForm()
        {
            txtName.Text = string.Empty;
            txtPhoneNumber.Text = string.Empty;
            txtNationality.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtIDProof.Text = string.Empty; 
            cbxGender.SelectedIndex = -1;
            txtPrice.Text = string.Empty;   
            cbxRoomNo.SelectedIndex = -1;
            cbxRoomType.SelectedIndex = -1;
            cbxBedType.SelectedIndex = -1;  
            
        }
    }
}
