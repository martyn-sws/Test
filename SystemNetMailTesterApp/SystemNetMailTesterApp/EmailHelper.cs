using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SystemNetMailTesterApp
{
    public class EmailHelper : MailMessage
    {
        #region Variables

        string _documentText;
        readonly string _host;
        private readonly string _overrideToAddress;

        #endregion

        #region Constructor

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public EmailHelper(string mailTo)
        {
            if (mailTo != null)
                _overrideToAddress = mailTo;
        }

        /// <summary>
        /// Must supply host name
        /// </summary>
        public EmailHelper(string host, string mailTo)
        {
            _host = host;
            if (mailTo != null)
                _overrideToAddress = mailTo;
        }

        #endregion

        #region Send

        /// <summary>
        /// Send the configured message
        /// </summary>
        /// <returns></returns>
        public void Send()
        {
            Send(false);
        }

        public void Send(bool isBodyHtml)
        {
            IsBodyHtml = isBodyHtml;

            // If in debug/testing mode, override the to address
            //AND Clear any CC or BCC as we don't want to accidentally send emails to anyone else
            if (!string.IsNullOrEmpty(_overrideToAddress))
            {
                To.Clear();
                To.Add(new MailAddress(_overrideToAddress));
                CC.Clear();
                Bcc.Clear();
            }

            var sender = new SmtpClient(_host);
            sender.Send(this);
        }

        public void Send(List<EmbeddedImage> embeddedImages)
        {
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(Body, null, MediaTypeNames.Text.Html);

            List<Stream> imageStreams = new List<Stream>();
            foreach (var image in embeddedImages)
            {
                // Not ideal loading the image like this, but for some reason I can't use the stream from an image saved within the resources.resx file
                //		(It just won't attach to / embed within the email properly)
                // Also, this is how it is loaded within SendGridEmailService
                var imageStream = System.Reflection.Assembly.LoadFrom(image.AssemblyName).GetManifestResourceStream(image.ManifestResourceName);
                htmlView.LinkedResources.Add(GetEmbedImageLinkedResource(image.ContentId, imageStream));
                imageStreams.Add(imageStream);
            }

            AlternateViews.Add(htmlView);

            Send(true);

            foreach (var stream in imageStreams)
            {
                stream.Dispose();
            }
        }
        public LinkedResource GetEmbedImageLinkedResource(string contentId, Stream imageContentStream)
        {
            LinkedResource img = new LinkedResource(imageContentStream, MediaTypeNames.Image.Jpeg);
            img.ContentId = contentId;
            return img;
        }

        public struct EmbeddedImage
        {
            public string AssemblyName;
            public string ManifestResourceName;
            public string ContentId;

            public EmbeddedImage(string assemblyName, string manifestResourceName, string contentId)
            {
                AssemblyName = assemblyName;
                ManifestResourceName = manifestResourceName;
                ContentId = contentId;
            }
        }

        private string GetAddresses(MailAddressCollection addresses)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var address in addresses)
            {
                if (sb.Length > 0)
                    sb.Append(", ");

                sb.Append(address.Address);
            }

            return sb.ToString();
        }

        public override string ToString()
        {
            return String.Format("To: {0}\nCC: {1}\nBCC: {2}\nSubject: {3}\n\n{4}",
                GetAddresses(To),
                GetAddresses(CC),
                GetAddresses(Bcc),
                Subject,
                Body);
        }

        /// <summary>
        /// Send message specifying to, from and subject
        /// </summary>
        /// <param name="to"></param>
        /// <param name="from"></param>
        /// <param name="subject"></param>
        /// <returns></returns>
        public void Send(string to, string from, string subject)
        {
            Send(to, from, subject, false);
        }

        public void Send(string to, string from, string subject, bool isBodyHtml)
        {
            Send(to, from, subject, _documentText, isBodyHtml);
        }

        /// <summary>
        /// Send message specifying to, from, subject and message body.
        /// </summary>
        /// <returns></returns>
        public void Send(string to, string from, string subject, string body)
        {
            Send(to, from, subject, body, false);
        }

        public void Send(string to, string from, string subject, string body, bool isBodyHtml)
        {
            To.Add(to);

            if (!string.IsNullOrEmpty(from))
                From = new MailAddress(from);

            Subject = subject;
            Body = body;
            Send(isBodyHtml);
        }

        /// <summary>
        /// Send message specifying to, from, cc, subject and message body.
        /// </summary>
        /// <returns></returns>
        public void Send(string to, string from, string cc, string subject, string body)
        {
            Send(to, from, cc, subject, body, false);
        }

        public void Send(string to, string from, string cc, string subject, string body, bool isBodyHtml)
        {
            if (!String.IsNullOrEmpty(to))
            {
                To.Add(new MailAddress((!string.IsNullOrEmpty(_overrideToAddress)
                                            ? _overrideToAddress
                                            : to)));
            }

            // If the from address has not been specified, we may be in a test environment where it has not been set up so use the override
            if (!string.IsNullOrEmpty(from))
                From = new MailAddress(from);
            else if (!string.IsNullOrEmpty(_overrideToAddress))
                From = new MailAddress(_overrideToAddress);

            if (!String.IsNullOrEmpty(cc))
            {
                CC.Add(new MailAddress((!string.IsNullOrEmpty(_overrideToAddress)
                                            ? _overrideToAddress
                                            : cc)));
            }

            Subject = subject;
            Body = body;
            Send(isBodyHtml);
        }

        public void Send(string to, string from, string cc, string bcc, string subject, string body, bool isBodyHtml)
        {
            if (!String.IsNullOrEmpty(bcc))
            {
                Bcc.Add(new MailAddress(bcc));
            }
            Send(to, from, cc, subject, body, isBodyHtml);
        }

        public void Send(string to, string from, string cc, string cc2, string bcc, string subject, string body, bool isBodyHtml)
        {
            if (!String.IsNullOrEmpty(cc2))
            {
                CC.Add(new MailAddress(cc2));
            }

            if (!String.IsNullOrEmpty(bcc))
            {
                Bcc.Add(new MailAddress(bcc));
            }

            Send(to, from, cc, subject, body, isBodyHtml);
        }

        #endregion

        #region Template Support

        /// <summary>
        /// Open a templated email text file and use it as the message text
        /// </summary>
        /// <param name="path"></param>
        public void LoadTemplate(string path)
        {
            StreamReader reader = null;

            try
            {
                reader = new StreamReader(path);
                _documentText = reader.ReadToEnd();
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        /// <summary>
        /// Set the template directly
        /// </summary>
        public string Template
        {
            set { _documentText = value; }
        }

        /// <summary>
        /// Replace a tag in the loaded template with the given text.
        /// </summary>
        /// <returns></returns>
        public bool ReplaceTag(string tagIndicator, string tagName, string text)
        {
            if (_documentText.Contains(tagIndicator + tagName + tagIndicator))
            {
                _documentText = _documentText.Replace(tagIndicator + tagName + tagIndicator, text);
                return true;
            }
            return false;
        }

        #endregion

        public static bool IsValidEmailAddress(object value)
        {
            string str = Convert.ToString(value, CultureInfo.CurrentCulture);
            if (string.IsNullOrEmpty(str))
            {
                return true;
            }
            Regex regex =
                new Regex(@"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$");
            Match match = regex.Match(str);
            return ((match.Success && (match.Index == 0)) && (match.Length == str.Length));
        }

    }
}
