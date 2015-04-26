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
        [Parameter(Position = 0)]
        
        public string Server
        {
            get { return server; }
            set { server = value; }
        }
        private string server;
		protected override void ProcessRecord()
		{
            if (Ser)
            WriteObject("Please enter your username.");
            String username = this.InvokeCommand.InvokeScript("Read-Host").ToString();
            //SshClient sshClient = new SshClient(server,)
		}
	}
}