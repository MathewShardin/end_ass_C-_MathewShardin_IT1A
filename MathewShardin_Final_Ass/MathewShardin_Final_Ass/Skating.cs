using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace MathewShardin_Final_Ass {
    public partial class Skating : Form {
        public Skating() {
            //Make splash screen appear
            Thread splashT = new Thread(new ThreadStart(StartSplash));
            splashT.Start();
            Thread.Sleep(3000);
            InitializeComponent();
            splashT.Abort();

        }

        public void StartSplash() {
            Application.Run(new Splash_Screen());
        }

        private void saveButton_Click(object sender, EventArgs e) {
            Skater skaterLoc = new Skater(nameTextBox.Text);
            List<String> stringTimesToSave = new List<String>();
            stringTimesToSave.Add(textBox1.Text);
            stringTimesToSave.Add(textBox2.Text);
            stringTimesToSave.Add(textBox3.Text);
            stringTimesToSave.Add(textBox4.Text);
            int counter = 1;
            int distReg = 500;
            foreach (String time in stringTimesToSave) {
                switch (counter) {
                    case 1:
                        distReg = 500;
                        break;
                    case 2:
                        distReg = 1500;
                        break;
                    case 3:
                        distReg = 5000;
                        break;
                    case 4:
                        distReg = 10000;
                        break;
                }        
                //Input validation
                if (time.Length == 12) {
                    try {
                        DateTime dateTimeLoc = DateTime.Parse(time);
                        skaterLoc.RegisterTime(dateTimeLoc, distReg);
                        //Reset error text if needed
                        errorLabel.Text = "";
                    }
                    catch {
                        errorLabel.Text = "Format Incorrect";
                        return;
                    }
                } else {
                    errorLabel.Text = "Format Incorrect";
                    return;
                }
                counter++;
            }
            //Save data int database
            string dBConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\University\Period_4\C#\Final_Assignment\end_ass_C-_MathewShardin_IT1A\MathewShardin_Final_Ass\MathewShardin_Final_Ass\SkatersDB.mdf;Integrated Security=True";
            SqlConnection dBcon = new SqlConnection(dBConnectionString);
            dBcon.Open();
            String query = String.Format(@"INSERT INTO Skater VALUES ('{0}', {1}, {2}, {3}, {4})", skaterLoc.GetName(), skaterLoc.GetTime(500), skaterLoc.GetTime(1500), skaterLoc.GetTime(5000), skaterLoc.GetTime(10000));
            SqlCommand cmd = new SqlCommand(query, dBcon);
            SqlDataAdapter adapSkate = new SqlDataAdapter();
            adapSkate.InsertCommand = new SqlCommand(query, dBcon);
            adapSkate.InsertCommand.ExecuteNonQuery();
            //Clean up after saving data into DB
            cmd.Dispose();
            dBcon.Close();
            skaterLoc = null;
            GC.Collect();
            //Clear the form
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            nameTextBox.Text = "";




        }
    }


}
