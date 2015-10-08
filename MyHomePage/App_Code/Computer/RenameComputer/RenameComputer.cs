using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Management;
using System.Collections.ObjectModel;

/// <summary>
/// Summary description for Class1
/// </summary>
public class RenameComputer
{
    private PowerShell ps;
    private Collection<PSObject> results;
    private string domainUName, domainPWord, localUName, localPWord, newComputerName, computerName;
    private System.Security.SecureString securestring;
    private PSCredential localCred, domainCred;
    private List<string> parameters;

    public RenameComputer(string computerName, string newComputerName)
	{
        ps = PowerShell.Create();

        domainUName = System.Configuration.ConfigurationManager.AppSettings["AddCUser"];
        domainPWord = System.Configuration.ConfigurationManager.AppSettings["AddCPW"];
        localUName = System.Configuration.ConfigurationManager.AppSettings["AdminUser"];
        localPWord = System.Configuration.ConfigurationManager.AppSettings["AdminPW"];

        securestring = new System.Security.SecureString();
        foreach (char c in localPWord) { securestring.AppendChar(c); }
        localCred = new PSCredential(domainUName, securestring);
        securestring.Clear();
        foreach (char c in domainPWord) { securestring.AppendChar(c); }
        domainCred = new PSCredential(domainUName, securestring);

        parameters = new List<string>();
        this.computerName = computerName;
        this.newComputerName = newComputerName;
	}

    /*
            string cmd = String.Format("Remove-Computer");
            ps.AddCommand(cmd);
            ps.AddParameter("ComputerName", name);
            ps.AddParameter("UnjoinDomainCredential", cred);
            ps.AddParameter("Force");
            ps.AddParameter("Restart");
            ps.Invoke();
    */
    public int UnJoin()
    {
        try
        {
            string path = @"C:\inetpub\wwwroot\HelpDesk\App_Code\Computer\RenameComputer\RemoveDomain-Win8.ps1";
            parameters.Add("-ComputerName " + computerName);
            results = PowershellRunner.run(path, parameters);
        }
        catch (Exception e)
        {
            Console.Write(e);
        }
        return 0;
    }

    public int Rename()
    {
        try
        {
            // Rename-Computer -NewName Test -ComputerName W8-TestMachine -LocalCredential Get-Credential -Force
            ps.AddCommand("Rename-Computer");
            ps.AddParameter("ComputerName", computerName);
            ps.AddParameter("NewName", newComputerName);
            ps.AddParameter("LocalCredential", localCred);
            ps.AddParameter("Force");
            ps.Invoke();
          /*  Console.WriteLine("Connecting to " + computerName + " WMI Namespace");
            ConnectionOptions connection = new ConnectionOptions();
            //connection.Authentication = AuthenticationLevel.PacketPrivacy;
            //connection.Impersonation = ImpersonationLevel.Impersonate;
            //connection.EnablePrivileges = true;
            connection.Username = "IMBAdmin";
            connection.Password = "c0unters1gn";
            connection.Authority = "ntlmdomain:W8-TestMachine";

            ManagementScope scope = new ManagementScope(
            "\\\\" + computerName + "\\root\\CIMV2", connection);
            scope.Connect();

            Console.WriteLine("Connection Succeeded");
            Console.WriteLine("Renaming " + computerName + " to " + computerName );
            Console.WriteLine("Rename Successful");
 
        Console.WriteLine("");
        Console.WriteLine("Workstation Must Be Restarted!");*/
        }
        catch (Exception e)
        {
            Console.Write(e);
        }
        return 0;
    }

    public int Join()
    {
        try
        {


        }
        catch (Exception e)
        {
            Console.Write(e);
        }
        return 0;
    }


    public void Restart()
    {
        try
        {
            //Restart-Computer -ComputerName computerName -Credential localCred -Wait -Force
            PowerShell ps = PowerShell.Create();
            ps.AddCommand("Restart-Computer");
            ps.AddParameter("ComputerName", computerName);
            ps.AddParameter("Force");
            results = ps.Invoke();
        }
        catch (Exception e)
        {
            Console.Write(e);
        }
    }

    public void Go()
    {
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Maximized;
        startInfo.FileName = @"cmd.exe";
        startInfo.UseShellExecute = false;
        // PSExec.exe \\ComputerName -u USER -p Password -h -c FILE
        startInfo.Arguments = @"/c PSExec.exe \\" + computerName + " -u " + localUName + " -p " + localPWord + " -h -d -c " +
                              @"App_Code\Computer\RenameComputer\RenameComputer.ps1 -newComputerName " + newComputerName;
        //startInfo.Arguments = @"psexec.exe \\" + computerName + " -h -i -d " + "notepad.exe";
        process.StartInfo = startInfo;
        process.Start();
        //process.WaitForExit();
        //Console.WriteLine(process.ExitCode);
        // Check for updates
    }

    public void Shutdown()
    {
        try
        {
            //Restart-Computer -ComputerName computerName -Credential localCred -Wait -Force
            PowerShell ps = PowerShell.Create();
            ps.AddCommand("Stop-Computer");
            ps.AddParameter("ComputerName", computerName);
            ps.AddParameter("Force");
            results = ps.Invoke();
        }
        catch (Exception e)
        {
            Console.Write(e);
        }
    }
}