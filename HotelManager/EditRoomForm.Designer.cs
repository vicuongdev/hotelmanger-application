namespace HotelManager
{
    partial class EditRoomForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            cbxBedType = new ComboBox();
            label4 = new Label();
            txtRoomNo = new Guna.UI2.WinForms.Guna2TextBox();
            cbxRoomType = new ComboBox();
            btnSave = new Guna.UI2.WinForms.Guna2Button();
            txtPrice = new Guna.UI2.WinForms.Guna2TextBox();
            label5 = new Label();
            label3 = new Label();
            label2 = new Label();
            SuspendLayout();
            // 
            // cbxBedType
            // 
            cbxBedType.FormattingEnabled = true;
            cbxBedType.Location = new Point(318, 244);
            cbxBedType.Name = "cbxBedType";
            cbxBedType.Size = new Size(341, 23);
            cbxBedType.TabIndex = 26;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Century Gothic", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            label4.ForeColor = Color.Black;
            label4.Location = new Point(318, 208);
            label4.Name = "label4";
            label4.Size = new Size(95, 22);
            label4.TabIndex = 25;
            label4.Text = "Bed Type";
            // 
            // txtRoomNo
            // 
            txtRoomNo.CustomizableEdges = customizableEdges1;
            txtRoomNo.DefaultText = "";
            txtRoomNo.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtRoomNo.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtRoomNo.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtRoomNo.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtRoomNo.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtRoomNo.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            txtRoomNo.ForeColor = Color.Black;
            txtRoomNo.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtRoomNo.Location = new Point(318, 86);
            txtRoomNo.Margin = new Padding(4, 2, 4, 2);
            txtRoomNo.Name = "txtRoomNo";
            txtRoomNo.PasswordChar = '\0';
            txtRoomNo.PlaceholderText = "";
            txtRoomNo.SelectedText = "";
            txtRoomNo.ShadowDecoration.CustomizableEdges = customizableEdges2;
            txtRoomNo.Size = new Size(348, 31);
            txtRoomNo.TabIndex = 24;
            // 
            // cbxRoomType
            // 
            cbxRoomType.FormattingEnabled = true;
            cbxRoomType.Location = new Point(318, 171);
            cbxRoomType.Name = "cbxRoomType";
            cbxRoomType.Size = new Size(345, 23);
            cbxRoomType.TabIndex = 23;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.Transparent;
            btnSave.BorderColor = Color.White;
            btnSave.BorderRadius = 20;
            btnSave.BorderStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            btnSave.CheckedState.FillColor = Color.FromArgb(0, 64, 64);
            btnSave.CheckedState.ForeColor = Color.White;
            btnSave.CustomizableEdges = customizableEdges3;
            btnSave.DisabledState.BorderColor = Color.DarkGray;
            btnSave.DisabledState.CustomBorderColor = Color.DarkGray;
            btnSave.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnSave.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnSave.FillColor = Color.LightGray;
            btnSave.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point);
            btnSave.ForeColor = Color.Black;
            btnSave.Location = new Point(512, 370);
            btnSave.Margin = new Padding(3, 2, 3, 2);
            btnSave.Name = "btnSave";
            btnSave.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnSave.Size = new Size(154, 45);
            btnSave.TabIndex = 22;
            btnSave.Text = "Save Room";
            btnSave.Click += btnSave_Click;
            // 
            // txtPrice
            // 
            txtPrice.CustomizableEdges = customizableEdges5;
            txtPrice.DefaultText = "";
            txtPrice.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtPrice.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtPrice.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtPrice.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtPrice.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtPrice.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            txtPrice.ForeColor = Color.Black;
            txtPrice.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtPrice.Location = new Point(318, 320);
            txtPrice.Margin = new Padding(4, 2, 4, 2);
            txtPrice.Name = "txtPrice";
            txtPrice.PasswordChar = '\0';
            txtPrice.PlaceholderText = "";
            txtPrice.SelectedText = "";
            txtPrice.ShadowDecoration.CustomizableEdges = customizableEdges6;
            txtPrice.Size = new Size(348, 31);
            txtPrice.TabIndex = 21;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Century Gothic", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            label5.ForeColor = Color.Black;
            label5.Location = new Point(318, 285);
            label5.Name = "label5";
            label5.Size = new Size(54, 22);
            label5.TabIndex = 20;
            label5.Text = "Price";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Century Gothic", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            label3.ForeColor = Color.Black;
            label3.Location = new Point(318, 132);
            label3.Name = "label3";
            label3.Size = new Size(112, 22);
            label3.TabIndex = 19;
            label3.Text = "Room Type";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Century Gothic", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            label2.ForeColor = Color.Black;
            label2.Location = new Point(318, 57);
            label2.Name = "label2";
            label2.Size = new Size(142, 22);
            label2.TabIndex = 18;
            label2.Text = "Room Number";
            // 
            // EditRoomForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(984, 472);
            Controls.Add(cbxBedType);
            Controls.Add(label4);
            Controls.Add(txtRoomNo);
            Controls.Add(cbxRoomType);
            Controls.Add(btnSave);
            Controls.Add(txtPrice);
            Controls.Add(label5);
            Controls.Add(label3);
            Controls.Add(label2);
            Margin = new Padding(3, 2, 3, 2);
            Name = "EditRoomForm";
            Text = "EditRoom";
            Load += EditRoomForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox cbxBedType;
        private Label label4;
        private Guna.UI2.WinForms.Guna2TextBox txtRoomNo;
        private ComboBox cbxRoomType;
        private Guna.UI2.WinForms.Guna2Button btnSave;
        private Guna.UI2.WinForms.Guna2TextBox txtPrice;
        private Label label5;
        private Label label3;
        private Label label2;
    }
}