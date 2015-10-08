using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Linq;
using System.Management;
using System.Management.Automation;
using System.Printing;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    private DataTable dtPrograms;
    private static string[] AD_Computers;

    protected void Page_Load(object sender, EventArgs e)
    {
        if ( !IsPostBack )
        {
            new Thread( new ThreadStart( metrics ) ).Start();

            PrincipalContext pc = new PrincipalContext( ContextType.Domain, "ITSERVICES" );
            ComputerPrincipal computer = new ComputerPrincipal( pc );
            computer.Name = "*"; //reg expression
            PrincipalSearcher ps = new PrincipalSearcher();
            ps.QueryFilter = computer;
            ((System.DirectoryServices.DirectorySearcher)ps.GetUnderlyingSearcher()).PageSize = 500;
            PrincipalSearchResult<Principal> psr = ps.FindAll();
            AD_Computers = new string[psr.Count()];
            int i = 0;
            foreach ( ComputerPrincipal cp in psr )
            {
                AD_Computers[i++] = cp.Name;
            }
        }
    }

    #region QuickFix

    protected void Button1_Click1(object sender, EventArgs e)
    {
        if ( !isName() )
        {
            return;
        }
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        startInfo.FileName = "cmd.exe";
        startInfo.Arguments = @"/c owexec.exe -nowait -k Remap_Drives.bat -copy -c " + ComputerTextBox.Text + @"""";
        process.StartInfo = startInfo;
        process.Start();
    }

    protected void GPupdateBtn_Click(object sender, EventArgs e)
    {
        if ( !isName() )
        {
            return;
        }
        string path = HttpContext.Current.Server.MapPath( "~/GPUpdate.bat" );
        string target = @"\\" + ComputerTextBox.Text + @"\c$\Windows\System32\GPUpdate.bat";
        System.IO.File.Copy( path, target, true );

        System.Diagnostics.Process process = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        startInfo.FileName = "cmd.exe";
        startInfo.Arguments = @"/c owexec.exe -nowait -k gpupdate.bat -c " + ComputerTextBox.Text + @"""";
        process.StartInfo = startInfo;
        process.Start();
        process.WaitForExit();

        System.IO.File.Delete( target );
        ComputerTextBox.Text = "";
    }

    protected void MailProfileBtn_Click(object sender, EventArgs e)
    {
        if ( !isName() )
        {
            return;
        }

        string path = HttpContext.Current.Server.MapPath( "~/App_Code/QuickFix/MailProfile.exe" );
        string target = @"\\" + ComputerTextBox.Text + @"\c$\Windows\System32\MailProfile.exe";
        System.IO.File.Copy( path, target, true );

        System.Diagnostics.Process process = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        startInfo.FileName = "cmd.exe";
        startInfo.Arguments = @"/c owexec.exe -nowait -k MailProfile.exe -c " + ComputerTextBox.Text + @"""";
        process.StartInfo = startInfo;
        process.Start();

        // new thread to delete transferred file
        System.Threading.ThreadStart work = delegate
        {
            System.Threading.Thread.Sleep( 30000 ); // wait to readd profile
            System.IO.File.Delete( target );
        };
        new System.Threading.Thread( work ).Start();
        ComputerTextBox.Text = "";
    }

    protected void NetworkBtn_Click(object sender, EventArgs e)
    {
        if ( !isName() )
        {
            return;
        }
        string localUName = System.Configuration.ConfigurationManager.AppSettings["AdminUser"];
        string localPWord = System.Configuration.ConfigurationManager.AppSettings["AdminPW"];
        string filePath = HttpContext.Current.Server.MapPath( "~/PsExec.exe" );
        string filePath2 = HttpContext.Current.Server.MapPath( "~/troubleshootNetwork.bat" );
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        startInfo.FileName = "cmd.exe";
        startInfo.Arguments = @"/c " + filePath + @" \\" + ComputerTextBox.Text + " -c -f -d " + filePath2 + " -u " + localUName + " -p " + localPWord;
        process.StartInfo = startInfo;
        process.Start();
        ComputerTextBox.Text = "";
    }

    #endregion QuickFix

    #region Computer

    protected void RenameBtn_Click(object sender, EventArgs e)
    {
        if ( !isName() )
        {
            return;
        }
        RenameComputer renameComputer = new RenameComputer( ComputerTextBox.Text, "W8-Test" );
        //renameComputer.Go();
        //renameComputer.UnJoin();
        //renameComputer.Restart();
        // renameComputer.Rename();
        renameComputer.Restart();
        //renameComputer.Join();
        //renameComputer.Restart();
    }

    protected void RestartBtn_Click(object sender, EventArgs e)
    {
        if ( !isName() )
        {
            return;
        }
        (new RenameComputer( ComputerTextBox.Text, null )).Restart();
    }

    protected void ShutdownBtn_Click(object sender, EventArgs e)
    {
        if ( !isName() )
        {
            return;
        }
        (new RenameComputer( ComputerTextBox.Text, null )).Shutdown();
    }

    protected void NewBtn_Click(object sender, EventArgs e)
    {
        Response.Redirect( "http://w8-rkoen" );
    }

    #endregion Computer

    #region Printer

    protected void AddPrinterBtn_Click(object sender, EventArgs e)
    {
        if ( !isName() )
        {
            return;
        }

        if ( printerDropDownLabel.Visible == true )
        {
            printerDropDownList.Items.Clear();
            printerDropDownList.Visible = false;
            printerDropDownLabel.Visible = false;
            InstallPrinterBtn.Visible = false;
        }

        this.mpeAddPrinter.Show();
    }

    protected void getPrinters_Click(object sender, EventArgs e)
    {
        if ( !isName() )
        {
            return;
        }

        string[] printerList = Printer.getPrinterList( floorList.SelectedValue );
        int i = 0;

        foreach ( string printer in printerList )
        {
            printerDropDownList.Items.Insert( i++, new ListItem( printer ) );
        }

        printerDropDownList.Visible = true;
        printerDropDownLabel.Visible = true;
        InstallPrinterBtn.Visible = true;
        this.mpeAddPrinter.Show();
    }

    protected void InstallPrinterBtn_Click(object sender, EventArgs e)
    {
        Printer.installPrinter( ComputerTextBox.Text, printerDropDownList.SelectedValue );
    }

    protected void DeletePrinterBtn_Click(object sender, EventArgs e)
    {
        if ( !isName() )
        {
            return;
        }

        DataTable installedPrinters = Printer.GetInstalledPrinters( ComputerTextBox.Text );

        if ( installedPrinters == null )
            throw new Exception( "No printers found!" );

        this.printerGridView.DataSource = installedPrinters;
        this.printerGridView.DataBind();

        if ( printerGridView.SelectedRow != null )
        {
            // Change backcolor
            for ( int i = 0; i < printerGridView.Rows.Count; i++ )
                printerGridView.Rows[i].BackColor = this.printerGridView.BackColor;
        }

        this.mpeDeletePrinter.Show();
    }

    protected void printerGridView_SelectedIndexChanged(object sender, EventArgs e)
    {
        for ( int i = 0; i < printerGridView.Rows.Count; i++ )
            printerGridView.Rows[i].BackColor = this.printerGridView.BackColor;
        printerGridView.SelectedRow.BackColor = System.Drawing.ColorTranslator.FromHtml( "#A1DCF2" );
        this.mpeDeletePrinter.Show();
    }

    protected void uninstallPrinterBtn_Click(object sender, EventArgs e)
    {
        string printer = printerGridView.SelectedRow.Cells[1].Text.Split()[1];
        Printer.DeletePrinter( ComputerTextBox.Text, printer );
    }

    #region Levels Button

    protected void LevelsBtn_Click(object sender, EventArgs e)
    {
        rblPrinters.Items.Clear();
        PrintServer server = new PrintServer( @"\\DR3PRINT" );
        foreach ( PrintQueue printer in server.GetPrintQueues() )
        {
            // if sharp use different webpage
            if ( printer.Comment.Contains( "SHARP MX-4141N" ) )
            {
                rblPrinters.Items.Add( new ListItem( printer.Name, "http://" + printer.Name + "/device_status.html" ) );
            }
            else if ( printer.Name.Contains( "HP" ) || printer.Name.Contains( "NPI9EA6A6" ) || printer.Name.Contains( "PLOTTER" ) || printer.Name.Contains( "AUDITS" ) ||
                      printer.Name.Contains( "MICR" ) || printer.Name.Contains( "KEN-SCOTT" ) || printer.Name.Contains( "COASTAL" ) )
            {
                continue;
            }
            else if ( printer.Name.Equals( "24-COLOR-01", StringComparison.OrdinalIgnoreCase ) ||
                      printer.Name.Equals( "17K-COLOR-03", StringComparison.OrdinalIgnoreCase ) )
            {
                ListItem invalid = new ListItem( printer.Name, "https://" + printer.Name );
                invalid.Enabled = false;
                invalid.Attributes.Add( "style", "color:#999" );
                rblPrinters.Items.Add( invalid );
            }
            else
            {
                rblPrinters.Items.Add( new ListItem( printer.Name, "https://" + printer.Name ) );
            }
        }

        mpePrinters.Show();
        mpeLevels.Hide();
    }

    protected void btnLevel_Click(object sender, EventArgs e)
    {
        Clear();
        if ( rblPrinters.SelectedItem == null )
        {
            return;
        }
        string printerName = rblPrinters.SelectedItem.Text;
        SNMP_Printer printer = new SNMP_Printer( printerName );
        string printerModel = printer.getModel();

        if ( String.IsNullOrEmpty( printerModel ) )
        {
            return;
        }

        if ( printerModel.Contains( "HP ETHERNET" ) )
        {
            printerModel = printer.getInfo( "1.3.6.1.2.1.25.3.2.1.3.1" );
        }

        if ( String.IsNullOrEmpty( printerModel ) )
        {
            return;
        }

        Dictionary<String, int> dictionary = printer.walkMIB();

        lblModel.Text = "Printer Model: " + printerModel;
        lblPrinter.Text = printerName;
        hlWebServer.Text = "<br />" + printerName + " Web Server";
        hlWebServer.Target = rblPrinters.SelectedValue;
        hlWebServer.NavigateUrl = rblPrinters.SelectedValue;
        fillAndShow( dictionary );
    }

    private void fillAndShow(Dictionary<string, int> dictionary)
    {
        if ( dictionary.Count == 1 )
        {
            // assume bw printer
            lblBlackPartKey.Text = "Black Toner Cartridge";
            lblBlackPartValue.Text = dictionary.ElementAt( 0 ).Key;
            trBlack.Visible = true;
            lblBlack.Text = dictionary.ElementAt( 0 ).Value + "%";
            tblcBlack.Width = Math.Abs( Convert.ToInt32( dictionary.ElementAt( 0 ).Value ) - 6 );
            tblcBlackFiller.Width = Math.Abs( 100 - Convert.ToInt32( dictionary.ElementAt( 0 ).Value ) - 6 );
            tblrBlack.Visible = true;
        }
        else
        {
            string key;
            foreach ( KeyValuePair<String, int> pair in dictionary )
            {
                key = pair.Key.ToUpper();
                if ( key.Contains( "BLACK" ) && !key.Contains( "DRUM" ) )
                {
                    lblBlackPartKey.Text = "Black Toner Cartridge";
                    lblBlackPartValue.Text = pair.Key;
                    trBlack.Visible = true;
                    lblBlack.Text = pair.Value + "%";
                    tblcBlack.Width = Math.Abs( Convert.ToInt32( pair.Value ) - 6 );
                    tblcBlackFiller.Width = Math.Abs( 100 - Convert.ToInt32( pair.Value ) - 6 );
                    tblrBlack.Visible = true;
                }
                else if ( key.Contains( "CYAN" ) && !key.Contains( "DRUM" ) )
                {
                    lblCyanPartKey.Text = "Cyan Toner Cartridge";
                    lblCyanPartValue.Text = pair.Key;
                    trCyan.Visible = true;
                    lblCyan.Text = pair.Value + "%";
                    tblcCyan.Width = Math.Abs( Convert.ToInt32( pair.Value ) - 6 );
                    tblcCyanFiller.Width = Math.Abs( 100 - Convert.ToInt32( pair.Value ) - 6 );
                    tblrCyan.Visible = true;
                }
                else if ( key.Contains( "MAGENTA" ) && !key.Contains( "DRUM" ) )
                {
                    lblMagentaPartKey.Text = "Magenta Toner Cartridge";
                    lblMagentaPartValue.Text = pair.Key;
                    trMagenta.Visible = true;
                    lblMagenta.Text = pair.Value + "%";
                    tblcMagenta.Width = Math.Abs( Convert.ToInt32( pair.Value ) - 6 );
                    tblcMagentaFiller.Width = Math.Abs( 100 - Convert.ToInt32( pair.Value ) - 6 );
                    tblrMagenta.Visible = true;
                }
                else if ( key.Contains( "YELLOW" ) && !key.Contains( "DRUM" ) )
                {
                    lblYellowPartKey.Text = "Yellow Toner Cartridge";
                    lblYellowPartValue.Text = pair.Key;
                    trYellow.Visible = true;
                    lblYellow.Text = pair.Value + "%";
                    tblcYellow.Width = Math.Abs( Convert.ToInt32( pair.Value ) - 6 );
                    tblcYellowFiller.Width = Math.Abs( 100 - Convert.ToInt32( pair.Value ) - 6 );
                    tblrYellow.Visible = true;
                }
                else if ( key.Contains( "BLACK" ) && key.Contains( "DRUM" ) )
                {
                    lblBlackPartDrumKey.Text = "Black Drum";
                    lblBlackPartDrumValue.Text = pair.Key;
                    trBlackDrum.Visible = true;
                    lblBlackDrum.Text = pair.Value + "%";
                    tblcBlackDrum.Width = Math.Abs( Convert.ToInt32( pair.Value ) - 6 );
                    tblcBlackDrumFiller.Width = Math.Abs( 100 - Convert.ToInt32( pair.Value ) - 6 );
                    tblrBlackDrum.Visible = true;
                }
                else if ( key.Contains( "CYAN" ) && key.Contains( "DRUM" ) )
                {
                    lblCyanPartDrumKey.Text = "Cyan Drum";
                    lblCyanPartDrumValue.Text = pair.Key;
                    trCyanDrum.Visible = true;
                    lblCyanDrum.Text = pair.Value + "%";
                    tblcCyanDrum.Width = Math.Abs( Convert.ToInt32( pair.Value ) - 6 );
                    tblcCyanDrumFiller.Width = Math.Abs( 100 - Convert.ToInt32( pair.Value ) - 6 );
                    tblrCyanDrum.Visible = true;
                }
                else if ( key.Contains( "MAGENTA" ) && key.Contains( "DRUM" ) )
                {
                    lblMagentaPartDrumKey.Text = "Magenta Drum";
                    lblMagentaPartDrumValue.Text = pair.Key;
                    trMagentaDrum.Visible = true;
                    lblMagentaDrum.Text = pair.Value + "%";
                    tblcMagentaDrum.Width = Math.Abs( Convert.ToInt32( pair.Value ) - 6 );
                    tblcMagentaDrumFiller.Width = Math.Abs( 100 - Convert.ToInt32( pair.Value ) - 6 );
                    tblrMagentaDrum.Visible = true;
                }
                else if ( key.Contains( "YELLOW" ) && key.Contains( "DRUM" ) )
                {
                    lblYellowPartDrumKey.Text = "Yellow Drum";
                    lblYellowPartDrumValue.Text = pair.Key;
                    trYellowDrum.Visible = true;
                    lblYellowDrum.Text = pair.Value + "%";
                    tblcYellowDrum.Width = Math.Abs( Convert.ToInt32( pair.Value ) - 6 );
                    tblcYellowDrumFiller.Width = Math.Abs( 100 - Convert.ToInt32( pair.Value ) - 6 );
                    tblrYellowDrum.Visible = true;
                }
                else if ( key.Contains( "FUSER" ) )
                {
                    lblFuserPartKey.Text = "Fuser Kit";
                    lblFuserPartValue.Text = pair.Key;
                    trFuser.Visible = true;
                    lblFuser.Text = pair.Value + "%";
                    tblcFuser.Width = Math.Abs( Convert.ToInt32( pair.Value ) - 6 );
                    tblcFuserFiller.Width = Math.Abs( 100 - Convert.ToInt32( pair.Value ) - 6 );
                    tblrFuser.Visible = true;
                }
                else if ( key.Contains( "TRANSFER" ) )
                {
                    lblTransferPartKey.Text = "Transfer Kit";
                    lblTransferPartValue.Text = pair.Key;
                    trTransfer.Visible = true;
                    lblTransfer.Text = pair.Value + "%";
                    tblcTransfer.Width = Math.Abs( Convert.ToInt32( pair.Value ) - 6 );
                    tblcTransferFiller.Width = Math.Abs( 100 - Convert.ToInt32( pair.Value ) - 6 );
                    tblrTransfer.Visible = true;
                }
            }
        }
        mpeLevels.Show();
    }

    private void Clear()
    {
        lblCyan.Text = "";
        lblMagenta.Text = "";
        lblYellow.Text = "";
        lblBlack.Text = "";
        lblBlackDrum.Text = "";
        lblMagentaDrum.Text = "";
        lblYellowDrum.Text = "";
        lblCyanDrum.Text = "";
        lblFuser.Text = "";
        lblTransfer.Text = "";

        tblrCyan.Visible = false;
        tblrMagenta.Visible = false;
        tblrYellow.Visible = false;
        tblrBlack.Visible = false;
        tblrBlackDrum.Visible = false;
        tblrCyanDrum.Visible = false;
        tblrYellowDrum.Visible = false;
        tblrMagentaDrum.Visible = false;
        tblrTransfer.Visible = false;
        tblrFuser.Visible = false;

        trBlack.Visible = false;
        trCyan.Visible = false;
        trYellow.Visible = false;
        trMagenta.Visible = false;
        trBlackDrum.Visible = false;
        trCyanDrum.Visible = false;
        trYellowDrum.Visible = false;
        trMagentaDrum.Visible = false;
        trTransfer.Visible = false;
        trFuser.Visible = false;
    }

    #endregion Levels Button

    #region Poll Button

    protected void PollBtn_Click(object sender, EventArgs e)
    {
        List<String> problems = new List<String>();
        PrintServer server = new PrintServer( @"\\DR3PRINT" );
        PrinterObj printerobj;
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
                //if ( printer.Name.Equals( "17K-ColorMFP-04", StringComparison.OrdinalIgnoreCase ) )
                //{
                //    ;
                //}
                printerobj = new PrinterObj( printer.Name );
                if ( printerobj.Low )
                {
                    printerobj.sendLowReport();
                }
            }
            catch ( Exception ex )
            {
                problems.Add( printer.Name );
                report( DateTime.Now.ToString() + printer.Name + " was not polled report. \n" + ex.ToString() );
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

        report( "END REPORT - " + DateTime.Now.ToString() );
    }

    private static void report(string str)
    {
        //System.IO.StreamWriter fstream = new System.IO.StreamWriter( @"S:\CIWMB-INFOTECH\HelpCenter\RKOEN\PollLog.log", true );
        System.IO.StreamWriter fstream = new System.IO.StreamWriter( @"\\Iwmdocs\iwm\CIWMB-INFOTECH\HelpCenter\RKOEN\PollLog.log", true );
        fstream.WriteLine( str );
        fstream.Close();
    }

    #endregion Poll Button

    #endregion Printer

    #region Update

    protected void WebconfBtn_Click(object sender, EventArgs e)
    {
        ArrayList webconfMachines = new ArrayList();
        webconfMachines.Add( "W8-Webconf1" );
        webconfMachines.Add( "W8-Webconf2" );
        webconfMachines.Add( "W8-Webconf3" );

        ArrayList availableWebconfMachines = getAvailableComputers( webconfMachines );
        if ( availableWebconfMachines.Count == 0 )
            throw new Exception( "No available machines" );

        // Edit remote registry for autologon and restart
        Update.EnableAutoLogon( availableWebconfMachines );

        // Wait for machines to boot
        System.Threading.Thread.Sleep( 15000 );
        int machinesOnline = (getAvailableComputers( availableWebconfMachines )).Count;
        while ( machinesOnline != availableWebconfMachines.Count )
        {
            System.Threading.Thread.Sleep( 15000 );
            machinesOnline = (getAvailableComputers( availableWebconfMachines )).Count;
        }

        // Disable AutoLogon
        Update.DisableAutoLogon( availableWebconfMachines );

        // Send updater application to avaibleWebconfMachines
        Update.CopyCleaner( availableWebconfMachines );

        // Run updater on machines
        Update.RunUpdates( availableWebconfMachines );
    }

    protected void TrainingSBtn_Click(object sender, EventArgs e)
    {
        ArrayList TRMachines = new ArrayList();
        TRMachines.Add( "W8-Train01S" );
        TRMachines.Add( "W8-Train02S" );
        TRMachines.Add( "W8-Train03S" );
        TRMachines.Add( "W8-Train04S" );
        TRMachines.Add( "W8-Train05S" );
        TRMachines.Add( "W8-Train06S" );
        TRMachines.Add( "W8-Train07S" );
        TRMachines.Add( "W8-Train08S" );
        TRMachines.Add( "W8-Train09S" );
        TRMachines.Add( "W8-Train10S" );
        TRMachines.Add( "W8-Train11S" );
        TRMachines.Add( "W8-Train12S" );
        TRMachines.Add( "W8-Train13S" );

        ArrayList availableTRMachines = getAvailableComputers( TRMachines );
        if ( availableTRMachines.Count == 0 )
            throw new Exception( "No available machines" );

        // Edit remote registry for autologon and restart
        Update.EnableAutoLogon( availableTRMachines );

        // Wait for machines to boot
        System.Threading.Thread.Sleep( 15000 );
        int machinesOnline = (getAvailableComputers( availableTRMachines )).Count;
        while ( machinesOnline != availableTRMachines.Count )
        {
            System.Threading.Thread.Sleep( 5000 );
            machinesOnline = (getAvailableComputers( availableTRMachines )).Count;
        }

        // Disable AutoLogon
        Update.DisableAutoLogon( availableTRMachines );

        // Send updater application to avaibleWebconfMachines
        Update.CopyCleaner( availableTRMachines );

        // Run updater on machines
        Update.RunUpdates( availableTRMachines );
    }

    protected void TrainingNBtn_Click(object sender, EventArgs e)
    {
        ArrayList TRMachines = new ArrayList();
        TRMachines.Add( "W8-Train01N" );
        TRMachines.Add( "W8-Train02N" );
        TRMachines.Add( "W8-Train03N" );
        TRMachines.Add( "W8-Train04N" );
        TRMachines.Add( "W8-Train05N" );
        TRMachines.Add( "W8-Train06N" );
        TRMachines.Add( "W8-Train07N" );
        TRMachines.Add( "W8-Train08N" );
        TRMachines.Add( "W8-Train09N" );
        TRMachines.Add( "W8-Train10N" );
        TRMachines.Add( "W8-Train11N" );
        TRMachines.Add( "W8-Train12N" );
        TRMachines.Add( "W8-Train13N" );

        ArrayList availableTRMachines = getAvailableComputers( TRMachines );
        if ( availableTRMachines.Count == 0 )
            throw new Exception( "No available machines" );

        // Edit remote registry for autologon and restart
        Update.EnableAutoLogon( availableTRMachines );
        System.Threading.Thread.Sleep( 15000 );

        // Wait for machines to boot
        System.Threading.Thread.Sleep( 15000 );
        int machinesOnline = (getAvailableComputers( availableTRMachines )).Count;
        while ( machinesOnline != availableTRMachines.Count )
        {
            System.Threading.Thread.Sleep( 15000 );
            machinesOnline = (getAvailableComputers( availableTRMachines )).Count;
        }

        // Disable AutoLogon
        Update.DisableAutoLogon( availableTRMachines );

        // Send updater application to avaibleWebconfMachines
        //Update.CopyCleaner(availableTRMachines);

        // Run updater on machines
        Update.RunUpdates( availableTRMachines );
    }

    private ArrayList getAvailableComputers(ArrayList computerList)
    {
        ArrayList availableComputers = new ArrayList();

        System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();
        foreach ( string computer in computerList )
        {
            try
            {
                System.Net.NetworkInformation.PingReply reply = ping.Send( computer );
                if ( reply.Status == System.Net.NetworkInformation.IPStatus.Success )
                {
                    availableComputers.Add( computer );
                }
            }
            catch ( System.Net.NetworkInformation.PingException )
            {
                // machine will not be available
            }
        }

        return availableComputers;
    }

    #endregion Update

    #region Audit

    protected void servicesBtn_Click(object sender, EventArgs e)
    {
        if ( !isName() )
        {
            return;
        }
        clearViews();
        DataTable dt = new DataTable();
        dt = Audit.getServicesInformation( ComputerTextBox.Text );
        this.dvAuditGrid.DataSource = dt;
        this.dvAuditGrid.DataBind();
        this.mpe.Show();
    }

    protected void ProgramsBtn_Click(object sender, EventArgs e)
    {
        if ( !isName() )
        {
            return;
        }
        dtPrograms = Audit.getInstalledPrograms( ComputerTextBox.Text );
        string[] columns = { "Program Name", "Vendor", "Installed Date", "Uninstall String", "GUID" };
        dtPrograms = dtPrograms.DefaultView.ToTable( true, columns );
        ViewState["dtPrograms"] = dtPrograms;
        this.programsGridView.Columns[3].Visible = false;
        this.programsGridView.Columns[4].Visible = false;
        this.programsGridView.Sort( "Program Name", SortDirection.Ascending );
        this.mpePrograms.Show();
    }

    protected void computerInfoBtn_Click(object sender, EventArgs e)
    {
        if ( !isName() )
        {
            return;
        }
        clearViews();
        DataTable dt = new DataTable();
        dt = Audit.getComputerInformation( ComputerTextBox.Text );
        this.dvAuditDetails.DataSource = dt;
        this.dvAuditDetails.DataBind();
        this.mpe.Show();
    }

    protected void monitorInfoBtn_Click(object sender, EventArgs e)
    {
        if ( !isName() )
        {
            return;
        }
        clearViews();
        this.dvAuditDetails.DataBind();
        DataTable dt = new DataTable();
        dt = Audit.getMonitorInformation( ComputerTextBox.Text );

        this.dvAuditDetails.DataSource = dt;
        this.dvAuditDetails.DataBind();
        this.mpe.Show();
    }

    protected void printerInfoBtn_Click(object sender, EventArgs e)
    {
        if ( !isName() )
        {
            return;
        }
        clearViews();
        this.dvAuditDetails.DataBind();
        DataTable dt = new DataTable();
        dt = Audit.getPrinterInformation( ComputerTextBox.Text );

        this.dvAuditGrid.DataSource = dt;
        this.dvAuditGrid.DataBind();
        this.mpe.Show();
    }

    protected void IPConfigBtn_Click(object sender, EventArgs e)
    {
        if ( !isName() )
        {
            return;
        }

        clearViews();
        this.dvAuditDetails.DataBind();
        DataTable dt = new DataTable();
        dt = Audit.getIPConfigInformation( ComputerTextBox.Text );

        this.dvAuditGrid.DataSource = dt;
        this.dvAuditGrid.DataBind();
        this.mpe.Show();
    }

    protected void uninstallBtn_Click(object sender, EventArgs e)
    {
        string program, GUID;
        if ( this.programsGridView.SelectedRow == null )
            return;
        DataTable dt = (DataTable)ViewState["dtPrograms"];
        int rowIndex = programsGridView.SelectedRow.DataItemIndex;

        program = dt.Rows[rowIndex].ItemArray[3].ToString();
        GUID = dt.Rows[rowIndex].ItemArray[4].ToString();
        Audit.uninstallPrograms( ComputerTextBox.Text, program, GUID );
    }

    private void clearViews()
    {
        this.dvAuditDetails.DataSource = null;
        this.dvAuditGrid.DataSource = null;
        this.dvAuditDetails.DataBind();
        this.dvAuditGrid.DataBind();
    }

    protected void dvAuditGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DataTable dt = new DataTable();
        dt = Audit.getServicesInformation( ComputerTextBox.Text );
        this.dvAuditGrid.DataSource = dt;
        this.dvAuditGrid.PageIndex = e.NewPageIndex;
        this.dvAuditGrid.DataBind();
        this.mpe.Show();
    }

    protected void programsGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.programsGridView.PageIndex = e.NewPageIndex;
        if ( this.programsGridView.SelectedRow != null )
            this.programsGridView.SelectedRow.BackColor = this.programsGridView.BackColor;
        this.programsGridView.Sort( "Program Name", SortDirection.Ascending );
        this.programUpdatePanel.Update();
        this.mpePrograms.Show();
    }

    protected void programsGridView_SelectedIndexChanged(object sender, EventArgs e)
    {
        for ( int i = 0; i < programsGridView.PageSize; i++ )
            programsGridView.Rows[i].BackColor = this.programsGridView.BackColor;
        programsGridView.SelectedRow.BackColor = System.Drawing.ColorTranslator.FromHtml( "#A1DCF2" );
        this.programUpdatePanel.Update();
        this.mpePrograms.Show();
    }

    protected void programsGridView_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = new DataTable();
        dt = (DataTable)ViewState["dtPrograms"];
        dt.DefaultView.Sort = "Program Name";
        programsGridView.DataSource = dt;
        programsGridView.DataBind();
    }

    #endregion Audit

    #region View

    protected void EventViewerBtn_Click(object sender, EventArgs e)
    {
        if ( !isName() )
        {
            return;
        }

        string client = System.Net.Dns.GetHostEntry( Request.ServerVariables["remote_addr"] ).HostName;
        View.OpenEventViewer( client, ComputerTextBox.Text );
    }

    protected void CDriveBtn_Click(object sender, EventArgs e)
    {
        if ( !isName() )
        {
            return;
        }
        string client = System.Net.Dns.GetHostEntry( Request.ServerVariables["remote_addr"] ).HostName;
        string target = @"\\" + ComputerTextBox.Text + @"\C$";

        View.OpenCDrive( client, target );
    }

    protected void RemoteAssistBtn_Click(object sender, EventArgs e)
    {
        if ( !isName() )
        {
            return;
        }
        string client = System.Net.Dns.GetHostEntry( Request.ServerVariables["remote_addr"] ).HostName;
        string target = ComputerTextBox.Text;

        View.OpenRA( client, target );
    }

    protected void ScreenshotBtn_Click(object sender, EventArgs e)
    {
        if ( !isName() )
        {
            return;
        }
        try
        {
            System.IO.File.Copy( HttpContext.Current.Server.MapPath( "~/Screenshot.exe" ), @"\\" + ComputerTextBox.Text + @"\c$\Windows\System32\Screenshot.exe", true );
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = @"/c owexec.exe -nowait -k screenshot.exe -c " + ComputerTextBox.Text + @"""";
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            // Download the image
            String FileName = "printscrn.png";
            String FilePath = @"\\" + ComputerTextBox.Text + @"\C$\Users\Public\Pictures\";
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.ClearContent();
            response.Clear();
            response.ContentType = "image/png";
            response.AddHeader( "Content-Disposition", "attachment; filename=" + FileName + ";" );
            response.TransmitFile( FilePath + FileName );
            response.Flush();
            System.IO.File.Delete( FilePath + FileName );
            response.End();
        }
        catch
        {
            this.ScreenshotBtn.BorderColor = System.Drawing.Color.Red;
        }
    }

    #endregion View

    #region Install

    protected void JavaUpdateBtn_Click(object sender, EventArgs e)
    {
        if ( !isName() )
        {
            return;
        }

        string path = HttpContext.Current.Server.MapPath( "~/App_Code/Install/JavaUpdate.exe" );
        string target = @"\\" + ComputerTextBox.Text + @"\c$\JavaUpdate.exe";
        System.IO.File.Copy( path, target, true );

        System.Diagnostics.Process process = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        startInfo.FileName = "cmd.exe";
        startInfo.Arguments = @"/c owexec.exe -nowait -k C:\JavaUpdate.exe -c " + ComputerTextBox.Text + @"";
        process.StartInfo = startInfo;
        process.Start();

        // new thread to delete transferred file
        System.Threading.ThreadStart work = delegate
        {
            System.Threading.Thread.Sleep( 1000 * 60 * 3 ); // wait to delte file
            System.IO.File.Delete( target );
        };
        new System.Threading.Thread( work ).Start();
        ComputerTextBox.Text = "";
    }

    protected void JavaDoriisBtn_Click(object sender, EventArgs e)
    {
        if ( !isName() )
        {
            return;
        }

        string path = HttpContext.Current.Server.MapPath( "~/App_Code/Install/jre-6u81-windows-i586.exe" );
        string path2 = HttpContext.Current.Server.MapPath( "~/App_Code/Install/DoriisJava.bat" );
        string target = @"\\" + ComputerTextBox.Text + @"\c$\jre-6u81-windows-i586.exe";
        string target2 = @"\\" + ComputerTextBox.Text + @"\c$\DoriisJava.bat";
        System.IO.File.Copy( path, target, true );
        System.IO.File.Copy( path2, target2, true );

        System.Diagnostics.Process process = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        startInfo.FileName = "cmd.exe";
        startInfo.Arguments = @"/c owexec.exe -nowait -k C:\DoriisJava.bat -c " + ComputerTextBox.Text + @"";
        process.StartInfo = startInfo;
        process.Start();

        // new thread to delete transferred file
        System.Threading.ThreadStart work = delegate
        {
            System.Threading.Thread.Sleep( 1000 * 60 ); // wait to delte file
            System.IO.File.Delete( target );
        };
        new System.Threading.Thread( work ).Start();
        ComputerTextBox.Text = "";
    }

    #region Reflection

    protected void ReflectionsInstall_Click(object sender, EventArgs e)
    {
        if ( !isName() )
        {
            return;
        }
        mpeReflection.Show();
    }

    protected void btnReflectionInstall_Click(object sender, EventArgs e)
    {
        string deviceNames = getDeviceNames();

        string path = HttpContext.Current.Server.MapPath( "~/App_Code/Install/ReflectionInstaller.exe" );
        string target = @"\\" + ComputerTextBox.Text + @"\c$\ReflectionInstaller.exe";
        System.IO.File.Copy( path, target, true );

        System.Diagnostics.Process process = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        startInfo.FileName = "cmd.exe";
        startInfo.Arguments = @"/c owexec.exe -nowait -k C:\ReflectionInstaller.exe -p """ + deviceNames + @""" -c " + ComputerTextBox.Text + @"""";
        process.StartInfo = startInfo;
        process.Start();

        // new thread to delete transferred file
        System.Threading.ThreadStart work = delegate
        {
            System.Threading.Thread.Sleep( 60000 * 10 ); // wait to delte file
            System.IO.File.Delete( target );
        };
        new System.Threading.Thread( work ).Start();
        ComputerTextBox.Text = "";
    }

    private string getDeviceNames()
    {
        List<String> deviceNames = new List<String>();
        if ( device1.Text.Length > 4 )
        {
            deviceNames.Add( device1.Text );
        }
        if ( device2.Text.Length > 4 )
        {
            deviceNames.Add( device2.Text );
        }
        if ( device3.Text.Length > 4 )
        {
            deviceNames.Add( device3.Text );
        }

        return concat( deviceNames.ToArray() );
    }

    private string concat(string[] args)
    {
        StringBuilder result = new StringBuilder();
        foreach ( string arg in args )
        {
            result.Append( arg + " " );
        }

        return result.ToString().TrimEnd();
    }

    #endregion Reflection

    #endregion Install

    protected void SearchBtn_Click(object sender, EventArgs e)
    {
        System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();
        try
        {
            if ( String.IsNullOrWhiteSpace( ComputerTextBox.Text ) )
            {
                ComputerTextBox.BorderColor = System.Drawing.Color.Red;
                return;
            }
            else
            {
                System.Net.NetworkInformation.PingReply reply = ping.Send( ComputerTextBox.Text );
                if ( reply.Status == System.Net.NetworkInformation.IPStatus.Success )
                {
                    ComputerTextBox.BorderColor = System.Drawing.Color.Green;
                    pingImage.ImageUrl = "Icons/Check.ico";
                    pingImage.Width = 12;
                    pingImage.Height = 12;
                    pingImage.Visible = true;
                }
                else
                {
                    ComputerTextBox.BorderColor = System.Drawing.Color.Red;
                    pingImage.ImageUrl = "Icons/X.ico";
                    pingImage.Width = 12;
                    pingImage.Height = 12;
                    pingImage.Visible = true;
                }
            }
        }
        catch ( System.Net.NetworkInformation.PingException )
        {
            ComputerTextBox.BorderColor = System.Drawing.Color.Red;
            pingImage.ImageUrl = "Icons/X.ico";
            pingImage.Width = 12;
            pingImage.Height = 12;
            pingImage.Visible = true;
        }
    }

    private bool isName()
    {
        if ( ComputerTextBox.Text.Equals( "" ) )
        {
            ComputerTextBox.BorderColor = System.Drawing.Color.Red;
            return false;
        }

        return true;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {
        return (from c in AD_Computers where c.StartsWith( prefixText, StringComparison.CurrentCultureIgnoreCase ) select c).Take( count ).ToArray();
    }

    private void metrics()
    {
        try
        {
            string client = System.Net.Dns.GetHostEntry( Request.ServerVariables["remote_addr"] ).HostName;
            using ( SqlConnection connection = new SqlConnection( "Data Source=W8-RKOEN;Initial Catalog=HomePageReporting;User ID=sa;Password=p@ssw0rd" ) )
            {
                try
                {
                    connection.Open();
                    try
                    {
                        using ( SqlCommand command = new SqlCommand( "INSERT INTO CommandCenter VALUES(@ComputerName, @Time)", connection ) )
                        {
                            command.Parameters.Add( new SqlParameter( "ComputerName", client.Split( '.' )[0] ) );
                            command.Parameters.Add( new SqlParameter( "Time", DateTime.Now ) );

                            command.ExecuteNonQuery();
                        }
                    }
                    catch { }
                }
                catch { }
            }
        }
        catch { }
    }
}