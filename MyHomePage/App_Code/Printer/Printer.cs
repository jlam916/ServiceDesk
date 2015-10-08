using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Management.Automation;
using System.Web;

/// <summary>
/// OLD Version, now uses printer.exe
/// Add: rundll32 printui.dll,PrintUIEntry /ga /c\\Test /n\\Dr3print\08-colormfp-03
/// Enum: rundll32 printui.dll,PrintUIEntry /ga /c\\Test /n\\Dr3print\08-colormfp-03
/// Delete: rundll32 printui.dll,PrintUIEntry /gd /c\\Test /n\\Dr3print\08-colormfp-03
/// </summary>

public class Printer
{
    public Printer()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static string[] getPrinterList(string floor)
    {
        string[] theList;
        //int i = 0;

        Collection<PSObject> results;
        List<string> parameters = new List<string>();
        string path = HttpContext.Current.Server.MapPath( "~/App_Code/Printer/Get-Printers.ps1" );
        parameters.Add( "-floor " + floor );
        results = PowershellRunner.run( path, parameters );

        theList = new string[results.Count];

        for ( int i = 0; i < results.Count; i++ )
            theList[i] = results[i].ToString();

        return theList;
    }

    public static void installPrinter(string computerName, string printerName)
    {
        #region OLD

        /*Collection<PSObject> results;
        List<string> parameters = new List<string>();
        string path = HttpContext.Current.Server.MapPath("~/App_Code/Printer/Install-Printer.ps1");
        parameters.Add("-computerName " + computerName);
        parameters.Add("-printerName " + printerName);
        results = PowershellRunner.run(path, parameters);*/

        #endregion OLD

        string path = HttpContext.Current.Server.MapPath( "~/App_Code/Printer/Printer.exe" );
        string arg = @"Add -c " + computerName + " -p " + printerName;
        System.Diagnostics.Process.Start( path, arg );
    }

    public static DataTable GetInstalledPrinters(string computerName)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add( "Printer Name" );
        DataRow row;

        RegistryKey key, subkey = null;
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

        if ( subkey != null )
        {
            subkey.Close();
        }

        key.Close();
        return dt;
    }

    public static void DeletePrinter(string computerName, string printerName)
    {
        #region OLD

        /*Collection<PSObject> results;
        List<string> parameters = new List<string>();
        string path = HttpContext.Current.Server.MapPath( "~/App_Code/Printer/Delete-Printer.ps1" );
        parameters.Add( "-computerName " + computerName );
        parameters.Add( "-printerName " + printerName );
        results = PowershellRunner.run( path, parameters );*/

        #endregion OLD

        string path = HttpContext.Current.Server.MapPath( "~/App_Code/Printer/Printer.exe" );
        string arg = @"Delete -c " + computerName + " -p " + printerName;
        System.Diagnostics.Process.Start( path, arg );
    }
}