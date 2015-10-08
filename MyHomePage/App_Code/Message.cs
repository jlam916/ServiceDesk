using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

public class Message
    {
        private List<String> to, replyTo, cc, bcc;
        private string subject, body,  sender;

        public Message()
        {
            to = new List<String>();
            replyTo = new List<String>();
            cc = new List<String>();
            bcc = new List<String>();
            subject = body = "";
            mailServer = "smtpmail.local";
            smtpClient = new SmtpClient();
            basicCredential = new NetworkCredential( "HCTools@calrecycle.ca.gov", "Branch-2468", "smtpmail.local" );
            message = new MailMessage();
            fromAddress = new MailAddress( "HCTools@calrecycle.ca.gov" );

            smtpClient.Host = mailServer;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = basicCredential;
            smtpClient.Timeout = timeout;

            // Send to self to save a copy in outlook
            bcc.Add( "HCTools@calrecycle.ca.gov" );
        }

        public void send()
        {
            message.From = fromAddress;
            message.Subject = subject;
            message.Body = body;
            to.ForEach( recipient => message.To.Add( recipient ) );
            message.IsBodyHtml = true;

            if ( replyTo.Count > 0 ) { replyTo.ForEach( recipient => message.ReplyToList.Add( recipient ) ); }
            if ( cc.Count > 0 ) { cc.ForEach( recipient => message.CC.Add( recipient ) ); }
            if ( bcc.Count > 0 ) { bcc.ForEach( recipient => message.Bcc.Add( recipient ) ); }

            smtpClient.Send( message );
        }

        #region Proerties
        public string Sender
        {
            get { return sender; }
            set { sender = value; }
        }

        public string Body
        {
            get { return body; }
            set { body = value; }
        }

        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }

        public void setTo(params string[] emailAddr)
        {
            emailAddr.ToList().ForEach( a => to.Add( a ) );
        }

        public void setCC(params string[] emailAddr)
        {
            emailAddr.ToList().ForEach( a => cc.Add( a ) );
        }

        public void setBCC(params string[] emailAddr)
        {
            emailAddr.ToList().ForEach( a => bcc.Add( a ) );
        }

        public void setReplyTo(params string[] emailAddr)
        {
            emailAddr.ToList().ForEach( a => replyTo.Add( a ) );
        }

        private string mailServer;
        private SmtpClient smtpClient;
        private NetworkCredential basicCredential;
        private MailAddress fromAddress;
        private MailMessage message;
        private int timeout = (60 * 2 * 1000); // 2 mins
        #endregion

        public void sendException(Exception ex)
        {
            message.From = fromAddress;
            message.Subject = "ALERT - Exception";

            StringBuilder body = new StringBuilder();
            
            if ( ex.Data.Count > 0 )
            {
                body.Append( "<b><u>Data</b></u></br>" );
                foreach ( System.Collections.DictionaryEntry de in ex.Data )
                {
                    body.Append( "Key: " + de.Key.ToString() + " | Value: " + de.Value  + "</br>");
                }
            }
            body.Append( "<b><u>HelpLink</b></u></br>" );
            body.Append( ex.HelpLink + "</br></br>" );
            body.Append( "<b><u>Inner Exception</b></u></br>" );
            body.Append( ex.InnerException + "</br></br>" );
            body.Append( "<b><u>Message</b></u></br>" );
            body.Append( ex.Message + "</br></br>" );
            body.Append( "<b><u>Source</b></u></br>" );
            body.Append( ex.Source + "</br></br>" );
            body.Append( "<b><u>Target Site</b></u></br>" );
            body.Append( ex.TargetSite + "</br></br>" );
            body.Append( "<b><u>Stack Trace</b></u></br>" );
            body.Append( ex.StackTrace + "</br></br>" );

            message.Body = body.ToString();
            message.To.Add( "Randell.Koen@calrecycle.ca.gov" );
            message.IsBodyHtml = true;

            smtpClient.Send( message );
        }
    }