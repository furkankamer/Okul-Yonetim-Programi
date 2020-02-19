using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Media;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
namespace WindowsFormsApp1
{

    public partial class Form1 : Form
    {
        
        DateTime myDate;
        bool kayitt = false;
        Color renk;
        int ccol=0;
        int crow=0;
        bool tablekayit = false;
        bool tablealinan = false;
        bool tableacilan = false;
        int clicked = 0;
        readonly System.Media.SoundPlayer player = new System.Media.SoundPlayer(Properties.Resources.dad2);
        public Form1()
        {
            
            InitializeComponent();
            Settings.GeneralSettings = String.Empty;
            cikis_butonu.Hide();
            Panel[] pnl = { giriss_paneli, Kayit_Paneli, Ders_Olusturma_Paneli, Ders_Secim_Paneli,
                excel_paneli, ogrenci_loggin_paneli,ogretmen_loggin_paneli,email_paneli };
            foreach (Panel panel in pnl)
                panel.Hide();
            button3.Enabled = false;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.SkyBlue;
            dateTimePicker1.MinDate = DateTime.Now.AddDays((8-DateTime.Today.DayOfWeek - DayOfWeek.Sunday));
            dateTimePicker1.MaxDate = DateTime.Now.AddDays((8-DateTime.Today.DayOfWeek - DayOfWeek.Sunday)).AddDays(5);
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy-MM-dddd";
            textBox2.PasswordChar = '*';
            dataGridView1.Hide();
            textBox2.MaxLength = 10;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            dateTimePicker1.Hide();
            dateTimePicker1.CustomFormat = "yyyy-MM-dddd";
            int[] sinif = { 12, 11, 10, 9 };
            foreach(int i in sinif)
            {
                comboBox7.Items.Insert(0, i);
                comboBox8.Items.Insert(0, i);
            }
            string[] dersler = { "Ingilizce", "Biyoloji", "Kimya", "Fizik", "Matematik", "Turkce", "Edebiyat" };
            foreach(string ders in dersler)
            {
                comboBox1.Items.Insert(0, ders);
                comboBox5.Items.Insert(0, ders);
            }
            ComboBox[] cmb = { comboBox3 , comboBox4, comboBox5, comboBox2, comboBox8 };
            foreach(ComboBox com in cmb)
                com.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        
        private bool blnButtonDown = false;
        public bool Tableacilan { get => tableacilan; set => tableacilan = value; }
        public SoundPlayer Player => player;

        public Color Renk { get => renk; set => renk = value; }

        static public void Email(string konu, string icerik, string maill)
        {
            SmtpClient sc = new SmtpClient
            {
                Port = 587,
                Host = "smtp.live.com",
                EnableSsl = true,
                Timeout = 50000,
                Credentials = new NetworkCredential("furkankamer@hotmail.com", "****kollama38****")
            };
            MailMessage mail = new MailMessage
            {
                From = new MailAddress("furkankamer@hotmail.com", "furkan")
            };
            mail.To.Add(maill);
            mail.Subject = icerik;
            mail.IsBodyHtml = true;
            mail.Body = konu;
            sc.Send(mail);
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value.Date > DateTime.Now && comboBox1.Text != "" && comboBox8.Text != "" && comboBox2.Text != "" && (comboBox9.Text != "" || textBox8.Text != ""))
            {
                string name;
                if (listeden_sec.Checked)
                    name = comboBox9.Text;
                else
                    name = textBox8.Text;
                myDate = dateTimePicker1.Value.Date;
                string gun = Helpers.Hangi_Gun(dateTimePicker1);
                string date = myDate.ToString("yyyy - MM - dd ");
                date += comboBox2.Text;
                string yeniders = $@"insert into Dersler(Sınıf,DersAdi,date2,DersGünü,DersHocasi,quota,enrolled)
                                     values('{comboBox8.Text}','{comboBox1.Text}','{date}','{gun}','{name}','1','0')";
                if(Helpers.Sqlexecuter(yeniders,0) == "null")
                    MessageBox.Show("Ders Saatleri Çakışıyor. Lütfen Ders Programını Kontrol Ediniz!");
            }
            else
            {
                MessageBox.Show("Lütfen Bütün Bilgileri Eksiksiz Doldurunuz");
            }
        }

        private void DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            comboBox2.Enabled = true;
            comboBox2.Items.Clear();
            string gun = Helpers.Hangi_Gun(dateTimePicker1);
            string[,] hours = { { "14:30:00", "14:00:00", "13:00:00", "13:00:00" },{"17:30:00","17:00:00","11:10:00","10:50:00" } };
            if(gun == "Cumartesi") for(int i=0;i<4;i++) { comboBox2.Items.Insert(0, hours[0,i]); }
            else for(int i = 0; i < 4; i++) { comboBox2.Items.Insert(0, hours[1, i]); }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox9.Items.Clear();
            comboBox9.SelectedIndex = -1;
            var a = new List<string>();
            string hocalar = $@"select Hocalar.isim [Hocalar] from Hocalar
            inner join Dersler on Dersler.hoca_id = Hocalar.hoca_id
            where Dersler.DersAdi = '{comboBox1.Text}'";
            Dictionary<string, List<string>> mydict = Helpers.Sqlreaderexecuter(hocalar);
            foreach(string hoca in mydict["Hocalar"])
            {
                if(!comboBox9.Items.Contains(hoca))
                   comboBox9.Items.Insert(0, hoca);
                   a.Add(hoca);
            }
            AutoCompleteStringCollection allowedTypes = new AutoCompleteStringCollection();
            allowedTypes.AddRange(a.ToArray());
            textBox8.AutoCompleteCustomSource = allowedTypes;
            textBox8.AutoCompleteMode = AutoCompleteMode.Suggest;
            textBox8.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        
        
        private void Giris_Click(object sender, EventArgs e)
        {
            Player.Play();
            giriss_paneli.Visible = true;
            Kayit_Paneli.Visible = false;
            email_paneli.Visible = false;
            kayitt = false;
        }

        private void Kayit_Click(object sender, EventArgs e)
        {
            Player.Play();
            Kayit_Paneli.Visible = true;
            giriss_paneli.Visible = true;
            email_paneli.Visible = false;
            kayitt = true;
        }

        private void Tamam_Click(object sender, EventArgs e)
        {
            if (kayitt == true)
            {
                if((ogretmen.Checked == true && textBox4.Text == "385") || ogrenci.Checked == true)
                {
                    string yenikayit = $@"INSERT INTO Kisiler (kullaniciadi,sifre,isim,soyisim,mail,unvan)
                                        values('{textBox1.Text}','{textBox2.Text}','{textBox6.Text}',
                                        '{textBox5.Text}','{textBox3.Text}',";
                    if (ogrenci.Checked) yenikayit += $@"'{ogrenci.Text}')";
                    else yenikayit += $@"'{ogretmen.Text}')";
                    string success = Helpers.Sqlexecuter(yenikayit, 0);
                    if (success == "")
                    {
                        MessageBox.Show("Kayit Basarili Giris Yapabilirsiniz!");
                        kayitt = false;
                        textBox1.Clear();
                        textBox2.Clear();
                        Kayit_Paneli.Hide();
                    }
                    else MessageBox.Show("Kullanici adi alinmis. Lutfen tekrar deneyiniz");
                }
                
            }
            else 
            {
                string checker = $@"select sifre from Kisiler where kullaniciadi = '{textBox1.Text}'";
                string sifre = Helpers.Sqlexecuter(checker, 1);
                if (sifre == textBox2.Text)
                {
                    MessageBox.Show("Giriş Başarılı!");
                    string personget = $@"select * from Kisiler where kullaniciadi = '{textBox1.Text}'";
                    Dictionary<string, List<string>> persondict = Helpers.Sqlreaderexecuter(personget);
                    Person person = new Person(persondict, textBox1.Text);
                    Settings.GeneralSettings = JsonConvert.SerializeObject(person);
                    if (persondict["unvan"].Contains(ogretmen.Text)) ogretmen_loggin_paneli.Show();
                    else ogrenci_loggin_paneli.Show();
                    giriss_paneli.Hide();
                    Giris_Paneli.Hide();
                    Kayit_Paneli.Hide();
                    cikis_butonu.Show();
                }
                else if (sifre == "null") MessageBox.Show("böyle bir kullanici adi yok");
                else MessageBox.Show("Şifre Yanlış Lütfen Tekrar Deneyiniz");
            }

        }

        private void Ogrenci_CheckedChanged(object sender, EventArgs e)
        {
            label4.Hide();
            textBox4.Hide();
        }

        private void Ogretmen_CheckedChanged(object sender, EventArgs e)
        {
            comboBox7.Enabled = false;
            label4.Show();
            textBox4.Show();
        }
        
        private void Button6_Click(object sender, EventArgs e)
        {
            button3.Enabled = false;
            comboBox6.SelectedIndex = -1;
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.Hide();
            Tableacilan = true;
            tablealinan = false;
            tablekayit = false;
            Player.Play();
            excel_paneli.Visible = true;
            Ders_Olusturma_Paneli.Visible = false;
            string str = $@"select Hocalar.isim [Hocalar] from Hocalar
                            inner join Dersler on Dersler.hoca_id = Hocalar.hoca_id";
            Dictionary<string, List<string>> mydict = Helpers.Sqlreaderexecuter(str);
            foreach(string hocaadi in mydict["Hocalar"])
            {
                if(!comboBox6.Items.Contains(hocaadi))
                    comboBox6.Items.Insert(0, hocaadi);
            }
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Show();
            listeden_sec.Checked = true;
            radioButton2.Checked = false;
            Player.Play();
            comboBox2.Enabled = false;
            Ders_Olusturma_Paneli.Visible = true;
            excel_paneli.Visible = false;
            dataGridView1.Hide();
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            comboBox2.Enabled = true;
            comboBox2.Items.Clear();
            string gun = Helpers.Hangi_Gun(dateTimePicker1);
            string[,] hours = { { "14:30:00", "14:00:00", "13:00:00", "13:00:00" }, { "17:30:00", "17:00:00", "11:10:00", "10:50:00" } };
            if (gun == "Cumartesi") for (int i = 0; i < 4; i++) { comboBox2.Items.Insert(0, hours[0, i]); }
            else for (int i = 0; i < 4; i++) { comboBox2.Items.Insert(0, hours[1, i]); }
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            Player.Play();
            MessageBox.Show("Lütfen belirtilen alana kayıt olduğunuz mail adresinizi giriniz");
            giriss_paneli.Visible = false;
            Kayit_Paneli.Visible = false;
            email_paneli.Visible = true;
        }

        private void Button9_Click(object sender, EventArgs e)
        {
            String sql1 = $@"SELECT sifre FROM Kisiler where mail='{textBox7.Text}'";
            string obj = Helpers.Sqlexecuter(sql1, 1);
            if (obj == "null")
            {
                MessageBox.Show("email adresi yanlış lütfen tekrar deneyiniz");
            }
            else
            {
                Email(obj.ToString(), "sifreniz", textBox7.Text);
                MessageBox.Show("sifreniz email adresinize gönderilmiştir");
            }

        }
        
        private void Button10_Click(object sender, EventArgs e)
        {
            Settings.GeneralSettings = String.Empty;
            clicked = 0;
            Player.Play();
            ComboBox[] cmb = { comboBox2, comboBox3, comboBox4, comboBox9, comboBox6 };
            foreach(ComboBox combo in cmb)
            {
                combo.Items.Clear();
                combo.SelectedIndex = -1;
            }
            Giris_Paneli.Show();
            Panel[] pnl = { ogrenci_loggin_paneli , ogretmen_loggin_paneli , Ders_Olusturma_Paneli, Ders_Secim_Paneli, excel_paneli,
            giriss_paneli,Kayit_Paneli,email_paneli};
            foreach (Panel panel in pnl)
                panel.Hide();
            dataGridView1.Hide();
            cikis_butonu.Hide();
        }


        private void Button15_Click(object sender, EventArgs e)
        {
            tablealinan = true;
            tablekayit= false;
            Tableacilan = false;
            Player.Play();
            Ders_Secim_Paneli.Visible = false;
            string[] headerscol = { "Pazartesi", "Sali", "Çarşamba", "Persembe", "Cuma", "Cumartesi" };
            string[] hoursrow = { "10:50:00", "11:10:00", "13:00:00", "13:30:00", "14:00:00", "14:30:00", "17:00:00", "17:30:00" };
            Helpers.Datagridviewformatter(dataGridView1, headerscol, hoursrow);
            Person person = JsonConvert.DeserializeObject<Person>(Settings.GeneralSettings);
            string dersler = $@"select Dersler.DersGünü,Dersler.DersAdi, Dersler.hoca_id[hocaid], cast(Dersler.date2 as time(0))[time]
                                from Dersler inner join DersKayit on DersKayit.ders_id = Dersler.Ders_ID
                                where derskayit.student_id = '{person.İd}' ";
            Dictionary<string, List<string>> mydict = Helpers.Sqlreaderexecuter(dersler);
            int timec = mydict["time"].Count;
            for (int i = 0; i < timec; i++)
            {
                int[] indexler = Helpers.Datagridcellreturner(dataGridView1, mydict["DersGünü"][i], mydict["time"][i]);
                string hocaname = $"select isim from Hocalar where Hoca_id = '{mydict["hocaid"][i]}'";
                hocaname = Helpers.Sqlexecuter(hocaname, 1);
                dataGridView1.Rows[indexler[0]].Cells[indexler[1]].Value = mydict["DersAdi"][i] + "\n" + hocaname;
                dataGridView1.Rows[indexler[0]].Cells[indexler[1]].Style.BackColor = Color.Blue;
            }
            MessageBox.Show("Bırakmak İstediğiniz Dersin Üzerine Çift Tıklayınız");
        }
        private void Button16_Click(object sender, EventArgs e)
        {
            clicked++;
            tablekayit = true;
            tablealinan = false;
            Tableacilan = false;
            Player.Play();
            dataGridView1.Hide();
            Ders_Secim_Paneli.Visible = true;
            comboBox5.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            comboBox4.SelectedIndex = -1;
            comboBox4.Enabled = false;
            comboBox3.Enabled = false;

        }

        private void Button14_Click(object sender, EventArgs e)
        {
            Player.Play();
            if (comboBox3.Text != "" && comboBox4.Text != "")
            {
                string[] headerscol = { comboBox4.Text };
                string[] hoursrow = { "10:50:00", "11:10:00", "13:00:00", "13:30:00", "14:00:00", "14:30:00", "17:00:00", "17:30:00" };
                Helpers.Datagridviewformatter(dataGridView1,hoursrow,headerscol);
                string str1 = $"select cast(date2 as time(0))[date] from Dersler where Quota != Enrolled " +
                        $"and Hoca_id = (select hoca_id from Hocalar where isim = '{comboBox3.Text}') " +
                        $"and DersGünü = '{comboBox4.Text}'";
                Dictionary<string,List<string>> mydict = Helpers.Sqlreaderexecuter(str1);
                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                {
                    
                    if (mydict["date"].Contains(dataGridView1.Columns[j].HeaderText))
                    {
                            
                            dataGridView1.Rows[0].Cells[j].Value = "Seç";
                            dataGridView1.Rows[0].Cells[j].Style.BackColor = Color.Green;
                    }
                    else
                    {
                            dataGridView1.Rows[0].Cells[j].Value = "Dolu";
                            dataGridView1.Rows[0].Cells[j].Style.BackColor = Color.Red;
                    }
                }
                
            }
            else
            {
                MessageBox.Show("Lütfen listeden hoca ve günü seçiniz");
            }
        }

        private void ComboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox3.Items.Clear();
            comboBox4.Items.Clear();
            string str = $@"select Hocalar.isim [Hocalar] from Hocalar
                        inner join Dersler on Dersler.hoca_id = Hocalar.hoca_id
                        where Dersler.DersAdi = '{comboBox5.Text}' and Dersler.Enrolled != Dersler.Quota";
            Dictionary<string, List<string>> mydict = Helpers.Sqlreaderexecuter(str);
            foreach(string hocaadi in mydict["Hocalar"])
            {
                if (!comboBox3.Items.Contains(hocaadi))
                    comboBox3.Items.Insert(0, hocaadi);
            }
            if(comboBox3.Items.Count == 0 && clicked > 1)
            {
                comboBox3.Enabled = false;
                comboBox4.ResetText();
                comboBox4.Enabled = false;
            }
            else if(comboBox3.Items.Count == 0)
            {
                comboBox3.Enabled = false;
                comboBox4.ResetText();
                MessageBox.Show("Sectiginiz Branşta Açık Ders Bulunmamakta Lütfen Farklı Bir Branş Seçiniz!");
                comboBox4.Enabled = false;
            }
            else
            {
                comboBox3.Enabled = true;
            }

        }

        private void ComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox4.Items.Clear();
            comboBox4.SelectedIndex = -1;
            string str = $"Select DersGünü from Dersler where Quota != Enrolled and DersAdi = '{comboBox5.Text}' and hoca_id = (select Hoca_id from Hocalar" +
                $" where isim ='{comboBox3.Text}')";
            Dictionary<string, List<string>> mydict = Helpers.Sqlreaderexecuter(str);
            foreach (string hocaadi in mydict["DersGünü"])
            {
                if (!comboBox4.Items.Contains(hocaadi))
                {
                    comboBox4.Items.Insert(0, hocaadi);
                }
            }

            if (comboBox4.Items.Count == 0)
            {
                comboBox4.Enabled = false;
                comboBox4.ResetText();
            }
            else
            {
                comboBox4.Enabled = true;
            }


        }
        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.Enabled = false;
            int rowind = dataGridView1.CurrentCell.RowIndex;
            int colind = dataGridView1.CurrentCell.ColumnIndex;
            if (tablekayit && dataGridView1.CurrentCell.Style.BackColor == Color.Green)
            {
                string dersgünü = dataGridView1.Rows[rowind].HeaderCell.Value.ToString();
                string derssaati = dataGridView1.Columns[colind].HeaderText;
                Person person = JsonConvert.DeserializeObject<Person>(Settings.GeneralSettings);
                string hocaid = $"select Hoca_id from Hocalar where isim = '{comboBox3.Text}'";
                hocaid = Helpers.Sqlexecuter(hocaid, 1);
                string ders_id = $@"select Ders_ID from Dersler where hoca_id = '{hocaid}'
                                    and DersGünü = '{dersgünü}' and DersAdi = '{comboBox5.Text}'
                                    and cast(date2 as time(0)) = '{derssaati}'";
                ders_id = Helpers.Sqlexecuter(ders_id, 1);
                if(ders_id == "null") return;
                string checkcollision = $@"select Dersler.DersGünü, cast(date2 as time(0))[time] from
                                           Dersler inner join Derskayit on Derskayit.ders_id = Dersler.Ders_ID
                                           where derskayit.student_id = '{person.İd}'";
                Dictionary<string, List<string>> mydict = Helpers.Sqlreaderexecuter(checkcollision);
                int countt = mydict["DersGünü"].Count;
                for (int i=0;i<countt;i++)
                {
                    if(mydict["DersGünü"][i] == dersgünü && mydict["time"][i] == derssaati)
                    {
                        MessageBox.Show("Ders Saatleriniz Çakışıyor");
                        dataGridView1.Enabled = true;
                        return;
                    }
                }

                string insertders = $@"insert into derskayit(student_id,ders_id) values('{person.İd}','{ders_id}')";
                string updateenroll = $@"update Dersler set enrolled = '1' where Ders_ID = '{ders_id}'";
                if (Helpers.Sqlexecuter(insertders, 0) != "null")
                    Helpers.Sqlexecuter(updateenroll, 0);
                else
                    MessageBox.Show("Basarisiz!");
            }
            dataGridView1.Enabled = true;
        }


        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.Enabled = false;
            tablealinan = true;
            if (tablealinan)
            { 
            int rowind = dataGridView1.CurrentCell.RowIndex;
            int colind = dataGridView1.CurrentCell.ColumnIndex;
                    DialogResult dr = MessageBox.Show("Dersi İptal Et",
                          "Ders İptali", MessageBoxButtons.YesNo);
                    switch (dr)
                    {
                        case DialogResult.Yes:
                            string cellval = dataGridView1.Rows[rowind].Cells[colind].Value.ToString();
                            string dersadi = cellval.Split('\n')[0];
                            string dershocasi = cellval.Split('\n')[1];
                            string dersgunu = dataGridView1.Columns[colind].HeaderText;
                            string derssaati = dataGridView1.Rows[rowind].HeaderCell.Value.ToString();
                            Person person = JsonConvert.DeserializeObject<Person>(Settings.GeneralSettings);
                            string unenroll = $@"delete from DersKayit where student_id = {person.İd} and
                                                 ders_id = (select Ders_ID from Dersler where 
                                                  hoca_id = (select Hoca_id from Hocalar where isim = '{dershocasi}') and DersAdi = '{dersadi}'
                                                  and cast(date2 as time(0)) = '{derssaati}'
                                                  and DersGünü = '{dersgunu}')";
                        if (Helpers.Sqlexecuter(unenroll, 0) == "null")
                            {
                            MessageBox.Show("Basarisiz!");
                            dataGridView1.Enabled = true;
                            return;
                            }
                            dataGridView1.Rows[rowind].Cells[colind].ReadOnly = true;
                            dataGridView1.Rows[rowind].Cells[colind].Value = null;
                            dataGridView1.ClearSelection();
                            break;
                        case DialogResult.No:
                            break;
                    }
                
            }
            dataGridView1.Enabled = true;
        }

        private void DataGridView1_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            int rowc = dataGridView1.Rows.Count;
            int colc = dataGridView1.Columns.Count;
            if (e.RowIndex < rowc && e.ColumnIndex < colc && e.RowIndex >= 0 && e.ColumnIndex >= 0 && dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor != Color.DarkGray)
            {

                if(e.RowIndex != crow || e.ColumnIndex != ccol)
                {
                    crow = e.RowIndex;
                    ccol = e.ColumnIndex;
                    Renk = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor;
                }
                
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.LightSkyBlue;
            }
            
            
        }

        private void DataGridView1_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            int rowc = dataGridView1.Rows.Count;
            int colc = dataGridView1.Columns.Count;
            if (e.RowIndex < rowc && e.ColumnIndex < colc && e.RowIndex >= 0 && e.ColumnIndex >= 0 && dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor != Color.DarkGray)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Dolu")
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Red;
                else if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Seç")
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Green;
                else
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Renk;
            }
        }

        private void Button15_Paint(object sender, PaintEventArgs e)
        {
            if (blnButtonDown == false)
            {
                    ControlPaint.DrawBorder(e.Graphics, (sender as System.Windows.Forms.Button).ClientRectangle,
                    System.Drawing.SystemColors.ActiveBorder, 1, ButtonBorderStyle.Outset,
                    System.Drawing.SystemColors.ActiveBorder, 1, ButtonBorderStyle.Outset,
                    System.Drawing.SystemColors.ActiveBorder, 1, ButtonBorderStyle.Outset,
                    System.Drawing.SystemColors.ActiveBorder, 1, ButtonBorderStyle.Outset);
            }
            else
            {
                    ControlPaint.DrawBorder(e.Graphics, (sender as System.Windows.Forms.Button).ClientRectangle,
                    System.Drawing.SystemColors.ActiveBorder, 1, ButtonBorderStyle.Inset,
                    System.Drawing.SystemColors.ActiveBorder, 1, ButtonBorderStyle.Inset,
                    System.Drawing.SystemColors.ActiveBorder, 1, ButtonBorderStyle.Inset,
                    System.Drawing.SystemColors.ActiveBorder, 1, ButtonBorderStyle.Inset);
            }
        }

        private void Button15_MouseDown(object sender, MouseEventArgs e)
        {
            blnButtonDown = true;
            (sender as System.Windows.Forms.Button).Invalidate();
        }

        private void Button15_MouseUp(object sender, MouseEventArgs e)
        {
            blnButtonDown = false;
            (sender as System.Windows.Forms.Button).Invalidate();
        }

        private void ComboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox6.Text !="")
            {
                button3.Enabled = true;
                string[] headerscol = {"Pazartesi","Sali","Çarşamba","Persembe","Cuma","Cumartesi" };
                string[] hoursrow = {"10:50:00","11:10:00","13:00:00","13:30:00","14:00:00","14:30:00","17:00:00","17:30:00" };
                Helpers.Datagridviewformatter(dataGridView1, headerscol, hoursrow);
                string schedule = $"select cast(date2 as time(0))[time],DersGünü, Enrolled from Dersler where " +
                                  $"Hoca_id = (select hoca_id from Hocalar where isim = '{comboBox6.Text}')";
                Dictionary<string, List<string>> mydict = Helpers.Sqlreaderexecuter(schedule);
                int timec = mydict["time"].Count;
                for(int i=0;i<timec;i++)
                {
                    int[] indexler = Helpers.Datagridcellreturner(dataGridView1, mydict["DersGünü"][i], mydict["time"][i]);
                    dataGridView1.Rows[indexler[0]].Cells[indexler[1]].Value = "Kayitli Ogrenci: " + mydict["Enrolled"][i];
                    dataGridView1.Rows[indexler[0]].Cells[indexler[1]].Style.BackColor = Color.Green;
                }
            }
        }

        private void TextBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = (e.KeyChar == (char)Keys.Space);
        }
        private void CopyAlltoClipboard()
        {
            dataGridView1.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            dataGridView1.MultiSelect = true;
            dataGridView1.SelectAll();
            DataObject dataObj = dataGridView1.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
        }
        private void Button3_Click_1(object sender, EventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog
            {
                FileName = comboBox6.Text,
                Filter = "Excel Files(*.xlsx)|"
            };
            if (savefile.ShowDialog() == DialogResult.OK)
            {
                CopyAlltoClipboard();
                Microsoft.Office.Interop.Excel.Application xlexcel;
                Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
                Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
                object misValue = System.Reflection.Missing.Value;
                xlexcel = new Excel.Application
                {
                    Visible = true
                };
                xlWorkBook = xlexcel.Workbooks.Add(misValue);
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                Excel.Range CR = (Excel.Range)xlWorkSheet.Cells[1, 1];
                CR.Select();
                xlWorkSheet.PasteSpecial(CR, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);
                xlWorkSheet.Columns.AutoFit();
                xlWorkSheet.SaveAs($@"{savefile.FileName}.xlsx");
            }
            
        }

        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(listeden_sec.Checked)
            {
                radioButton2.Checked = false;
                textBox8.Text = "";
                comboBox9.Show();
                comboBox9.Items.Clear();
                Dictionary<string, List<string>> mydict = Helpers.Sqlreaderexecuter($"select hoca_id[hocaid] from Dersler where DersAdi = '{comboBox1.Text}'");
                foreach(string hocadata in mydict["hocaid"])
                {
                    string hocaisim = $"select isim from Hocalar where hoca_id = '{hocadata}'";
                    hocaisim = Helpers.Sqlexecuter(hocaisim, 1);
                    comboBox9.Items.Insert(0, hocaisim);
                }
                textBox8.Hide();
            }
            
        }

        private void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton2.Checked)
            {
                comboBox9.SelectedIndex = -1;
                listeden_sec.Checked = false;
                textBox8.Show();
                comboBox9.Hide();
            }
            
        }

    }
}
