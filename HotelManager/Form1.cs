using System;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;
using Azure.Identity;
using HotelManager;


namespace HotelManager
{
    public partial class Form1 : Form
    {
        private Connection dbConnection;

        public Form1()
        {
            InitializeComponent();
            dbConnection = new Connection();

            // Đặt thuộc tính PasswordChar để ẩn mật khẩu
            txtPassword.PasswordChar = '*';

            // Thiết lập sự kiện KeyDown cho txtPassword
            txtPassword.KeyDown += TxtPassword_KeyDown;

        }

        private void TxtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            // Bắt sự kiện khi nhấn phím Enter trong ô mật khẩu
            if (e.KeyCode == Keys.Enter)
            {
                // Gọi sự kiện của nút đăng nhập
                btnLogin_Click(sender, e);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (CheckLogin(username, password))
            {
                Dashboard f = new Dashboard();
                f.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Sai tài khoản hoặc mật khẩu");
                txtPassword.Clear();
            }
        }

        private bool CheckLogin(string username, string password)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username AND Password = @Password";

                int count = Convert.ToInt32(dbConnection.GetData(query, ("@Username", username), ("@Password", password)).Tables[0].Rows[0][0]);

                return count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi kiểm tra đăng nhập: " + ex.Message);
                return false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
