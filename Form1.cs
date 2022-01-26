//*******************************************************************
//TO DO LIST:
//1. Implement solving of aerofoil data to display function
//2. Implement Thin aerofoil theory to calculate and display
//3. Refactor Plotting function to be its own independant universal function
//4. Refactor Input validation to be its own independant function
//5. Refactor NACA code parsing into its own independant function
//6. Add Comment lines to entire project
//7. Clean/Tidy the UI and spice it up a bit
//8. Refactor Coords_4d/5d to one universal coordinate solving function
//9. Design universal parameter passing system to easily move data between functions using arrays
//10.Design UI system that changes the UI to display different data for the appropriate NACA code
//11.Move as many functions to object oriented system as possible! - Solver, Coords, Plotting, etc.
//*******************************************************************
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
            NACA_Parse NACA1 = new NACA_Parse(NACAtextBox.Text);

            //NACA code input validation
            if (NACA1.Valid() == false) { return; }


            //Input and settings validation
            //Solver selected validation - must have one box selected
            if (AnalyticalSolveButton.Checked == false && NumericalSolveButton.Checked == false) { MessageBox.Show("Please Choose a Solver method!"); return; }

            //Angle of attack validation
            //validation-must be a float
            try { float AttackAngle = Convert.ToSingle(AttackAngletextBox.Text); }

            catch (Exception) { MessageBox.Show("Please enter a valid Angle of Attack"); return; }

            //Parsing NACA wing code
            // [ For NACA code 1234 -> 1 = max camber as % of chord, 2 = position of max camber as chord/10, 34 = max thickness as % of chord ]
            // [ For NACA code 12345 -> 1 = optimal CL at ideal AoA, 2 = position of max camber, 3 = camber reflex yes/no, 45 = max thickness ]

            string NACAcode = NACAtextBox.Text; //Reading NACA code from textbox to string variable
            double maxCamber = new();
            double posCamber = new();
            double thickness = new();
            double optimalCL = new();
            int reflex = new();

            if (NACAcode.Length == 4)   //Parsing NACA info for 4 digit
            {
                maxCamber = double.Parse(Convert.ToString(NACAcode[0])) / 100;
                posCamber = double.Parse(Convert.ToString(NACAcode[1])) / 10;
                thickness = Convert.ToDouble(Convert.ToString(NACAcode[2]) + Convert.ToString(NACAcode[3])) / 100; //Get max thickness from last 2 digits of NACA code + divide by 100 for 0-1 chord range
            }
            else if (NACAcode.Length == 5)  //Parsing NACA info for 5 digit
            {
                optimalCL = double.Parse(Convert.ToString(NACAcode[0])) * 0.15;
                posCamber = double.Parse(Convert.ToString(NACAcode[1])) * 0.05;
                reflex = int.Parse(Convert.ToString(NACAcode[2]));
                thickness = Convert.ToDouble(Convert.ToString(NACAcode[3]) + Convert.ToString(NACAcode[4])) / 100; //Get max thickness from last 2 digits of NACA code + divide by 100 for 0-1 chord range
            }

            //Plotting NACA profiles

            if (NACAcode.Length == 4) { Plot4d(maxCamber, posCamber, thickness); }
            if (NACAcode.Length == 5) { Plot5d(maxCamber, reflex, thickness); }

        }

        //Functions for returning aerofoil sections (4 digit)
        private static double[] Coords_4d(double x, double m, double p, double t) //Coords 4digit NACA - returns list[]
            //x-coordinate, m-maximum camber, p-position of max camber
            //RETURNS list[length=6]
            //list[[0].x-upper, [1].x-lower, [2].y-upper, [3].y-lower, [4].y-chord, [5].y-symmetric] - CAMBERED
            //list[[0].x-upper, [1].x-lower, [2].y-upper, [3].y-lower, [4].0, [5].0] - SYMMETRIC
        {
            double yt =  5 * t * (0.2969 * Math.Sqrt(x) - 0.126 * x - 0.3516 * x * x + 0.2843 * x * x * x - 0.1036 * x * x * x * x); //symmetric thickness/shape y value
    
            double yc = new(); //cambered wing camber line
            double dy = new(); //dyc/dx

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
            double[] ycamb = new double[res];
            double[] ysymm = new double[res];


            for (int i = 0; i < res; i++)
            {
                double[] coords = Coords_4d(xrange[i], m, p, t);
                xupper[i] = coords[0];
                xlower[i] = coords[1];
                yupper[i] = coords[2];
                ylower[i] = coords[3];
                ycamb[i] = coords[4];
                ysymm[i] = coords[5];
            }
           

            WingPlot.Reset();

            WingPlot.Plot.AddScatterLines(xupper, yupper, Color.Black, label: "Aerofoil Profile");
            WingPlot.Plot.AddScatterLines(xlower, ylower, Color.Black);
            if (NACAtextBox.Text[0] != '0' & NACAtextBox.Text[1] != '0') //Plot camber line  and symmetric shape if assymmetric
            {
                WingPlot.Plot.AddScatterLines(xrange, ycamb, Color.Blue, label: "Camber Line");
                WingPlot.Plot.AddScatterLines(xrange, ysymm, Color.Red, label: "Symmetric Shape");
            } 
            WingPlot.Plot.AddScatterLines(xrange, yzero, Color.Black, lineStyle: ScottPlot.LineStyle.Dash, label: "Chord");

            WingPlot.Plot.SetAxisLimitsX(-0.01, 1.01);
            WingPlot.Plot.SetAxisLimitsY(-0.4, 0.4);
            WingPlot.Plot.Title("NACA " + Convert.ToString(NACAtextBox.Text) + " Aerofoil");
            WingPlot.Plot.Legend(Enabled = true);

            WingPlot.Refresh();

        }
        
        private double[] Coords_5d(double x, double reflex, double t) //Coords 5digit NACA - returns list[]
        {
            double[] Normal_pList = { 0.05, 0.1, 0.15, 0.2, 0.25 };
            double[] Normal_rList = { 0.058, 0.126, 0.2025, 0.29, 0.391 };
            double[] Normal_k1List = { 361.4, 51.64, 15.957, 6.643, 3.23 };
            double[] Reflex_pList = { 0.1, 0.15, 0.2, 0.25 };
            double[] Reflex_rList = { 0.13, 0.217, 0.318, 0.441 };
            double[] Reflex_k1List = { 51.99, 15.793, 6.52, 3.191 };
            double[] Reflex_k21List = { 0.000764, 0.00677, 0.0303, 0.1355 };

            string[] Normal_profileList = { "210", "220", "230", "240", "250" };
            string[] Reflex_profileList = { "221", "231", "241", "251" };

            string profile = Convert.ToString(NACAtextBox.Text[0..3]);

            

            double p = new();
            double r = new();
            double k1 = new();
            double k21 = new();

            if (reflex == 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (profile == Normal_profileList[i])
                    {
                        p = Normal_pList[i];
                        r = Normal_rList[i];
                        k1 = Normal_k1List[i];

                        break;
                    }
                }
            }
            else if (reflex == 1)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (profile == Reflex_profileList[i])
                    {
                        p = Reflex_pList[i];
                        r = Reflex_rList[i];
                        k1 = Reflex_k1List[i];
                        k21 = Reflex_k21List[i];

                        break;
                    }
                }
            }

            double chord = 1;

            double yc = new();
            double dyc = new();

            if (reflex == 0)
            {
                if (x < r) 
                {
                    yc = (k1 / 6) * (x * x * x - 3 * r * x * x + r * r * (3 - r) * x);
                    dyc = (k1 / 6) * (3 * x * x - 6 * r * x + r * r * (3 - r));

                }
                else 
                { 
                    yc = ((k1 * r * r * r) / 6) * (1 - x);
                    dyc = ((-k1 * r * r * r) / 6);
                }
            }
            else if (reflex == 1)
            {
                if (x/chord <= r)
                {
                    yc = chord * (k1 / 6) * ((Math.Pow((x / chord) - r, 3) - k21 * Math.Pow(1 - r, 3) * (x / chord) - r * r * r * (x / chord) + r * r * r));
                    dyc = (k1 / 6) * ((3 * Math.Pow((x / chord) - r, 2) - k21 * Math.Pow(1 - r, 3) - r * r * r));
                }
                else
                {
                    yc = chord * (k1 / 6) * (k21 * (Math.Pow((x / chord) - r, 3) - k21 * Math.Pow(1 - r, 3) * (x / chord) - r * r * r * (x / chord) + r * r * r));
                    dyc = (k1 / 6) * ((3 * k21) * (Math.Pow((x / chord) - r, 2) - k21 * Math.Pow(1 - r, 3) - r * r * r));
                }
            }
            double yt = 5 * t * (0.2969 * Math.Sqrt(x) - 0.126 * x - 0.3516 * x * x + 0.2843 * x * x * x - 0.1036 * x * x * x * x); //symmetric thickness/shape y value

            double theta = Math.Atan(dyc); //Calculating theta angle FOR ASYMMETRIC wings

            double[] output = new double[6] { x - yt * Math.Sin(theta), x + yt * Math.Sin(theta), yc + yt * Math.Cos(theta), yc - yt * Math.Cos(theta), yc, yt }; //output list
            return output;
        }

        private void Plot5d(double m, double reflex, double t) 
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
            double[] ycamb = new double[res];
            double[] ysymm = new double[res];


            for (int i = 0; i < res; i++)
            {
                double[] coords = Coords_5d(xrange[i], reflex, t);
                xupper[i] = coords[0];
                xlower[i] = coords[1];
                yupper[i] = coords[2];
                ylower[i] = coords[3];
                ycamb[i] = coords[4];
                ysymm[i] = coords[5];
            }


            WingPlot.Reset();

            WingPlot.Plot.AddScatterLines(xupper, yupper, Color.Black, label: "Aerofoil Profile");
            WingPlot.Plot.AddScatterLines(xlower, ylower, Color.Black);
            if (NACAtextBox.Text[0] != '0' & NACAtextBox.Text[1] != '0') //Plot camber line  and symmetric shape if assymmetric
            {
                WingPlot.Plot.AddScatterLines(xrange, ycamb, Color.Blue, label: "Camber Line");
                WingPlot.Plot.AddScatterLines(xrange, ysymm, Color.Red, label: "Symmetric Shape");
            }
            WingPlot.Plot.AddScatterLines(xrange, yzero, Color.Black, lineStyle: ScottPlot.LineStyle.Dash, label: "Chord");

            WingPlot.Plot.SetAxisLimitsX(-0.01, 1.01);
            WingPlot.Plot.SetAxisLimitsY(-0.4, 0.4);
            WingPlot.Plot.Title("NACA " + Convert.ToString(NACAtextBox.Text) + " Aerofoil");
            WingPlot.Plot.Legend(Enabled = true);

            WingPlot.Refresh();
        }

        public class NACA_Parse //NACA aerofoil code parsing class
        {
            public string NACA { get; set; } //NACA CODE string property

            public NACA_Parse(string NACA_code = "0012") //Constructor - takes in naca code (default 0012)
            {
                NACA = NACA_code;
            }

            public bool Valid() //NACA Code Validation method
                //Encapsulates error message boxes also
            {
                //checks - valid int, length, 4digit symmetrically valid, 5digit camber-line valid

                //Error Message Strings + List of 5-digit available camber profiles
                string NACA_error = "Please enter a valid NACA aerofoil code";
                string CamberLine_error = "Your NACA 5 Digit Camber Profile is unavailable, the following camber profiles are available:\nNormal Camber:\n210\n220\n230\n240\n250\n\nReflex Camber:\n221\n231\n241\n251";
                string[] profileList = { "210", "220", "230", "240", "250", "221", "231", "241", "251" };

                //Checking if input is valid number sequence (valid int check)
                try { Convert.ToInt32(NACA); }
                catch (Exception) { MessageBox.Show(NACA_error); return false; }

                //Checking if length is valid (string length 4 or 5)
                if (NACA.Length != 4 && NACA.Length != 5) { MessageBox.Show(NACA_error); return false; }

                if (NACA.Length == 4) //4digit validity check (cant have first two digits without agreeing zero or nonzero)
                {
                    if (NACA[0] == '0' && NACA[1] != '0') { MessageBox.Show(NACA_error); return false; }
                    if (NACA[0] != '0' && NACA[1] == '0') { MessageBox.Show(NACA_error); return false; }
                }
                
                if (NACA.Length == 5) //validation for 5 digit
                {
                    bool Valid5Digit = false;
                    //Checking if 5digit camber line profile matches in available list with validity check variable Valid5digit
                    foreach (string profile in profileList) { if (NACA[0..3] == profile) { Valid5Digit = true; break; } }

                    if (Valid5Digit == false) { MessageBox.Show(CamberLine_error); return false; } //5digit camber error message

                }

                return true;
            }
            /*
            public double[] Parse() //NACA Code Parser method - ASSUMES VALID!!!
            {
                int len = new();

                if (NACA.Length == 4) //4digit parsing routine
                {
                    len = 4;
                }
                
            }
            */
        }

    }
}