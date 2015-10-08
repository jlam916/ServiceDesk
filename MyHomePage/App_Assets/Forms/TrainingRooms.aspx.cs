using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class App_Assets_Forms_TrainingRooms : System.Web.UI.Page
{
    private readonly string NORTH_URL = "http://10.255.0.15/cgi/admin";
    private readonly string NORTH_URL_SET_PRESET = "http://10.255.0.15/cgi/admin?op=1";
    private readonly string NORTH_URL_VIEW_PRESET = "http://10.255.0.15/cgi/admin?op=v";
    private readonly string SOUTH_URL = "http://10.255.0.14/cgi/admin";
    private readonly string SOUTH_URL_SET_PRESET = "http://10.255.0.14/cgi/admin?op=1";
    private readonly string SOUTH_URL_VIEW_PRESET = "http://10.255.0.14/cgi/admin?op=v";

    private string northNetwork = null;
    private string southNetwork = null;
    private bool scriptOverride = false;

    public string activeConnectionNorth = null;
    public string activeConnectionSouth = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        string today = DateTime.Now.ToShortDateString();
        linkSouth.OnClientClick = "window.open('" + "http://epanet.ca.gov/Rooms/dayMtgs.asp?ROOMID=36&BLDGID=15&VW=DAY&DATE=" + today + "&CLONEID=\')";
        linkNorth.OnClientClick = "window.open('" + "http://epanet.ca.gov/Rooms/dayMtgs.asp?ROOMID=35&BLDGID=15&VW=DAY&DATE=" + today + "&CLONEID=\')";
        if ( !IsPostBack )
        {
            // Check North
            Thread threadNorth = new Thread( () =>
            {
                using ( System.Windows.Forms.WebBrowser webBrowser1 = new System.Windows.Forms.WebBrowser() )
                {
                    webBrowser1.DocumentCompleted += loadTRSPage;
                    webBrowser1.Navigate( NORTH_URL );
                    System.Windows.Forms.Application.Run();
                }
            } );

            // Check South
            Thread threadSouth = new Thread( () =>
            {
                using ( System.Windows.Forms.WebBrowser webBrowser1 = new System.Windows.Forms.WebBrowser() )
                {
                    webBrowser1.DocumentCompleted += loadTRSPage;
                    webBrowser1.Navigate( SOUTH_URL );
                    System.Windows.Forms.Application.Run();
                }
            } );

            threadNorth.Name = "North";
            threadSouth.Name = "South";
            threadNorth.SetApartmentState( ApartmentState.STA );
            threadSouth.SetApartmentState( ApartmentState.STA );
            threadNorth.Start();
            threadSouth.Start();
            threadNorth.Join();
            threadSouth.Join();
        }
    }
    
    protected void setNorth_Click(object sender, EventArgs e)
    {
        northNetwork = (sender as Button).Text.ToUpper();

        // Do work!
        Thread threadNorth = new Thread( () =>
        {
            using ( System.Windows.Forms.WebBrowser webBrowser1 = new System.Windows.Forms.WebBrowser() )
            {
                webBrowser1.DocumentCompleted += loadPresetsPage;
                webBrowser1.Navigate( NORTH_URL_SET_PRESET );
                System.Windows.Forms.Application.Run();
            }
        } );
        threadNorth.Name = "North";
        threadNorth.SetApartmentState( ApartmentState.STA );
        threadNorth.Start();
        threadNorth.Join();
    }

    protected void setSouth_Click(object sender, EventArgs e)
    {
        southNetwork = (sender as Button).Text.ToUpper();
        // Do work!
        Thread thread = new Thread( () =>
        {
            using ( System.Windows.Forms.WebBrowser webBrowser1 = new System.Windows.Forms.WebBrowser() )
            {
                webBrowser1.DocumentCompleted += loadPresetsPage;
                webBrowser1.Navigate( SOUTH_URL_SET_PRESET );
                System.Windows.Forms.Application.Run();
            }
        } );
        thread.Name = "South";
        thread.SetApartmentState( ApartmentState.STA );
        thread.Start();
        thread.Join();
    }

    private void loadPresetsPage(object sender, System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e)
    {
        System.Windows.Forms.WebBrowser webBrowser1 = sender as System.Windows.Forms.WebBrowser;
        webBrowser1.DocumentCompleted -= loadPresetsPage;
        webBrowser1.DocumentCompleted += changePresets;
        webBrowser1.ProgressChanged += javaScriptOverride;

        if ( Thread.CurrentThread.Name.Equals( "NORTH", StringComparison.OrdinalIgnoreCase ) )
        {
            webBrowser1.Navigate( NORTH_URL_SET_PRESET );
        }
        else
        {
            webBrowser1.Navigate( SOUTH_URL_SET_PRESET );
        }
        
    }

    private void changePresets(object sender, System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e)
    {
        System.Windows.Forms.WebBrowser webBrowser1 = sender as System.Windows.Forms.WebBrowser;
        webBrowser1.DocumentCompleted -= changePresets;
        String network = null;

        if ( Thread.CurrentThread.Name.Equals( "NORTH", StringComparison.OrdinalIgnoreCase ) )
        {
            if ( !String.IsNullOrWhiteSpace(northNetwork) )
            {
                network = northNetwork;
            }
        }
        else
        {
            if ( !String.IsNullOrWhiteSpace( southNetwork ) )
            {
                network = southNetwork;
            }
        }

        if ( !String.IsNullOrWhiteSpace( network ) )
        {
            switch ( network )
            {
                case "ARB":
                    webBrowser1.Document.GetElementsByTagName( "option" )[0].SetAttribute( "selected", "selected" );
                    break;
                case "OEHHA":
                    webBrowser1.Document.GetElementsByTagName( "option" )[1].SetAttribute( "selected", "selected" );
                    break;
                case "CALRECYCLE":
                    webBrowser1.Document.GetElementsByTagName( "option" )[2].SetAttribute( "selected", "selected" );
                    break;
                case "DTSC":
                    webBrowser1.Document.GetElementsByTagName( "option" )[3].SetAttribute( "selected", "selected" );
                    break;
                case "DPR":
                    webBrowser1.Document.GetElementsByTagName( "option" )[4].SetAttribute( "selected", "selected" );
                    break;
                case "SWRCB":
                    webBrowser1.Document.GetElementsByTagName( "option" )[5].SetAttribute( "selected", "selected" );
                    break;
            }

            webBrowser1.Document.GetElementsByTagName( "input" )[1].InvokeMember( "click" );
        }
    }

    private void javaScriptOverride(object sender, System.Windows.Forms.WebBrowserProgressChangedEventArgs e)
    {
        System.Windows.Forms.WebBrowser webBrowser1 = sender as System.Windows.Forms.WebBrowser;

        if ( scriptOverride )
        {
            webBrowser1.ProgressChanged -= javaScriptOverride;
            return;
        }
        if ( webBrowser1.ReadyState == System.Windows.Forms.WebBrowserReadyState.Complete )
        {
            System.Windows.Forms.HtmlElement head = webBrowser1.Document.GetElementsByTagName( "head" )[0];
            System.Windows.Forms.HtmlElement scriptEl = webBrowser1.Document.CreateElement( "script" );
            mshtml.IHTMLScriptElement element = (mshtml.IHTMLScriptElement)scriptEl.DomElement;
            string alertBlocker = "doConfirm = function () { }";
            element.text = alertBlocker;
            head.AppendChild( scriptEl );
        }
    }


    #region VIEW_PRESET
    private void loadTRSPage(object sender, System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e)
    {
        System.Windows.Forms.WebBrowser webBrowser1 = sender as System.Windows.Forms.WebBrowser;
        String thread = Thread.CurrentThread.Name.ToUpper();

        if ( webBrowser1.Document.GetElementById( "uid" ) != null )
        {
            webBrowser1.Document.GetElementById( "uid" ).SetAttribute( "value", "Recycle" );
            webBrowser1.Document.GetElementById( "pwd" ).SetAttribute( "value", "recycle" );
            webBrowser1.Document.GetElementsByTagName( "input" )[2].InvokeMember( "click" );
            Thread.Sleep( 200 );

            webBrowser1.DocumentCompleted -= loadTRSPage;
            webBrowser1.DocumentCompleted += getPatches;

            if ( thread.Equals( "NORTH" ) )
            {
                webBrowser1.Navigate( NORTH_URL_VIEW_PRESET );
            }
            else
            {
                webBrowser1.Navigate( SOUTH_URL_VIEW_PRESET );
            }
            
        }
        else
        {
            webBrowser1.DocumentCompleted -= loadTRSPage;
            webBrowser1.DocumentCompleted += getPatches;

            if ( thread.Equals( "NORTH" ) )
            {
                webBrowser1.Navigate( NORTH_URL_VIEW_PRESET );
            }
            else
            {
                webBrowser1.Navigate( SOUTH_URL_VIEW_PRESET );
            }
        }
    }

    private void getPatches(object sender, System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e)
    {
        System.Windows.Forms.WebBrowser webBrowser1 = sender as System.Windows.Forms.WebBrowser;
        string active = webBrowser1.Document.GetElementsByTagName( "DIV" )[21].InnerText.ToUpper();
        string thread = Thread.CurrentThread.Name.ToUpper();

        if ( thread.Equals( "NORTH" ) )
        {
            switch ( active )
            {
                case "ARB":
                    arbNorth.CssClass = "btn btn-success";
                    activeConnectionNorth = arbNorth.ClientID;
                    break;

                case "CALRECYCLE":
                    recycleNorth.CssClass = "btn btn-success";
                    activeConnectionNorth = recycleNorth.ClientID;
                    break;

                case "DPR":
                    dprNorth.CssClass = "btn btn-success";
                    activeConnectionNorth = dprNorth.ClientID;
                    break;

                case "DTSC":
                    dtscNorth.CssClass = "btn btn-success";
                    activeConnectionNorth = dtscNorth.ClientID;
                    break;
                case "OEHHA":
                    oehhaNorth.CssClass = "btn btn-success";
                    activeConnectionNorth = oehhaNorth.ClientID;
                    break;
                case "SWRCB":
                    swrcbNorth.CssClass = "btn btn-success";
                    activeConnectionNorth = swrcbNorth.ClientID;
                    break;
            }
        }
        else
        {
            switch ( active )
            {
                case "ARB":
                    arbSouth.CssClass = "btn btn-success";
                    activeConnectionSouth = arbSouth.ClientID;
                    break;

                case "CALRECYCLE":
                    recycleSouth.CssClass = "btn btn-success";
                    activeConnectionSouth = recycleSouth.ClientID;
                    break;

                case "DPR":
                    dprSouth.CssClass = "btn btn-success";
                    activeConnectionSouth = dprSouth.ClientID;
                    break;

                case "DTSC":
                    dtscSouth.CssClass = "btn btn-success";
                    activeConnectionSouth = dtscSouth.ClientID;
                    break;
                case "OEHHA":
                    oehhaSouth.CssClass = "btn btn-success";
                    activeConnectionSouth = oehhaSouth.ClientID;
                    break;
                case "SWRCB":
                    swrcbSouth.CssClass = "btn btn-success";
                    activeConnectionNorth = swrcbSouth.ClientID;
                    break;
            }
        }
        System.Windows.Forms.Application.ExitThread();
    }
    #endregion
}