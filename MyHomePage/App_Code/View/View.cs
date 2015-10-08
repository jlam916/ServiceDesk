using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

/// <summary>
/// Summary description for View
/// </summary>
public class View
{
	public View()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static void OpenEventViewer(string client, string target)
    {
        string clientFile = @"\\" + client + @"\C$\Windows\System32\EventViewer.bat";

        System.IO.File.Copy(HttpContext.Current.Server.MapPath("~/App_Code/View/EventViewer.bat"), clientFile, true);

        System.Diagnostics.Process process = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        startInfo.FileName = "cmd.exe";
        startInfo.Arguments = @"/c owexec.exe -nowait -k EventViewer.bat -p " + target + " -c " + client + @"""";
        process.StartInfo = startInfo;
        process.Start();

        // new thread to delete transferred file
        ThreadStart work = delegate
        {
            delete(clientFile);
        };
        new Thread(work).Start();

    }

    public static void OpenCDrive(string client, string target)
    {
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        startInfo.FileName = "cmd.exe";
        startInfo.Arguments = @"/c owexec.exe -nowait -k explorer.exe -p " + target + " -c " + client + @"""";
        process.StartInfo = startInfo;
        process.Start();
    }

    public static void OpenRA(string client, string target)
    {
        string clientFile = @"\\" + client + @"\C$\Windows\System32\RemoteAssist.bat";
        System.IO.File.Copy(HttpContext.Current.Server.MapPath("~/App_Code/View/RemoteAssist.bat"), clientFile, true);

        System.Diagnostics.Process process = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        startInfo.FileName = "cmd.exe";
        startInfo.Arguments = @"/c owexec.exe -nowait -k RemoteAssist.bat -p " + target + " -c " + client + @"""";
        process.StartInfo = startInfo;
        process.Start();

        // new thread to delete transferred file
        ThreadStart work = delegate
        {
            delete(clientFile);
        };
        new Thread(work).Start();
    }

    private static void delete(string filePath)
    {
        Thread.Sleep(5000);
        System.IO.File.Delete(filePath);
    }
}