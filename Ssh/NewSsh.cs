using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;
using Renci.SshNet;
using System.IO;
namespace NewSsh
{
	[Cmdlet(VerbsCommon.New, "Ssh")]
	public class NewSsh : PSCmdlet
	{
        //Declare the first parameter (by position, not a flag) and specify help message.
        [Parameter(
            Position = 0,
            HelpMessage = "Server to connect to via ssh."
        )]
        //The public string here is the actual parameter
        public String Server
        {
        get {return server;}
        set {server = value;}
        }
        private string server;
        protected override void ProcessRecord()
		{
            //WriteObject(server);
            if (server == null)
            {
                server = serverPrompt();
                String username = usernamePrompt();
                String password = passwordPrompt();
                SshClient sshClient = new SshClient(server, username, password);
                sshClient.Connect();
                ShellStream sshStream = sshClient.CreateShellStream("dumb", 80, 24, 800,600, 1024);
                StreamWriter output = new StreamWriter(sshStream);
                StreamReader input = new StreamReader(sshStream);
                output.AutoFlush = true;

                while (sshStream.Length.Equals(0))
                {
                    s
                }
            }
            else
            {

                usernamePrompt();
            }
		}
        private String usernamePrompt()
        {
            WriteObject("Please enter your username.");
            var input = this.InvokeCommand.InvokeScript("Read-Host");
            String username = input.First().BaseObject.ToString();
            return username;
        }
        private String serverPrompt()
        {
            WriteObject("Please enter the server name or ip address.");
            var input = this.InvokeCommand.InvokeScript("Read-Host");
            String server = input.First().BaseObject.ToString();
            return server;
        }
        private String passwordPrompt()
        {
            WriteObject("Please enter your password.");
            var input = this.InvokeCommand.InvokeScript("Read-Host -asecurestring");
            String password = input.First().BaseObject.ToString();
            return password;
        }
        private void processOutput(string output)
        {
            WriteObject(output);
        }
	}
}