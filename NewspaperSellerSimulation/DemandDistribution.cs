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
    public partial class DemandDistribution : Form
    {
        public DemandDistribution(SimulationSystem system)
        {
            InitializeComponent();
            this.system = system;
        }
        SimulationSystem system;
        private void DemandDistribution_Load(object sender, EventArgs e)
        {
            DataTable d2 = new DataTable();

            d2.Columns.Add("Demand", typeof(int));
            d2.Columns.Add("Good", typeof(decimal));
            d2.Columns.Add("Fair", typeof(decimal));
            d2.Columns.Add("Poor", typeof(decimal));
            d2.Columns.Add("Good MinRang", typeof(int));
            d2.Columns.Add("Good MaxRang", typeof(int));
            d2.Columns.Add("Fair MinRang", typeof(int));
            d2.Columns.Add("Fair MaxRang", typeof(int));
            d2.Columns.Add("Poor MinRang", typeof(int));
            d2.Columns.Add("Poor MaxRang", typeof(int));
            for (int i = 0; i < system.DemandDistributions.Count; i++)
            {
                for (int j = 0; j < system.DemandDistributions[i].DayTypeDistributions.Count; j++){
                    if (system.DemandDistributions[i].DayTypeDistributions[j].MinRange>100){
                        system.DemandDistributions[i].DayTypeDistributions[j].MinRange = 0;
                        system.DemandDistributions[i].DayTypeDistributions[j].MaxRange = 0;
                    }
                }
                    d2.Rows.Add(system.DemandDistributions[i].Demand, system.DemandDistributions[i].DayTypeDistributions[0].CummProbability,system.DemandDistributions[i].DayTypeDistributions[1].CummProbability,system.DemandDistributions[i].DayTypeDistributions[2].CummProbability,system.DemandDistributions[i].DayTypeDistributions[0].MinRange,system.DemandDistributions[i].DayTypeDistributions[0].MaxRange,system.DemandDistributions[i].DayTypeDistributions[1].MinRange,system.DemandDistributions[i].DayTypeDistributions[1].MaxRange,system.DemandDistributions[i].DayTypeDistributions[2].MinRange,system.DemandDistributions[i].DayTypeDistributions[2].MaxRange);
            }
            dataGridView1.DataSource = d2;
        }
    }
}
