using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Web;

/// <summary>
/// Summary description for Audit
/// </summary>
public class Audit
{
    public Audit()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static DataTable getComputerInformation(string computerName)
    {
        DataTable dt = new DataTable();
        Collection<PSObject> results;
        List<string> parameters = new List<string>();
        string path = HttpContext.Current.Server.MapPath( "~/App_Code/Audit/Get-ComputerInfo.ps1" );
        parameters.Add( "-ComputerName " + computerName );

        results = PowershellRunner.run( path, parameters );

        // put results into datatable
        dt.Columns.Add( "Name" );
        dt.Columns.Add( "Manufacturer" );
        dt.Columns.Add( "Model" );
        dt.Columns.Add( "System Type" );
        dt.Columns.Add( "Description" );
        dt.Columns.Add( "RAM" );
        dt.Columns.Add( "Serial Number" );
        dt.Columns.Add( "Number Of Processors" );
        dt.Columns.Add( "Number Of Logical Processors" );
        DataRow row;
        Hashtable ht;
        foreach ( PSObject pso in results )
        {
            ht = pso.ImmediateBaseObject as Hashtable;
            row = dt.NewRow();
            row["Name"] = (string)ht["Name"];
            row["Manufacturer"] = (string)ht["Manufacturer"];
            row["Model"] = (string)ht["Model"];
            row["System Type"] = (string)ht["SystemType"];
            row["Description"] = (string)ht["Description"];
            row["Number Of Processors"] = ht["NumberOfProcessors"];
            row["Number Of Logical Processors"] = ht["NumberOfLogicalProcessors"];
            row["RAM"] = ht["RAM"];
            row["Serial Number"] = (string)ht["Serial"];
            dt.Rows.Add( row );
        }

        return dt;

        #region OLD

        // string path = @"C:\Users\Randell\Dropbox\WebSite2\App_Code\Audit\Get-ComputerInfo.ps1";
        // List<string> parameters = new List<string>();
        // System.Collections.ObjectModel.Collection<System.Management.Automation.PSObject> results = PowershellRunner.run(path, parameters);
        /* string OS = "";
         ManagementScope scope =
             new ManagementScope(
             "\\\\" + ComputerTextBox.Text + "\\root\\cimv2");
         scope.Connect();

         //Query system for Operating System information
         ObjectQuery query = new ObjectQuery(
             "SELECT * FROM Win32_OperatingSystem");
         ManagementObjectSearcher searcher =
             new ManagementObjectSearcher(scope, query);

         ManagementObjectCollection queryCollection = searcher.Get();
         foreach (ManagementObject m in queryCollection)
         {
             // Display the remote computer information
             OS += String.Format("Computer Name : {0}\n", m["csname"]);
             OS += String.Format("Windows Directory : {0}\n", m["WindowsDirectory"]);
             OS += String.Format("Operating System: {0}\n", m["Caption"]);
             OS += String.Format("Version: {0}\n", m["Version"]);
             OS += String.Format("Manufacturer : {0}\n", m["Manufacturer"]);
             Console.WriteLine("Computer Name : {0}",
                 m["csname"]);
             Console.WriteLine("Windows Directory : {0}",
                 m["WindowsDirectory"]);
             Console.WriteLine("Operating System: {0}",
                 m["Caption"]);
             Console.WriteLine("Version: {0}", m["Version"]);
             Console.WriteLine("Manufacturer : {0}",
                 m["Manufacturer"]);
         }

         //Query system for Serial Numbers
         string bla;
         query = new ObjectQuery("SELECT SerialNumber FROM WIN32_SystemEnclosure");
         searcher = new ManagementObjectSearcher(scope, query);
         queryCollection = searcher.Get();
         foreach (ManagementObject m in queryCollection)
         {
             bla = String.Format("SerialNumber : {0}\n", m["SerialNumber"]);
         }

         // Monitors
         */

        #endregion OLD
    }

    public static DataTable getMonitorInformation(string computerName)
    {
        DataTable dt = new DataTable();

        Collection<PSObject> results;
        List<string> parameters = new List<string>();
        string path = HttpContext.Current.Server.MapPath( "~/App_Code/Audit/Get-MonitorInfo1.ps1" );
        parameters.Add( "-ComputerName " + computerName );

        results = PowershellRunner.run( path, parameters );

        DataRow row;
        //  if ( results.Count > 1 )
        //  {
        int i;
        // Add Coulmn Labels
        for ( i = 1; i <= results.Count; i++ )
        {
            dt.Columns.Add( "****[" + i + "] - Monitor****" ); // Column seperator
            dt.Columns.Add( "[" + i + "] Manufacturer" );
            dt.Columns.Add( "[" + i + "] Name" );
            dt.Columns.Add( "[" + i + "] Serial Number" );
            dt.Columns.Add( "[" + i + "] Year" );
            dt.Columns.Add( "[" + i + "] Product Code" );
        }

        row = dt.NewRow();
        i = 1;
        foreach ( PSObject pso in results )
        {
            row["[" + i + "] Manufacturer"] = pso.Properties["Manufacturer"].Value.ToString();
            row["[" + i + "] Name"] = pso.Properties["Name"].Value.ToString();
            row["[" + i + "] Serial Number"] = pso.Properties["SerialNumber"].Value.ToString();
            row["[" + i + "] Year"] = pso.Properties["Year"].Value.ToString();
            row["[" + i + "] Product Code"] = pso.Properties["ProductCode"].Value.ToString();
            i++;
        }
        dt.Rows.Add( row );
        return dt;
    }

    public static DataTable getServicesInformation(string computerName)
    {
        DataTable dt = new DataTable();
        Collection<PSObject> results;
        PowerShell ps = PowerShell.Create();
        ps.AddCommand( "Get-Service" );
        ps.AddParameter( "ComputerName", computerName );
        results = ps.Invoke();

        dt.Columns.Add( "Display Name" );
        dt.Columns.Add( "Status" );
        DataRow row;
        string displayName, status;
        foreach ( PSObject pso in results )
        {
            row = dt.NewRow();

            displayName = pso.Properties["DisplayName"].Value.ToString();

            status = pso.Properties["status"].Value.ToString();

            dt.Rows.Add( displayName, status );
        }
        return dt;

        #region old

        /*try
        {
            ConnectionOptions connection = new ConnectionOptions();

            ManagementScope scope = new ManagementScope(
            "\\\\w8-jlam\\root\\CIMV2", connection);
            scope.Connect();

            ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_Service");

            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);

            foreach (ManagementObject queryObj in searcher.Get())
            {
                Console.WriteLine("-----------------------------------");
                Console.WriteLine("Win32_Service instance");
                Console.WriteLine("-----------------------------------");
                Console.WriteLine("Caption: {0}", queryObj["Caption"]);
                Console.WriteLine("Description: {0}", queryObj["Description"]);
                Console.WriteLine("Name: {0}", queryObj["Name"]);
                Console.WriteLine("PathName: {0}", queryObj["PathName"]);
                Console.WriteLine("State: {0}", queryObj["State"]);
                Console.WriteLine("Status: {0}", queryObj["Status"]);
            }
        }
        catch (ManagementException err)
        {
            Console.WriteLine("An error occured while querying for WMI data: " + err.Message);
        }
        catch (System.UnauthorizedAccessException unauthorizedErr)
        {
            Console.WriteLine("Connection error " + "(user name or password might be incorrect): " + unauthorizedErr.Message);
        }*/

        #endregion old
    }

    public static DataTable getInstalledPrograms(string computerName)
    {
        DataTable dt = new DataTable();
        Collection<PSObject> results;
        List<string> parameters = new List<string>();
        string path = HttpContext.Current.Server.MapPath( "~/App_Code/Audit/Get-InstalledPrograms.ps1" );
        parameters.Add( "-ComputerName " + computerName );

        results = PowershellRunner.run( path, parameters );

        dt.Columns.Add( "Program Name" );
        dt.Columns.Add( "Vendor" );
        dt.Columns.Add( "Installed Date" );
        dt.Columns.Add( "Uninstall String" );
        dt.Columns.Add( "GUID" );
        DataRow row;
        string programName = "", vendor = "", installDate = "", uninstallString = "", GUID = "";
        foreach ( PSObject pso in results )
        {
            row = dt.NewRow();
            if ( pso.Properties["AppName"].Value != null )
                programName = pso.Properties["AppName"].Value.ToString();
            if ( pso.Properties["AppVendor"].Value != null )
                vendor = pso.Properties["AppVendor"].Value.ToString();
            if ( pso.Properties["InstalledDate"].Value != null )
                installDate = pso.Properties["InstalledDate"].Value.ToString();
            if ( pso.Properties["UninstallKey"].Value != null )
                uninstallString = pso.Properties["UninstallKey"].Value.ToString();
            if ( pso.Properties["AppGUID"].Value != null )
                GUID = pso.Properties["AppGUID"].Value.ToString();

            dt.Rows.Add( programName, vendor, installDate, uninstallString, GUID );
        }
        return dt;
    }

    public static Boolean uninstallPrograms(string computerName, string program, string GUID)
    {
        if ( program.Equals( "" ) && GUID.Equals( "" ) )
            return false;

        Collection<PSObject> results;
        List<string> parameters = new List<string>();
        string path = HttpContext.Current.Server.MapPath( "~/App_Code/Audit/Uninstall-Program.ps1" );
        parameters.Add( "-ComputerName " + computerName );
        parameters.Add( "-uninstallString " + program );
        results = PowershellRunner.run( path, parameters );

        Hashtable ht;
        foreach ( PSObject pso in results )
        {
            ht = pso.ImmediateBaseObject as Hashtable;
            if ( (int)ht["ExitCode"] == 1 )
            {
                parameters.Clear();
                parameters.Add( "-ComputerName " + computerName );
                parameters.Add( "-appGUID " + GUID );
                results = PowershellRunner.run( path, parameters );

                Hashtable ht1;
                foreach ( PSObject pso1 in results )
                {
                    ht1 = pso.ImmediateBaseObject as Hashtable;
                    if ( (int)ht["ExitCode"] == 1 )
                        return false;
                }
            }
        }
        return true;
    }

    public static DataTable getPrinterInformation(string computerName)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add( "Printer Name" );
        DataRow row;

        RegistryKey key, subkey;
        key = RegistryKey.OpenRemoteBaseKey( RegistryHive.Users, computerName );

        // Get username and then get printers for that user sid

        foreach ( string value in key.GetSubKeyNames() )
        {
            // If printer connection exist save the user and list of printers
            subkey = key.OpenSubKey( value + @"\Printers\Connections" );
            if ( subkey == null )
                continue;

            foreach ( string printer in subkey.GetSubKeyNames() )
            {
                row = dt.NewRow();
                dt.Rows.Add( printer.Replace( ',', ' ' ).Trim() );
            }
        }
        return dt;
    }

    public static DataTable getIPConfigInformation(string computerName)
    {
        DataTable dt = new DataTable();
        Collection<PSObject> results;
        List<string> parameters = new List<string>();
        string path = HttpContext.Current.Server.MapPath( "~/App_Code/Audit/Get-IPConfig.ps1" );
        parameters.Add( "-ComputerName " + computerName );

        results = PowershellRunner.run( path, parameters );

        dt.Columns.Add( "IP Address" );
        dt.Columns.Add( "MAC Address" );
        dt.Columns.Add( "DNS Domain" );
        dt.Columns.Add( "Description" );

        DataRow row;
        string ip, mac, dns = "", description;
        foreach ( PSObject pso in results )
        {
            row = dt.NewRow();
            ip = pso.Properties["IP"].Value.ToString();
            mac = pso.Properties["MAC"].Value.ToString();
            if ( pso.Properties["DNS"].Value != null )
                dns = pso.Properties["DNS"].Value.ToString();
            else
                dns = "";
            description = pso.Properties["description"].Value.ToString();

            dt.Rows.Add( ip, mac, dns, description );
        }

        return dt;
    }
}