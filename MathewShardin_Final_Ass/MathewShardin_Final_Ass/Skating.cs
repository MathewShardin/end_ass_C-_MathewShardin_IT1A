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

            //Initialize context menu
            CreateContextMenu();
            //Initialize Toolbar context menu
            CreateNotifyContextMenu();
        }

        private void CreateNotifyContextMenu() {
            MenuItem menuItem1 = new MenuItem("Add+");
            menuItem1.Click += new EventHandler(addToolStripMenuItem_Click);
            MenuItem menuItem2 = new MenuItem("Winner");
            menuItem2.Click += new EventHandler(winnerToolStripMenuItem_Click);
            MenuItem menuItem3 = new MenuItem("Result");
            menuItem3.Click += new EventHandler(resultToolStripMenuItem_Click);
            MenuItem menuItem4 = new MenuItem("About");
            menuItem4.Click += new EventHandler(aboutToolStripMenuItem_Click);
            MenuItem menuItem5 = new MenuItem("Close");
            menuItem5.Click += new EventHandler(closeToolStripMenuItem_Click);
            MenuItem menuItem6 = new MenuItem("Open");
            menuItem6.Click += new EventHandler(addToolStripMenuItem_Click);

            ContextMenu menuNotify = new ContextMenu();
            menuNotify.MenuItems.Clear();

            menuNotify.MenuItems.Add(menuItem1);
            menuNotify.MenuItems.Add(menuItem2);
            menuNotify.MenuItems.Add(menuItem3);
            menuNotify.MenuItems.Add(menuItem4);
            menuNotify.MenuItems.Add(menuItem5);
            menuNotify.MenuItems.Add(menuItem6);

            notifyIcon1.ContextMenu = menuNotify;
        }

        private void CreateContextMenu() {
            ContextMenuStrip menuStrip = new ContextMenuStrip();

            //Add page
            ToolStripMenuItem menuItem = new ToolStripMenuItem("Add+");
            menuItem.Click += new EventHandler(addToolStripMenuItem_Click);
            menuItem.Name = "Add+";
            menuStrip.Items.Add(menuItem);
            //Winner page
            menuItem = new ToolStripMenuItem("Winner");
            menuItem.Click += new EventHandler(winnerToolStripMenuItem_Click);
            menuItem.Name = "Winner";
            menuStrip.Items.Add(menuItem);
            //Result page
            menuItem = new ToolStripMenuItem("Result");
            menuItem.Click += new EventHandler(resultToolStripMenuItem_Click);
            menuItem.Name = "Result";
            menuStrip.Items.Add(menuItem);
            //About page
            menuItem = new ToolStripMenuItem("About");
            menuItem.Click += new EventHandler(aboutToolStripMenuItem_Click);
            menuItem.Name = "About";
            menuStrip.Items.Add(menuItem);
            //Close button
            menuItem = new ToolStripMenuItem("Close");
            menuItem.Click += new EventHandler(closeToolStripMenuItem_Click);
            menuItem.Name = "Close";
            menuStrip.Items.Add(menuItem);

            this.ContextMenuStrip = menuStrip;
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
            //Save skater into distance
            
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

        private void button2_Click(object sender, EventArgs e) {
            Championship championship = new Championship();
            championship.ClearSkaters();
            championship = null;
            GC.Collect();
            //Refresh table
            button3.PerformClick();

        }

        private void button3_Click(object sender, EventArgs e) {
            Championship championship = new Championship();
            DataSet result = championship.UpdateTable();
            //Display data
            tableLabel.Text = String.Empty;
            tableLabel.Text = "Name - 500m 1500m 5000m 10000m - Total Points\n\n";
            foreach (DataRow row in result.Tables[0].Rows) {
                Skater skaterTemp = new Skater(row["name"].ToString());
                skaterTemp.time500 = Convert.ToInt64(row["500m"]);
                skaterTemp.time1500 = Convert.ToInt64(row["1500m"]);
                skaterTemp.time5000 = Convert.ToInt64(row["5000m"]);
                skaterTemp.time10000 = Convert.ToInt64(row["10000"]);
                tableLabel.Text += String.Format("{0} - {1} {2} {3} {4} - {5}\n", row["name"], row["500m"], row["1500m"], row["5000m"], row["10000"], championship.GetTotalPoints(skaterTemp));
            }
            championship = null;
            GC.Collect();
        }

        //Events
        private void button1_Click(object sender, EventArgs e) {
            //Get winner
            Championship championship = new Championship();
            label8.Text = championship.GetWinnerName();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e) {
            Show();
            this.WindowState = FormWindowState.Normal;
            tabControl1.SelectTab(0);
        }

        private void winnerToolStripMenuItem_Click(object sender, EventArgs e) {
            Show();
            this.WindowState = FormWindowState.Normal;
            tabControl1.SelectTab(1);
        }

        private void resultToolStripMenuItem_Click(object sender, EventArgs e) {
            Show();
            this.WindowState = FormWindowState.Normal;
            tabControl1.SelectTab(2);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
            //Open the about page
            var aboutForm = new About();
            aboutForm.Show();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e) {
            Application.Exit();
        }

        private void Skating_Resize(object sender, EventArgs e) {
            if (this.WindowState == FormWindowState.Minimized) {
                Hide();
                notifyIcon1.Visible = true;
            } else {
                notifyIcon1.Visible = false;
            }
        }
    }


}
