using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Blu_CalibrationConversion
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            The_Beginning:
            Console.Clear();
            Console.WriteLine("Please make a selection:");
            Console.WriteLine("1. Modify Orientation for Blu Sensor for your dental software");
            Console.WriteLine("2. Disable the Timeout feature for the Blu Sensor");
            Console.WriteLine("3. Reset the Sensor firmware to its default settings");
            Console.WriteLine("4. Configure the Sensor for Direct Integration");
            Console.WriteLine("5. Exit this application");
            Console.WriteLine("");
            Console.Write("Choose: ");
            string CalDir = "C:\\IOS\\MultiSensor\\";
            List<string> sensorList = new List<string>();
            foreach (string directory in Directory.GetDirectories(CalDir))
            {
                sensorList.Add(directory);
            }

            int x = Int32.Parse(Console.ReadLine());
            List<KeyValuePair<string, string>> vals = new List<KeyValuePair<string, string>>();
            if (x == 1)
            {
                Console.WriteLine("--------------------------------------");
                Console.WriteLine("Please make an orientation selection:");
                Console.WriteLine("1. Process for Eaglesoft integration");
                Console.WriteLine("2. Process for PlanetDDS integration");
                Console.WriteLine("3. Process for ClearDent integration");
                Console.WriteLine("4. Process for DTX Studio integration");
                Console.WriteLine("5. Back to previous menu");
                Console.WriteLine("");
                Console.Write("Choose: ");
                int y = Int32.Parse(Console.ReadLine());

                if (y == 1) //1. Process for Eaglesoft integration
                {
                    Console.WriteLine("Processing Calibration Files for Eaglesoft...");
                    vals.Add(new KeyValuePair<string, string>("Rotation", "1"));
                    vals.Add(new KeyValuePair<string, string>("Flip", "1"));
                    ProcessIniFile("Settings", sensorList, vals);
                    goto The_Beginning;
                }
                else if (y == 2) //2. Process for PlanetDDS integration
                {
                    Console.WriteLine("Processing Calibration Files for PlanetDDS...");
                    vals.Add(new KeyValuePair<string, string>("Rotation", "0"));
                    vals.Add(new KeyValuePair<string, string>("Flip", "0"));
                    ProcessIniFile("Settings", sensorList, vals);
                    goto The_Beginning;
                }
                else if (y == 3) //3. Process for ClearDent integration
                {
                    Console.WriteLine("Processing Calibration Files for ClearDent...");
                    vals.Add(new KeyValuePair<string, string>("Rotation", "2"));
                    vals.Add(new KeyValuePair<string, string>("Flip", "3"));
                    ProcessIniFile("Settings", sensorList, vals);
                    goto The_Beginning;
                }
                else if (y == 4) //4. Process for DTX Studio integration
                {
                    Console.WriteLine("Processing Calibration Files for DTX Studio...");
                    vals.Add(new KeyValuePair<string, string>("Rotation", "1"));
                    vals.Add(new KeyValuePair<string, string>("Flip", "0"));
                    ProcessIniFile("Settings", sensorList, vals);
                    goto The_Beginning;
                }
                else //5. Back to previous menu  (Any other key will take you back to start)
                {
                    goto The_Beginning;
                }
            }
            if (x == 2) //2. Disable the Timeout feature for the Blu Sensor.
            {
                Console.WriteLine("Processing Calibration Files...");
                vals.Add(new KeyValuePair<string, string>("TimeOut", "0"));
                ProcessIniFile("Settings", sensorList, vals);

                goto The_Beginning;
            }
            if (x == 3) //3. Reset the Sensor firmware to its default settings
            {
                Console.WriteLine("Resetting Device Firmware to Factory Parameters");
                vals.Add(new KeyValuePair<string, string>("TimeOut", "300"));
                vals.Add(new KeyValuePair<string, string>("Rotation", "0"));
                vals.Add(new KeyValuePair<string, string>("Flip", "3"));
                ProcessIniFile("Settings", sensorList, vals);

                goto The_Beginning;
            }
            if (x == 4) //4. Configure the Sensor for Direct Integration
            {
                Console.WriteLine("Processing Calibration Files for Direct Integration");

                foreach (string sensor in sensorList)
                {
                    
                    string sensorSerial = sensor.Substring(19);//Get just the installed Serial #s
                    CopyFiles(sensor, "C:\\IOSensor\\CalibSensor\\" + sensorSerial);
                    string integratedIni = "C:\\IOSensor\\CalibSensor\\" + sensorSerial + "\\IOSensor.ini";

                    //Copy the default Ini Contents to the new directory
                    File.Copy(sensor + "\\IOS.ini",integratedIni,true);
                    Ini.WriteValue("Settings", "HomeDir", @"C:\IOSensor\", integratedIni);
                }
                goto The_Beginning;
            }
            if (x == 5) //5. Exit this application
            {
                System.Windows.Forms.Application.Exit();
            }
        }

        private static void ProcessIniFile(string Heading, List<string> sensorList, List<KeyValuePair<string, string>> ValuesToChange)
        {
            int count = 0;
            foreach (string sensor in sensorList)
            {
                count++;
                Console.WriteLine("Processing: [" + count + "/" + sensorList.Count + "] " + sensor);
                foreach (KeyValuePair<string, string> kvp in ValuesToChange)
                {
                    bool Success = Ini.WriteValue(Heading, kvp.Key, kvp.Value, sensor + "\\IOS.ini");
                }
            }
            Console.WriteLine();
            Console.WriteLine("Processing complete!       Press any key to continue.");
            Console.ReadKey();

        }


        private static void CopyFiles(string sourcePath, string targetPath)
        {
            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                if (!Directory.Exists(targetPath)) 
                {
                    Directory.CreateDirectory(targetPath);
                }
                File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
            }
        }
    }
}