﻿using System.Diagnostics;
using System.IO;

namespace Ntegrity
{
	public class GitSourceControlHelper : ISourceControlHelper
	{
		public string GetCurrentBranch()
		{
			Process p = new Process();

			p.StartInfo.UseShellExecute = false;
			p.StartInfo.RedirectStandardOutput = true;
			p.StartInfo.FileName = "git";
            p.StartInfo.Arguments = "branch";
		    p.StartInfo.CreateNoWindow = true;
		    p.Start();

			string output = p.StandardOutput.ReadToEnd();
			p.WaitForExit();

			return output;
		}
	}
}