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
    public partial class NewsDayDistribution : Form
    {
        public NewsDayDistribution(SimulationSystem system)
        {
            InitializeComponent();
            this.system = system;
        }
        SimulationSystem system;

        private void NewsDayDistribution_Load(object sender, EventArgs e)
        {
            DataTable d1 = new DataTable();
            d1.Columns.Add("Type of Newsday", typeof(Enums.DayType));
            d1.Columns.Add("Probability", typeof(decimal));
            d1.Columns.Add("Cumulative Probability", typeof(decimal));
            d1.Columns.Add("MinRang", typeof(int));
            d1.Columns.Add("MaxRang", typeof(int));
            for (int i = 0; i < system.DayTypeDistributions.Count; i++)
            {
                d1.Rows.Add(system.DayTypeDistributions[i].DayType, system.DayTypeDistributions[i].Probability, system.DayTypeDistributions[i].CummProbability, system.DayTypeDistributions[i].MinRange, system.DayTypeDistributions[i].MaxRange);

            }
            dataGridView1.DataSource = d1;
        }
    }
}
