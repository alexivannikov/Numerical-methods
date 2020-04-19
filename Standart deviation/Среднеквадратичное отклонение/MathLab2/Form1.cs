using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace MathLab2
{
    public partial class Form1 : Form
    {
        static public List<Point> points = new List<Point>();
        static public List<Point> result = new List<Point>();
        static public List<double> coefficients = new List<double>();
        static public int POINTS = 10;

        public Form1()
        {
            InitializeComponent();

            DataInit();
            TableInit();
            GraphInit();
            Calculation();
        }

        public void GraphInit() { 

            chart1.Series.Clear();

            chart1.ChartAreas[0].AxisX.Interval = 0.5;
            chart1.ChartAreas[0].AxisX.Minimum = -POINTS/2;
            chart1.ChartAreas[0].AxisX.Maximum = POINTS / 2;

            chart1.ChartAreas[0].AxisY.Interval = 0.5;
            chart1.ChartAreas[0].AxisY.Minimum = -POINTS / 2;
            chart1.ChartAreas[0].AxisY.Maximum = POINTS / 2;

            var series1 = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "Series1",
                Color = System.Drawing.Color.Blue,
                IsVisibleInLegend = false,
                BorderWidth=5,
                ChartType = SeriesChartType.Point
            };

            chart1.Series.Add(series1);
            foreach(Point data in points)
                chart1.Series[0].Points.AddXY(data.x, data.y);

            var series2 = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "Series2",
                Color = System.Drawing.Color.Red,
                IsVisibleInLegend = false,
                BorderWidth=5,
                ChartType = SeriesChartType.Spline
            };

            chart1.Series.Add(series2);
            foreach(Point point in result)
                chart1.Series[1].Points.AddXY(point.x, point.y);

            chart1.Invalidate();

        }

        public void DataInit(){
            Random rnd = new Random();

            for (int i = -POINTS / 2; i <= POINTS; i++) {
                Point point = new Point(i, rnd.NextDouble()*rnd.Next(-POINTS/ 2, POINTS/2));
                points.Add(point);
            }
        }

        public void TableInit() {
            dataGridView1.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridView1.Columns[0].DefaultCellStyle.Format = "n2";
            dataGridView1.Columns[1].DefaultCellStyle.Format = "n3";
            dataGridView1.Columns[2].DefaultCellStyle.Format = "n3";

            for (int i = 0; i <= POINTS; i++)
            {
                dataGridView1.Rows.Add();
                dataGridView1[0, i].Value = Convert.ToString(Math.Round((points[i].x), 2));
                dataGridView1[1, i].Value = Convert.ToString(Math.Round((points[i].y), 3));
                dataGridView1[2, i].Value = Convert.ToString(Math.Round((points[i].weight), 3));
            }

        }

        public void Calculation() {
            for (int i = 0; i <= POINTS; i++) {
                points[i].x = Convert.ToDouble(dataGridView1[0, i].Value);
                points[i].y = Convert.ToDouble(dataGridView1[1, i].Value);
                points[i].weight = Convert.ToDouble(dataGridView1[2, i].Value);
            }

            MatrixCreation();
            EquationBuild();
            GraphInit();
        }

        public void MatrixCreation() {
            //Степень полинома
            int n;

            if (domainUpDown1.SelectedIndex == -1)
                n = 0;
            else
                n = Convert.ToInt32(domainUpDown1.SelectedItem.ToString());
          
            int str = n + 1; //Количество строк в матрице 
            int stlb = n + 2; //Количество столбцов в матрице 

            double sumItems = 0;

            int xItems = str + (str - 1);
            int yItems = str;

            double[,] matrix = new double[str, stlb]; //Матрица 
            double[] XArray = new double[xItems]; //Вектор сумм ro*x
            double[] YArray = new double[yItems]; //Вектор сумм ro*x*y
            
            for (int i = 0; i < xItems; i++) //От 0 до 2*n
            {
                for (int j = 0; j <= POINTS; j++)
                    sumItems += Math.Pow(points[j].x, i) * points[j].weight;
                XArray[i] = sumItems;
                sumItems = 0;
            }
            
            sumItems = 0;

            for (int i = 0; i < yItems; i++) {
                for (int j = 0; j <= POINTS; j++)
                    sumItems += points[j].y * Math.Pow(points[j].x, i)*points[j].weight;
                YArray[i] = sumItems;
                sumItems = 0;
            }

            //Заполнение матрицы построчно
            int item = 0, count = 0, index = 0;

             for (int i = 0; i < str; i++) {
                 for (; item < str; item++)
                     matrix[i, item] = XArray[index++];

                    matrix[i, item] = YArray[i];
                 item = 0; count++; index = count;               
            }

            string s;

            listBox1.Items.Clear();
            listBox1.Items.Add("Matrix");

            for (int i = 0; i < str; i++)
            {
                s = "";
                for (int j = 0; j < str+1; j++)
                    s += (matrix[i, j]).ToString() + " " + " ";
                listBox1.Items.Add(s);
            }

            listBox1.Items.Add(" ");
            listBox1.Items.Add("Diagonal Matrix");

            for (int i = 0; i < str; i++)
            {
                s = "";
                for (int j = 0; j < str+1; j++)
                {
                    GaussianMethod(matrix, n);
                    s += (matrix[i, j]).ToString() + " " + " ";
                }
                listBox1.Items.Add(s);
            }

        }

            public void GaussianMethod(double[,] matrix, int n) {
            int eqQty = n + 1;
            double mult;
            double[] tempCoff = new double[eqQty];

            coefficients = new List<double>();

            for (int i = 0; i < (eqQty - 1); i++)
            {
                for (int j = i + 1; j < eqQty; j++)
                {
                    mult = (-1) * matrix[j, i] / matrix[i, i];
                    for (int k = i; k <= eqQty; k++)
                        matrix[j, k] += matrix[i, k] * mult;
                }
            }
         
            double number;

            for (int i = (eqQty - 1); i >= 0; i--) {
                number = matrix[i, eqQty];
                for (int j = (eqQty - 1); j > i; j--)
                    number -= matrix[i, j]*tempCoff[j];

                tempCoff[i] = number / matrix[i, i];
            }

            foreach (double num in tempCoff)
                coefficients.Add(num);
        }

        public void EquationBuild() {
            double res; int power;

            result = new List<Point>();

            for (int i = 0; i <= POINTS; i++) {
                power = 0; res = 0;
                foreach (double val in coefficients)
                {
                    res += val * Math.Pow(points[i].x, power);
                    power++;
                }
                Point p = new Point(points[i].x, res, points[i].weight);
                result.Add(p);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Calculation();
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void domainUpDown1_SelectedItemChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }

    public class Point{
        public double x;
        public double y;
        public double weight;

        public Point(){
            x=0; y=0; weight=1;
        }

        public Point(double ix, double iy, double iw=1) {
            x = ix; y = iy; weight = iw;
        }
    }
}
