using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NewspaperSellerModels;

namespace NewspaperSellerSimulation
{
    public partial class ReadData : Form
    {
        public ReadData()
        {
            InitializeComponent();
        }
        SimulationSystem system = new SimulationSystem();
        private void Form2_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("Test Case 1");
            comboBox1.Items.Add("Test Case 2");
            comboBox1.Items.Add("Test Case 3");

            
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Results res = new Results(system,comboBox1.SelectedIndex);
            this.Hide();
            res.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NewsDayDistribution news = new NewsDayDistribution(system);
            news.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DemandDistribution demand = new DemandDistribution(system);
            demand.Show();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            dataGridView2.DataSource = null;
            dataGridView2.Rows.Clear();
            dataGridView2.Refresh();
            dataGridView3.DataSource = null;
            dataGridView3.Rows.Clear();
            dataGridView3.Refresh();
            string path = "";
            if (comboBox1.SelectedIndex == 0)
            {
                path = "../../TestCases/TestCase1.txt";
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                path = "../../TestCases/TestCase2.txt";
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                path = "../../TestCases/TestCase3.txt";
            }
            system.ReadFile(path);
            DataTable dt = new DataTable();
            dt.Columns.Add("Number of Newspapers", typeof(int));
            dt.Columns.Add("Number of Records", typeof(int));
            dt.Columns.Add("Purchase Price", typeof(decimal));
            dt.Columns.Add("Scrap Price", typeof(decimal));
            dt.Columns.Add("Selling Price", typeof(decimal));
            dt.Rows.Add(system.NumOfNewspapers, system.NumOfRecords, system.PurchasePrice, system.ScrapPrice, system.SellingPrice);
            dataGridView1.DataSource = dt;

            DataTable dt2 = new DataTable();
            dt2.Columns.Add("Type of Newsday", typeof(int));
            dt2.Columns.Add("Probability", typeof(decimal));
            for (int i = 0; i < system.DayTypeDistributions.Count; i++)
            {
                dt2.Rows.Add((int)system.DayTypeDistributions[i].DayType, system.DayTypeDistributions[i].Probability);
            }
            dataGridView2.DataSource = dt2;
            DataTable dt3 = new DataTable();
            dt3.Columns.Add("Demand", typeof(int));
            dt3.Columns.Add("Good", typeof(decimal));
            dt3.Columns.Add("Fair", typeof(decimal));
            dt3.Columns.Add("Bad", typeof(decimal));
            for (int i = 0; i < system.DemandDistributions.Count; i++)
            {
                dt3.Rows.Add(system.DemandDistributions[i].Demand, system.DemandDistributions[i].DayTypeDistributions[0].Probability, system.DemandDistributions[i].DayTypeDistributions[1].Probability, system.DemandDistributions[i].DayTypeDistributions[2].Probability);
            }
            dataGridView3.DataSource = dt3;
        }

    }
}
