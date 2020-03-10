namespace WindowsFormsApp1
{
    partial class Form2
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
            this.kullanici_adi = new System.Windows.Forms.Label();
            this.kullaniciadi_text = new System.Windows.Forms.TextBox();
            this.sifre = new System.Windows.Forms.Label();
            this.sifre_text = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.email_text = new System.Windows.Forms.TextBox();
            this.kaydet = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // kullanici_adi
            // 
            this.kullanici_adi.AutoSize = true;
            this.kullanici_adi.BackColor = System.Drawing.Color.Brown;
            this.kullanici_adi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.kullanici_adi.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.25F);
            this.kullanici_adi.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.kullanici_adi.Location = new System.Drawing.Point(-1, 28);
            this.kullanici_adi.Name = "kullanici_adi";
            this.kullanici_adi.Size = new System.Drawing.Size(138, 26);
            this.kullanici_adi.TabIndex = 45;
            this.kullanici_adi.Text = "Kullanıcı Adı:";
            // 
            // kullaniciadi_text
            // 
            this.kullaniciadi_text.BackColor = System.Drawing.Color.Brown;
            this.kullaniciadi_text.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.kullaniciadi_text.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.kullaniciadi_text.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.kullaniciadi_text.Location = new System.Drawing.Point(152, 28);
            this.kullaniciadi_text.Name = "kullaniciadi_text";
            this.kullaniciadi_text.Size = new System.Drawing.Size(100, 29);
            this.kullaniciadi_text.TabIndex = 46;
            // 
            // sifre
            // 
            this.sifre.AutoSize = true;
            this.sifre.BackColor = System.Drawing.Color.Brown;
            this.sifre.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.sifre.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.25F);
            this.sifre.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.sifre.Location = new System.Drawing.Point(-1, 59);
            this.sifre.Name = "sifre";
            this.sifre.Size = new System.Drawing.Size(63, 26);
            this.sifre.TabIndex = 47;
            this.sifre.Text = "Şifre:";
            // 
            // sifre_text
            // 
            this.sifre_text.BackColor = System.Drawing.Color.Brown;
            this.sifre_text.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sifre_text.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.sifre_text.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.sifre_text.Location = new System.Drawing.Point(152, 59);
            this.sifre_text.Name = "sifre_text";
            this.sifre_text.PasswordChar = '*';
            this.sifre_text.Size = new System.Drawing.Size(100, 29);
            this.sifre_text.TabIndex = 48;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Brown;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.25F);
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(-1, 87);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 26);
            this.label1.TabIndex = 49;
            this.label1.Text = "Email:";
            // 
            // email_text
            // 
            this.email_text.BackColor = System.Drawing.Color.Brown;
            this.email_text.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.email_text.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.email_text.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.email_text.Location = new System.Drawing.Point(152, 87);
            this.email_text.Name = "email_text";
            this.email_text.Size = new System.Drawing.Size(270, 29);
            this.email_text.TabIndex = 50;
            // 
            // kaydet
            // 
            this.kaydet.BackColor = System.Drawing.Color.Brown;
            this.kaydet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.kaydet.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.25F);
            this.kaydet.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.kaydet.Location = new System.Drawing.Point(289, 133);
            this.kaydet.Name = "kaydet";
            this.kaydet.Size = new System.Drawing.Size(120, 35);
            this.kaydet.TabIndex = 51;
            this.kaydet.Text = "Kaydet";
            this.kaydet.UseVisualStyleBackColor = false;
            this.kaydet.Click += new System.EventHandler(this.Kaydet_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 218);
            this.Controls.Add(this.kaydet);
            this.Controls.Add(this.email_text);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.sifre_text);
            this.Controls.Add(this.sifre);
            this.Controls.Add(this.kullaniciadi_text);
            this.Controls.Add(this.kullanici_adi);
            this.Name = "Form2";
            this.Text = "Form2";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label kullanici_adi;
        private System.Windows.Forms.TextBox kullaniciadi_text;
        private System.Windows.Forms.Label sifre;
        private System.Windows.Forms.TextBox sifre_text;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox email_text;
        private System.Windows.Forms.Button kaydet;
    }
}