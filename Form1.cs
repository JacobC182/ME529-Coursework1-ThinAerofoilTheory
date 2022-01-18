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

            //Angle of attack validation
            //validation-must be a float
            try { float AttackAngle = Convert.ToSingle(AttackAngletextBox.Text); }

            catch (Exception) { MessageBox.Show("Please enter a valid Angle of Attack"); return; }

            //Parsing NACA wing code
            // [ For NACA code 1234 -> 1 = max camber as % of chord, 2 = position of max camber as chord/10, 34 = max thickness as % of chord ]
            // [ For NACA code 12345 -> 1 = optimal CL at ideal AoA, 2 = position of max camber, 3 = camber reflex yes/no, 45 = max thickness ]

            string NACAcode = NACAtextBox.Text; //Reading NACA code from textbox to string variable
            if (NACAcode.Length == 4)   //Parsing NACA info for 4 digit
            {
                int maxCamber = NACAcode[0];
                int posCamber = NACAcode[1];
                int thickness = NACAcode[2] + NACAcode[3];
            }
            else if (NACAcode.Length == 5)  //Parsing NACA info for 5 digit
            {
                int optimalCL = NACAcode[0];
                int maxCamber = NACAcode[1];
                int reflex = NACAcode[2];
                int thickness = NACAcode[3] + NACAcode[4];
            }

            static double symmetric_4d()
            {
                return 0;
            }

        }

    }
}