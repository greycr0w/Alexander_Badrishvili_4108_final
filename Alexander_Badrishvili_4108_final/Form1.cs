using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
namespace Alexander_Badrishvili_4108_final
{
    public partial class Form1 : Form
    {
        SqlConnection connection;
        SqlCommandBuilder cmdbuilder;
        SqlCommand command;
        SqlDataAdapter DataAdapter1, DataAdapter2, DataAdapter3, DataAdapter4;
        DataSet Dataset1, Dataset2, Dataset3, Dataset4;
        BindingSource BindingSource1, BindingSource2, BindingSource3, BindingSource4;

        private void button1_Click(object sender, EventArgs e)
        {
            String openPath;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                openPath = openFileDialog1.InitialDirectory + openFileDialog1.FileName;
                textBox24.Text = openPath;
                pictureBox1.Image = Image.FromFile(openPath);
                command = new SqlCommand("update PELATHS set FOTO='" + openPath + "' where KOD_PELATH=" + textBox1.Text + ";", connection);
                command.ExecuteNonQuery();
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            connection.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String openPath;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                openPath = openFileDialog1.InitialDirectory + openFileDialog1.FileName;
                textBox25.Text = openPath;
                pictureBox2.Image = Image.FromFile(openPath);
                command = new SqlCommand("update APOTHIKI set FOTO='" + openPath + "' where KE=" + textBox18.Text + ";", connection);
                command.ExecuteNonQuery();
            }
        }

        public Form1()
        {
            InitializeComponent();
            connection = new SqlConnection(@"Data Source=DESKTOP-JL4BPLI\WORKSTATIONSQL;Initial Catalog=APOTHIKI_4108;Integrated Security=True");
            connection.Open();

            //Kwdikas gia to gridViewPelaths
            DataAdapter1 = new SqlDataAdapter("Select * from PELATHS", connection);
            DataTable dt1 = new DataTable();
            DataAdapter1.Fill(dt1);
            comboBox1.DataSource = dt1;
            comboBox1.DisplayMember = "EPITHETO";

            //Kwdikas gia to gridViewApothiki
            DataAdapter4 = new SqlDataAdapter("Select * from APOTHIKI", connection);
            DataTable dt3 = new DataTable();
            DataAdapter4.Fill(dt3);
            comboBox2.DataSource = dt3;
            comboBox2.DisplayMember = "EIDOS";

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'aPOTHIKI_4108DataSet.PARAGELIA' table. You can move, or remove it, as needed.
            this.pARAGELIATableAdapter.Fill(this.aPOTHIKI_4108DataSet.PARAGELIA);
            // TODO: This line of code loads data into the 'aPOTHIKI_4108DataSet.APOTHIKI' table. You can move, or remove it, as needed.
            this.aPOTHIKITableAdapter.Fill(this.aPOTHIKI_4108DataSet.APOTHIKI);
            // TODO: This line of code loads data into the 'aPOTHIKI_4108DataSet.PELATHS' table. You can move, or remove it, as needed.
            this.pELATHSTableAdapter.Fill(this.aPOTHIKI_4108DataSet.PELATHS);


            this.reportViewer1.RefreshReport();
            this.reportViewer1.RefreshReport();
            this.reportViewer1.RefreshReport();
            this.reportViewer2.RefreshReport();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(pictureBox2.Image == null && File.Exists(textBox25.Text))
                pictureBox2.Image = Image.FromFile(textBox25.Text);
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillDataSetIstorikoPar();
        }

        //ISTORIKO PARAGELIWN
        private void fillDataSetIstorikoPar()
        {
            DataAdapter2 = new SqlDataAdapter("SELECT PELATHS.EPONYMIA, PELATHS.AFM, APOTHIKI.EIDOS, APOTHIKI.KATHGORIA, APOTHIKI.TIMH_POLHSHS, APOTHIKI.FPA, PROIONTA_PARAGELIAS.POSOTHTA FROM PELATHS " +
                "INNER JOIN PARAGELIA ON PELATHS.KOD_PELATH = PARAGELIA.K_PEL" +
                " INNER JOIN PROIONTA_PARAGELIAS ON PARAGELIA.KOD_PAR = PROIONTA_PARAGELIAS.K_PAR " +
                "INNER JOIN APOTHIKI ON APOTHIKI.KE = PROIONTA_PARAGELIAS.K_E" +
                " WHERE  PELATHS.EPITHETO = '" + comboBox1.Text.ToString() + "'", connection);
            Dataset2 = new DataSet();
            DataAdapter2.Fill(Dataset2);
            BindingSource2 = new BindingSource();
            DataTable dt1 = new DataTable();
            BindingSource2.DataSource = Dataset2.Tables[0].DefaultView;
            dataGridView1.DataSource = BindingSource2;
            float sum = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                sum += Convert.ToSingle(dataGridView1.Rows[i].Cells[6].Value) * Convert.ToSingle(dataGridView1.Rows[i].Cells[4].Value) * Convert.ToSingle(dataGridView1.Rows[i].Cells[5].Value);
            }
            label27.Text = sum.ToString();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillDataSetIstorikoApo();
        }

        //ISTORIKO KINHSHS APOTHIKIS
        private void fillDataSetIstorikoApo()
        {
            DataAdapter3 = new SqlDataAdapter("SELECT APOTHIKI.EIDOS, APOTHIKI.KATHGORIA, APOTHIKI.TIMH_POLHSHS, APOTHIKI.FPA, PARAGELIA.HMER_PARAGELIAS, PROIONTA_PARAGELIAS.POSOTHTA FROM PARAGELIA " +
               " INNER JOIN PROIONTA_PARAGELIAS ON PARAGELIA.KOD_PAR = PROIONTA_PARAGELIAS.K_PAR " +
               "INNER JOIN APOTHIKI ON APOTHIKI.KE = PROIONTA_PARAGELIAS.K_E" +
               " WHERE  APOTHIKI.EIDOS = '" + comboBox2.Text.ToString() + "'", connection);
            Dataset3 = new DataSet();
            DataAdapter3.Fill(Dataset3);
            BindingSource3 = new BindingSource();
            DataTable dt = new DataTable();
            BindingSource3.DataSource = Dataset3.Tables[0].DefaultView;
            dataGridView2.DataSource = BindingSource3;
            float sum = 0;
            for (int i1 = 0; i1 < dataGridView2.Rows.Count; i1++)
            {
                sum += Convert.ToSingle(dataGridView2.Rows[i1].Cells[2].Value) * Convert.ToSingle(dataGridView2.Rows[i1].Cells[5].Value);
            }
            label30.Text = sum.ToString();
        }
        // 
        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            String checkNav = ((ToolStripButton)sender).Owner.Text; //type casting, for the purpose of using 1, and only 1 function for all three binding navigators
            if (checkNav.Equals(bindingNavigator1.Text))
            {
                try
                {
                    this.Validate();
                    this.pELATHSBindingSource.EndEdit();
                    this.pELATHSTableAdapter.Update(this.aPOTHIKI_4108DataSet.PELATHS);
                    MessageBox.Show("Pelaths Table Updated", "Success");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Not successful", "Error");

                }
            }
            else if (checkNav.Equals(bindingNavigator2.Text))
            {
                try
                {
                    this.Validate();
                    this.aPOTHIKIBindingSource.EndEdit();
                    this.aPOTHIKITableAdapter.Update(this.aPOTHIKI_4108DataSet.APOTHIKI);
                    MessageBox.Show("Apothiki Table Updated", "Success");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Not successful", "Error");

                }
            }
            else
            {
                try
                {
                    this.Validate();
                    this.pARAGELIABindingSource.EndEdit();
                    this.pARAGELIATableAdapter.Update(this.aPOTHIKI_4108DataSet.PARAGELIA);
                    MessageBox.Show("Paragelia Table Updated", "Success");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Not successful", "Error");

                }
            }
        }

        //GIA THN PHOTO KATHE PELATH
        private void refreshImagePelaths()
        {
            String imgPath;

            imgPath = textBox24.Text;
            if (File.Exists(imgPath))
            {
                pictureBox1.Image = Image.FromFile(imgPath);
            }
        }

        //GIA THN PHOTO KATHE PROIONTOS THS APOTHIKIS
        private void refreshImageApothiki()
        {
            String imgPath;

            imgPath = textBox25.Text;
            if (File.Exists(imgPath))
            {
                pictureBox2.Image = Image.FromFile(imgPath);
            }
        }

        //GIA THN ANANAIWSH TWN ANTIKEIMENWN PELATH, PHOTO STHN PERIPTWSH MAS
        private void bindingNavigator1_RefreshItems(object sender, EventArgs e)
        {

            pictureBox1.Image = null;
            if (textBox24.Text != null)
            {
                refreshImagePelaths();
            }

        }
        private void bindingNavigator2_RefreshItems(object sender, EventArgs e)
        {
            pictureBox2.Image = null;
            if (textBox25.Text != null)
            {
                refreshImageApothiki();
            }
        }
    }
}


    


