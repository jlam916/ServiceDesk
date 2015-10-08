using SnmpSharpNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// Summary description for PrinterObj
/// </summary>
public class PrinterObj
{
    private string printerName, printerModel, blackTonerModel, cyanTonerModel, yellowTonerModel, magentaTonerModel, fuserModel, transferModel,
                   blackDrumModel, cyanDrumModel, yellowDrumModel, magentaDrumModel;

    private int blackPercentage, cyanPercentage, yellowPercentage, magentaPercentage, fuserPercentage, transferPercentage, threshold,
                blackDrumPercentage, cyanDrumPercentage, yellowDrumPercentage, magentaDrumPercentage;

    private bool low;

    public readonly string printerOID = "1.3.6.1.2.1.43.11.1.1";
    public readonly string part = "1.3.6.1.2.1.43.11.1.1.6";
    public readonly string levelMax = "1.3.6.1.2.1.43.11.1.1.8";
    public readonly string levelCur = "1.3.6.1.2.1.43.11.1.1.9";

    public PrinterObj()
    {
    }

    public PrinterObj(string printerName)
    {
        threshold = 3;
        blackPercentage = cyanPercentage = yellowPercentage = magentaPercentage = fuserPercentage = transferPercentage = -1;
        blackDrumPercentage = cyanDrumPercentage = yellowDrumPercentage = magentaDrumPercentage = -1;
        Dictionary<String, String> dictionary;
        this.printerName = printerName;
        getModel();

        if ( String.IsNullOrEmpty( printerModel ) )
        {
            throw new MissingFieldException();
        }

        if ( printerModel.Contains( "HP ETHERNET" ) )
        {
            printerModel = getInfo( "1.3.6.1.2.1.25.3.2.1.3.1" );
        }

        if ( String.IsNullOrEmpty( printerModel ) )
        {
            throw new MissingFieldException();
        }
        dictionary = getData();
        remove( ref dictionary );
        setData( dictionary );
        low = isLow();
    }

    private bool isLow()
    {
        return (blackPercentage < threshold || (cyanPercentage < threshold && cyanPercentage != -1) ||
               (yellowPercentage < threshold && yellowPercentage != -1) || (magentaPercentage < threshold && magentaPercentage != -1) ||
               (blackDrumPercentage < threshold && blackDrumPercentage != -1) || (cyanDrumPercentage < threshold && cyanDrumPercentage != -1) ||
               (yellowDrumPercentage < threshold && yellowDrumPercentage != -1) || (magentaDrumPercentage < threshold && magentaDrumPercentage != -1) ||
               (fuserPercentage == 0 && fuserPercentage != -1) || (transferPercentage == 0 && transferPercentage != -1));
    }

    /// <summary>
    /// Will get the rest of the variables, parts and percentages
    /// </summary>
    /// <param name="dictionary"></param>
    private void setData(Dictionary<string, string> dictionary)
    {
        int count = dictionary.Count / 3;
        string tempPart;
        double level;
        for ( int i = 1; i <= count; i++ )
        {
            try
            {
                tempPart = dictionary[part + ".1." + i].ToLower().Replace("\0", String.Empty);
                level = (Convert.ToDouble( dictionary[levelCur + ".1." + i] ) / Convert.ToDouble( dictionary[levelMax + ".1." + i] )) * 100;

                if ( tempPart.Contains( "black" ) && !tempPart.Contains( "drum" ) )
                {
                    blackTonerModel = tempPart;
                    blackPercentage = Convert.ToInt32( level );
                }
                else if ( tempPart.Contains( "cyan" ) && !tempPart.Contains( "drum" ) )
                {
                    cyanTonerModel = tempPart;
                    cyanPercentage = Convert.ToInt32( level );
                }
                else if ( tempPart.Contains( "yellow" ) && !tempPart.Contains( "drum" ) )
                {
                    yellowTonerModel = tempPart;
                    yellowPercentage = Convert.ToInt32( level );
                }
                else if ( tempPart.Contains( "magenta" ) && !tempPart.Contains( "drum" ) )
                {
                    magentaTonerModel = tempPart;
                    magentaPercentage = Convert.ToInt32( level );
                }
                else if ( tempPart.Contains( "black" ) && tempPart.Contains( "drum" ) )
                {
                    blackDrumModel = tempPart;
                    blackDrumPercentage = Convert.ToInt32( level );
                }
                else if ( tempPart.Contains( "cyan" ) && tempPart.Contains( "drum" ) )
                {
                    cyanDrumModel = tempPart;
                    cyanDrumPercentage = Convert.ToInt32( level );
                }
                else if ( tempPart.Contains( "yellow" ) && tempPart.Contains( "drum" ) )
                {
                    yellowTonerModel = tempPart;
                    yellowDrumPercentage = Convert.ToInt32( level );
                }
                else if ( tempPart.Contains( "magenta" ) && tempPart.Contains( "drum" ) )
                {
                    magentaTonerModel = tempPart;
                    magentaDrumPercentage = Convert.ToInt32( level );
                }
                else if ( tempPart.Contains( "fuser" ) )
                {
                    fuserModel = tempPart;
                    fuserPercentage = Convert.ToInt32( level );
                }
                else if ( tempPart.Contains( "transfer" ) )
                {
                    transferModel = tempPart;
                    transferPercentage = Convert.ToInt32( level );
                }
                else if ( tempPart.Contains( "feeder" ) )
                {
                    // ignore
                }
                else
                {
                    // Will assume just a bw printer
                    blackTonerModel = tempPart;
                    blackPercentage = Convert.ToInt32( level );
                }
            }
            catch { }
        }
    }

    /// <summary>
    /// Will get the rest of the variables, parts and percentages
    /// </summary>
    private Dictionary<String, String> getData()
    {
        Dictionary<String, String> dictionary = new Dictionary<String, String>();
        OctetString community = new OctetString( "public" );
        AgentParameters param = new AgentParameters( community );
        param.Version = SnmpVersion.Ver1;
        IpAddress agent = new IpAddress( printerName );

        UdpTarget target = new UdpTarget( (System.Net.IPAddress)agent, 161, 2000, 1 );
        Oid rootOid = new Oid( printerOID ); // ifDescr
        Oid lastOid = (Oid)rootOid.Clone();
        Pdu pdu = new Pdu( PduType.GetNext );

        while ( lastOid != null )
        {
            // When Pdu class is first constructed, RequestId is set to a random value
            // that needs to be incremented on subsequent requests made using the
            // same instance of the Pdu class.
            if ( pdu.RequestId != 0 )
            {
                pdu.RequestId += 1;
            }
            // Clear Oids from the Pdu class.
            pdu.VbList.Clear();
            // Initialize request PDU with the last retrieved Oid
            pdu.VbList.Add( lastOid );
            // Make SNMP request
            SnmpV1Packet result = null;
            try
            {
                result = (SnmpV1Packet)target.Request( pdu, param );
            }
            catch
            {
                return null;
            }

            // If result is null then agent didn't reply or we couldn't parse the reply.
            if ( result != null )
            {
                // ErrorStatus other then 0 is an error returned by
                // the Agent - see SnmpConstants for error definitions
                if ( result.Pdu.ErrorStatus != 0 )
                {
                    // agent reported an error with the request
                    Console.WriteLine( "Error in SNMP reply. Error {0} index {1}",
                        result.Pdu.ErrorStatus,
                        result.Pdu.ErrorIndex );
                    lastOid = null;
                    break;
                }
                else
                {
                    // Walk through returned variable bindings
                    foreach ( Vb v in result.Pdu.VbList )
                    {
                        // Check that retrieved Oid is "child" of the root OID
                        if ( rootOid.IsRootOf( v.Oid ) )
                        {
                            dictionary.Add( v.Oid.ToString(), v.Value.ToString() );
                            lastOid = v.Oid;
                        }
                        else
                        {
                            lastOid = null;
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine( "No response received from SNMP agent." );
            }
        }
        target.Close();

        return dictionary;
    }

    /// <summary>
    /// Will remove the OIDs im not interested in
    /// </summary>
    /// <param name="dictionary"></param>
    private void remove(ref Dictionary<string, string> dictionary)
    {
        List<String> remove = new List<String>();
        foreach ( KeyValuePair<String, String> pair in dictionary )
        {
            try
            {
                if ( !(pair.Key.Contains( part ) || pair.Key.Contains( levelCur ) || pair.Key.Contains( levelMax )) )
                {
                    remove.Add( pair.Key );
                    continue;
                }
                else if ( Convert.ToInt32( pair.Value ) < 0 )
                {
                    // look for invalid values returned in levles
                    remove.Add( pair.Key );
                    continue;
                }
            }
            catch { }
        }

        foreach ( String item in remove )
        {
            dictionary.Remove( item );
        }
    }

    /// <summary>
    /// Gets the model of the printer requested
    /// </summary>
    private void getModel()
    {
        // SNMP community name
        OctetString community = new OctetString( "public" );

        // Define agent parameters class
        AgentParameters param = new AgentParameters( community );
        // Set SNMP version to 1 (or 2)
        param.Version = SnmpVersion.Ver1;
        // Construct the agent address object
        // IpAddress class is easy to use here because
        //  it will try to resolve constructor parameter if it doesn't
        //  parse to an IP address
        IpAddress agent = new IpAddress( printerName );

        // Construct target
        UdpTarget target = new UdpTarget( (System.Net.IPAddress)agent, 161, 2000, 3 );

        // Pdu class used for all requests
        Pdu pdu = new Pdu( PduType.Get );
        pdu.VbList.Add( "1.3.6.1.2.1.1.1.0" ); //sysDescr

        // Make SNMP request
        SnmpV1Packet result = null;
        try
        {
            result = (SnmpV1Packet)target.Request( pdu, param );
        }
        catch
        {
        }
        // If result is null then agent didn't reply or we couldn't parse the reply.
        if ( result != null )
        {
            // ErrorStatus other then 0 is an error returned by
            // the Agent - see SnmpConstants for error definitions
            if ( result.Pdu.ErrorStatus != 0 )
            {
                // agent reported an error with the request
                Console.WriteLine( "Error in SNMP reply. Error {0} index {1}",
                    result.Pdu.ErrorStatus,
                    result.Pdu.ErrorIndex );
            }
            else
            {
                // Reply variables are returned in the same order as they were added
                //  to the VbList
                printerModel = result.Pdu.VbList[0].Value.ToString();
            }
        }
        else
        {
            Console.WriteLine( "No response received from SNMP agent." );
        }
        target.Close();
    }

    public string getInfo(string OID)
    {
        string returnString = "";

        // SNMP community name
        OctetString community = new OctetString( "public" );

        // Define agent parameters class
        AgentParameters param = new AgentParameters( community );
        // Set SNMP version to 1 (or 2)
        param.Version = SnmpVersion.Ver1;
        // Construct the agent address object
        // IpAddress class is easy to use here because
        //  it will try to resolve constructor parameter if it doesn't
        //  parse to an IP address
        IpAddress agent = new IpAddress( printerName );

        // Construct target
        UdpTarget target = new UdpTarget( (System.Net.IPAddress)agent, 161, 2000, 3 );

        // Pdu class used for all requests
        Pdu pdu = new Pdu( PduType.Get );
        pdu.VbList.Add( OID );

        // Make SNMP request
        SnmpV1Packet result;
        try
        {
            result = (SnmpV1Packet)target.Request( pdu, param );
        }
        catch
        {
            return null;
        }
        // If result is null then agent didn't reply or we couldn't parse the reply.
        if ( result != null )
        {
            // ErrorStatus other then 0 is an error returned by
            // the Agent - see SnmpConstants for error definitions
            if ( result.Pdu.ErrorStatus != 0 )
            {
                // agent reported an error with the request
                Console.WriteLine( "Error in SNMP reply. Error {0} index {1}",
                    result.Pdu.ErrorStatus,
                    result.Pdu.ErrorIndex );
            }
            else
            {
                // Reply variables are returned in the same order as they were added
                //  to the VbList
                returnString = result.Pdu.VbList[0].Value.ToString();
            }
        }
        else
        {
            Console.WriteLine( "No response received from SNMP agent." );
        }
        target.Close();
        return returnString;
    }

    /// <summary>
    /// Sends email to Track-It if a leve is below 2%
    /// </summary>
    public void sendLowReport()
    {
        if ( !low ) return;
        int count = 0; // verify not sending blank msg
        
        // build message
        StringBuilder body = new StringBuilder();
        body.Append( @"<b><u>Low Toner Alert for Printer " + printerName + " (" + printerModel + @")</u></b>" );
        body.Append( "<ul>" );
        if ( blackPercentage < threshold && blackPercentage != -1 )
        {
            body.Append( "<li>Black @ " + blackPercentage + "% (" + blackTonerModel + ")</li>" );
            count++;
        }
        if ( cyanPercentage < threshold && cyanPercentage != -1 )
        {
            body.Append( "<li>Cyan @ " + cyanPercentage + "% (" + cyanTonerModel + ")</li>" );
            count++;
        }
        if ( yellowPercentage < threshold && yellowPercentage != -1 )
        {
            body.Append( "<li>Yellow @ " + yellowPercentage + "% (" + yellowTonerModel + ")</li>" );
            count++;
        }
        if ( magentaPercentage < threshold && magentaPercentage != -1 )
        {
            body.Append( "<li>Magenta @ " + magentaPercentage + "% (" + magentaTonerModel + ")</li>" );
            count++;
        }
        if ( blackDrumPercentage < threshold && blackDrumPercentage != -1 )
        {
            body.Append( "<li>Black Drum @ " + blackDrumPercentage + "% (" + blackDrumModel + ")</li>" );
            count++;
        }
        if ( cyanDrumPercentage < threshold && cyanDrumPercentage != -1 )
        {
            body.Append( "<li>Cyan Drum @ " + cyanDrumPercentage + "% (" + cyanDrumModel + ")</li>" );
            count++;
        }
        if ( yellowDrumPercentage < threshold && yellowDrumPercentage != -1 )
        {
            body.Append( "<ul><li>Yellow Drum @ " + yellowDrumPercentage + "% (" + yellowDrumModel + ")</li>" );
            count++;
        }
        if ( magentaDrumPercentage < threshold && magentaDrumPercentage != -1 )
        {
            body.Append( "<li>Magenta Drum @ " + magentaDrumPercentage + "% (" + magentaDrumModel + ")</li>" );
            count++;
        }
        if ( fuserPercentage == 0 && fuserPercentage != -1 )
        {
            body.Append( "<li>Fuser Kit@ " + fuserPercentage + "% (" + fuserModel + ")</li>" );
            count++;
        }
        if ( transferPercentage == 0 && transferPercentage != -1 ){
            body.Append( "<li>Transfer Kit @ " + transferPercentage + "% (" + transferModel + ")</li>" );
            count++;
        }

        body.Append( "<li>https://" + printerName + "</li></ul>");

        if ( count > 0 )
        {
            //Message msg = new Message();
            //msg.setTo( "HelpCenter@calrecycle.ca.gov" );
            //msg.setSubject( "ALERT - Low Toner - " + printerName );
            //msg.setMessage( body.ToString() );
            //msg.send();
        }
    }

    private static void report(string str)
    {
        System.IO.StreamWriter fstream = new System.IO.StreamWriter( @"C:\log.log", true );
        fstream.WriteLine( str );
        fstream.Close();
    }

    #region Properties

    public bool Low
    {
        get { return low; }
    }

    public string BlackTonerModel
    {
        get { return blackTonerModel; }
    }

    public string CyanTonerModel
    {
        get { return cyanTonerModel; }
    }

    public string YellowTonerModel
    {
        get { return yellowTonerModel; }
    }

    public string MagentaTonerModel
    {
        get { return magentaTonerModel; }
    }

    public string FuserModel
    {
        get { return fuserModel; }
    }

    public string TransferModel
    {
        get { return transferModel; }
    }

    public string PrinterModel
    {
        get { return printerModel; }
    }

    public string PrinterName
    {
        get { return printerName; }
    }

    public int BlackPercentage
    {
        get { return blackPercentage; }
    }

    public int CyanPercentage
    {
        get { return cyanPercentage; }
    }

    public int YellowPercentage
    {
        get { return yellowPercentage; }
    }

    public int MagentaPercentage
    {
        get { return magentaPercentage; }
    }

    public int BlackDrumPercentage
    {
        get { return blackDrumPercentage; }
    }

    public int CyanDrumPercentage
    {
        get { return cyanDrumPercentage; }
    }

    public int YellowDrumPercentage
    {
        get { return yellowDrumPercentage; }
    }

    public int MagentaDrumPercentage
    {
        get { return magentaDrumPercentage; }
    }

    public int FuserPercentage
    {
        get { return fuserPercentage; }
    }

    public int TransferPercentage
    {
        get { return transferPercentage; }
    }

    #endregion Properties
}