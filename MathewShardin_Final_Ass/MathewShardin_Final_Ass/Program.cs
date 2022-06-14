using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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


            //Initialize distances
            Distance distance500 = new Distance(500);
            Distance distance1500 = new Distance(1500);
            Distance distance5000 = new Distance(5000);
            Distance distance10000 = new Distance(10000);


            //Initialize championship class
            Championship championship = new Championship();
            championship.AddDistance(distance500);
            championship.AddDistance(distance1500);
            championship.AddDistance(distance5000);
            championship.AddDistance(distance10000);

    }
}

    public class Skater {
        private String name;
        private long time500;
        private long time1500;
        private long time5000;
        private long time10000;

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
        private int distanceMeters { get; }
        private List<Skater> skatersOnDistance = new List<Skater>();
        public Distance(int distanceInp) {
            distanceMeters = distanceInp;
        }
        public List<Skater> GetSkatersOnDistance() {
            return skatersOnDistance;
        }
        public void registerSkater(Skater skaterInp) {
            if (!skatersOnDistance.Contains(skaterInp)) {
                skatersOnDistance.Add(skaterInp);
            }
        }
        public void ClearSkatersInList() {
            skatersOnDistance.Clear();
        }
    }
    public class Championship {
        private List<Distance> distancesCompleted = new List<Distance>();
        public Championship() {}
        public void ClearSkaters() {
            foreach (Distance distance in distancesCompleted) {
                distance.ClearSkatersInList();
            }
        }
        public void AddDistance(Distance distanceInp) {
            distancesCompleted.Add(distanceInp);
        }
    }
}
