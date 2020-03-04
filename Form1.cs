using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
namespace WindowsFormsApp1
{

    public partial class Form1 : Form
    {
        int ccol = 0;
        int crow = 0;
        public Form1()
        {
            InitializeComponent();
            Settings.GeneralSettings = string.Empty;
            Helpers.DateTimePickerFormatter(dateTimePicker1);
            textBox2.PasswordChar = '*';
            textBox2.MaxLength = 10;
            string[] sinif = { "12", "11", "10", "9"};
            comboBox7.Items.AddRange(sinif);
            comboBox8.Items.AddRange(sinif);
            string[] dersler = { "Ingilizce", "Biyoloji", "Kimya", "Fizik", "Matematik", "Turkce", "Edebiyat" };
            comboBox1.Items.AddRange(dersler);
            comboBox5.Items.AddRange(dersler);
            ComboBox[] cmb = { kullanici_tipi,comboBox1, comboBox3 , comboBox4, comboBox5, comboBox2, comboBox8 };
            Helpers.combobox_dropdown(cmb);
            kullanici_tipi.Items.Add("Ogrenci");
            kullanici_tipi.Items.Add("Ogretmen");
        }
        
        private bool blnButtonDown = false;
        public SoundPlayer Player { get; } = new SoundPlayer(Properties.Resources.dad2);
        public Color Renk { get; set; }

        private void Button1_Click(object sender, EventArgs e)
        {
            
            if (dateTimePicker1.Value.Date > DateTime.Now && comboBox1.Text != "" && comboBox8.Text != "" && comboBox2.Text != "" && (comboBox9.Text != "" || textBox8.Text != ""))
            {
                string name;
                if (listeden_sec.Checked) name = comboBox9.Text;
                else name = textBox8.Text;
                DateTime myDate = dateTimePicker1.Value.Date;
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
            string[] hourscmt = { "14:30:00", "14:00:00", "13:00:00", "13:00:00" };
            string[] hours_weekday = {"17:30:00","17:00:00","11:10:00","10:50:00" };
            if (gun == "Cumartesi")
                comboBox2.Items.AddRange(hourscmt);
            else
                comboBox2.Items.AddRange(hours_weekday);
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
            string[] hocas = mydict["hocalar"].ToArray();
            mydict["hocalar"] = mydict["hocalar"].Distinct().ToList();
            comboBox9.Items.AddRange(hocas);
            a.AddRange(hocas);
            AutoCompleteStringCollection allowedTypes = new AutoCompleteStringCollection();
            allowedTypes.AddRange(a.ToArray());
            textBox8.AutoCompleteCustomSource = allowedTypes;
            textBox8.AutoCompleteMode = AutoCompleteMode.Suggest;
            textBox8.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        
        
        private void Giris_Click(object sender, EventArgs e)
        {
            Player.Play();
            giriss_paneli.Show();
            Control[] controls = { Kayit_Paneli , email_paneli };
            Helpers.control_hide(controls);
            tamam.Show();
        }

        private void Kayit_Click(object sender, EventArgs e)
        {
            Player.Play();
            Control[] tohide = { email_paneli, tamam };
            Control[] toshow = { Kayit_Paneli, giriss_paneli };
            Helpers.control_hide(tohide);
            Helpers.control_show(toshow);
        }

        private void Tamam_Click(object sender, EventArgs e)
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
               if (persondict["unvan"].Contains("Ogretmen"))
                    ogretmen_loggin_paneli.Show();
               else
                    ogrenci_loggin_paneli.Show();
               Panel[] pnl = { giriss_paneli , Giris_Paneli , Kayit_Paneli };
               Helpers.control_hide(pnl);
               cikis_butonu.Show();
           }
           else if (sifre == "null")
                MessageBox.Show("böyle bir kullanici adi yok");
           else
                MessageBox.Show("Şifre Yanlış Lütfen Tekrar Deneyiniz");
        }
        private void Button6_Click(object sender, EventArgs e)
        {
            comboBox6.SelectedIndex = -1;
            Helpers.Datagridviewformatter(dersprogrami, null, null, Color.DarkGray,false);
            Player.Play();
            excel_paneli.Visible = true;
            Ders_Olusturma_Paneli.Visible = false;
            string str = $@"select Hocalar.isim [Hocalar] from Hocalar
                            inner join Dersler on Dersler.hoca_id = Hocalar.hoca_id";
            Dictionary<string, List<string>> mydict = Helpers.Sqlreaderexecuter(str);
            foreach(string hocaadi in mydict["Hocalar"])
            {
                if(!comboBox6.Items.Contains(hocaadi))
                    comboBox6.Items.Add(hocaadi);
            }
        }
        private void Button7_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Show();
            listeden_sec.Checked = true;
            radioButton2.Checked = false;
            Player.Play();
            comboBox2.Enabled = false;
            Ders_Olusturma_Paneli.Show();
            excel_paneli.Hide();
            Helpers.Datagridviewformatter(dersprogrami, null, null, Color.DarkGray,false);
            comboBox2.Enabled = true;
            comboBox2.Items.Clear();
            string gun = Helpers.Hangi_Gun(dateTimePicker1);
            string[,] hours = { { "14:30:00", "14:00:00", "13:30:00", "13:00:00" }, 
                { "17:30:00", "17:00:00", "11:10:00", "10:50:00" } };
            if (gun == "Cumartesi")
                for (int i = 0; i < 4; i++) {
                    comboBox2.Items.Insert(0, hours[0, i]); }
            else
                for (int i = 0; i < 4; i++) {
                    comboBox2.Items.Insert(0, hours[1, i]); }
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            Player.Play();
            MessageBox.Show("Lütfen belirtilen alana kayıt olduğunuz mail adresinizi giriniz");
            Helpers.control_hide(new Control[] { giriss_paneli, Kayit_Paneli });
            Helpers.control_show(new Control[] { email_paneli });
        }
        private void Button9_Click(object sender, EventArgs e)
        {
            string sql1 = $@"SELECT sifre FROM Kisiler where mail='{textBox7.Text}'";
            string obj = Helpers.Sqlexecuter(sql1, 1);
            if (obj == "null")
            {
                MessageBox.Show("email adresi yanlış lütfen tekrar deneyiniz");
            }
            else
            {
                if (Helpers.Email(obj.ToString(), "sifreniz", textBox7.Text))
                    MessageBox.Show("sifreniz email adresinize gönderilmiştir");
                else
                    MessageBox.Show("Email gönderiminde bir sorun oluştu lütfen tekrar deneyiniz");
            }
        }
        private void Button10_Click(object sender, EventArgs e)
        {

            Giris_Paneli.Show();
            Player.Play();
            Settings.GeneralSettings = string.Empty;
            Helpers.combobox_clear(new ComboBox[] { comboBox2, comboBox3, comboBox4, comboBox9, comboBox6 });
            Helpers.control_hide(new Control[]{ cikis_butonu,dersprogrami,ogrenci_loggin_paneli , ogretmen_loggin_paneli , Ders_Olusturma_Paneli,
                Ders_Secim_Paneli, excel_paneli,giriss_paneli,Kayit_Paneli,email_paneli});
        }
        private void Button15_Click(object sender, EventArgs e)
        {
            if (Settings.GeneralSettings == string.Empty) return;
            Player.Play();
            Ders_Secim_Paneli.Hide();
            string[] headerscol = { "Pazartesi", "Sali", "Çarşamba", "Persembe", "Cuma", "Cumartesi" };
            string[] hoursrow = { "10:50:00", "11:10:00", "13:00:00", "13:30:00", "14:00:00", "14:30:00", "17:00:00", "17:30:00" };
            Helpers.Datagridviewformatter(dersprogrami, headerscol, hoursrow,Color.DarkGray);
            Person person = JsonConvert.DeserializeObject<Person>(Settings.GeneralSettings);
            string dersler = $@"select Dersler.DersGünü,Dersler.DersAdi, Dersler.hoca_id[hocaid], 
                                cast(Dersler.date2 as time(0))[time] from Dersler inner join DersKayit 
                                on DersKayit.ders_id = Dersler.Ders_ID where derskayit.student_id = '{person.Id}' ";
            Dictionary<string, List<string>> mydict = Helpers.Sqlreaderexecuter(dersler);
            int timec = mydict["time"].Count;
            for (int i = 0; i < timec; i++)
            {
                DataGridViewCell cell = Helpers.Datagridcellreturner(dersprogrami, mydict["DersGünü"][i], mydict["time"][i]);
                string hocaname = $"select isim from Hocalar where Hoca_id = '{mydict["hocaid"][i]}'";
                hocaname = Helpers.Sqlexecuter(hocaname, 1);
                cell.Value = mydict["DersAdi"][i] + "\n" + hocaname;
                cell.Style.BackColor = Color.Blue;
            }
            MessageBox.Show("Bırakmak İstediğiniz Dersin Üzerine Çift Tıklayınız");
        }
        private void Button16_Click(object sender, EventArgs e)
        {
            Player.Play();
            dersprogrami.Hide();
            Ders_Secim_Paneli.Show();
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
                Helpers.Datagridviewformatter(dersprogrami,hoursrow,headerscol,Color.Red);
                string str1 = $"select cast(date2 as time(0))[date] from Dersler where Quota != Enrolled " +
                        $"and Hoca_id = (select hoca_id from Hocalar where isim = '{comboBox3.Text}') " +
                        $"and DersGünü = '{comboBox4.Text}'";
                Dictionary<string,List<string>> mydict = Helpers.Sqlreaderexecuter(str1);
                for (int j = 0; j < dersprogrami.Columns.Count; j++)
                {
                    string header = dersprogrami.Columns[j].HeaderText;
                    DataGridViewCell cell = dersprogrami.Rows[0].Cells[j];
                    if (mydict["date"].Contains(header))
                    {
                            cell.Value = "Seç";
                            cell.Style.BackColor = Color.Green;
                    }
                    else
                    {
                            cell.Value = "Dolu";
                            cell.Style.BackColor = Color.Red;
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
            Helpers.combobox_clear(new ComboBox[] {comboBox3,comboBox4 });
            string str = $@"select Hocalar.isim [Hocalar] from Hocalar
                        inner join Dersler on Dersler.hoca_id = Hocalar.hoca_id
                        where Dersler.DersAdi = '{comboBox5.Text}' and Dersler.Enrolled != Dersler.Quota";
            Dictionary<string, List<string>> mydict = Helpers.Sqlreaderexecuter(str);
            foreach(string hocaadi in mydict["Hocalar"])
            {
                if (!comboBox3.Items.Contains(hocaadi))
                    comboBox3.Items.Insert(0, hocaadi);
            }
           if(comboBox3.Items.Count == 0)
            {
                Helpers.combobox_clear(new ComboBox[] { comboBox3, comboBox4 });
                Helpers.control_disable(new Control[] { comboBox3, comboBox4 });
                MessageBox.Show("Sectiginiz Branşta Açık Ders Bulunmamakta Lütfen Farklı Bir Branş Seçiniz!");
            }
            else
            {
                Helpers.control_enable(new Control[] { comboBox3 });
            }
        }

        private void ComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox4.Items.Clear();
            comboBox4.SelectedIndex = -1;
            string str = $"Select DersGünü from Dersler where Quota != Enrolled and" +
                $" DersAdi = '{comboBox5.Text}' and hoca_id = (select Hoca_id from Hocalar" +
                $" where isim ='{comboBox3.Text}')";
            Dictionary<string, List<string>> mydict = Helpers.Sqlreaderexecuter(str);
            foreach (string hocaadi in mydict["DersGünü"])
            {
                if (!comboBox4.Items.Contains(hocaadi))
                    comboBox4.Items.Insert(0, hocaadi);
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
            dersprogrami.Enabled = false;
            int rowind = dersprogrami.CurrentCell.RowIndex;
            int colind = dersprogrami.CurrentCell.ColumnIndex;
            if (dersprogrami.CurrentCell.Style.BackColor == Color.Green)
            {
                string dersgünü = dersprogrami.Rows[rowind].HeaderCell.Value.ToString();
                string derssaati = dersprogrami.Columns[colind].HeaderText;
                Person person = JsonConvert.DeserializeObject<Person>(Settings.GeneralSettings);
                string hocaid = $"select Hoca_id from Hocalar where isim = '{comboBox3.Text}'";
                hocaid = Helpers.Sqlexecuter(hocaid, 1);
                string ders_id = $@"select Ders_ID from Dersler where hoca_id = '{hocaid}'
                                    and DersGünü = '{dersgünü}' and DersAdi = '{comboBox5.Text}'
                                    and cast(date2 as time(0)) = '{derssaati}'";
                ders_id = Helpers.Sqlexecuter(ders_id, 1);
                if(ders_id == "null")
                    return;
                string checkcollision = $@"select Dersler.DersGünü, cast(date2 as time(0))[time] from
                                           Dersler inner join Derskayit on Derskayit.ders_id = Dersler.Ders_ID
                                           where derskayit.student_id = '{person.Id}'";
                Dictionary<string, List<string>> mydict = Helpers.Sqlreaderexecuter(checkcollision);
                int countt = mydict["DersGünü"].Count;
                for (int i=0;i<countt;i++)
                {
                    if(mydict["DersGünü"][i] == dersgünü && mydict["time"][i] == derssaati)
                    {
                        MessageBox.Show("Ders Saatleriniz Çakışıyor");
                        dersprogrami.Enabled = true;
                        return;
                    }
                }
                string insertders = $@"insert into derskayit(student_id,ders_id) values('{person.Id}','{ders_id}')";
                string updateenroll = $@"update Dersler set enrolled = '1' where Ders_ID = '{ders_id}'";
                if (Helpers.Sqlexecuter(insertders, 0) != "null")
                    Helpers.Sqlexecuter(updateenroll, 0);
                else
                    MessageBox.Show("Basarisiz!");
            }
            dersprogrami.Enabled = true;
        }


        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            dersprogrami.Enabled = false;
            Person person = JsonConvert.DeserializeObject<Person>(Settings.GeneralSettings);
            if (person.Unvan == "Ogrenci")
            { 
                int rowind = dersprogrami.CurrentCell.RowIndex;
                int colind = dersprogrami.CurrentCell.ColumnIndex;
                DialogResult dr = MessageBox.Show("Dersi İptal Et","Ders İptali", MessageBoxButtons.YesNo);
                if(dr == DialogResult.Yes)
                {
                     string cellval = dersprogrami.Rows[rowind].Cells[colind].Value.ToString();
                     string dersadi = cellval.Split('\n')[0];
                     string dershocasi = cellval.Split('\n')[1];
                     string dersgunu = dersprogrami.Columns[colind].HeaderText;
                     string derssaati = dersprogrami.Rows[rowind].HeaderCell.Value.ToString();
                     string unenroll = $@"delete from DersKayit where student_id = {person.Id} and
                                                  ders_id = (select Ders_ID from Dersler where 
                                                  hoca_id = (select Hoca_id from Hocalar where 
                                                  isim = '{dershocasi}') and DersAdi = '{dersadi}'
                                                  and cast(date2 as time(0)) = '{derssaati}'
                                                  and DersGünü = '{dersgunu}')";
                     if (Helpers.Sqlexecuter(unenroll, 0) == "null")
                     {
                        MessageBox.Show("Basarisiz!");
                        dersprogrami.Enabled = true;
                         return;
                     }
                     dersprogrami.Rows[rowind].Cells[colind].ReadOnly = true;
                     dersprogrami.Rows[rowind].Cells[colind].Value = null;
                     dersprogrami.ClearSelection();
                }
            }
            dersprogrami.Enabled = true;
        }

        private void DataGridView1_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            int rowc = dersprogrami.Rows.Count;
            int colc = dersprogrami.Columns.Count;
            if (e.RowIndex < rowc && e.ColumnIndex < colc && e.RowIndex >= 0 && e.ColumnIndex >= 0 && dersprogrami.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor != Color.Empty)
            {
                if(e.RowIndex != crow || e.ColumnIndex != ccol)
                {
                    crow = e.RowIndex;
                    ccol = e.ColumnIndex;
                    Renk = dersprogrami.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor;
                }
                dersprogrami.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.LightSkyBlue;
            }
        }

        private void DataGridView1_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            int rowc = dersprogrami.Rows.Count;
            int colc = dersprogrami.Columns.Count;
            if (e.RowIndex < rowc && e.ColumnIndex < colc && e.RowIndex >= 0 && e.ColumnIndex >= 0 && dersprogrami.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor != Color.Empty)
            {
                if (dersprogrami.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Dolu")
                    dersprogrami.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Red;
                else if (dersprogrami.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Seç")
                    dersprogrami.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Green;
                else
                    dersprogrami.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Renk;
            }
        }

        private void Button15_Paint(object sender, PaintEventArgs e)
        {
            if (blnButtonDown == false)
            {
                    ControlPaint.DrawBorder(e.Graphics, (sender as Button).ClientRectangle,
                    SystemColors.ActiveBorder, 1, ButtonBorderStyle.Outset,
                    SystemColors.ActiveBorder, 1, ButtonBorderStyle.Outset,
                    SystemColors.ActiveBorder, 1, ButtonBorderStyle.Outset,
                    SystemColors.ActiveBorder, 1, ButtonBorderStyle.Outset);
            }
            else
            {
                    ControlPaint.DrawBorder(e.Graphics, (sender as Button).ClientRectangle,
                    SystemColors.ActiveBorder, 1, ButtonBorderStyle.Inset,
                    SystemColors.ActiveBorder, 1, ButtonBorderStyle.Inset,
                    SystemColors.ActiveBorder, 1, ButtonBorderStyle.Inset,
                    SystemColors.ActiveBorder, 1, ButtonBorderStyle.Inset);
            }
        }

        private void Button15_MouseDown(object sender, MouseEventArgs e)
        {
            blnButtonDown = true;
            (sender as Button).Invalidate();
        }

        private void Button15_MouseUp(object sender, MouseEventArgs e)
        {
            blnButtonDown = false;
            (sender as Button).Invalidate();
        }
        private void ComboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox6.Text !="")
            {
                button3.Enabled = true;
                string[] headerscol = {"Pazartesi","Sali","Çarşamba","Persembe","Cuma","Cumartesi" };
                string[] hoursrow = {"10:50:00","11:10:00","13:00:00","13:30:00","14:00:00","14:30:00","17:00:00","17:30:00" };
                Helpers.Datagridviewformatter(dersprogrami, headerscol, hoursrow,Color.DarkGray);
                string schedule = $"select cast(date2 as time(0))[time],DersGünü, Enrolled from Dersler where " +
                                  $"Hoca_id = (select hoca_id from Hocalar where isim = '{comboBox6.Text}')";
                Dictionary<string, List<string>> mydict = Helpers.Sqlreaderexecuter(schedule);
                int timec = mydict["time"].Count;
                for(int i=0;i<timec;i++)
                {
                    DataGridViewCell cell = Helpers.Datagridcellreturner(dersprogrami, mydict["DersGünü"][i], mydict["time"][i]);
                    cell.Value = "Kayitli Ogrenci: " + mydict["Enrolled"][i];
                    cell.Style.BackColor = Color.Green;
                }
            }
        }

        private void TextBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = (e.KeyChar == (char)Keys.Space);
        }
        private void CopyAlltoClipboard()
        {
            dersprogrami.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            dersprogrami.MultiSelect = true;
            dersprogrami.SelectAll();
            DataObject dataObj = dersprogrami.GetClipboardContent();
            if (dataObj != null) Clipboard.SetDataObject(dataObj);
        }
        private void Button3_Click_1(object sender, EventArgs e)
        {
            if (comboBox6.SelectedIndex == -1) return;
            SaveFileDialog savefile = new SaveFileDialog
            {
                FileName = comboBox6.Text,
                Filter = "Excel Files(*.xlsx)|"
            };
            if (savefile.ShowDialog() == DialogResult.OK)
            {
                CopyAlltoClipboard();
                Excel.Application xlexcel;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;
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
                    comboBox9.Items.Add(hocaisim);
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

        private void Kullanici_tipi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(kullanici_tipi.Text == "Ogretmen")
            {
                textBox4.Show();
                label4.Show();
                MessageBox.Show("Öğretmen olarak kayıt olabilmek için lütfen onay kodunu giriniz");
            }
            else
            {
                textBox4.Hide();
                label4.Hide();
            }
                
        }

        private void Kayit_tamamla_Click(object sender, EventArgs e)
        {
            if ((kullanici_tipi.Text == "Ogretmen" && textBox4.Text == "385") || kullanici_tipi.Text == "Ogrenci")
            {
                string yenikayit = $@"INSERT INTO Kisiler (kullaniciadi,sifre,isim,soyisim,mail,unvan)
                                        values('{textBox1.Text}','{textBox2.Text}','{textBox6.Text}',
                                        '{textBox5.Text}','{textBox3.Text}','{kullanici_tipi.Text}')";
                string success = Helpers.Sqlexecuter(yenikayit, 0);
                if (success == "")
                {
                    MessageBox.Show("Kayit Basarili Giris Yapabilirsiniz!");
                    textBox1.Clear();
                    textBox2.Clear();
                    Kayit_Paneli.Hide();
                }
                else MessageBox.Show("Kullanici adi alinmis. Lutfen tekrar deneyiniz");
            }
        }

    }
}
