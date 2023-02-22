// See https://aka.ms/new-console-template for more information
using SystemNetMailTesterApp;

Console.WriteLine("Software Solved <> System.Net.Mail tester");
Console.Write("Enter SMTP server: ");
string server = Console.ReadLine();
Console.Write("Enter email address you want to send a email to: ");
string emailTo = Console.ReadLine();
string subject = "Software Solved System.Net.Mail tester email";
string body = "This email was sent from the Software Solved Sytem.Net.Mail tester application at sender DateTime: " + DateTime.Now.ToString();
string sender = "support@softwaresolved.com";

try
{
    EmailHelper email = new EmailHelper(server, emailTo);

    email.From = new System.Net.Mail.MailAddress("support@softwaresolved.com");
    email.To.Add(emailTo);
    email.Subject = subject;
    email.Body = body;
    email.Send();

} catch (Exception ex)
{
    Console.WriteLine("!!ERROR!!");
    Console.WriteLine("!--Message--!");
    Console.WriteLine(ex.Message);
    if (ex.InnerException != null) {
        Console.WriteLine("!--Inner Exception--!");
        Console.WriteLine(ex.InnerException.Message);
    }
    Console.WriteLine("!--StackTrace--!");
    Console.WriteLine(ex.StackTrace);
}
finally
{
    Console.WriteLine("--Email--");
    Console.Write("SMTP: ");
    Console.WriteLine(server);
    Console.Write("Receiver: ");
    Console.WriteLine(emailTo);
    Console.Write("Sender: ");
    Console.WriteLine(sender);
    Console.Write("Subject: ");
    Console.WriteLine(subject);
    Console.Write("Body: ");
    Console.WriteLine(body);
}