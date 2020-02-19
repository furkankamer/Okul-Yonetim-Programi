using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System.Configuration;
namespace WindowsFormsApp1
{
    class Helpers
    {
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
                    catch (Exception exp)
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

        static public void datagridviewformatter(DataGridView datag, string[] columns, string[] rows)
        {
            datag.Show();
            datag.Rows.Clear();
            datag.Columns.Clear();
            foreach (string header in columns)
            {
                DataGridViewColumn d = new DataGridViewTextBoxColumn();
                d.HeaderText = header;
                datag.Columns.Add(d);
            }
            if (rows.Length > 1) datag.Rows.Add(rows.Length - 1);
            for (int i = 0; i < rows.Length; i++)
            {
                datag.Rows[i].HeaderCell.Value = rows[i];
            }
            for (int i = 0; i < datag.Columns.Count; i++)
            {
                datag.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            foreach (DataGridViewRow row in datag.Rows)
            {
                row.Height = (datag.ClientRectangle.Height - datag.ColumnHeadersHeight) / datag.Rows.Count;
            }
            datag.RowHeadersWidth = 80;
            datag.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            datag.ReadOnly = true;
        }
    }
}
