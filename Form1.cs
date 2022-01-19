namespace Coursework_1
{
    public partial class Form1 : Form
    {
        public Form1() //initialising main form Form1
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) // on form1 loading (anything that should be run immediately on launch)
        {
            
        }

        private void InfoButton_Click(object sender, EventArgs e)//Info Button clicked
        {
            MessageBox.Show("Helpful Info!\n\nChoose a Solver using the Analytical and Numerical Boxes!\n\nInput your 4 or 5 digit NACA Aerofoil number in the NACA code box!\n\nInput your angle of attack (in degrees) in the AoA box!");
        }

        private void SolveButton_Click(object sender, EventArgs e)
        {
            //Input and settings validation
            //Solver selected validation - must have one box selected
            if (AnalyticalSolveButton.Checked == false && NumericalSolveButton.Checked == false) { MessageBox.Show("Please Choose a Solver method!"); return; }

            //NACA code input validation
            //validation-must be an integer of length 4 or 5
            try { Convert.ToInt32(NACAtextBox.Text); }

            catch (Exception) { MessageBox.Show("Please enter a valid NACA aerofoil code"); return; }

            if (NACAtextBox.Text.Length != 4 && NACAtextBox.Text.Length != 5) { MessageBox.Show("Please enter a valid NACA aerofoil code"); return; }
            if (NACAtextBox.Text[0] == '0' && NACAtextBox.Text[1] != '0') { MessageBox.Show("Please enter a valid NACA aerofoil code"); return; }
            if (NACAtextBox.Text[0] != '0' && NACAtextBox.Text[1] == '0') { MessageBox.Show("Please enter a valid NACA aerofoil code"); return; }

            //Angle of attack validation
            //validation-must be a float
            try { float AttackAngle = Convert.ToSingle(AttackAngletextBox.Text); }

            catch (Exception) { MessageBox.Show("Please enter a valid Angle of Attack"); return; }

            //Parsing NACA wing code
            // [ For NACA code 1234 -> 1 = max camber as % of chord, 2 = position of max camber as chord/10, 34 = max thickness as % of chord ]
            // [ For NACA code 12345 -> 1 = optimal CL at ideal AoA, 2 = position of max camber, 3 = camber reflex yes/no, 45 = max thickness ]

            string NACAcode = NACAtextBox.Text; //Reading NACA code from textbox to string variable
            double maxCamber = 0;
            double posCamber = 0;
            double thickness = 0;
            double optimalCL = 0;
            int reflex = 0;

            if (NACAcode.Length == 4)   //Parsing NACA info for 4 digit
            {
                maxCamber = double.Parse(Convert.ToString(NACAcode[0])) / 100;
                posCamber = double.Parse(Convert.ToString(NACAcode[1])) / 10;
                thickness = Convert.ToDouble(Convert.ToString(NACAcode[2]) + Convert.ToString(NACAcode[3])) / 100; //Get max thickness from last 2 digits of NACA code + divide by 100 for 0-1 chord range
            }
            else if (NACAcode.Length == 5)  //Parsing NACA info for 5 digit
            {
                optimalCL = double.Parse(Convert.ToString(NACAcode[0])) * 0.15;
                maxCamber = double.Parse(Convert.ToString(NACAcode[1])) / 10;
                reflex = int.Parse(Convert.ToString(NACAcode[2]));
                thickness = Convert.ToDouble(Convert.ToString(NACAcode[3]) + Convert.ToString(NACAcode[4])) / 100; //Get max thickness from last 2 digits of NACA code + divide by 100 for 0-1 chord range
            }

            Plot4d(maxCamber, posCamber, thickness);
            
        }

        //Functions for returning aerofoil sections
        private static double[] coords_4d(double x, double m, double p, double t) //Symmetric 4digit NACA - returns list[]
            //x-coordinate, m-maximum camber, p-position of max camber
            //RETURNS list[length=6]
            //list[[0].x-upper, [1].x-lower, [2].y-upper, [3].y-lower, [4].y-chord, [5].y-symmetric] - CAMBERED
            //list[[0].x-upper, [1].x-lower, [2].y-upper, [3].y-lower, [4].0, [5].0] - SYMMETRIC
        {
            double yt =  5 * t * (0.2969 * Math.Sqrt(x) - 0.126 * x - 0.3516 * x * x + 0.2843 * x * x * x - 0.1036 * x * x * x * x); //symmetric thickness/shape y value
    
            double yc = 0; //cambered wing camber line
            double dy = 0; //dyc/dx

            double[] output = Array.Empty<double>();
            
            if (m != 0) //Routine for cambered aerofoils
            {
                if (x <= p) //for fore/aft sections for x<p // x>p
                {
                    yc = (m / (p * p)) * (2 * p * x - x * x); //y - mean camber line
                    dy = ((2 * m) / (p * p)) * (p - x);  //dy - dyc/dx
                }

                else
                {
                    yc = (m / (Math.Pow(1 - p, 2)) * ((1 - 2 * p) + 2 * p * x - x * x));
                    dy = ((2 * m) / (Math.Pow(1 - p, 2))) * (p - x);
                }

                double theta = Math.Atan(dy); //Calculating theta angle FOR ASYMMETRIC wings

                output = new double[6] { x - yt * Math.Sin(theta), x + yt * Math.Sin(theta), yc + yt * Math.Cos(theta), yc - yt * Math.Cos(theta), yc, yt }; //output list
                return output;
            }

            output = new double[6] { x, x, yt, -yt, 0, 0 };
            return output;

        }

        private void Plot4d(double m, double p, double t)
        {
            int res = 250; //number of points to use for plotting - (resolution control)
            //creating x linspace array
            double[] xrange = new double[res];
            double[] yzero = new double[res];

            for (double i = 0; i < res; i++)
            {
                xrange[Convert.ToInt32(i)] = Convert.ToDouble(i / res);
                yzero[Convert.ToInt32(i)] = 0;
            }

            double[] xupper = new double[res];
            double[] yupper = new double[res];
            double[] xlower = new double[res];
            double[] ylower = new double[res];
            double[] ychord = new double[res];
            double[] ysymm = new double[res];

            for (int i = 0; i < res; i++)
            {
                double[] coords = coords_4d(xrange[i], m, p, t);
                xupper[i] = coords[0];
                xlower[i] = coords[1];
                yupper[i] = coords[2];
                ylower[i] = coords[3];
                ychord[i] = coords[4];
                ysymm[i] = coords[5];
            }

            WingPlot.Reset();

            WingPlot.Plot.AddScatterLines(xupper, yupper, Color.Black);
            WingPlot.Plot.AddScatterLines(xlower, ylower, Color.Black);
            WingPlot.Plot.AddScatterLines(xrange, ychord, Color.Blue);
            WingPlot.Plot.AddScatterLines(xrange, yzero, Color.Black);

            WingPlot.Plot.SetAxisLimitsX(-0.05, 1.01);
            WingPlot.Plot.SetAxisLimitsY(-1, 1);

            WingPlot.Refresh();

        }

    }
}