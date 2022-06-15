/*
 * Mathew Shardin
 * 4951735
 * 15 June 2022
 * Ass 5
 * An application to judge a speed skating champiosnhip
 * Rob Loves
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

namespace MathewShardin_Final_Ass {
    internal static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Skating());

    }
}

    public class Skater {
        private String name;
        internal long time500 { set; get; }
        internal long time1500 { set; get; }
        internal long time5000 { set; get; }
        internal long time10000 { set; get; }

        public Skater(String nameInp) {
            name = nameInp;
        }
        public String GetName() {
            return name;
        }
        public long GetTime(int distanceInp) {
            switch (distanceInp) {
                case 500:
                    return time500;
                case 1500:
                    return time1500;
                case 5000:
                    return time5000;
                case 10000:
                    return time10000;
                default:
                    return 0;
            }
        }
        /// <summary>
        /// Saves the time in milliseconds into the relevant field
        /// </summary>
        /// <param name="timeInp">Time in DateTime format/param>
        /// <param name="distanceInp">Distance int on which the time was achieved</param>
        public void RegisterTime(DateTime timeInp, int distanceInp) {
            //Convert time into milliseconds by removing todays date to account for the Date portion of DateTime
            long onlyDistTime = timeInp.Ticks - DateTime.Today.Ticks;
            onlyDistTime = onlyDistTime / TimeSpan.TicksPerMillisecond;
            switch (distanceInp) {
                case 500:
                    time500 = onlyDistTime;
                    break;
                case 1500:
                    time1500 = onlyDistTime;
                    break;
                case 5000:
                    time5000 = onlyDistTime;
                    break;
                case 10000:
                    time10000 = onlyDistTime;
                    break;
            }
        }
    }
    public class Distance {
        public Distance() {
        }
        public double Get500mPoints(Skater skaterInp, int distanceInp) {
            //Distance distanceTemp = new Distance(distanceInp);
            long timeTemp = skaterInp.GetTime(distanceInp);
            timeTemp = timeTemp / (distanceInp / 500);
            return timeTemp;
        }
    }
    public class Championship {
        public Championship() {}
        public void ClearSkaters() {
            //Delete all table entries
            string dBConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\University\Period_4\C#\Final_Assignment\end_ass_C-_MathewShardin_IT1A\MathewShardin_Final_Ass\MathewShardin_Final_Ass\SkatersDB.mdf;Integrated Security=True";
            SqlConnection dBcon = new SqlConnection(dBConnectionString);
            dBcon.Open();
            String query = String.Format(@"DELETE FROM Skater");
            SqlCommand cmd = new SqlCommand(query, dBcon);
            SqlDataAdapter adapSkate = new SqlDataAdapter();
            adapSkate.InsertCommand = new SqlCommand(query, dBcon);
            adapSkate.InsertCommand.ExecuteNonQuery();
            //Clean up after
            cmd.Dispose();
            dBcon.Close();
        }
        public DataSet UpdateTable() {
            //Fetch data from db
            string dBConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\University\Period_4\C#\Final_Assignment\end_ass_C-_MathewShardin_IT1A\MathewShardin_Final_Ass\MathewShardin_Final_Ass\SkatersDB.mdf;Integrated Security=True";
            SqlConnection dBcon = new SqlConnection(dBConnectionString);
            dBcon.Open();
            String query = String.Format(@"SELECT * FROM Skater");
            DataSet result = new DataSet();
            SqlDataAdapter adapSkate = new SqlDataAdapter(query, dBcon);
            adapSkate.Fill(result);
            //Clean up after
            adapSkate.Dispose();
            dBcon.Close();
            return result;
        }
        public double GetTotalPoints(Skater skaterInp) {
            Distance distance = new Distance();
            double totalPointsReturn = 0;
            totalPointsReturn += distance.Get500mPoints(skaterInp, 500);
            totalPointsReturn += distance.Get500mPoints(skaterInp, 1500);
            totalPointsReturn += distance.Get500mPoints(skaterInp, 5000);
            totalPointsReturn += distance.Get500mPoints(skaterInp, 10000);
            return totalPointsReturn;
        }

        public String GetWinnerName() {
            //Fetch data from db
            string dBConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\University\Period_4\C#\Final_Assignment\end_ass_C-_MathewShardin_IT1A\MathewShardin_Final_Ass\MathewShardin_Final_Ass\SkatersDB.mdf;Integrated Security=True";
            SqlConnection dBcon = new SqlConnection(dBConnectionString);
            dBcon.Open();
            String query = String.Format(@"SELECT * FROM Skater");
            DataSet result = new DataSet();
            SqlDataAdapter adapSkate = new SqlDataAdapter(query, dBcon);
            adapSkate.Fill(result);
            double pointsTemp = long.MaxValue;
            String returnName = String.Empty;
            foreach (DataRow row in result.Tables[0].Rows) {
                //Create a Skater object
                Skater skaterTemp = new Skater(row["name"].ToString());
                skaterTemp.time500 = Convert.ToInt64(row["500m"]);
                skaterTemp.time1500 = Convert.ToInt64(row["1500m"]);
                skaterTemp.time5000 = Convert.ToInt64(row["5000m"]);
                skaterTemp.time10000 = Convert.ToInt64(row["10000"]);
                //Get its total points and save the Name if it the lowest
                if (GetTotalPoints(skaterTemp) < pointsTemp) {
                    pointsTemp = GetTotalPoints(skaterTemp);
                    returnName = skaterTemp.GetName();
                }
                skaterTemp = null;
            }
            //Clean up after
            adapSkate.Dispose();
            dBcon.Close();
            return returnName;
        }
    }
}
