using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System.Configuration;
using System.Net.Mail;
using System.Drawing;
using Excel = Microsoft.Office.Interop.Excel;
using static WindowsFormsApp1.Program;
namespace WindowsFormsApp1
{
    class Helpers
    {
        private readonly static Dictionary<string, string> Days = new Dictionary<string, string> {
            { "Monday", "Pazartesi" },
            { "Tuesday", "Salı" },
            { "Wednesday", "Çarşamba" },
            { "Thursday", "Perşembe" },
            { "Friday", "Cuma" },
            { "Saturday", "Cumartesi" },
            { "Sunday", "Pazar" }
        };
        static public DataGridViewCell Datagridcellreturner(DataGridView datag,string gun,string saat)
        {
            int rowindex = FindRow(datag, saat);
            int colindex = FindCol(datag, gun);
            MessageBox.Show(rowindex.ToString() + "\n" + colindex.ToString());
            if (rowindex == -1 || colindex == -1)
                return null;
            using(DataGridViewCell cell = datag.Rows[rowindex].Cells[colindex])
            {
                Color cellcolor = cell.Style.BackColor;
                if (cellcolor != Color.Blue && cellcolor != Color.Green)
                    cell.Style.BackColor = Color.DarkGray;
                cell.ReadOnly = false;
                return cell;
            }
        }
        static public int FindRow(DataGridView dview,string value)
        {
            var rowlist = dview.Rows.Cast<DataGridViewRow>().ToList();
            int rowindex = rowlist.FindIndex(c => c.HeaderCell.Value.ToString() == value);
            return rowindex;
        }
        static public int FindCol(DataGridView dview, string value)
        {
            var columnlist = dview.Columns.Cast<DataGridViewColumn>().ToList();
            int colindex = columnlist.FindIndex(c => c.HeaderText == value);
            return colindex;
        }
        static public string Sqlexecuter(string command, int type)
        {
            string constr = ConfigurationManager.ConnectionStrings["derssecimconnection"].ConnectionString.ToString();
            using (SqlConnection con = new SqlConnection(constr))
            {
                try
                {
                    con.Open();
                }
                catch
                {
                    MessageBox.Show("Sql connection error");
                    return "null";
                }
                using (SqlCommand comm = new SqlCommand(command, con))
                {
                    try
                    {
                        if (type == 0)
                        {
                            comm.ExecuteNonQuery();
                            con.Close();
                            return "";
                        }
                        else
                        {
                            var obj = comm.ExecuteScalar();
                            con.Close();
                            return obj.ToString();
                        }
                    }
                    catch
                    {
                        con.Close();
                        return "null";
                    }
                }
            }
        }

        static public Dictionary<string, List<string>> Sqlreaderexecuter(string comm)
        {
            string constr = ConfigurationManager.ConnectionStrings["derssecimconnection"].ConnectionString.ToString();
            using (SqlConnection conne = new SqlConnection(constr))
            {
                conne.Open();
                Dictionary<string, List<string>> mydict = new Dictionary<string, List<string>>();
                using (SqlCommand a = new SqlCommand(comm, conne))
                {
                    using (SqlDataReader dataread = a.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(dataread);
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            string colname = dt.Columns[j].ColumnName;
                            mydict[colname] = new List<string>();
                        }
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            for (int j = 0; j < dt.Columns.Count; j++)
                            {
                                string colname = dt.Columns[j].ColumnName;
                                mydict[colname].Add(dt.Rows[i][colname].ToString());
                            }

                        }
                        return mydict;
                    }
                }
            }
        }

        static public void Gun_Saat_Duzenleyici(DateTimePicker Tarih_Secici,ComboBox combobox)
        {
            string gun = Hangi_Gun(Tarih_Secici);
            if (gun == "Cumartesi")
                combobox.Items.AddRange(Ders_saatleri.Take(Ders_saatleri.Length / 2).ToArray());
            else
                combobox.Items.AddRange(Ders_saatleri.Skip(Ders_saatleri.Length / 2).ToArray());
        }

        static public string Hangi_Gun(DateTimePicker datepicker)
        {
            string Day = datepicker.Value.Date.DayOfWeek.ToString();
            return Days.ToList().Find(keyvalue => keyvalue.Key == Day).Value;
        }

        static public void Datagridviewformatter(DataGridView datag, string[] columns, string[] rows, Color color,bool mode=true)
        {
            if(mode == true)
            {
                datag.Show();
                datag.Rows.Clear();
                datag.Columns.Clear();
                datag.DefaultCellStyle.SelectionBackColor = Color.SkyBlue;
                datag.DefaultCellStyle.BackColor = color;
                datag.ReadOnly = true;
                columns.Select(header => new DataGridViewTextBoxColumn { HeaderText = header })
                       .ToList().ForEach(col => datag.Columns.Add(col));
                if (rows.Length > 1)
                    datag.Rows.Add(rows.Length - 1);
                for (int i = 0; i < rows.Length; i++)
                    datag.Rows[i].HeaderCell.Value = rows[i];
                for (int i = 0; i < datag.Columns.Count; i++)
                    datag.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                foreach (DataGridViewRow row in datag.Rows)
                    row.Height = (datag.ClientRectangle.Height - datag.ColumnHeadersHeight) / datag.Rows.Count;
                datag.RowHeadersWidth = 80;
                datag.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                datag.ReadOnly = true;
            }
            else
            {
                datag.Hide();
                datag.Rows.Clear();
                datag.Columns.Clear();
            }
        }

        static public void FillDataGridView(DataGridView datagridview,Dictionary<string,List<string>> mydict,bool isstudent)
        {
            int timec = mydict["time"].Count;
            for (int i = 0; i < timec; i++)
            {
                DataGridViewCell cell = Datagridcellreturner(datagridview, mydict["DersGünü"][i], mydict["time"][i]);
                if(isstudent)
                {
                    string hocaname = $"select isim from Hocalar where Hoca_id = '{mydict["hocaid"][i]}'";
                    hocaname = Sqlexecuter(hocaname, 1);
                    cell.Value = mydict["DersAdi"][i] + "\n" + hocaname;
                }
                else
                    cell.Value = "Kayitli Ogrenci: " + mydict["Enrolled"][i];
                cell.Style.BackColor = Color.Blue;
            }
        }
        static public bool Email(string konu, string icerik, string maill)
        {
            using (SmtpClient client = new SmtpClient())
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.To.Add(maill);
                    mail.Subject = icerik;
                    mail.IsBodyHtml = true;
                    mail.Body = konu;
                    try
                    {
                        client.Send(mail);
                        return true;
                    }
                    catch
                    {   
                        return false;
                    }
                }
            }   
        }
        static public void DateTimePickerFormatter(DateTimePicker datet)
        {
            datet.MinDate = DateTime.Now.AddDays((8 - DateTime.Today.DayOfWeek - DayOfWeek.Sunday));
            datet.MaxDate = DateTime.Now.AddDays((8 - DateTime.Today.DayOfWeek - DayOfWeek.Sunday)).AddDays(5);
            datet.Format = DateTimePickerFormat.Custom;
            datet.CustomFormat = "yyyy-MM-dddd";
            datet.Hide();
        }
        static public void Control_hide(Control[] controls) =>
            controls.ToList().ForEach(control => control.Hide());
        public static void Control_show(Control[] controls) =>
            controls.ToList().ForEach(control => control.Show());
        static public void Control_enable(Control[] controls)=>
            controls.ToList().ForEach(control => control.Enabled = true);
        static public void Control_disable(Control[] controls)
        {
            controls.ToList().ForEach(control => control.Enabled = false);
        }
        static public void Combobox_dropdown(ComboBox[] comboboxes) =>
            comboboxes.ToList().ForEach(combobox => combobox.DropDownStyle = ComboBoxStyle.DropDownList);
        public static void Combobox_clear(ComboBox[] comboboxes, bool clear, bool indexclear) =>
            comboboxes.ToList().ForEach(combobox =>
            {
                if (clear) combobox.Items.Clear();
                if (indexclear) combobox.SelectedIndex = -1;
            });
        static public void FillComboBoxWithList(ComboBox comboBox, List<string> mylist) =>
            comboBox.Items.AddRange(mylist.Distinct().ToArray());
        static public void CopyAlltoClipboard(DataGridView dersprogrami)
        {
            dersprogrami.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            dersprogrami.MultiSelect = true;
            dersprogrami.SelectAll();
            DataObject dataObj = dersprogrami.GetClipboardContent();
            if (dataObj != null) Clipboard.SetDataObject(dataObj);
        }
        static public void ToExcel(SaveFileDialog savefile)
        {
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
}
