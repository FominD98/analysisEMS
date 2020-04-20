using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomVer2
{
    class CalculateEMS
    {
        private readonly double wireLength;
        private readonly double resistanceZ1;
        private readonly double resistanceZ2;
        private readonly double distanceBetweenWire;
        private readonly double radiusWire;
        private readonly double electricAntennaWire;
        private readonly double lengthAntennfWire;
        private readonly double dielectricConstant;
        private readonly double circularFrequency;
        private readonly double wavelengthOfField;
        private readonly double mediumPower;
        private readonly double сoeffAntenn;

        public string ProjectName { get; set; }

        public CalculateEMS(string projectName, double wireLength, double resistanceZ1, double resistanceZ2, double distanceBetweenWire, double radiusWire, double electricAntennaWire, double lengthAntennfWire, double dielectricConstant, double circularFrequency, double wavelengthOfField, double mediumPower, double сoeffAntenn)
        {
            this.ProjectName = projectName;
            this.wireLength = wireLength;
            this.resistanceZ1 = resistanceZ1;
            this.resistanceZ2 = resistanceZ2;
            this.distanceBetweenWire = distanceBetweenWire;
            this.radiusWire = radiusWire;
            this.electricAntennaWire = electricAntennaWire;
            this.lengthAntennfWire = lengthAntennfWire;
            this.dielectricConstant = dielectricConstant;
            this.circularFrequency = circularFrequency;
            this.wavelengthOfField = wavelengthOfField;
            this.mediumPower = mediumPower;
            this.сoeffAntenn = сoeffAntenn;
        }

        private double ElectricShortVoltage()
        {
            return electricAntennaWire * lengthAntennfWire / 2 * Math.PI * dielectricConstant *

                circularFrequency * Math.Pow(distanceBetweenWire, 3); 
        }

        private double ElectricLongVoltage()
        {
            return Math.Sqrt(30 * mediumPower * сoeffAntenn) / distanceBetweenWire;
        }

        private double MagneticShortVoltage()
        {
            return electricAntennaWire * lengthAntennfWire / 4 * Math.PI *

                dielectricConstant * Math.Pow(distanceBetweenWire, 2);
        }
        
        private double CalculateVoltage(double Voltage, int time)
        {
            return (2 * Math.PI * dielectricConstant * wireLength * (distanceBetweenWire / 2)) /

                (Math.Log10(2 * (distanceBetweenWire / 2) / radiusWire)) *

                (resistanceZ1 * resistanceZ2 / (resistanceZ1 + resistanceZ2)) *

                Voltage / time; 
        }

        //
        //время колебайний от 0мс до "time"мс с интервалом "timeInterval"
        private Dictionary<int, double> TimeValueVoltage(int time, int timeInterval, double voltage)
        {
            Dictionary<int, double> keyValuePairs = new Dictionary<int, double>();
            
            for(int i = timeInterval; i < time; i += timeInterval)
            {
                keyValuePairs.Add(i, CalculateVoltage(voltage, i));
            }

            return keyValuePairs;
        }

        public Dictionary<int, double> TimeValueElecticLongVoltage(int time, int timeInterval)
        {
            return TimeValueVoltage(time, timeInterval, MagneticShortVoltage());
        }

        public Dictionary<int, double> TimeValueElecticShortVoltage(int time, int timeInterval)
        {
            return TimeValueVoltage(time, timeInterval, ElectricLongVoltage());
        }
        public Dictionary<int, double> TimeValueMagneticShortVoltage(int time, int timeInterval)
        {
            return TimeValueVoltage(time, timeInterval, ElectricShortVoltage());
        }



    }
}
