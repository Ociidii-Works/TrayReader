﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace MaxwellGPUIdle
{
    public class ProcessDestroyer
    {
        public static List<string> compiler_processes = new List<string>();

        public static void KillCompilerProcesses()
        {
            // TODO: Move to idle loop
            //compiler_processes = Helper.Convert(Properties.Settings.Default.KnownGPUProcesses);
            foreach (string process_name in compiler_processes)
            {
                KillProcessByName(process_name);
            }
        }

        public static void KillProcessByName(string processToKill)
        {
            if (processToKill == null)
                return;
            // save cycles and do this once
            //Process[] processes_list = Process.GetProcessesByName(processToKill);
            //processToKill = "Dropbox";
            //System.Collections.Generic.IEnumerable<Process> processes_list = Process.GetProcesses().Where(pr => pr.MainWindowTitle == processToKill); // Kill by window Title! Works with UWP Apps!
            System.Collections.Generic.IEnumerable<Process> processes_list = Process.GetProcesses();
            foreach (Process x in processes_list)
            {
                if (x.MainWindowTitle == processToKill // Kill by window Title! Works with UWP Apps!
                    || x.ProcessName == processToKill // Kill by app name!
                    )
                {
                    string process_with_id = processToKill + " [" + x.Id + "]";
                    try
                    {
                        x.Kill();
                        x.WaitForExit();
                        x.Dispose();
                    }
#if DEBUG
                    catch (System.Exception e)
                    {
                        System.Console.WriteLine(e.Message);
                        MessageBox.Show("I'm sorry master..." + System.Environment.NewLine + System.Environment.NewLine +
                            "I couldn't kill " + process_with_id + "... (シ_ _)シ" + System.Environment.NewLine + System.Environment.NewLine +
                            "It said:" + System.Environment.NewLine + e.Message, "Gomenasai!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
#endif
                    finally
                    {
                    }
                }
            }
        }
    }
}