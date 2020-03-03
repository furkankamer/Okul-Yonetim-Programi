using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Drawing;

namespace WindowsFormsApp1
{
    class Helpers
    {
        static public DataGridViewCell Datagridcellreturner(DataGridView datag,string gun,string saat)
        {
            int rowindex = FindRow(datag, saat);
            int colindex = FindCol(datag, gun);
            DataGridViewCell cell = datag.Rows[rowindex].Cells[colindex];
            Color cellcolor = cell.Style.BackColor;
            if (cellcolor != Color.Blue && cellcolor != Color.Green)
                cell.Style.BackColor = Color.DarkGray;
            cell.ReadOnly = false;
            return cell;

        }
        static public int FindRow(DataGridView dview,string value)
        {
            int rowindex = -1;
            for (int i = 0; i < dview.Rows.Count; i++)
                if (dview.Rows[i].HeaderCell.Value.ToString() == value)
                    rowindex = i;
            return rowindex;
        }
        static public int FindCol(DataGridView dview, string value)
        {
            int colindex = -1;
            for (int i = 0; i < dview.Columns.Count; i++)
                if (dview.Columns[i].HeaderText == value)
                    colindex = i;
            return colindex;
        }
        static public string Sqlexecuter(string command, int type)
        {
            string constr = ConfigurationManager.ConnectionStrings["derssecimconnection"].ConnectionString.ToString();
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
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

        static public string Hangi_Gun(DateTimePicker datepicker)
        {
            DateTime myDate = datepicker.Value.Date;
            string gun = "";
            if (myDate.DayOfWeek.ToString() == "Monday") gun = "Pazartesi";
            else if (myDate.DayOfWeek.ToString() == "Tuesday") gun = "Salı";
            else if (myDate.DayOfWeek.ToString() == "Wednesday") gun = "Çarşamba";
            else if (myDate.DayOfWeek.ToString() == "Thursday") gun = "Perşembe";
            else if (myDate.DayOfWeek.ToString() == "Friday") gun = "Cuma";
            else if (myDate.DayOfWeek.ToString() == "Saturday") gun = "Cumartesi";
            return gun;
        }

        static public void Datagridviewformatter(DataGridView datag, string[] columns, string[] rows,bool mode=true)
        {
            if(mode == true)
            {
                datag.Show();
                datag.Rows.Clear();
                datag.Columns.Clear();
                datag.DefaultCellStyle.SelectionBackColor = Color.SkyBlue;
                datag.DefaultCellStyle.BackColor = Color.DarkGray;
                datag.ReadOnly = true;
                foreach (string header in columns)
                {
                    DataGridViewColumn d = new DataGridViewTextBoxColumn
                    {
                        HeaderText = header
                    };
                    datag.Columns.Add(d);
                }
                if (rows.Length > 1) datag.Rows.Add(rows.Length - 1);
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
        static public void DateTimePickerFormatter(DateTimePicker datet)
        {
            datet.MinDate = DateTime.Now.AddDays((8 - DateTime.Today.DayOfWeek - DayOfWeek.Sunday));
            datet.MaxDate = DateTime.Now.AddDays((8 - DateTime.Today.DayOfWeek - DayOfWeek.Sunday)).AddDays(5);
            datet.Format = DateTimePickerFormat.Custom;
            datet.CustomFormat = "yyyy-MM-dddd";
            datet.Hide();
        }
    }
}
