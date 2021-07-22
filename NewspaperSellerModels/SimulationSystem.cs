using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewspaperSellerModels
{
    public class SimulationSystem
    {
        public SimulationSystem()
        {
            DayTypeDistributions = new List<DayTypeDistribution>();
            DemandDistributions = new List<DemandDistribution>();
            SimulationTable = new List<SimulationCase>();
            PerformanceMeasures = new PerformanceMeasures();
        }
        ///////////// INPUTS /////////////
        public int NumOfNewspapers { get; set; }
        public int NumOfRecords { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal ScrapPrice { get; set; }
        public decimal UnitProfit { get; set; }
        public List<DayTypeDistribution> DayTypeDistributions { get; set; }
        public List<DemandDistribution> DemandDistributions { get; set; }

        ///////////// OUTPUTS /////////////
        public List<SimulationCase> SimulationTable { get; set; }
        public PerformanceMeasures PerformanceMeasures { get; set; }



        public void ReadFile(string path){
            string[] lines = System.IO.File.ReadAllLines(path);
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i] == "") { continue; }
                else if (lines[i] == "NumOfNewspapers"){
                    this.NumOfNewspapers = int.Parse(lines[i + 1]);
                    i++;
                }
                else if (lines[i] == "NumOfRecords"){
                    this.NumOfRecords = int.Parse(lines[i + 1]);
                    i++;
                }
                else if (lines[i] == "PurchasePrice"){
                    this.PurchasePrice = decimal.Parse(lines[i + 1]);
                    i++;
                }
                else if (lines[i] == "ScrapPrice"){
                    this.ScrapPrice = decimal.Parse(lines[i + 1]);
                    i++;
                }
                else if (lines[i] == "SellingPrice"){
                    this.SellingPrice = decimal.Parse(lines[i + 1]);
                    i++;
                }
                else if (lines[i] == "DayTypeDistributions"){
                    string []line =lines[i + 1].Split(',');
                    decimal cumProb =0;
                    int min=1;
                    int max;
                    for (int j = 0; j < 3; j++){
                        DayTypeDistribution dtd = new DayTypeDistribution();
                        if (j==0){
                            dtd.DayType = Enums.DayType.Good;
                        }
                        else if (j == 1){
                            dtd.DayType = Enums.DayType.Fair;
                        }
                        else if (j == 2){
                            dtd.DayType = Enums.DayType.Poor;
                        }
                        dtd.Probability = decimal.Parse(line[j].TrimStart());
                        cumProb += dtd.Probability;
                        dtd.CummProbability = cumProb;
                        dtd.MinRange = min;
                        max = (int)(cumProb * 100);
                        dtd.MaxRange = max;
                        min = max + 1;
                        this.DayTypeDistributions.Add(dtd);
                    }
                    i++;
                }
                else if (lines[i] == "DemandDistributions"){
                    i++;
                    decimal []cumProb = new decimal[3];
                    int []min = new int[3];
                    min[0] = min[1] = min[2] = 1;
                    int []max=new int[3];
                    while (i<lines.Length)
                    {
                        string[] line = lines[i].Split(',');
                        DemandDistribution dd = new DemandDistribution();
                        dd.Demand = int.Parse(line[0]);
                        for (int j = 1; j < 4; j++){
                            DayTypeDistribution dtd = new DayTypeDistribution();
                            if (j == 1){
                                dtd.DayType = Enums.DayType.Good;
                            }
                            else if (j == 2){
                                dtd.DayType = Enums.DayType.Fair;
                            }
                            else if (j == 3){
                                dtd.DayType = Enums.DayType.Poor;
                            }
                            dtd.Probability = decimal.Parse(line[j].TrimStart());
                            cumProb[j-1] += dtd.Probability;
                            dtd.CummProbability = cumProb[j-1];
                            dtd.MinRange = min[j-1];
                            max[j-1] = (int)(cumProb[j-1] * 100);
                            dtd.MaxRange = max[j-1];
                            min[j-1] = max[j-1] + 1;
                            dd.DayTypeDistributions.Add(dtd);
                        }
                        this.DemandDistributions.Add(dd);
                        i++;
                    }
                }

            }
        }


        public void CreateTable(){
            this.UnitProfit = this.NumOfNewspapers * this.PurchasePrice;
            var rand = new Random();
            for (int i = 0; i < this.NumOfRecords; i++){
                SimulationCase row = new SimulationCase();
                row.DayNo = i + 1;
                row.RandomNewsDayType = rand.Next(1, 100);
                row.NewsDayType = (Enums.DayType)Map_DayType(row.RandomNewsDayType);
                row.RandomDemand = rand.Next(1, 100);
                row.Demand = Map_Demand(row.RandomDemand, row.NewsDayType);
                row.DailyCost = this.UnitProfit;
                if (row.Demand<=this.NumOfNewspapers){
                    row.SalesProfit = row.Demand * this.SellingPrice;
                    row.LostProfit = 0;
                    row.ScrapProfit = (this.NumOfNewspapers - row.Demand) * this.ScrapPrice;
                    if (row.Demand < this.NumOfNewspapers){
                        this.PerformanceMeasures.DaysWithUnsoldPapers++;
                    }
                    this.PerformanceMeasures.TotalScrapProfit += row.ScrapProfit;
                }
                else{
                    row.SalesProfit = this.NumOfNewspapers * this.SellingPrice;
                    row.LostProfit = (row.Demand-this.NumOfNewspapers)*(this.SellingPrice-this.PurchasePrice);
                    row.ScrapProfit = 0;
                    this.PerformanceMeasures.DaysWithMoreDemand++;
                    this.PerformanceMeasures.TotalLostProfit += row.LostProfit;
                }
                row.DailyNetProfit = row.SalesProfit - row.DailyCost - row.LostProfit + row.ScrapProfit;
                this.PerformanceMeasures.TotalSalesProfit += row.SalesProfit;
                this.PerformanceMeasures.TotalNetProfit += row.DailyNetProfit;
                this.SimulationTable.Add(row);
            }
                this.PerformanceMeasures.TotalCost = this.UnitProfit*this.NumOfRecords;
                

        }

        private int Map_DayType(int randomNum)
        {
            foreach (DayTypeDistribution dtd in this.DayTypeDistributions){
                if (randomNum <= dtd.MaxRange)
                {
                    return (int)dtd.DayType;
                }
            }
            return 0;
        }

        private int Map_Demand(int randomNum,Enums.DayType dt)
        {
            foreach (DemandDistribution dd in this.DemandDistributions){
                if (randomNum <= dd.DayTypeDistributions[(int)dt].MaxRange)
                {
                    return (int)dd.Demand;
                }
            }
            return 0;
        }
    }
}
