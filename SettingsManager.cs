using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.ServiceModel.Syndication;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using MaxwellGPUIdle.Properties;

namespace MaxwellGPUIdle
{
    public class SettingsManager
    {
        private static bool NeedUpgrade = true;

        public static void LoadProcessesList()
        {
            StringCollection known_processes_from_settings = Settings.Default.KnownGPUProcesses;
            StringCollection processes_list = new StringCollection();
            //NotificationManager.PushNotificationToOS("Loading processes list...");

            foreach (string url_iter in known_processes_from_settings)
            {
                bool success = Helper.ValidateExecutableName(url_iter);

                if (success)
                {
                    success = !processes_list.Contains(url_iter);
                }

                if (success)
                {
                    processes_list.Add(url_iter);
                }
            }

            // Must be saved after the foreach loop to prevent overwriting the working data
            SettingsManager.WriteNewProcessesList(processes_list);
            List<string> processes_list_real = Helper.Convert(processes_list);
            string first = processes_list_real.First();
            string last = processes_list_real.Last();
            string processes_list_string = "";
            foreach (string process in processes_list_real)
            {
                if (process == first)
                {
                    processes_list_string = process;
                }
                else if (process != last)
                {
                    processes_list_string += ", " + process;
                }
                else
                {
                    processes_list_string += " and " + process;
                }
            }
            NotificationManager.PushNotificationToOS("Processes that will be killed: " + processes_list_string);
        }

        public static void Refresh()
        {
            if (NeedUpgrade)
            {
                Settings.Default.Upgrade();
                Settings.Default.Save();
            }
            Settings.Default.Reload();
            NeedUpgrade = false;
        }

        public static void WriteNewProcessesList(StringCollection coll)
        {
            Settings.Default.KnownGPUProcesses.Clear();
            Settings.Default.KnownGPUProcesses = coll;
            NeedUpgrade = true;
            Refresh();
        }
    }
}