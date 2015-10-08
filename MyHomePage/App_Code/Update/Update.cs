using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Update
/// </summary>
public class Update
{
	public Update()
	{
		//
		// TODO: Add constructor logic here
		//
	}



    public static void RunUpdates(ArrayList computerList)
    {
        string filePath = HttpContext.Current.Server.MapPath("~/Owexec.exe");
        string cmd = @"C:\Updater\updateHelper.bat";
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        startInfo.FileName = filePath;

        foreach (string computer in computerList)
        {
            startInfo.Arguments = @"-nowait -k " + cmd + " -c " + computer;
            process.StartInfo = startInfo;
            process.Start();
        }
    }

    public static void CopyCleaner(ArrayList computerList)
    {
        string sourcePath, targetPath;

        foreach (string computer in computerList)
        {
            targetPath = @"\\" + computer + @"\C$\Updater";
            sourcePath = HttpContext.Current.Server.MapPath("~/Apps/Updater/");
            copyFiles(sourcePath, targetPath);
        }
    }

    private static void copyFiles(string sourcePath, string targetPath)
    {
        // Get the subdirectories for the specified directory.
        DirectoryInfo dir = new DirectoryInfo(sourcePath);
        DirectoryInfo[] dirs = dir.GetDirectories();

        if (!dir.Exists)
        {
            throw new DirectoryNotFoundException(
                "Source directory does not exist or could not be found: "
                + sourcePath);
        }

        // To copy a folder's contents to a new location:
        // Create a new target folder, if necessary.
        if (!Directory.Exists(targetPath))
        {
            Directory.CreateDirectory(targetPath);
        }

        // Get the files in the directory and copy them to the new location.
        FileInfo[] files = dir.GetFiles();
        foreach (FileInfo file in files)
        {
            string temppath = Path.Combine(targetPath, file.Name);
            file.CopyTo(temppath, true);
        }

        // copying subdirectories, copy them and their contents to new location. 
        foreach (DirectoryInfo subdir in dirs)
        {
            string temppath = Path.Combine(targetPath, subdir.Name);
            copyFiles(subdir.FullName, temppath);
        }
    }

    public static void EnableAutoLogon(ArrayList computerList)
    {
        foreach (string computer in computerList)
        {
            string localUName = System.Configuration.ConfigurationManager.AppSettings["AdminUser"];
            string localPWord = System.Configuration.ConfigurationManager.AppSettings["AdminPW"];
            string filePath = HttpContext.Current.Server.MapPath("~/PSExec.exe");
            string cmd = HttpContext.Current.Server.MapPath("~/App_Code/Update/Enable-AutoLogon.exe");

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = filePath;
            startInfo.Arguments = @" \\" + computer + @" -d -s -c -f " + cmd;
            process.StartInfo = startInfo;
            process.Start();
        }

        System.Threading.Thread.Sleep(5000);

        foreach (string computer in computerList)
        {
            (new RenameComputer(computer, null)).Restart();
        }
    }

    public static void DisableAutoLogon(ArrayList computerList)
    {
        foreach (string computer in computerList)
        {
            string localUName = System.Configuration.ConfigurationManager.AppSettings["AdminUser"];
            string localPWord = System.Configuration.ConfigurationManager.AppSettings["AdminPW"];
            string filePath = HttpContext.Current.Server.MapPath("~/PSExec.exe");
            string cmd = HttpContext.Current.Server.MapPath("~/App_Code/Update/Disable-AutoLogon.exe");

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = filePath;
            startInfo.Arguments = @" \\" + computer + @" -d -s -c -f " + cmd;
            process.StartInfo = startInfo;
            process.Start();
        }
    }
}