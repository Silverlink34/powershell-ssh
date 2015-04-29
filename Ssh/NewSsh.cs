using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;
using Renci.SshNet;
using System.IO;
using System.Threading;
using System.Security;
using System.Runtime.InteropServices;
namespace NewSsh
{
	[Cmdlet(VerbsCommon.New, "Ssh")]
	public class NewSsh : PSCmdlet
	{
        //Positional parameters ex. mycommand param1 param2
        [Parameter(
            Position = 0,
            HelpMessage = "Specify a server to connect to via ssh."
        )]
        //The public string here is the actual parameter
        public String Server
        {
            get {return server;}
            set {server = value;}
        }
        //this is the variable that contains parameter's data
        private string server;

        //Flag parameters ex. mycommand -myparameter
        [Parameter(HelpMessage = "Specify path to private key file.")]
        public String PrivateKey
        {
            get { return privateKey; }
            set { privateKey = value; }
        }
        private string privateKey;

        //global variables
        private SshClient sshClient;

        protected override void ProcessRecord()
		{
            if (server == null)
            {
                server = serverPrompt();
                if (privateKey == null)
                {
                    passwordAuthenticate();
                }
                else
                {
                    privateKeyAuthenticate();
                }
            }
            else
            {
                if (privateKey == null)
                {
                    passwordAuthenticate();
                }
                else
                {
                    privateKeyAuthenticate();
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
            SecureString securePassword = (System.Security.SecureString)input.First().BaseObject;
            String password = SecureStringToString(securePassword);
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
        String SecureStringToString(SecureString value)
        {
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }
        private void passwordAuthenticate()
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
        private void privateKeyAuthenticate()
        {
            String username = usernamePrompt();
            sshClient = new SshClient(server, username, 
                new PrivateKeyFile(File.OpenRead(privateKey)));
            sshClient.Connect();
            if (sshClient.IsConnected)
            {
                processOutput("Successfully connected to " + server);
                sshPrompt();
            }
        }
	}
}