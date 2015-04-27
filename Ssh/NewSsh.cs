using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;
using Renci.SshNet;
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
            if (server.Equals(""))
            {
                serverPrompt();
            }
            else 
            {
                WriteObject("The specified server is: " + server);
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
	}
}