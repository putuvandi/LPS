using CrystalDecisions.CrystalReports.Engine;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LPS
{
    public partial class Form2 : Form
    {
        MySqlConnection con;
        ConnectionDB db = new ConnectionDB();
        String bulan2;
        int kolom = 0;
        int nomer = 1;
        int datadelete;
        Boolean status = false;
        String message3;
        String date;
        string year;
        string tempyear;
        string supir2="";
        string mobil2="";
        List<String> areaList = new List<String>();
        
        public Form2(int kode)
        {
            InitializeComponent();
            con = new MySqlConnection(db.getConnection());
            if (kode == 1)
            {
                Form4 f = new Form4(this, 1);
                f.ShowDialog();
            }
            else if (kode == 2)
            {
                Form4 f = new Form4(this, 2);
                f.ShowDialog();
            }
            else
            {
                getid();
                timer1.Start();
            }
            
            
        }

        private void getid()
        {
            try
            {
                nomer = 1;
                this.ActiveControl = textBox1;
                comboBox1.Items.Clear();
                comboBox1.Text = "";
                comboBox2.Items.Clear();
                comboBox2.Text = "";
                comboBox3.Items.Clear();
                comboBox3.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox1.Text = "";
                textBox2.Text = "";
                mobil2 = "";
                supir2 = "";
                this.dataGridView1.DataSource = null;
                this.dataGridView1.Rows.Clear();
                DateTime dateTime = DateTime.UtcNow.Date;
                date = dateTime.ToString("dd");
                year = dateTime.ToString("dd/MM/yyyy");
                dateTimePicker1.Value = DateTime.Now;
                // textBox6.Text = date;
                String bulan1 = dateTime.ToString("MM");
                bulan2 = formatmonnth(bulan1);
                
              //  textBox5.Text = bulan2;
                string sql = "SELECT max(id_lps) FROM lps";
                MySqlCommand cmd = new MySqlCommand(sql, con);

                if (con.State != ConnectionState.Open)
                {
                    con.Close();
                    con.Open();
                }
                else
                {
                    con.Open();
                }

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int angka;
                    if (reader.GetString("max(id_lps)") != null)
                    {
                        angka = reader.GetInt32("max(id_lps)") + 1;
                    }
                    else
                    {
                        angka = 1;
                    }


                    label2.Text = angka.ToString();
                    //this.dataGridView2.Rows.Add($"{reader.GetString("nota")}", $"{reader.GetString("customer")}", $"{reader.GetString("tempo")}", textBox3.Text, "", "");

                }
                con.Close();

                string sql2 = "select * from tbmobil";
                MySqlCommand cmd2 = new MySqlCommand(sql2, con);

                if (con.State != ConnectionState.Open)
                {
                    con.Close();
                    con.Open();
                }
                else
                {
                    con.Open();
                }

                MySqlDataReader reader2 = cmd2.ExecuteReader();

                while (reader2.Read())
                {
                    comboBox1.Items.Add(reader2.GetString("nopol") + " (" + reader2.GetString("jenis") + ")");
                }
                con.Close();
                string sql3 = "select * from tbdriver";
                MySqlCommand cmd3 = new MySqlCommand(sql3, con);

                if (con.State != ConnectionState.Open)
                {
                    con.Close();
                    con.Open();
                }
                else
                {
                    con.Open();
                }

                MySqlDataReader reader3 = cmd3.ExecuteReader();

                while (reader3.Read())
                {
                    comboBox2.Items.Add(reader3.GetString("driver"));
                }
                con.Close();
                string sql4 = "select * from tbhelper";
                MySqlCommand cmd4 = new MySqlCommand(sql4, con);
                if (con.State != ConnectionState.Open)
                {
                    con.Close();
                    con.Open();
                }
                else
                {
                    con.Open();
                }

                MySqlDataReader reader4 = cmd4.ExecuteReader();

                while (reader4.Read())
                {
                    comboBox3.Items.Add(reader4.GetString("namaHelper"));
                }
                con.Close();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error");
            }
        }

        private void save()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (con.State != ConnectionState.Open)
                {
                    con.Close();
                    con.Open();
                }
                else
                {
                    con.Open();
                }
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    string daytemp = dateTimePicker1.Value.ToString("dd/MM/yyyy");
                    string[] subs3 = daytemp.Split('/');
                    cmd.CommandText = @"insert into lps (id_lps,no_kendaraan,driver,helper,area,periode,tgl,time,year)values(@id_lps,@no_kendaraan,@driver,@helper,@area,@periode,@tgl,@time,@year)";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;

                    cmd.Parameters.Add("@id_lps", MySqlDbType.VarChar).Value = label2.Text;
                    cmd.Parameters.Add("@periode", MySqlDbType.VarChar).Value =  bulan2;
                    cmd.Parameters.Add("@no_kendaraan", MySqlDbType.VarChar).Value = comboBox1.Text+mobil2;
                    cmd.Parameters.Add("@driver", MySqlDbType.VarChar).Value = comboBox2.Text+supir2;
                    cmd.Parameters.Add("@tgl", MySqlDbType.VarChar).Value = subs3[0];
                    cmd.Parameters.Add("@time", MySqlDbType.VarChar).Value = label4.Text;
                    cmd.Parameters.Add("@helper", MySqlDbType.VarChar).Value = textBox3.Text;
                    cmd.Parameters.Add("@area", MySqlDbType.VarChar).Value = textBox4.Text;
                    cmd.Parameters.Add("@year", MySqlDbType.VarChar).Value = dateTimePicker1.Value.ToString("dd/MM/yyyy");

                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {

                        string aa = Convert.ToString(dataGridView1.Rows[i].Cells[1].Value);
                        if (aa.Equals(""))
                        {

                        }
                        else
                        {
                            string namacust = dataGridView1.Rows[i].Cells[2].Value.ToString();
                            string fixnama = namacust.Replace("'", "\'");
                            cmd.CommandText = @"insert into notaproses (nota,customer,area,br,tempo,loading,id_lps,idUrutan)values(@nota2,@customer2,@area2,@br2,@tempo2,@loading2,@id_lps2,@idUrutan2)";
                            cmd.CommandType = CommandType.Text;
                            cmd.Connection = con;

                            cmd.Parameters.Add("@nota2", MySqlDbType.VarChar).Value = dataGridView1.Rows[i].Cells[1].Value.ToString();
                            cmd.Parameters.Add("@customer2", MySqlDbType.VarChar).Value = fixnama;
                            cmd.Parameters.Add("@area2", MySqlDbType.VarChar).Value = textBox4.Text;
                            cmd.Parameters.Add("@br2", MySqlDbType.VarChar).Value = dataGridView1.Rows[i].Cells[10].Value.ToString();
                            cmd.Parameters.Add("@tempo2", MySqlDbType.VarChar).Value = dataGridView1.Rows[i].Cells[3].Value.ToString();
                            cmd.Parameters.Add("@loading2", MySqlDbType.VarChar).Value = dataGridView1.Rows[i].Cells[5].Value.ToString();
                            cmd.Parameters.Add("@id_lps2", MySqlDbType.VarChar).Value = label2.Text;
                            cmd.Parameters.Add("@idUrutan2", MySqlDbType.VarChar).Value = dataGridView1.Rows[i].Cells[0].Value.ToString();
                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }

                    }
                    con.Close();
                    MessageBox.Show("Berhasil Menyimpan data!!");
                }
                print();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error6");
            }


        }

        private void update()
        {
            try
            {

                if (con.State != ConnectionState.Open)
                {
                    con.Close();
                    con.Open();
                }
                else
                {
                    //con.Close();
                    con.Open();
                }
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    string daytemp = dateTimePicker1.Value.ToString("dd/MM/yyyy");
                    string[] subs2 = daytemp.Split('/');
                    cmd.CommandText = @"update lps set no_kendaraan=@no_kendaraan3,driver=@driver3,helper=@helper3,area=@area3,periode=@periode3,tgl=@tgl3,time=@time3 where id_lps=@id_lps3";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;

                    //MessageBox.Show("1", subs2[0]);

                    cmd.Parameters.Add("@no_kendaraan3", MySqlDbType.VarChar).Value = comboBox1.Text+mobil2;
                    cmd.Parameters.Add("@driver3", MySqlDbType.VarChar).Value = comboBox2.Text+supir2;
                    cmd.Parameters.Add("@helper3", MySqlDbType.VarChar).Value = textBox3.Text;
                    cmd.Parameters.Add("@area3", MySqlDbType.VarChar).Value = textBox4.Text;
                    cmd.Parameters.Add("@periode3", MySqlDbType.VarChar).Value = formatmonnth(subs2[1]);
                    cmd.Parameters.Add("@tgl3", MySqlDbType.VarChar).Value = subs2[0];
                    cmd.Parameters.Add("@time3", MySqlDbType.VarChar).Value = label4.Text;
                    cmd.Parameters.Add("@id_lps3", MySqlDbType.VarChar).Value = label2.Text;

                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    // string terkirim;
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {

                        string check = "DELIVERD";
                        if (dataGridView1.Rows[i].Cells[9].Value != null && dataGridView1.Rows[i].Cells[9].Value.Equals(check))
                        {
                            dataGridView1.Rows[i].Cells[6].Value = dataGridView1.Rows[i].Cells[5].Value;
                        }
                        cmd.CommandText = @"select nota from notaproses where nota=@nota4 and id_lps=@id_lps4";
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;

                        cmd.Parameters.Add("@nota4", MySqlDbType.VarChar).Value = dataGridView1.Rows[i].Cells[1].Value.ToString();
                        cmd.Parameters.Add("@id_lps4", MySqlDbType.VarChar).Value = label2.Text;
                        // MySqlCommand cmm = new MySqlCommand("select nota from notaproses where nota='" + dataGridView1.Rows[i].Cells[1].Value + "' and id_lps='" + label2.Text + "'", con);
                        Object obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        if (obj != null)
                        {
                            cmd.CommandText = @"UPDATE notaproses SET tempo=@tempo5,tonase=@tonase5,loading=@loading5,terkirim=@terkirim5,kembali=@kembali5,keterangan=@keterangan5,keterangan2=@keterangan25,id_lps=@id_lps5,idUrutan=@idUrutan5 where nota=@nota5 and id_lps=@id_lps5";
                            cmd.CommandType = CommandType.Text;
                            cmd.Connection = con;

                            

                            cmd.Parameters.Add("@tempo5", MySqlDbType.VarChar).Value = dataGridView1.Rows[i].Cells[3].Value;
                            cmd.Parameters.Add("@tonase5", MySqlDbType.VarChar).Value = dataGridView1.Rows[i].Cells[4].Value;
                            cmd.Parameters.Add("@loading5", MySqlDbType.VarChar).Value = dataGridView1.Rows[i].Cells[5].Value;
                            cmd.Parameters.Add("@terkirim5", MySqlDbType.VarChar).Value = dataGridView1.Rows[i].Cells[6].Value;
                            cmd.Parameters.Add("@kembali5", MySqlDbType.VarChar).Value = dataGridView1.Rows[i].Cells[7].Value;
                            cmd.Parameters.Add("@keterangan5", MySqlDbType.VarChar).Value = dataGridView1.Rows[i].Cells[9].Value;
                            cmd.Parameters.Add("@keterangan25", MySqlDbType.VarChar).Value = dataGridView1.Rows[i].Cells[8].Value;
                            cmd.Parameters.Add("@id_lps5", MySqlDbType.VarChar).Value = label2.Text;
                            cmd.Parameters.Add("@idUrutan5", MySqlDbType.VarChar).Value = dataGridView1.Rows[i].Cells[0].Value;
                            cmd.Parameters.Add("@nota5", MySqlDbType.VarChar).Value = dataGridView1.Rows[i].Cells[1].Value;

                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }
                        else
                        {
                            string aa = Convert.ToString(dataGridView1.Rows[i].Cells[1].Value);
                            if (aa.Equals(""))
                            {

                            }
                            else
                            {
                                string namacust = dataGridView1.Rows[i].Cells[2].Value.ToString();
                                string fixnama = namacust.Replace("'", "''");
                                // MessageBox.Show(fixnama);
                                // String StrQuery = "insert into notaproses (nota,customer,tempo,loading,id_lps,idUrutan)values('" + dataGridView1.Rows[i].Cells[1].Value + "','" + fixnama + "','" + dataGridView1.Rows[i].Cells[3].Value + "','" + dataGridView1.Rows[i].Cells[5].Value + "','" + label2.Text + "','" + dataGridView1.Rows[i].Cells[0].Value + "')";
                                // String StrQuery2 = @"insert into TBNOTA SET id_lps='" + label2.Text + "',tempo='" + dataGridView1.Rows[i].Cells[3].Value + "',loading='" + dataGridView1.Rows[i].Cells[5].Value + "',idUrutan='"+dataGridView1.Rows[i].Cells[0].Value+"' where nota='"
                                // + dataGridView1.Rows[i].Cells[1].Value + "'";
                                cmd.CommandText = @"insert into notaproses (nota,customer,tempo,loading,terkirim,kembali,keterangan,keterangan2,id_lps,idUrutan)values(@nota6,@customer6,@tempo6,@loading6,@terkirim6,@kembali6,@keterangan6,@keterangan26,@id_lps6,@idUrutan6)";
                                cmd.CommandType = CommandType.Text;
                                cmd.Connection = con;

                                cmd.Parameters.Add("@nota6", MySqlDbType.VarChar).Value = dataGridView1.Rows[i].Cells[1].Value.ToString();
                                cmd.Parameters.Add("@customer6", MySqlDbType.VarChar).Value = fixnama;
                                cmd.Parameters.Add("@tempo6", MySqlDbType.VarChar).Value = dataGridView1.Rows[i].Cells[3].Value.ToString();
                                cmd.Parameters.Add("@loading6", MySqlDbType.VarChar).Value = dataGridView1.Rows[i].Cells[5].Value.ToString();
                                cmd.Parameters.Add("@terkirim6", MySqlDbType.VarChar).Value = dataGridView1.Rows[i].Cells[6].Value;
                                cmd.Parameters.Add("@kembali6", MySqlDbType.VarChar).Value = dataGridView1.Rows[i].Cells[7].Value;
                                cmd.Parameters.Add("@keterangan6", MySqlDbType.VarChar).Value = dataGridView1.Rows[i].Cells[9].Value;
                                cmd.Parameters.Add("@keterangan26", MySqlDbType.VarChar).Value = dataGridView1.Rows[i].Cells[8].Value;
                                cmd.Parameters.Add("@id_lps6", MySqlDbType.VarChar).Value = label2.Text;
                                cmd.Parameters.Add("@idUrutan6", MySqlDbType.VarChar).Value = dataGridView1.Rows[i].Cells[0].Value.ToString();
                                // cmd.ExecuteNonQuery();

                                /*cmd.Parameters.Add("@nota4", MySqlDbType.VarChar).Value = dataGridView1.Rows[i].Cells[1].Value;
                                cmd.Parameters.Add("@customer4", MySqlDbType.VarChar).Value = dataGridView1.Rows[i].Cells[2].Value;
                                cmd.Parameters.Add("@tempo4", MySqlDbType.VarChar).Value = dataGridView1.Rows[i].Cells[3].Value;
                                cmd.Parameters.Add("@loading4", MySqlDbType.VarChar).Value = dataGridView1.Rows[i].Cells[5].Value;
                                cmd.Parameters.Add("@id_lps4", MySqlDbType.VarChar).Value = label2.Text;
                                cmd.Parameters.Add("@idUrutan4", MySqlDbType.VarChar).Value = dataGridView1.Rows[i].Cells[0].Value;*/
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                            }

                        }

                    }
                    con.Close();
                    button2.Text = "Print";
                    MessageBox.Show("Berhasil Mengupdate data!!");
                    getid();

                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error7");
            }
        }

        public void getdata(String lps, int kode)
        {
            if (kode == 1)
            {
                button2.Text = "Update";
            }
            // MessageBox.Show(lps);
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                getid();
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = @"SELECT * FROM notaproses where id_lps=@id_lps7 order by idUrutan asc";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;

                cmd.Parameters.Add("@id_lps7", MySqlDbType.VarChar).Value = lps;
                //MySqlCommand cmd = new MySqlCommand(sql, con);

                if (con.State != ConnectionState.Open)
                {
                    con.Close();
                    con.Open();
                }
                else
                {
                    con.Open();
                }

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    dataGridView1.Rows.Add(nomer, $"{reader.GetString("nota")}", $"{reader.GetString("customer")}", $"{reader.GetString("tempo")}", $"{reader.GetString("tonase")}", $"{reader.GetString("loading")}", $"{reader.GetString("terkirim")}", $"{reader.GetString("kembali")}", $"{reader.GetString("keterangan2")}", $"{reader.GetString("keterangan")}", $"{reader.GetString("br")}");
                    nomer += 1;

                }
                con.Close();
                MySqlCommand cmd3 = new MySqlCommand();
                cmd3.CommandText = @"select * from lps where id_lps=@id_lps8";
                cmd3.CommandType = CommandType.Text;
                cmd3.Connection = con;

                cmd3.Parameters.Add("@id_lps8", MySqlDbType.VarChar).Value = lps;

                if (con.State != ConnectionState.Open)
                {
                    con.Close();
                    con.Open();
                }
                else
                {
                    con.Open();
                }

                MySqlDataReader reader3 = cmd3.ExecuteReader();

                while (reader3.Read())
                {
                    label4.Text = (reader3.GetString("time"));
                    label2.Text = (reader3.GetString("id_lps"));
                    // textBox5.Text = (reader3.GetString("periode"));
                    dateTimePicker1.CustomFormat = "dd/MM/yyyy";
                    tempyear = (reader3.GetString("year"));
                    String[] subs = tempyear.Split('/');
                    dateTimePicker1.Value = new DateTime(Int16.Parse(subs[2]), Int16.Parse(subs[1]), Int16.Parse(subs[0]));
                    //  textBox6.Text = (reader3.GetString("tgl"));
                    textBox3.Text = (reader3.GetString("helper"));
                    textBox4.Text = (reader3.GetString("area"));
                    string drivertemp = (reader3.GetString("driver"));
                    string mobiltemp = (reader3.GetString("no_kendaraan"));
                    string[] driver = drivertemp.Split(',');

                    string[] mobil = mobiltemp.Split(',');
                    // int Max = textSplitArray.Max(x => drivertemp.Split(',').Length);
                    if (drivertemp.Contains(","))
                    {
                        supir2 = ", " + driver[1];
                        mobil2 = ", " + mobil[1];
                    }
                    else
                    {
                        
                    }
                   
                    comboBox1.SelectedIndex = comboBox1.FindString(mobil[0]);
                    comboBox2.SelectedIndex =comboBox2.FindString(driver[0]);
                    if (comboBox2.Text.Equals(""))
                    {
                        comboBox2.Text = (reader3.GetString("driver"));
                    }
                   // MessageBox.Show(reader3.GetString("driver"));
                   // MessageBox.Show(reader3.GetString("no_kendaraan"));
                }
                con.Close();
                
               if (kode == 2)
                {
                    print();
                }
                Refresh();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error8");
            }

        }

        private void print()
        {
            string daytemp = dateTimePicker1.Value.ToString("dd/MM/yyyy");
            string[] subs4 = daytemp.Split('/');
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                dt.Columns.Add("Faktur", typeof(string));
                dt.Columns.Add("Customer", typeof(string));
                dt.Columns.Add("Payment", typeof(string));
                dt.Columns.Add("Tonase", typeof(string));
                dt.Columns.Add("Loading", typeof(string));
                dt.Columns.Add("Terkirim", typeof(string));
                dt.Columns.Add("Return", typeof(string));
                dt.Columns.Add("Keterangan", typeof(string));

                //for (int i = kolom; i < 15; i++)
                //{
                //  dataGridView1.Rows.Add();
                //}

                foreach (DataGridViewRow dgv in dataGridView1.Rows)
                {
                 //   string aa = Convert.ToString(dgv.Cells[0].Value);
                    // MessageBox.Show(aa);
                  //  if (aa.Equals(""))
                 //   {

                 //   }
                //    else
                //    {
                        dt.Rows.Add(dgv.Cells[1].Value, dgv.Cells[2].Value, dgv.Cells[3].Value, dgv.Cells[4].Value, dgv.Cells[5].Value, dgv.Cells[6].Value, dgv.Cells[7].Value, dgv.Cells[8].Value);
                  //  }

                }
                ds.Tables.Add(dt);
                ds.WriteXmlSchema("applicant2.xml");
                Form3 f3 = new Form3();
                CrystalReport2 cr = new CrystalReport2();
                cr.SetDataSource(ds);
                TextObject txtbln = (TextObject)cr.ReportDefinition.Sections["Section2"].ReportObjects["txtbulan"];
                TextObject nopol = (TextObject)cr.ReportDefinition.Sections["Section2"].ReportObjects["txtnopol"];
                TextObject driver = (TextObject)cr.ReportDefinition.Sections["Section2"].ReportObjects["txtdriver"];
                TextObject tgl = (TextObject)cr.ReportDefinition.Sections["Section2"].ReportObjects["txttgl"];
                TextObject area = (TextObject)cr.ReportDefinition.Sections["Section2"].ReportObjects["txtarea"];
                TextObject helper = (TextObject)cr.ReportDefinition.Sections["Section2"].ReportObjects["txthelper"];
                TextObject lps = (TextObject)cr.ReportDefinition.Sections["Section1"].ReportObjects["Text1"];
                TextObject time = (TextObject)cr.ReportDefinition.Sections["Section1"].ReportObjects["Text9"];

                lps.Text = label2.Text;
                txtbln.Text = bulan2;
                nopol.Text = comboBox1.Text + mobil2;
                driver.Text = comboBox2.Text + supir2 ;
                tgl.Text = subs4[0];
                area.Text = textBox4.Text;
                helper.Text = textBox3.Text;
                time.Text = label4.Text;
                f3.crystalReportViewer1.ReportSource = cr;
                f3.crystalReportViewer1.Refresh();
                f3.Show();
                Cursor.Current = Cursors.Default;
                getid();
                kolom = 0;
                nomer = 1;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error5");
            }

        }

        private void tambahitem()
        {
            bool exist;
            if (con.State != ConnectionState.Open)
            {
                con.Close();
                con.Open();
            }
            else
            {
                con.Open();
            }
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"select keterangan from notaproses where nota=@nota9";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;

                cmd.Parameters.Add("@nota9", MySqlDbType.VarChar).Value = textBox1.Text;
                Object obj2 = cmd.ExecuteScalar();
                MySqlDataReader reader3 = cmd.ExecuteReader();
                reader3.Read();
                cmd.Parameters.Clear();
                if (obj2 == null)
                {
                    con.Close();
                    con.Open();
                    cmd.CommandText = @"select tbcust.area, tbcust.kontak from tbcust join tbnota on tbnota.kodecust = tbcust.id where tbnota.nota=@nota10";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    cmd.Parameters.Add("@nota10", MySqlDbType.VarChar).Value = textBox1.Text;
                    Object obj = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    if (obj != null)
                    {
                        cmd.CommandText = @"SELECT tbnota.nota, tbnota.customer,tbcust.kontak, tbnota.tempo,tbnota.br ,tbcust.area FROM tbnota JOIN tbcust ON tbnota.kodecust = tbcust.id where tbnota.nota=@nota11";
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        cmd.Parameters.Add("@nota11", MySqlDbType.VarChar).Value = textBox1.Text;

                        try
                        {


                            MySqlDataReader reader = cmd.ExecuteReader();

                            //while (reader.Read())
                            //   {
                            reader.Read();
                            //textBox3.Text += $"{reader.GetString("customer")}";
                            this.dataGridView1.Rows.Add(nomer, $"{reader.GetString("nota")}", $"{reader.GetString("customer")}"+" ("+ $"{reader.GetString("kontak")}"+")", $"{reader.GetString("tempo")}", "", textBox2.Text, "", "", "", "",$"{reader.GetString("br")}");

                            if (textBox4.Text.Equals(""))
                            {
                                areaList.Add(reader.GetString("area"));
                                textBox4.Text = reader.GetString("area");
                            }
                            //exist = areaList.Contains(reader.GetString("area"));
                            else if (exist = areaList.Contains(reader.GetString("area")))
                            {
                                //textBox4.Text = reader.GetString("area");
                            }
                            else
                            {
                                textBox4.Text += "," + reader.GetString("area");
                                areaList.Add(reader.GetString("area"));
                            }

                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.ToString(), "Error1");
                        }
                    }
                    else
                    {
                        cmd.CommandText = @"SELECT nota, customer, tempo,br FROM tbnota where nota=@nota12";
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        cmd.Parameters.Add("@nota12", MySqlDbType.VarChar).Value = textBox1.Text;

                        try
                        {
                            MySqlDataReader reader2 = cmd.ExecuteReader();
                            reader2.Read();
                            cmd.Parameters.Clear();
                            this.dataGridView1.Rows.Add(nomer, $"{reader2.GetString("nota")}", $"{reader2.GetString("customer")}", $"{reader2.GetString("tempo")}", "", textBox2.Text, "", "", "","", $"{reader2.GetString("br")}");
                            // MessageBox.Show("Data Area tidak ditemukan", "Peringatan!");

                            string namacusttemp11 = reader2.GetString("customer");
                            string namacust11 = namacusttemp11.Replace("'", "''");
                            con.Close();
                            con.Open();
                            cmd.CommandText = @"select area from area where customer LIKE '%@customer13%'";
                            cmd.CommandType = CommandType.Text;
                            cmd.Connection = con;
                            cmd.Parameters.Add("@customer13", MySqlDbType.VarChar).Value = namacust11;
                            //MySqlCommand cmm11 = new MySqlCommand("select area from area where customer LIKE '%" + namacust11 + "%'", con);
                            Object obj22 = cmd.ExecuteScalar();
                            cmd.Parameters.Clear();
                            if (obj22 != null)
                            {
                                MySqlDataReader reader = cmd.ExecuteReader();
                                reader.Read();

                                if (textBox4.Text.Equals(""))
                                {
                                    areaList.Add(reader.GetString("area"));
                                    textBox4.Text = reader.GetString("area");
                                }
                                else if (exist = areaList.Contains(reader.GetString("area")))
                                {

                                }
                                else
                                {
                                    textBox4.Text += "," + reader.GetString("area");
                                    areaList.Add(reader.GetString("area"));
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.ToString(), "Error2");
                        }
                    }
                }
                else
                {
                    con.Close();
                    con.Open();
                    cmd.CommandText = @"SELECT tbnota.nota, tbnota.customer,tbcust.kontak, tbnota.tempo,tbnota.br ,tbcust.area FROM tbnota JOIN tbcust ON tbnota.kodecust = tbcust.id where tbnota.nota=@nota14";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    cmd.Parameters.Add("@nota14", MySqlDbType.VarChar).Value = textBox1.Text;
                    //MySqlCommand cmm = new MySqlCommand("select area.area from area join tbnota on tbnota.customer = area.customer where tbnota.nota='" + textBox1.Text + "'", con);
                    Object obj = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    if (obj != null)
                    {
                        // MessageBox.Show("ada");
                       // cmd.CommandText = @"SELECT tbnota.nota, tbnota.customer, tbnota.tempo ,area.area,tbnota.br FROM tbnota JOIN area ON tbnota.customer = area.customer where tbnota.nota=@nota15";
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        cmd.Parameters.Add("@nota14", MySqlDbType.VarChar).Value = textBox1.Text;
                        //string sql = "SELECT tbnota.nota, tbnota.customer, tbnota.tempo ,area.area FROM tbnota JOIN area ON tbnota.customer = area.customer where tbnota.nota='" + textBox1.Text + "'";
                        // MySqlCommand cmd = new MySqlCommand(sql, con);

                        try
                        {


                            MySqlDataReader reader = cmd.ExecuteReader();

                            //while (reader.Read())
                            //   {
                            reader.Read();
                            //textBox3.Text += $"{reader.GetString("customer")}";
                            this.dataGridView1.Rows.Add(nomer, $"{reader.GetString("nota")}", $"{reader.GetString("customer")}"+" ("+ $"{reader.GetString("kontak")}"+")", $"{reader.GetString("tempo")}", "", textBox2.Text, "", "", "", "",$"{reader.GetString("br")}");

                            if (textBox4.Text.Equals(""))
                            {
                                areaList.Add(reader.GetString("area"));
                                textBox4.Text = reader.GetString("area");
                            }
                            //exist = areaList.Contains(reader.GetString("area"));
                            else if (exist = areaList.Contains(reader.GetString("area")))
                            {
                                //textBox4.Text = reader.GetString("area");
                            }
                            else
                            {
                                textBox4.Text += "," + reader.GetString("area");
                                areaList.Add(reader.GetString("area"));
                            }

                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.ToString(), "Error3");
                        }
                    }
                    else
                    {
                        cmd.CommandText = @"SELECT nota, customer, tempo,br FROM tbnota where nota=@nota16";
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        cmd.Parameters.Add("@nota16", MySqlDbType.VarChar).Value = textBox1.Text;
                        //string sql2 = "SELECT nota, customer, tempo FROM tbnota where nota='" + textBox1.Text + "'";
                        //MySqlCommand cmd2 = new MySqlCommand(sql2, con);

                        try
                        {
                            MySqlDataReader reader2 = cmd.ExecuteReader();
                            reader2.Read();
                            cmd.Parameters.Clear();
                            this.dataGridView1.Rows.Add(nomer, $"{reader2.GetString("nota")}", $"{reader2.GetString("customer")}", $"{reader2.GetString("tempo")}", "", textBox2.Text, "", "", "","", $"{reader2.GetString("br")}");

                            //MessageBox.Show("Data Area tidak ditemukan", "Peringatan!");
                            string namacusttemp = reader2.GetString("customer");
                            string namacust = namacusttemp.Replace("'", "''");
                            con.Close();
                            con.Open();
                            cmd.CommandText = @"select * from area where customer LIKE '%@customer17%'";
                            cmd.CommandType = CommandType.Text;
                            cmd.Connection = con;
                            cmd.Parameters.Add("@customer17", MySqlDbType.VarChar).Value = namacust;
                            //MySqlCommand cmm22 = new MySqlCommand("select area from area where customer LIKE '%" + namacust + "%'", con);
                            Object obj22 = cmd.ExecuteScalar();
                            cmd.Parameters.Clear();
                            if (obj22 != null)
                            {
                                MySqlDataReader reader = cmd.ExecuteReader();
                                reader.Read();

                                if (textBox4.Text.Equals(""))
                                {
                                    areaList.Add(reader.GetString("area"));
                                    textBox4.Text = reader.GetString("area");
                                }
                                else if (exist = areaList.Contains(reader.GetString("area")))
                                {

                                }
                                else
                                {
                                    textBox4.Text += "," + reader.GetString("area");
                                    areaList.Add(reader.GetString("area"));
                                }
                            }
                            else
                            {
                                MessageBox.Show("Data Area tidak ditemukan", "Peringatan!!");
                                //MessageBox.Show(namacust);
                            }
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.ToString(), "Error4");
                        }
                    }


                }
            }
            con.Close();
            textBox1.Text = "";
            textBox2.Text = "";
            this.ActiveControl = textBox1;
            kolom += 1;
            nomer += 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tambahitem();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text.Equals("Print"))
            {
                save();
            }
            else if (button2.Text.Equals("Update"))
            {
                update();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label4.Text = DateTime.Now.ToString("HH:mm");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox3.Text.Equals(""))
            {
                textBox3.Text += comboBox3.Text;
            }
            else
            {
                textBox3.Text += "," + comboBox3.Text;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (button2.Text.Equals("Print"))
            {
                checkdriver(comboBox1.Text);
               // MessageBox.Show("test");
            }
            else
            {

            }
            
        }

        private void checkdriver(string nopol)
        {
            //MessageBox.Show(nopol);
            if (nopol.Equals("DK 8090 BO (FUSO)"))
            {
                comboBox2.Text = "Ari W";
            }
            else if (nopol.Equals("DK 8918 BO (FUSO)"))
            {
                comboBox2.Text = "Irwanto";
            }
            else if (nopol.Equals("DK 8478 AW (FUSO)"))
            {
                comboBox2.Text = "Pak Ketut";
            }
            else if (nopol.Equals("DK 8646 CW (ENGKEL BOX)"))
            {
                comboBox2.Text = "Ari W";
            }
            else if (nopol.Equals("DK 8183 AF (ENGKEL BUKA)"))
            {
                comboBox2.Text = "Irwanto";
            }
            else if (nopol.Equals("DK 8916 CP (L300)"))
            {
                comboBox2.Text = "Adi";
            }
            else if (nopol.Equals("DK 8584 AC (L300)"))
            {
                comboBox2.Text = "Deni";
            }
            else if (nopol.Equals("DK 8979 AW (L300 BUKA)"))
            {
                comboBox2.Text = "Dani";
            }
            else if (nopol.Equals("DK 8175 BH (L300)"))
            {
                comboBox2.Text = "Putu Yohan";
            }
            else if (nopol.Equals("DK 8428 CO (L300)"))
            {
                comboBox2.Text = "Ricky";
            }
            else if (nopol.Equals("DK 8963 AG (L300 BUKA)"))
            {
                comboBox2.Text = "Harry";
            }
        }

        public void changetime(string time)
        {
            timer1.Stop();
            label4.Text = time;
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Form4 f = new Form4(this, 3);
            f.Show();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 6)
            {
                this.textBox2.Focus();
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
           /* if (button2.Text.Equals("Print"))
            {
                if (textBox6.Text.Equals(date))
                {
                    timer1.Start();
                }
                else
                {
                    timer1.Stop();
                    label4.Text = "08:00";
                }
            }else
            {

            }*/
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (status == false)
            {
                MessageBox.Show("Pilih terlebih dahulu data yang akan dihapus!!", "Peringatan!!");
            }
            else if (status == true)
            {
                deletedata();
            }
        }

        private void deletedata()
        {
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Close();
                    con.Open();
                }
                else
                {
                    con.Open();
                }
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.CommandText = @"delete from notaproses where nota=@nota18 and id_lps=@id_lps18";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    cmd.Parameters.Add("@nota18", MySqlDbType.VarChar).Value = message3;
                    cmd.Parameters.Add("@id_lps18", MySqlDbType.VarChar).Value = label2.Text;
                    //String query = @"delete from notaproses where nota='" + message3 + "' and id_lps='" + label2.Text + "'";
                    //cmd.CommandText = query;
                    //cmd.Connection = con;

                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    con.Close();
                    dataGridView1.Rows.RemoveAt(datadelete);
                    // getid();

                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error9");
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    //message = (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                    datadelete = e.RowIndex;
                    status = true;
                    message3 = (dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());
                    //MessageBox.Show(message3);
                    //full = message;
                    // MessageBox.Show(full);
                    // Clipboard.SetText(full);
                }
            }
            catch (Exception he)
            {
                MessageBox.Show(he.ToString(), "Error");
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                tambahitem();
            }
        }
        private string formatmonnth(string bulan)
        {
            if (bulan.Equals("01"))
            {
                bulan = "Januari";
            }
            else if (bulan.Equals("02"))
            {
                bulan = "Februari";
            }
            else if (bulan.Equals("03"))
            {
                bulan = "Maret";
            }
            else if (bulan.Equals("04"))
            {
                bulan = "April";
            }
            else if (bulan.Equals("05"))
            {
                bulan = "Mei";
            }
            else if (bulan.Equals("06"))
            {
                bulan = "Juni";
            }
            else if (bulan.Equals("07"))
            {
                bulan = "Juli";
            }
            else if (bulan.Equals("08"))
            {
                bulan = "Agustus";
            }
            else if (bulan.Equals("09"))
            {
                bulan = "September";
            }
            else if (bulan.Equals("10"))
            {
                bulan = "Oktober";
            }
            else if (bulan.Equals("11"))
            {
                bulan = "November";
            }
            else if (bulan.Equals("12"))
            {
                bulan = "Desember";
            }
            return bulan;
        }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (button2.Text.Equals("Print"))
            {
                if (dateTimePicker1.Value.ToString("dd").Equals(date))
                {
                    timer1.Start();
                }
                else
                {
                    timer1.Stop();
                    label4.Text = "08:00";
                    bulan2 = formatmonnth(dateTimePicker1.Value.ToString("MM"));
                    MessageBox.Show("asdad");
                }
            }
            else
            {
                
            }
        }

        private void Form2_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.F1)
            {
                dataGridView1.Rows.Add(nomer, "SJ", "", "", "", "", "", "", "", "", "");
                nomer += 1;
            }else if(e.KeyCode == Keys.F2)
            {
                supir2 = ", "+comboBox2.Text;
                mobil2 = ", "+comboBox1.Text;
                MessageBox.Show("Berhasil menambahkan Driver 2","Information");
            }
        }
    }
}
