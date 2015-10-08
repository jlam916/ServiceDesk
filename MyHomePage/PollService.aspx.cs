using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Printing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PollService : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblDate.Text = "Printers polled @ " + DateTime.Now.ToString();

        List<String> problems = new List<String>();
        PrintServer server = new PrintServer( @"\\DR3PRINT" );
        PrinterObj printerobj;
        DataTable table = new DataTable();
        table.Columns.Add( "Printer", typeof( String ) );
        table.Columns.Add( "Black Percent", typeof( String ) );
        table.Columns.Add( "Cyan Percent", typeof( String ) );
        table.Columns.Add( "Yellow Percent", typeof( String ) );
        table.Columns.Add( "Magenta Percent", typeof( String ) );
        table.Columns.Add( "Black Drum Percent", typeof( String ) );
        table.Columns.Add( "Cyan Drum Percent", typeof( String ) );
        table.Columns.Add( "Yellow Drum Percent", typeof( String ) );
        table.Columns.Add( "Magenta Drum Percent", typeof( String ) );
        table.Columns.Add( "Fuser", typeof( String ) );
        table.Columns.Add( "Transfer", typeof( String ) );
        table.Columns.Add( "Webpage", typeof( String ) );

        foreach ( PrintQueue printer in server.GetPrintQueues() )
        {
            try
            {
                if ( printer.Name.Contains( "HP" ) || printer.Name.Contains( "NPI9EA6A6" ) || printer.Name.ToUpper().Contains( "PLOTTER" ) || printer.Name.Contains( "AUDITS" ) ||
                     printer.Name.Contains( "MICR" ) || printer.Name.Contains( "KEN-SCOTT" ) || printer.Name.Contains( "COASTAL" ) ||
                     printer.Name.Contains( "19-BW-MFP-01" ) || printer.Name.Contains( "17K-COLOR-03" ) )
                {
                    continue;
                }
                // For Debugging
                if ( printer.Name.Equals( "09-ColorMFP-01", StringComparison.OrdinalIgnoreCase ) )
                {
                    ;
                }
                printerobj = new PrinterObj( printer.Name );
                if ( printerobj.Low )
                {
                    addRow(ref table, printerobj);
                    printerobj.sendLowReport();
                }
            }
            catch ( Exception ex )
            {
                problems.Add( printer.Name );
                report( DateTime.Now.ToString() + printer.Name + " was not emailed/polled report. \n" + ex.ToString() );
            }
        }

        if ( problems.Count > 0 )
        {
            report( "" );
            report( "================================" );
            report( "Problem checking printers: " + DateTime.Now.ToString() );
            foreach ( String printer in problems )
            {
                report( printer );
            }
        }
        gridView.DataSource = table;
        gridView.DataBind();

        report( "END REPORT - " + DateTime.Now.ToString() );
    }

    private void addRow(ref DataTable table, PrinterObj printerobj)
    {
        table.Rows.Add( printerobj.PrinterName, printerobj.BlackPercentage, printerobj.CyanPercentage, printerobj.YellowPercentage, printerobj.MagentaPercentage,
                       printerobj.BlackDrumPercentage, printerobj.CyanDrumPercentage, printerobj.YellowDrumPercentage, printerobj.MagentaDrumPercentage,
                       printerobj.FuserPercentage, printerobj.TransferPercentage, "http://" + printerobj.PrinterName);
    }

    private static void report(string str)
    {
        System.IO.StreamWriter fstream = new System.IO.StreamWriter( @"\\Iwmdocs\iwm\CIWMB-INFOTECH\HelpCenter\RKOEN\PollLog.log", true );
        fstream.WriteLine( str );
        fstream.Close();
    }
}