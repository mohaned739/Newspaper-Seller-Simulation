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
using NewspaperSellerTesting;


namespace NewspaperSellerSimulation
{
    public partial class Results : Form
    {
        public Results(SimulationSystem system,int caseNo)
        {
            InitializeComponent();
            this.system = system;
            this.caseNo = caseNo;
        }
        SimulationSystem system;
        int caseNo;
        private void Results_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Day", typeof(int));
            dt.Columns.Add("Random Digits for Type of Newsday", typeof(int));
            dt.Columns.Add("Type of Newsday", typeof(Enums.DayType));
            dt.Columns.Add("Random Digits of Demand", typeof(int));
            dt.Columns.Add("Demand", typeof(int));
            dt.Columns.Add("Revenue from Sales", typeof(decimal));
            dt.Columns.Add("Lost Profit from Excess Demand", typeof(decimal));
            dt.Columns.Add("Salvage from Sale of Scrap", typeof(decimal));
            dt.Columns.Add("Daily Profit", typeof(decimal));
            system.CreateTable();
            dataGridView1.DataSource = dt;
            textBox1.Text = system.PerformanceMeasures.TotalSalesProfit.ToString();
            textBox2.Text = system.UnitProfit.ToString();
            textBox3.Text = system.PerformanceMeasures.TotalCost.ToString();
            textBox4.Text = system.PerformanceMeasures.TotalLostProfit.ToString();
            textBox5.Text = system.PerformanceMeasures.TotalScrapProfit.ToString();
            textBox6.Text = system.PerformanceMeasures.TotalNetProfit.ToString();
            textBox7.Text = system.NumOfNewspapers.ToString();
            textBox8.Text = system.PurchasePrice.ToString();
            textBox9.Text = system.SellingPrice.ToString();
            textBox10.Text = system.ScrapPrice.ToString();
            textBox11.Text = system.PerformanceMeasures.DaysWithMoreDemand.ToString();
            textBox12.Text = system.PerformanceMeasures.DaysWithUnsoldPapers.ToString();

            foreach (SimulationCase row in system.SimulationTable){
                dt.Rows.Add(row.DayNo, row.RandomNewsDayType, row.NewsDayType, row.RandomDemand, row.Demand, row.SalesProfit, row.LostProfit, row.ScrapProfit, row.DailyNetProfit);
            }
            if (caseNo==0){
                string result = TestingManager.Test(system, Constants.FileNames.TestCase1);
                MessageBox.Show(result);
            }
            else if (caseNo == 1){
                string result = TestingManager.Test(system, Constants.FileNames.TestCase2);
                MessageBox.Show(result);
            }
            else{
                string result = TestingManager.Test(system, Constants.FileNames.TestCase3);
                MessageBox.Show(result);
            }
            
        }

      
   
        private void Results_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReadData read = new ReadData();
            read.Show();
            this.Hide();
        }
      
    }
}
