//Main Form - Form1 Code Solution - ME529 Coursework 1
//Written by: Jacob Currie - 201718558 - February/March 2022
namespace Coursework_1
{
    public partial class Form1 : Form
    {
        public Form1() //initialising main form Form1
        {
            InitializeComponent();

            int centreX = panel1.Width / 2;
            int centreY = panel1.Height / 2;

        }

        private void Form1_Load(object sender, EventArgs e) // on form1 loading (anything that should be run immediately on launch)
        {

        }

        private void InfoButton_Click(object sender, EventArgs e)//Info Button clicked
        {
            MessageBox.Show("Helpful Info!\n\nChoose a Solver using the Analytical and Numerical Boxes!\n\nInput your 4 or 5 digit NACA Aerofoil number in the NACA code box!\n\nInput your angle of attack (in degrees) in the AoA box!");
        }

        public void SolveButton_Click(object sender, EventArgs e)
        {
            NACA_Parse NACA1 = new NACA_Parse(NACAtextBox.Text);

            //NACA code input validation
            if (NACA1.Valid() == false) { NACAtextBox.Text = ""; return; }

            //Input and settings validation
            //Solver selected validation - must have one box selected
            if (AnalyticalSolveButton.Checked == false && NumericalSolveButton.Checked == false) { MessageBox.Show("Please Choose a Solver method!"); return; }

            //Angle of attack validation
            //validation-must be a float
            try { float AttackAngle = Convert.ToSingle(AttackAngletextBox.Text); }
            catch (Exception) { MessageBox.Show("Please enter a valid Angle of Attack"); return; }

            //Angle of attack between -90 and 90 degreees - input validation
            if (Convert.ToSingle(AttackAngletextBox.Text) <  -90 || Convert.ToSingle(AttackAngletextBox.Text)  > 90)
            {
                MessageBox.Show("Please enter an Angle of Attack between -90 and 90 degrees"); return;
            }

            //Simpsons Rule number of points Input Validation

            if (NumericalSolveButton.Checked == true)
            {
                try { int UnusedNPoints = Convert.ToInt32(textBox8.Text); }
                catch (Exception) { MessageBox.Show("Please enter a valid number of points for Integration"); return; }

                if (Int32.Parse(textBox8.Text) > 5000 || Int32.Parse(textBox8.Text) < 2 || Int32.Parse(textBox8.Text) % 2 != 0)
                {
                    MessageBox.Show("Please enter an EVEN integration number of points between 2 and 5000"); return;
                }
            }
            

            

            //Parsing NACA wing code
            // [ For NACA code 1234 -> 1 = max camber as % of chord, 2 = position of max camber as chord/10, 34 = max thickness as % of chord ]
            // [ For NACA code 12345 -> 1 = optimal CL at ideal AoA, 2 = position of max camber, 3 = camber reflex yes/no, 45 = max thickness ]


            string NACAcode = NACAtextBox.Text; //Reading NACA code from textbox to string variable
            double maxCamber = new();
            double posCamber = new();
            double thickness = new();
            double optimalCL = new();
            int reflex = new();

            bool symmFoil = false;

            if (NACAcode.Length == 4)   //Parsing NACA info for 4 digit
            {
                maxCamber = double.Parse(Convert.ToString(NACAcode[0])) / 100;
                posCamber = double.Parse(Convert.ToString(NACAcode[1])) / 10;
                thickness = Convert.ToDouble(Convert.ToString(NACAcode[2]) + Convert.ToString(NACAcode[3])) / 100; //Get max thickness from last 2 digits of NACA code + divide by 100 for 0-1 chord range


                if (maxCamber + posCamber == 0) { symmFoil = true; }
            }
            else if (NACAcode.Length == 5)  //Parsing NACA info for 5 digit
            {
                optimalCL = double.Parse(Convert.ToString(NACAcode[0])) * 0.15;
                posCamber = double.Parse(Convert.ToString(NACAcode[1])) * 0.05;
                reflex = int.Parse(Convert.ToString(NACAcode[2]));
                thickness = Convert.ToDouble(Convert.ToString(NACAcode[3]) + Convert.ToString(NACAcode[4])) / 100; //Get max thickness from last 2 digits of NACA code + divide by 100 for 0-1 chord range
            }

            //Plotting NACA profiles
            if (NACAcode.Length == 4) { Plot(maxCamber, posCamber, thickness, true, symmFoil); }
            if (NACAcode.Length == 5) { Plot(maxCamber, reflex, thickness, false, symmFoil); }


            



        }

        //Functions for returning aerofoil sections (4 digit)
        private static double[] Coords_4d(double x, double m, double p, double t) //Coords 4digit NACA - returns list[]
                                                                                  //x-coordinate, m-maximum camber, p-position of max camber
                                                                                  //RETURNS list[length=6]
                                                                                  //list[[0].x-upper, [1].x-lower, [2].y-upper, [3].y-lower, [4].y-chord, [5].y-symmetric] - CAMBERED
                                                                                  //list[[0].x-upper, [1].x-lower, [2].y-upper, [3].y-lower, [4].0, [5].0] - SYMMETRIC
        {
            double yt = 5 * t * (0.2969 * Math.Sqrt(x) - 0.126 * x - 0.3516 * x * x + 0.2843 * x * x * x - 0.1036 * x * x * x * x); //symmetric thickness/shape y value

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
                if (x / chord < r)
                {
                    yc = chord * (k1 / 6) * ((Math.Pow((x / chord) - r, 3) - k21 * Math.Pow(1 - r, 3) * (x / chord) - r * r * r * (x / chord) + r * r * r));
                    dyc = (k1 / 6) * ((3 * Math.Pow((x / chord) - r, 2) - k21 * Math.Pow(1 - r, 3) - r * r * r));
                }
                else
                {
                    yc = (k1 / 6) * (k21 * Math.Pow(x - r, 3) - k21 * x * Math.Pow(1 - r, 3) - x * Math.Pow(r, 3) + Math.Pow(r, 3));

                    dyc = (k1 / 6) * ((3 * k21) * (Math.Pow((x / chord) - r, 2) - k21 * Math.Pow(1 - r, 3) - Math.Pow(r, 3)));
                }
            }
            double yt = 5 * t * (0.2969 * Math.Sqrt(x) - 0.126 * x - 0.3516 * x * x + 0.2843 * x * x * x - 0.1036 * x * x * x * x); //symmetric thickness/shape y value

            double theta = Math.Atan(dyc); //Calculating theta angle FOR ASYMMETRIC wings

            double[] output = new double[6] { x - yt * Math.Sin(theta), x + yt * Math.Sin(theta), yc + yt * Math.Cos(theta), yc - yt * Math.Cos(theta), yc, yt }; //output list
            return output;
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

                //Checking for negative valid integer
                if (Convert.ToInt32(NACA) < 0) { MessageBox.Show(NACA_error); return false; }

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

        }
        //Built in panel paint method override
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        //Aerofoil section drawing subroutine
        public void Plot(double m, double p, double t, bool Is4Digit, bool symmFoil)
        {
            //centre of panel coordinates
            int centreX = panel1.Width / 2;
            int centreY = panel1.Height / 2;

            panel1.Refresh(); //refresh drawing panel (Clean the whiteboard)

            int res = 250; //number of points to use for plotting - (resolution control)
            //creating x linspace array
            double[] xrange = new double[res];
            double[] yzero = new double[res];

            for (double i = 0; i < res; i++)
            {
                xrange[Convert.ToInt32(i)] = Convert.ToDouble(i / res);
                yzero[Convert.ToInt32(i)] = 0;
            }

            //empty coordinate arrays
            double[] xupper = new double[res];
            double[] yupper = new double[res];
            double[] xlower = new double[res];
            double[] ylower = new double[res];
            double[] ycamb = new double[res];
            double[] ysymm = new double[res];

            if (Is4Digit) //4digit coordinate routine
            {
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
            }

            if (!Is4Digit) //5digit coordinate routine
            {
                for (int i = 0; i < res; i++)
                {
                    double[] coords = Coords_5d(xrange[i], p, t);
                    xupper[i] = coords[0];
                    xlower[i] = coords[1];
                    yupper[i] = coords[2];
                    ylower[i] = coords[3];
                    ycamb[i] = coords[4];
                    ysymm[i] = coords[5];
                }
            }

            //Drawing Pens
            Pen pen_camber = new Pen(Color.Blue);
            Pen pen_symmetrical = new Pen(Color.Red);
            Pen pen_Normal = new Pen(Color.Black);

            Graphics gp = panel1.CreateGraphics();

            //Drawing
            gp.TranslateTransform(centreX, centreY); //setting origin to centre of panel

            Font LegendFont = new Font("Arial", 10.0f); //Setting font object for legend

            Brush LegendBrush1 = new SolidBrush(Color.Black); //setting black brush
            Brush LegendBrush2 = new SolidBrush(Color.Blue);//setting black brush
            Brush LegendBrush3 = new SolidBrush(Color.Red);//setting black brush

            if (Is4Digit && symmFoil) //4digit symmetrical
            {
                //Drawing legend
                gp.DrawString("- Aerofoil Shape", LegendFont, LegendBrush1, Convert.ToInt32(0 - 0.96 * centreX), Convert.ToInt32(0 - 0.98 * centreY));
                gp.DrawString("- Chord", LegendFont, LegendBrush3, Convert.ToInt32(0 - 0.96 * centreX), Convert.ToInt32(0 - 0.94 * centreY));

            }

            if (Is4Digit && !symmFoil) //4digit asymmetrical
            {
                //Drawing legend
                gp.DrawString("- Aerofoil Shape", LegendFont, LegendBrush1, Convert.ToInt32(0 - 0.96 * centreX), Convert.ToInt32(0 - 0.98 * centreY));
                gp.DrawString("- Camber Line", LegendFont, LegendBrush2, Convert.ToInt32(0 - 0.96 * centreX), Convert.ToInt32(0 - 0.94 * centreY));
                gp.DrawString("- Symmetrical Shape", LegendFont, LegendBrush3, Convert.ToInt32(0 - 0.96 * centreX), Convert.ToInt32(0 - 0.895 * centreY));
            }

            if (!Is4Digit && symmFoil) //5digit symmetrical
            {
                //Drawing legend
                gp.DrawString("- Aerofoil Shape", LegendFont, LegendBrush1, Convert.ToInt32(0 - 0.96 * centreX), Convert.ToInt32(0 - 0.98 * centreY));
                gp.DrawString("- Chord", LegendFont, LegendBrush3, Convert.ToInt32(0 - 0.96 * centreX), Convert.ToInt32(0 - 0.94 * centreY));
            }

            if (!Is4Digit && !symmFoil) //5digit asymmetrical
            {
                //Drawing legend
                gp.DrawString("- Aerofoil Shape", LegendFont, LegendBrush1, Convert.ToInt32(0 - 0.96 * centreX), Convert.ToInt32(0 - 0.98 * centreY));
                gp.DrawString("- Camber Line", LegendFont, LegendBrush2, Convert.ToInt32(0 - 0.96 * centreX), Convert.ToInt32(0 - 0.94 * centreY));
                gp.DrawString("- Symmetrical Shape", LegendFont, LegendBrush3, Convert.ToInt32(0 - 0.96 * centreX), Convert.ToInt32(0 - 0.895 * centreY));
            }

            //rotate render option
            if (AoARenderCheckBox.Checked) { gp.RotateTransform(Convert.ToSingle(AttackAngletextBox.Text)); } //rotate transform for angle of attack

            //actually drawing the wings with drawWing subrouting
            DrawWing(xrange, ycamb, pen_camber, gp);
            DrawWing(xrange, ysymm, pen_symmetrical, gp);
            DrawWing(xlower, ylower, pen_Normal, gp);
            DrawWing(xupper, yupper, pen_Normal, gp);

            //Displaying Details of Aerofoil below plot
            DisplayDetails(Is4Digit, symmFoil);

            //Analytical / Numerical Solver option
            bool IsNumericalSolver = true;

            if (AnalyticalSolveButton.Checked) { IsNumericalSolver = false; }

            //Calling Thin Aerofoil Theory Calculation Routine
            ThinAerofoilSolve(IsNumericalSolver, Is4Digit, xrange, ycamb);
        }

        //DrawWing function - draws a set of points given the x and y lists, pen, and graphics object
        private void DrawWing(double[] xList, double[] yList, Pen gPen, Graphics gp)
        {
            int scale = 500; //points are assumed [0 1] bounded - scale to increase size - 550i good car

            for (int i = 1; i < xList.Length; i++)
            {
                Point initial = new Point(Convert.ToInt32((xList[i - 1] - 0.5) * scale), Convert.ToInt32(-yList[i - 1] * scale));
                Point next = new Point(Convert.ToInt32((xList[i] - 0.5) * scale), Convert.ToInt32(-yList[i] * scale));

                gp.DrawLine(gPen, initial, next);
            }


        }

        //Below-Plot Aerofoil Detail Display routine
        public void DisplayDetails(bool Is4Digit, bool symmFoil)
        {
            //making all labels visible
            label8.Visible = true; label9.Visible = true; label10.Visible = true; label11.Visible = true; label12.Visible = true; label13.Visible = true;label14.Visible = true;
            label15.Visible = true; label16.Visible = true; label17.Visible = true; label18.Visible = true; label19.Visible = true; label20.Visible = true; label21.Visible = true;
            
            //naca profiile textbox
            label11.Text = NACAtextBox.Text;


            if (Is4Digit && symmFoil) //4digit symmetrical
            {
                label12.Text = "Yes";//symmetrical
                label13.Text = NACAtextBox.Text[2..4] + "%";//thickness
                label14.Text = "None";//camber
                label16.Text = "None";//camber position

                label19.Visible = false;
                label18.Visible = false;
                label20.Visible = false;
                label21.Visible = false;
            }

            if (Is4Digit && !symmFoil) //4digit asymmetrical
            {
                label12.Text = "No";
                label13.Text = NACAtextBox.Text[2..4] + "%";
                label14.Text = NACAtextBox.Text[0] + "%";
                label16.Text = NACAtextBox.Text[1] + "0%";

                label19.Visible = false;
                label18.Visible = false;
                label20.Visible = false;
                label21.Visible = false;
            }

            if (!Is4Digit && symmFoil) //5digit symmetrical
            {
                label12.Text = "Yes";
                label13.Text = NACAtextBox.Text[3..5] + "%";
                label14.Text = "None";
                label16.Text = "None";

                label19.Visible = true;
                label18.Visible = true;
                label20.Visible = true;
                label21.Visible = true;

                if (Convert.ToString(NACAtextBox.Text[2]) == "0") { label18.Text = "No"; }//reflex
                if (Convert.ToString(NACAtextBox.Text[2]) == "1") { label18.Text = "Yes"; }

                

                label20.Text = Convert.ToString(Convert.ToDouble(Convert.ToString(NACAtextBox.Text[0])) * (3 / 20)); //design lift coefficient

            }

            if (!Is4Digit && !symmFoil) //5digit asymmetrical
            {
                label12.Text = "No";
                label13.Text = NACAtextBox.Text[3..5] + "%";
                label14.Text = "None";

                label16.Text = Convert.ToString(Convert.ToSingle(Convert.ToString(NACAtextBox.Text[1]))/0.2) + "%";

                label19.Visible = true;
                label18.Visible = true;
                label20.Visible = true;
                label21.Visible = true;

                if (Convert.ToString(NACAtextBox.Text[2]) == "0") { label18.Text = "No"; }
                if (Convert.ToString(NACAtextBox.Text[2]) == "1") { label18.Text = "Yes"; }

                double optimalCL = double.Parse(Convert.ToString(NACAtextBox.Text[0])) * 0.15;
                label20.Text = Convert.ToString(optimalCL);
            }

        }

        //Calculation Routine
        public void ThinAerofoilSolve(bool IsNumerical, bool Is4Digit, double[] xrange, double[] ycamb)
        {
            double angleDeg = Convert.ToDouble(Convert.ToString(AttackAngletextBox.Text));
            //Convert to radians
            double angle = angleDeg * Math.PI / 180;

            AnalyticalSolver Asolver = new AnalyticalSolver(angle);

            //Numerical Solver Routine
            if (IsNumerical)
            {

                int nP = Convert.ToInt32(textBox8.Text);

                if (Is4Digit) //4digit numerical solve routine
                {
                    NumericalSolver Nsolver = new NumericalSolver(angle, nP);

                    Nsolver.m = Convert.ToDouble(Convert.ToString(NACAtextBox.Text[0])) / 100;
                    Nsolver.p = Convert.ToDouble(Convert.ToString(NACAtextBox.Text[1])) / 10;

                    Nsolver.naca4dLimit = Math.Acos(1 - 2 * Convert.ToDouble(Convert.ToString(NACAtextBox.Text[1])) / 10);

                    double[] A_array = Nsolver.naca4d();

                    double A0 = A_array[0]; double A1 = A_array[1]; double A2 = A_array[2];

                    double dx = angle - A0;

                    double CL = Math.PI * (2 * A0 + A1);

                    textBox2.Text = Convert.ToString(CL); //CL
                    textBox1.Text = Convert.ToString(Convert.ToDecimal(-Math.PI * 0.5 * (A0 + A1 - A2 / 2))); //CM LE
                    textBox7.Text = Convert.ToString(Convert.ToDecimal(-Math.PI * 0.25 * (A1 - A2))); //CM AC
                    textBox3.Text = Convert.ToString(Nsolver.naca4dLimit); //integration limit

                    textBox4.Text = Convert.ToString(Convert.ToDecimal(A0)); //A0
                    textBox5.Text = Convert.ToString(Convert.ToDecimal(A1)); //A1
                    textBox6.Text = Convert.ToString(Convert.ToDecimal(A2)); //A2

                    double zeroAngle = dx - A1 / 2;

                    ZeroAngletextBox.Text = Convert.ToString(Convert.ToDecimal((180 / Math.PI) * zeroAngle));

                }

                if (!Is4Digit) //5digit numerical solve routine
                {
                    string profile = NACAtextBox.Text[0..3];
                    NumericalSolver Nsolver = new NumericalSolver(angle, nP, profile);

                    double[] A_array = Nsolver.naca5d();

                    double A0 = A_array[0]; double A1 = A_array[1]; double A2 = A_array[2];

                    double dx = angle - A0;

                    double CL = Math.PI * (2 * A0 + A1);

                    textBox2.Text = Convert.ToString(CL); //CL
                    textBox1.Text = Convert.ToString(Convert.ToDecimal(-Math.PI * 0.5 * (A0 + A1 - A2 / 2))); //CM LE
                    textBox7.Text = Convert.ToString(Convert.ToDecimal(-Math.PI * 0.25 * (A1 - A2))); //CM AC
                    textBox3.Text = Convert.ToString(Nsolver.naca5dLimit); //integration limit

                    textBox4.Text = Convert.ToString(Convert.ToDecimal(A0)); //A0
                    textBox5.Text = Convert.ToString(Convert.ToDecimal(A1)); //A1
                    textBox6.Text = Convert.ToString(Convert.ToDecimal(A2)); //A2

                    double zeroAngle = dx - A1 / 2;

                    ZeroAngletextBox.Text = Convert.ToString(Convert.ToDecimal((180 / Math.PI) * zeroAngle));

                }
            }

            //Analytical Solver Routine
            if (!IsNumerical)
            {
                if (Is4Digit) //4digit analytical solve
                {
                    double lim = Math.Acos(1 - 2 * Convert.ToDouble(Convert.ToString(NACAtextBox.Text[1])) / 10);

                    double m = Convert.ToDouble(Convert.ToString(NACAtextBox.Text[0])) / 100;
                    double p = Convert.ToDouble(Convert.ToString(NACAtextBox.Text[1])) / 10;

                    double A0 = Asolver.naca4digitA0(m, p, lim);
                    double A1 = Asolver.naca4digitA1(m, p, lim);
                    double A2 = Asolver.naca4digitA2(m, p, lim);

                    double dx = angle - A0;

                    double CL = Math.PI * (2 * A0 + A1);

                    textBox2.Text = Convert.ToString(CL); //CL
                    textBox1.Text = Convert.ToString(Convert.ToDecimal(-Math.PI * 0.5 * (A0 + A1 - A2 / 2))); //CM LE
                    textBox7.Text = Convert.ToString(Convert.ToDecimal(-Math.PI * 0.25 * (A1 - A2))); //CM AC
                    textBox3.Text = Convert.ToString(lim); //integration limit

                    textBox4.Text = Convert.ToString(Convert.ToDecimal(A0)); //A0
                    textBox5.Text = Convert.ToString(Convert.ToDecimal(A1)); //A1
                    textBox6.Text = Convert.ToString(Convert.ToDecimal(A2)); //A2

                    double zeroAngle = dx - A1 / 2;

                    ZeroAngletextBox.Text = Convert.ToString(Convert.ToDecimal((180 / Math.PI) * zeroAngle));
                }

                if (!Is4Digit) //5digit analytical solve
                {
                    string profile = NACAtextBox.Text[0..3];

                    double[] Avalues = Asolver.naca5digit(profile);

                    double A0 = Avalues[0]; double A1 = Avalues[1]; double A2 = Avalues[2];

                    double CL = Math.PI * (2 * A0 + A1);

                    textBox2.Text = Convert.ToString(CL); //CL
                    textBox1.Text = Convert.ToString(Convert.ToDecimal(-Math.PI * 0.5 * (A0 + A1 - A2 / 2))); //CM LE
                    textBox7.Text = Convert.ToString(Convert.ToDecimal(-Math.PI * 0.25 * (A1 - A2))); //CM AC
                    textBox3.Text = Convert.ToString(Asolver.naca5dLimit); //integration limit

                    textBox4.Text = Convert.ToString(Convert.ToDecimal(A0)); //A0
                    textBox5.Text = Convert.ToString(Convert.ToDecimal(A1)); //A1
                    textBox6.Text = Convert.ToString(Convert.ToDecimal(A2)); //A2

                    double zeroAngle = (angle - A0) - A1 / 2;

                    ZeroAngletextBox.Text = Convert.ToString(Convert.ToDecimal((180 / Math.PI) * zeroAngle));
                }
            }


        }

        //AnalyticalSolver Class - contains all functions for solving analytically NACA coefficients
        public class AnalyticalSolver
        {
            //AoA property
            public double AoA { get; set; }

            public double naca5dLimit { get; set; } 

            //Constructor
            public AnalyticalSolver(double angleOfAttack = 0)
            {
                AoA = angleOfAttack;
            }

            //Convert to theta from x method
            public double XtoAngle(double x)
            {
                return Math.Acos(1 - 2 * x);
            }           

            public double naca4digitA0(double m, double p, double limit)
            {
                static double naca4digitA0Fore(double m, double p, double x)
                {
                    return (((m * x) * (2 * p - 1)) / (p * p)) + ((m * Math.Sin(x)) / (p * p));
                }

                static double naca4digitA0Aft(double m, double p, double x)
                {
                    return ((m * x) * (2 * p - 1)) / (Math.Pow(1 - p, 2)) + ((m * Math.Sin(x)) / (Math.Pow(1 - p, 2)));
                }

                return AoA - (1/Math.PI) * ((naca4digitA0Fore(m, p, limit) - naca4digitA0Fore(m, p, 0)) + (naca4digitA0Aft(m, p, Math.PI) - naca4digitA0Aft(m, p, limit)));
            }

            public double naca4digitA1(double m, double p, double limit)
            {
                static double naca4digitA1Fore(double m, double p, double x)
                {
                    return ((m * ((2 * p) - 1) * Math.Sin(x)) / (p * p)) + (m / (p * p)) * ((x / 2) + 0.25 * Math.Sin(2 * x));
                }

                static double naca4digitA1Aft(double m, double p, double x)
                {
                    return ((m * ((2 * p) - 1) * Math.Sin(x)) / ((1 - p) * (1 - p))) + (m / ((1 - p) * (1 - p)) * ((x / 2) + 0.25 * Math.Sin(2 * x)));
                }

                return ((2 / Math.PI) * (naca4digitA1Fore(m, p, limit) - naca4digitA1Fore(m, p, 0)) + (2 / Math.PI) * (naca4digitA1Aft(m, p, Math.PI) - naca4digitA1Aft(m, p, limit)));
            }

            public double naca4digitA2(double m, double p, double limit)
            {
                static double naca4digitA2Fore(double m, double p, double x)
                {
                    return ((m * ((2 * p) - 1)) / Math.Pow(p, 2)) * (0.5 * Math.Sin(2 * x)) + (m / Math.Pow(p, 2)) * (Math.Sin(3 * x) / 6 + Math.Sin(x) / 2);
                }

                static double naca4digitA2Aft(double m, double p, double x)
                {
                    return ((m * ((2 * p) - 1)) / Math.Pow(1 - p, 2)) * (0.5 * Math.Sin(2 * x)) + (m / Math.Pow(1 - p, 2)) * (Math.Sin(3 * x) / 6 + Math.Sin(x) / 2);
                }

                return (2 / Math.PI) * (naca4digitA2Fore(m, p, limit) - naca4digitA2Fore(m, p, 0)) + (2 / Math.PI) * (naca4digitA2Aft(m, p, Math.PI) - naca4digitA2Aft(m, p, limit));
            }

            public double[] naca5digit(string profile)
            {
                double[] Normal_rList = { 0.058, 0.126, 0.2025, 0.29, 0.391 };
                double[] Normal_k1List = { 361.4, 51.64, 15.957, 6.643, 3.23 };
                double[] Reflex_pList = { 0.1, 0.15, 0.2, 0.25 };
                double[] Reflex_rList = { 0.13, 0.217, 0.318, 0.441 };
                double[] Reflex_k1List = { 51.99, 15.793, 6.52, 3.191 };
                double[] Reflex_k21List = { 0.000764, 0.00677, 0.0303, 0.1355 };

                string[] Normal_profileList = { "210", "220", "230", "240", "250" };
                string[] Reflex_profileList = { "221", "231", "241", "251" };

                double r = new();
                double k = new();
                double k21 = new();

                int reflex = 0;
                if (Convert.ToString(profile[2]) == "1") { reflex = 1; }

                if (reflex == 0)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (profile == Normal_profileList[i])
                        {
                            r = Normal_rList[i];
                            k = Normal_k1List[i];

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
                            r = Reflex_rList[i];
                            k = Reflex_k1List[i];
                            k21 = Reflex_k21List[i];

                            break;
                        }
                    }
                }

                double limit = Math.Acos(1 - 2 * r);
                naca5dLimit = limit;


                double An = k / 8 - (k * r) / 2 + (k * r * r * (3 - r)) / 6;
                double Bn = (k * r) / 2 - k / 4;
                double Cn = k / 8;
                double Dn = (-k * r * r * r) / 6;

                double k2 = k21 * k;

                double Ar = k / 8 - k * r / 2 + k * r * r / 2 - (k2 / 6 * Math.Pow(1 - r, 3)) - k * r * r * r / 6;
                double Br = k * r / 2 - k / 4;
                double Cr = k / 8;

                double Dr = k21 * k * r * r * r / 6 - k * r * r * r / 6 - k21 * k / 24;
                double Er = k21 * k * r / 2 - k21 * k / 4;
                double Fr = k21 * k / 8;

                double naca5digitA0(double x)
                {
                    double naca5digitA0ForeNormal(double x)
                    {
                        return An * x + Bn * Math.Sin(x) + Cn * (x / 2 + 0.25 * Math.Sin(2 * x));
                    }

                    double naca5digitA0AftNormal(double x)
                    {
                        return Dn * x;
                    }

                    double naca5digitA0ForeReflex(double x)
                    {
                        return Ar * x + Br * Math.Sin(x) + Cr * (x / 2 + 0.25 * Math.Sin(2 * x));
                    }

                    double naca5digitA0AftReflex(double x)
                    {
                        return Dr * x + Er * Math.Sin(x) + Fr * (x / 2 + 0.25 * Math.Sin(2 * x));
                    }

                    if (reflex == 0)
                    {
                        return AoA - (1/Math.PI) * ((naca5digitA0ForeNormal(x) - naca5digitA0ForeNormal(0)) + (naca5digitA0AftNormal(Math.PI) - naca5digitA0AftNormal(x)));
                    }
                    else
                    {
                        return AoA - (1 / Math.PI) * ((naca5digitA0ForeReflex(x) - naca5digitA0ForeReflex(0)) + (naca5digitA0AftReflex(Math.PI) - naca5digitA0AftReflex(x)));
                    }
                }

                double naca5digitA1(double x)
                {
                    double naca5digitA1ForeNormal(double x)
                    {
                        return An * Math.Sin(x) + Bn * (x / 2 + Math.Sin(2 * x) / 4) + Cn * (Math.Sin(x) - Math.Pow(Math.Sin(x), 3) / 3);
                    }

                    double naca5digitA1AftNormal(double x)
                    {
                        return Dn * Math.Sin(x);
                    }

                    double naca5digitA1ForeReflex(double x)
                    {
                        return Ar * Math.Sin(x) + Br * (x / 2 + Math.Sin(2 * x) / 4) + Cr * (Math.Sin(x) - Math.Pow(Math.Sin(x), 3) / 3);
                    }

                    double naca5digitA1AftReflex(double x)
                    {
                        return Dr * Math.Sin(x) + Er * (x / 2 + Math.Sin(2 * x) / 4) + Fr * (Math.Sin(x) - Math.Pow(Math.Sin(x), 3) / 3);
                    }

                    if (reflex == 0)
                    {
                        return (2 / Math.PI) * ((naca5digitA1ForeNormal(x) - naca5digitA1ForeNormal(0)) + (naca5digitA1AftNormal(Math.PI) - naca5digitA1AftNormal(x)));
                    }
                    else
                    {
                        return (2 / Math.PI) * ((naca5digitA1ForeReflex(x) - naca5digitA1ForeReflex(0)) + (naca5digitA1AftReflex(Math.PI) - naca5digitA1AftReflex(x)));
                    }
                }


                double naca5digitA2(double x)
                {
                    double naca5digitA2ForeNormal(double x)
                    {
                        return An * Math.Sin(2 * x) / 2 + Bn * (Math.Sin(3 * x) / 6 + Math.Sin(x) / 2) + Cn * (x / 4 + Math.Sin(2 * x) / 4 + Math.Sin(4 * x) / 16);
                    }

                    double naca5digitA2AftNormal(double x)
                    {
                        return Dn * Math.Sin(2 * x) / 2;
                    }

                    double naca5digitA2ForeReflex(double x)
                    {
                        return Ar * Math.Sin(2 * x) / 2 + Br * (Math.Sin(3 * x) / 6 + Math.Sin(x) / 2) + Cr * (x / 4 + Math.Sin(2 * x) / 4 + Math.Sin(4 * x) / 16);
                    }

                    double naca5digitA2AftReflex(double x)
                    {
                        return Dr * Math.Sin(2 * x) / 2 + Er * (Math.Sin(3 * x) / 6 + Math.Sin(x) / 2) + Fr * (x / 4 + Math.Sin(2 * x) / 4 + Math.Sin(4 * x) / 16);
                    }

                    if (reflex == 0)
                    {
                        return (2 / Math.PI) * ((naca5digitA2ForeNormal(x) - naca5digitA2ForeNormal(0)) + (naca5digitA2AftNormal(Math.PI) - naca5digitA2AftNormal(x)));
                    }
                    else
                    {
                        return (2 / Math.PI) * ((naca5digitA2ForeReflex(x) - naca5digitA2ForeReflex(0)) + (naca5digitA2AftReflex(Math.PI) - naca5digitA2AftReflex(x)));
                    }
                }

                //ending return A cofficient list
                double[] Aarray = {naca5digitA0(limit), naca5digitA1(limit), naca5digitA2(limit)};

                return Aarray;
            }
        }


        //NumericalSolver Class - contains all functions for solving numerically with simpsons rule
        public class NumericalSolver
        {
            //AoA property
            public double AoA { get; set; }

            //naca 5digit integration limit
            public double naca5dLimit { get; set; }

            //naca 4digit integration limit
            public double naca4dLimit { get; set; }

            //n - number of points/sub intervals for simpsons rule
            public int n { get; set; }

            private double r { get; set; }

            private double k1 { get; set; }

            private double k2 { get; set; }

            public double m { get; set; }

            public double p { get; set; }

            private int reflex { get; set; }

            //Constructor
            public NumericalSolver(double angleOfAttack = 0, int nPoints = 10, string profile = "230")
            {
                AoA = angleOfAttack;
                n = nPoints;

                double[] arr = Naca5dTable(profile);

                r = arr[0];
                k1 = arr[1];
                k2 = arr[2];
                reflex = Convert.ToInt32(arr[3]);

            }
            
            public double[] Naca5dTable(string profile)
            {
                double[] Normal_rList = { 0.058, 0.126, 0.2025, 0.29, 0.391 };
                double[] Normal_k1List = { 361.4, 51.64, 15.957, 6.643, 3.23 };
                double[] Reflex_pList = { 0.1, 0.15, 0.2, 0.25 };
                double[] Reflex_rList = { 0.13, 0.217, 0.318, 0.441 };
                double[] Reflex_k1List = { 51.99, 15.793, 6.52, 3.191 };
                double[] Reflex_k21List = { 0.000764, 0.00677, 0.0303, 0.1355 };

                string[] Normal_profileList = { "210", "220", "230", "240", "250" };
                string[] Reflex_profileList = { "221", "231", "241", "251" };

                double r = new();
                double k = new();
                double k21 = new();

                int reflex = 0;
                if (Convert.ToString(profile[2]) == "1") { reflex = 1; }

                if (reflex == 0)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (profile == Normal_profileList[i])
                        {
                            r = Normal_rList[i];
                            k = Normal_k1List[i];

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
                            r = Reflex_rList[i];
                            k = Reflex_k1List[i];
                            k21 = Reflex_k21List[i];

                            break;
                        }
                    }
                }

                double limit = Math.Acos(1 - 2 * r);
                naca5dLimit = limit;
                

                double[] arrayOut = { r, k, k21*k, reflex, limit }; //FORMAT - r, k1, k2, reflex, limit

                return arrayOut;
            }

            //NACA aerofoil camber line equations
            public double Fore4d(double x) { return (2 * m) / (p * p) * (p - (0.5 * (1 - Math.Cos(x))));  }
            
            public double Aft4d(double x) { return (2 * m) / Math.Pow(1 - p, 2) * (p - (0.5 * (1 - Math.Cos(x)))); }

            public double Fore5dNormal(double x) { return (k1 / 6) * (3 * (0.25*(1-2*Math.Cos(x)+Math.Pow(Math.Cos(x),2))) - 6 * r * (0.5 * (1 - Math.Cos(x))) + r * r * (3 - r)); }

            public double Aft5dNormal(double x) { return (-k1 / 6) * (r * r * r); }

            public double Fore5dReflex(double x) { return (k1 / 6) * (3 * Math.Pow((0.5 * (1 - Math.Cos(x))) - r, 2) - (k2 / k1) * Math.Pow(1 - r, 3) - Math.Pow(r, 3)); }

            public double Aft5dReflex(double x) { return (k1 / 6) * (3 * (k2 / k1) * Math.Pow((0.5 * (1 - Math.Cos(x))) - r, 2) - (k2 / k1) * Math.Pow(1 - r, 3) - Math.Pow(r, 3)); }

            //Simpsons composite rule routine
            public double Simpson(int nPoints, int A, Func<double, double> fNACA)
            {
                //Creating xrange array of points between 0 and 1
                double[] xrange = new double[nPoints];
                for (int i = 0; i < nPoints; i++) { xrange[i] = Convert.ToDouble(i / nPoints); }

                double h = 1 / Convert.ToDouble(nPoints); //step size

                double sum = 0;

                if (A == 1)
                {
                    //first sum operator
                    for (int i = 1; i < nPoints / 2; i++) { sum += fNACA(xrange[2 * i - 1]) * Math.Cos(xrange[2 * i - 1]); }
                    sum *= 4;

                    //second sum operator
                    for (int i = 1; i < (nPoints / 2) - 1; i++) { sum += fNACA(xrange[2 * i]) * Math.Cos(xrange[2 * i]); }
                    sum *= 2;

                    //add f(x0) and f(xN) and include h/3 term
                    sum += (fNACA(0) * Math.Cos(0) + fNACA(1) * Math.Cos(1));
                    sum *= h / 3;
                }
                if (A == 2)
                {
                    //first sum operator
                    for (int i = 1; i < nPoints / 2; i++) { sum += fNACA(xrange[2 * i - 1]) * Math.Cos(2*xrange[2 * i - 1]); }
                    sum *= 4;

                    //second sum operator
                    for (int i = 1; i < (nPoints / 2) - 1; i++) { sum += fNACA(xrange[2 * i]) * Math.Cos(2*xrange[2 * i]); }
                    sum *= 2;

                    //add f(x0) and f(xN) and include h/3 term
                    sum += (fNACA(0) * Math.Cos(2*0) + fNACA(1) * Math.Cos(2*1));
                    sum *= h / 3;
                }
                else
                {
                    //first sum operator
                    for (int i = 1; i < nPoints / 2; i++) { sum += fNACA(xrange[2 * i - 1]); }
                    sum *= 4;

                    //second sum operator
                    for (int i = 1; i < (nPoints / 2) - 1; i++) { sum += fNACA(xrange[2 * i]); }
                    sum *= 2;

                    //add f(x0) and f(xN) and include h/3 term
                    sum += (fNACA(0) + fNACA(1));
                    sum *= h / 3;
                }

                return sum;

            }

            public double[] naca4d()
            {
                int[] nP = PointSplit(naca4dLimit);

                double A0 = AoA - (1 / Math.PI) * (Simpson(nP[0], 0, Fore4d) + Simpson(nP[1], 0, Aft4d));
                double A1 = (2 / Math.PI) * (Simpson(nP[0], 1, Fore4d) + Simpson(nP[1], 1, Aft4d));
                double A2 = (2 / Math.PI) * (Simpson(nP[0], 2, Fore4d) + Simpson(nP[1], 2, Aft4d));

                double[] output = { A0, A1, A2 };

                return output;
            }

            public double[] naca5d()
            {
                int[] nP = PointSplit(naca5dLimit);

                double A0 = new(); double A1 = new(); double A2 = new();

                if (reflex == 0)
                {
                    A0 = AoA - (1 / Math.PI) * (Simpson(nP[0], 0, Fore5dNormal) + Simpson(nP[1], 0, Aft5dNormal));
                    A1 = (2 / Math.PI) * (Simpson(nP[0], 1, Fore5dNormal) + Simpson(nP[1], 1, Aft5dNormal));
                    A2 = (2 / Math.PI) * (Simpson(nP[0], 2, Fore5dNormal) + Simpson(nP[1], 2, Aft5dNormal));
                }
                else
                {
                    A0 = AoA - (1 / Math.PI) * (Simpson(nP[0], 0, Fore5dReflex) + Simpson(nP[1], 0, Aft5dReflex));
                    A1 = (2 / Math.PI) * (Simpson(nP[0], 1, Fore5dReflex) + Simpson(nP[1], 1, Aft5dReflex));
                    A2 = (2 / Math.PI) * (Simpson(nP[0], 2, Fore5dReflex) + Simpson(nP[1], 2, Aft5dReflex));
                }

                double[] output = { A0, A1, A2 };

                return output;
            }


            public int[] PointSplit(double limit)
            {
                int nFore = Convert.ToInt32(Math.Round(n * limit));
                int nAft = Convert.ToInt32(Math.Round(n * 1 - limit));

                return new int[] { nFore, nAft };
            }
        }


        private void NumericalSolveButton_CheckedChanged(object sender, EventArgs e)
        {
            if (NumericalSolveButton.Checked == true)
            {
                label25.Visible = true;
                textBox8.Visible = true;
            }
            else
            {
                label25.Visible = false;
                textBox8.Visible = false;
            }
        }
    }
}