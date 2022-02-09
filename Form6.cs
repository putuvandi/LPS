using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace LPS
{
    public partial class Form6 : Form
    {
        MySqlConnection con;
        ConnectionDB db = new ConnectionDB();
        string awal, akhir, tahun;
        public Form6()
        {
            InitializeComponent();
            con = new MySqlConnection(db.getConnection());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            string choose1 = dateTimePicker1.Value.ToString("dd/MM/yyyy");
            string choose2 = dateTimePicker2.Value.ToString("dd/MM/yyyy");
            awal = dateTimePicker1.Value.ToString("dd");
            akhir = dateTimePicker2.Value.ToString("dd");
            tahun = dateTimePicker2.Value.ToString("yy");
            String periodestring = convertMonth(dateTimePicker1.Value.ToString("MM"));
            String periodestring2 = convertMonth(dateTimePicker2.Value.ToString("MM"));
            //MessageBox.Show(tahun);
            if (periodestring.Equals(periodestring2))
            {
                loadData(choose1,choose2,periodestring);
               // MessageBox.Show(choose1);
            //}
           // else if (dateTimePicker1.Value.ToString("yyyy").Equals(dateTimePicker2.Value.ToString("yyyy")))
         ////   {
            //    loadData2(awal, akhir, periodestring, periodestring2, dateTimePicker2.Value.ToString("yyyy"));
            }else
            {
                loadData2(choose1, choose2, periodestring, periodestring2);
                //   loadData3(awal, akhir, periodestring, periodestring2, dateTimePicker1.Value.ToString("yyyy"), dateTimePicker2.Value.ToString("yyyy"));
            }
        }

        private void loadData3(string awal, string akhir,string bulan1,string bulan2,string tahun1, string tahun2)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            export();
        }

        private String convertMonth(String month)
        {
            int a = Int16.Parse(month);
            switch (a)
            {
                case 1:
                    // code block
                    month = "Januari";
                    break;
                case 2:
                    // code block
                    month = "Februari";
                    break;
                case 3:
                    // code block
                    month = "Maret";
                    break;
                case 4:
                    // code block
                    month = "April";
                    break;
                case 5:
                    // code block
                    month = "Mei";
                    break;
                case 6:
                    // code block
                    month = "Juni";
                    break;
                case 7:
                    // code block
                    month = "Juli";
                    break;
                case 8:
                    // code block
                    month = "Agustus";
                    break;
                case 9:
                    // code block
                    month = "September";
                    break;
                case 10:
                    // code block
                    month = "Oktober";
                    break;
                case 11:
                    // code block
                    month = "November";
                    break;
                case 12:
                    // code block
                    month = "Desember";
                    break;
            }
            return month;
        }

        private void loadData(string awal, string akhir,string bulan)
        {
            this.dataGridView1.DataSource = null;
            this.dataGridView1.Rows.Clear();
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.CommandText = @"select notaproses.nota ,notaproses.br,notaproses.keterangan2, notaproses.tempo,notaproses.customer, lps.tgl, lps.periode, notaproses.tonase, lps.driver, lps.helper,notaproses.kembali, notaproses.keterangan,lps.time,lps.id_lps from notaproses JOIN lps ON lps.id_lps = notaproses.id_lps where lps.year between @awal and @akhir and lps.periode = @bulan order by lps.year asc, lps.driver asc, lps.time asc,notaproses.id_lps asc,notaproses.idUrutan asc";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;

                    //cmd.Parameters.Add("@bulan", MySqlDbType.VarChar).Value = bulan;
                    cmd.Parameters.Add("@awal", MySqlDbType.VarChar).Value = awal;
                    cmd.Parameters.Add("@akhir", MySqlDbType.VarChar).Value = akhir;
                    cmd.Parameters.Add("@bulan", MySqlDbType.VarChar).Value = bulan;
                    // cmd.Parameters.Add("@year", MySqlDbType.VarChar).Value = tahun;
                    con.Open();
                    cmd.ExecuteNonQuery();


                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {

                        //textBox3.Text += $"{reader.GetString("customer")}";
                        String cat = reader.GetString("nota");
                        string res = cat.Substring(0, 1);
                        string year = bulan.Substring(0, 3);
                        //string fixtgl=
                        string category;
                        if (res.Equals("1"))
                        {
                            category = "SURYA PANGAN";
                        }
                        else
                        {
                            category = "SUMBER HIDUP";
                        }
                        dataGridView1.Rows.Add($"{reader.GetString("tgl") + "-" + year + "-" + tahun}", $"{reader.GetString("periode")}", $"{reader.GetString("time")}", $"{reader.GetString("nota")}", $"{category}", $"{reader.GetString("tempo")}", $"{reader.GetString("customer")}", $"{reader.GetString("br")}", $"{reader.GetString("tonase")}", $"{reader.GetString("driver")}", $"{reader.GetString("helper")}", $"{reader.GetString("keterangan")}", $"{reader.GetString("kembali")}", $"{reader.GetString("keterangan2")}", $"{reader.GetString("id_lps")}");

                    }
                    cmd.Parameters.Clear();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

            con.Close();
            Cursor.Current = Cursors.Default;
        }

        private void loadData2(string awal, string akhir, string bulan, string bulan2)
        {
            this.dataGridView1.DataSource = null;
            this.dataGridView1.Rows.Clear();
            Cursor.Current = Cursors.WaitCursor;
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"select notaproses.nota ,notaproses.br,notaproses.keterangan2, notaproses.tempo,notaproses.customer, lps.tgl, lps.periode, notaproses.tonase, lps.driver, lps.helper,notaproses.kembali, notaproses.keterangan,lps.time,lps.id_lps from notaproses JOIN lps ON lps.id_lps = notaproses.id_lps where lps.year between @awal and 31 and lps.periode = @bulan order by lps.year asc, lps.driver asc, lps.time asc,notaproses.id_lps asc,notaproses.idUrutan asc";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;

                cmd.Parameters.Add("@bulan", MySqlDbType.VarChar).Value = bulan;
                //cmd.Parameters.Add("@bulan2", MySqlDbType.VarChar).Value = bulan2;
                cmd.Parameters.Add("@awal", MySqlDbType.VarChar).Value = awal;
                cmd.Parameters.Add("@akhir", MySqlDbType.VarChar).Value = akhir;
                con.Open();
                cmd.ExecuteNonQuery();


                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {

                    //textBox3.Text += $"{reader.GetString("customer")}";
                    String cat = reader.GetString("nota");
                    String bln = reader.GetString("periode");
                    string res = cat.Substring(0, 1);
                    string category;
                    string year = bln.Substring(0, 3);
                    if (res.Equals("1"))
                    {
                        category = "SURYA PANGAN";
                    }
                    else
                    {
                        category = "SUMBER HIDUP";
                    }
                    dataGridView1.Rows.Add($"{reader.GetString("tgl") + "-" + year + "-" + tahun}", $"{reader.GetString("periode")}", $"{reader.GetString("time")}", $"{reader.GetString("nota")}", $"{category}", $"{reader.GetString("tempo")}", $"{reader.GetString("customer")}", $"{reader.GetString("br")}", $"{reader.GetString("tonase")}", $"{reader.GetString("driver")}", $"{reader.GetString("helper")}", $"{reader.GetString("keterangan")}", $"{reader.GetString("kembali")}", $"{reader.GetString("keterangan2")}");

                }
                cmd.Parameters.Clear();
                con.Close();

                cmd.CommandText = @"select notaproses.nota , notaproses.br,notaproses.keterangan2, notaproses.tempo,notaproses.customer, lps.tgl, lps.periode, notaproses.tonase, lps.driver, lps.helper,notaproses.kembali, notaproses.keterangan,lps.time from notaproses JOIN lps ON lps.id_lps = notaproses.id_lps where lps.periode = @bulan2 and lps.tgl BETWEEN 1 AND @akhir order by lps.year asc, lps.driver asc, lps.time asc,notaproses.id_lps asc,notaproses.idUrutan asc";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                con.Open();
                cmd.Parameters.Add("@bulan2", MySqlDbType.VarChar).Value = bulan2;
                cmd.Parameters.Add("@akhir", MySqlDbType.VarChar).Value = akhir;
                //cmd.Parameters.Add("@akhir", MySqlDbType.VarChar).Value = akhir;

                cmd.ExecuteNonQuery();

                MySqlDataReader reader2 = cmd.ExecuteReader();
                while (reader2.Read())
                {

                    //textBox3.Text += $"{reader.GetString("customer")}";
                    String cat = reader2.GetString("nota");
                    string res = cat.Substring(0, 1);
                    String bln2 = reader2.GetString("periode");
                    string category;
                    string year2 = bln2.Substring(0, 3);
                    if (res.Equals("1"))
                    {
                        category = "SURYA PANGAN";
                    }
                    else
                    {
                        category = "SUMBER HIDUP";
                    }
                    dataGridView1.Rows.Add($"{reader2.GetString("tgl") + "-" + year2 + "-" + tahun}", $"{reader2.GetString("periode")}", $"{reader2.GetString("time")}", $"{reader2.GetString("nota")}", $"{category}", $"{reader2.GetString("tempo")}", $"{reader2.GetString("customer")}", $"{reader2.GetString("br")}", $"{reader2.GetString("tonase")}", $"{reader2.GetString("driver")}", $"{reader2.GetString("helper")}", $"{reader2.GetString("keterangan")}", $"{reader2.GetString("kembali")}", $"{reader2.GetString("keterangan2")}");

                }
                cmd.Parameters.Clear();
                con.Close();
            }
            Cursor.Current = Cursors.Default;
        }

        private void export()
        {

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel Documents (*.xls)|*.xls";
            sfd.FileName = "Laporan.xls";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                Cursor.Current = Cursors.WaitCursor;
                // Copy DataGridView results to clipboard
                copyAlltoClipboard();


                object misValue = System.Reflection.Missing.Value;
                Excel.Application xlexcel = new Excel.Application();

                xlexcel.DisplayAlerts = false; // Without this you will get two confirm overwrite prompts
                Excel.Workbook xlWorkBook = xlexcel.Workbooks.Add(misValue);
                Excel.Worksheet xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                // Format column D as text before pasting results, this was required for my data
                Excel.Range rng = xlWorkSheet.get_Range("A:Z").Cells;
                rng.NumberFormat = "@";
                // rng.AutoFit();
                xlWorkSheet.Cells[2, 2] = "Information Data Report";
                xlWorkSheet.Cells[3, 2] = "Reason Category :";
                xlWorkSheet.Cells[4, 2] = "Vehicle Type :";
                xlWorkSheet.Cells[5, 2] = "Payment Status :";
                xlWorkSheet.Cells[6, 2] = "Status Delivery :";
                xlWorkSheet.Cells[7, 2] = "Category Nota :";

                xlWorkSheet.Cells[3, 3] = "P1 (Salah Loading)";
                xlWorkSheet.Cells[4, 3] = "Fuso 7,5 ton";
                xlWorkSheet.Cells[5, 3] = "Cash by Driver";
                xlWorkSheet.Cells[6, 3] = "Deliverd";
                xlWorkSheet.Cells[7, 3] = "Surya Pangan";

                xlWorkSheet.Cells[3, 4] = "P2 (Tidak Order)";
                xlWorkSheet.Cells[4, 4] = "Engkel Open 5 ton";
                xlWorkSheet.Cells[5, 4] = "Cash by Transfer";
                xlWorkSheet.Cells[6, 4] = "Pending";
                xlWorkSheet.Cells[7, 4] = "Sumber Hidup";

                xlWorkSheet.Cells[3, 5] = "P3 (Toko Tutup)";
                xlWorkSheet.Cells[4, 5] = "Engkel Box 3,5 ton";
                xlWorkSheet.Cells[5, 5] = "TOP";
                xlWorkSheet.Cells[6, 5] = "Cancel";
                xlWorkSheet.Cells[7, 5] = "Sumber Hidup Jaya";

                xlWorkSheet.Cells[3, 6] = "P4 (Barang Rusak)";
                xlWorkSheet.Cells[4, 6] = "L300 Box 2,5 ton";

                xlWorkSheet.Cells[2, 10] = "RETURN";
                xlWorkSheet.Cells[3, 10] = "P1";
                xlWorkSheet.Cells[4, 10] = "P2";
                xlWorkSheet.Cells[5, 10] = "P3";
                xlWorkSheet.Cells[6, 10] = "P4";
                xlWorkSheet.Cells[7, 10] = "P5";

                xlWorkSheet.Cells[2, 11] = "TOTAL";

                Excel.Range range = xlWorkSheet.get_Range("B2:F7").Cells;
                range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                range.Borders.Weight = Excel.XlBorderWeight.xlThin;

                Excel.Range range2 = xlWorkSheet.get_Range("J2:K7").Cells;
                range2.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                range2.Borders.Weight = Excel.XlBorderWeight.xlThin;

                int count = dataGridView1.Rows.Count + 11;
                Excel.Range range3 = xlWorkSheet.get_Range("B12:O" + count).Cells;
                range3.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                range3.Borders.Weight = Excel.XlBorderWeight.xlThin;

                Excel.Range range4 = xlWorkSheet.get_Range("B2:F2").Cells;
                range4.Merge();

                Excel.Range range5 = xlWorkSheet.get_Range("B:N").Cells;
                range5.Columns.AutoFit();

                for (int i = 1; i < dataGridView1.Columns.Count ; i++)
                {
                    xlWorkSheet.Cells[12, i + 1] = dataGridView1.Columns[i - 1].HeaderText;
                }
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < dataGridView1.Columns.Count-1; j++)
                    {
                        xlWorkSheet.Cells[i + 13, j + 2] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                    }
                }

                // Save the excel file under the captured location from the SaveFileDialog
                xlWorkBook.SaveAs(sfd.FileName, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlexcel.DisplayAlerts = true;
                xlWorkBook.Close(true, misValue, misValue);
                xlexcel.Quit();

                releaseObject(xlWorkSheet);
                releaseObject(xlWorkBook);
                releaseObject(xlexcel);

                // Clear Clipboard and DataGridView selection
                Clipboard.Clear();
                dataGridView1.ClearSelection();

                // Open the newly saved excel file
                if (File.Exists(sfd.FileName))
                    System.Diagnostics.Process.Start(sfd.FileName);

            }
            Cursor.Current = Cursors.Default;
        }

        private void copyAlltoClipboard()
        {
            dataGridView1.SelectAll();
            DataObject dataObj = dataGridView1.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception Occurred while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}
