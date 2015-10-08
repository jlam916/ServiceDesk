using SnmpSharpNet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SNMP_Printer
/// </summary>
public class SNMP_Printer
{
    public string printerName;
    public readonly string printerOID = "1.3.6.1.2.1.43.11.1.1";
    public readonly string part = "1.3.6.1.2.1.43.11.1.1.6";
    public readonly string levelMax = "1.3.6.1.2.1.43.11.1.1.8";
    public readonly string levelCur = "1.3.6.1.2.1.43.11.1.1.9";

    public SNMP_Printer()
    {
    }

    public SNMP_Printer(string printerName)
    {
        this.printerName = printerName;
    }

    public string getModel()
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
        pdu.VbList.Add( "1.3.6.1.2.1.1.1.0" ); //sysDescr

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

    public Dictionary<String, int> walkMIB()
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

        // remove info I dont care about
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

        int count = dictionary.Count / 3;
        string tempPart;
        double level;
        Dictionary<String, int> results = new Dictionary<String, int>();
        for ( int i = 1; i <= count; i++ )
        {
            try
            {
                tempPart = dictionary[part + ".1." + i];
                level = (Convert.ToDouble( dictionary[levelCur + ".1." + i] ) / Convert.ToDouble( dictionary[levelMax + ".1." + i] )) * 100;
                results.Add( tempPart, Convert.ToInt32( level ) );
            }
            catch { }
        }
        return results;
    }
}