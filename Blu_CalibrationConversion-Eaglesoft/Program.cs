using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Blu_CalibrationConversion_Eaglesoft
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Please make a selection:");
            Console.WriteLine("1. Modify Orientation for Blu Sensor to be used in Eaglesoft.");
            Console.WriteLine("2. Disable the Timeout feature for the Blu Sensor.");
            Console.WriteLine("3. Reset the Sensor firmware to its default settings");
            Console.WriteLine("4. Configure the Sensor for Direct Integration");
            Console.WriteLine("5. Exit this application");

            string CalDir = "C:\\IOS\\MultiSensor\\";
            List<string> sensorList = new List<string>();
            foreach (string directory in Directory.GetDirectories(CalDir))
            {
                sensorList.Add(directory);
            }

            int x = Int32.Parse(Console.ReadLine());
            if (x == 1)
            {
                Console.WriteLine("Processing Calibration Files...");
                foreach (string sensor in sensorList)
                {
                    Console.WriteLine("Processing: " + sensor);
                    ChangeINI(sensor + "\\IOS.ini", 71, "Rotation=1");
                    ChangeINI(sensor + "\\IOS.ini", 72, "Flip=1");
                    Thread.Sleep(250);
                }
                PromptForAdditionalTasks();
            }//Changes the Orientation of the sensor for Eaglesoft integration.
            if (x == 2)
            {
                Console.WriteLine("Processing Calibration Files...");
                foreach (string sensor in sensorList)
                {
                    Console.WriteLine("Processing: " + sensor);
                    ChangeINI(sensor + "\\IOS.ini", 111, "TimeOut=0");
                }

                PromptForAdditionalTasks();
            }//Disables the timeout feature.
            if (x == 3)
            {
                Console.WriteLine("Resetting Device Firmware to Factory Parameters");
                foreach (string sensor in sensorList)
                {
                    Console.WriteLine("Processing: " + sensor);
                    ChangeINI(sensor + "\\IOS.ini", 111, "TimeOut=300");
                    ChangeINI(sensor + "\\IOS.ini", 71, "Rotation=0");
                    ChangeINI(sensor + "\\IOS.ini", 72, "Flip=3");
                }

                PromptForAdditionalTasks();
            }//Reset the install to factory spec.
            if (x == 4)
            {
                Console.WriteLine("Processing Calibration Files for Direct Integration");
                

                foreach (string sensor in sensorList)
                {
                    
                    string sensorSerial = sensor.Substring(19);//Get just the installed Serial #s
                    CopyFiles(sensor, "C:\\IOSensor\\CalibSensor\\" + sensorSerial);
                    string integratedIni = "C:\\IOSensor\\CalibSensor\\" + sensorSerial + "\\IOSensor.ini";

                    //Copy the default Ini Contents to the new directory
                    File.Copy(sensor + "\\IOS.ini",integratedIni,true);
                    ChangeINI(integratedIni, 73, @"HomeDir=C:\IOSensor\");
                    
                }
            }//Create the new ini file for use in Pixel and other Direct integrations.           
            if (x == 5) 
            {
                System.Windows.Forms.Application.Exit();
            }//Close it up.
        }

        private static void PromptForAdditionalTasks()
        {
            Console.WriteLine("Would you like to perform additional tasks?");
            Console.WriteLine("1. YES\r\n2. NO");
            int y = Int32.Parse(Console.ReadLine());
            if (y == 1)
                System.Windows.Forms.Application.Restart();
            else
                return;
        }

        private static void ChangeINI(string sourceFile, int lineToEdit, string LineContent)
        {
            string lineToWrite = "";
            string destinationFile = sourceFile;
            using (StreamReader reader = new StreamReader(sourceFile))
            {
                for (int i = 1; i <= lineToEdit; ++i)
                    lineToWrite = reader.ReadLine();
            }

            if (lineToWrite == null)
                throw new InvalidDataException("Line:" + lineToEdit + " does not exist in " + sourceFile);

            string[] lines = File.ReadAllLines(destinationFile);
            using (StreamWriter writer = new StreamWriter(destinationFile))
            {
                for (int currentLine = 1; currentLine <= lines.Length; ++currentLine)
                {
                    if (currentLine == lineToEdit)
                        writer.WriteLine(LineContent);
                    else
                        writer.WriteLine(lines[currentLine - 1]);
                }
            }
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
                File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
            }
        }
    }
}