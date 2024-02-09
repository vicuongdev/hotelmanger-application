using Microsoft.Data.SqlClient;
using System.Data;

namespace HotelManager
{
    public class Connection
    {
        protected SqlConnection GetConnection()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = "Data Source=VICUONG;Initial Catalog=HotelManagement;User Id=sa;Password=vicuong0110;TrustServerCertificate=True;";
            return con;
        }

        public DataSet GetData(string query, params (string, object)[] parameters)
        {
            using (SqlConnection con = GetConnection())
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();

                    foreach ((string paramName, object paramValue) in parameters)
                    {
                        cmd.Parameters.AddWithValue(paramName, paramValue);
                    }

                    cmd.CommandText = query;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    return ds;
                }
            }
        }

        public void SetData(string query, string message, params (string, object)[] parameters)
        {
            using (SqlConnection con = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();

                    foreach ((string paramName, object paramValue) in parameters)
                    {
                        cmd.Parameters.AddWithValue(paramName, paramValue);
                    }

                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show(message, "Success SetData", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public int GetRoomTypeId(string roomType)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT RoomTypeID FROM RoomType WHERE TypeName = @RoomType";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RoomType", roomType);
                    return (int)command.ExecuteScalar();
                }
            }
        }

        public int GetBedTypeId(string bedType)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT BedTypeID FROM BedType WHERE TypeName = @BedType";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BedType", bedType);
                    return (int)command.ExecuteScalar();
                }
            }
        }

        public int GetRoomId(string roomNumber)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT RoomID FROM Rooms WHERE RoomNumber = @RoomNumber";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RoomNumber", roomNumber);
                    var result = command.ExecuteScalar();
                    return result != null ? (int)result : -1;
                }
            }
        }

        public object ExecuteScalar(string query, params (string, object)[] parameters)
        {
            using (SqlConnection con = GetConnection())
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();

                    foreach ((string paramName, object paramValue) in parameters)
                    {
                        cmd.Parameters.AddWithValue(paramName, paramValue);
                    }

                    cmd.CommandText = query;

                    return cmd.ExecuteScalar();
                }
            }
        }

    }
}