using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;
using Renci.SshNet;
using System.IO;
using System.Threading;
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

        private SshClient sshClient;

        protected override void ProcessRecord()
		{
            //WriteObject(server);
            if (server == null)
            {
                server = serverPrompt();
                String username = usernamePrompt();
                String password = passwordPrompt();
                WriteObject(password);
                //sshClient = new SshClient(server, username, password);
                //sshClient.Connect();
                //if (sshClient.IsConnected)
                //{
                //    processOutput("Successfully connected to " + server);
                //    sshPrompt();
                //}
                
            }
            else
            {
                String username = usernamePrompt();
                String password = passwordPrompt();
                sshClient = new SshClient(server, username, password);
                sshClient.Connect();
                if (sshClient.IsConnected)
                {
                    processOutput("Successfully connected to " + server);
                    sshPrompt();
                }
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
            var input = this.InvokeCommand.InvokeScript("Read-Host -assecurestring");
            System.Security.SecureString securePassword = (System.Security.SecureString)input.First().BaseObject;

            return password;
        }
        private void processOutput(string output)
        {
            WriteObject(output);
        }
        private void sshPrompt()
        {
            while (sshClient.IsConnected)
            {
                var input = this.InvokeCommand.InvokeScript("Read-Host \"#\"");
                String commandString = input.First().BaseObject.ToString();
                var command = sshClient.RunCommand(commandString);
                command.Execute();
                var output = command.Result;
                processOutput(output);
            }
          
        }
	}
}