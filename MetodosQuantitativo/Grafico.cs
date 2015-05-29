using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MetodosQuantitativo
{
    public partial class Grafico : Form
    {
        public Grafico()
        {
            InitializeComponent();
            
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series1.Legend = "Função newton";
            series1.Name = "Newton";
            this.chart1.Series.Add(series1);
        }

    }
}
