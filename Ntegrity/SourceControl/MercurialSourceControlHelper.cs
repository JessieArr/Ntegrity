using System.Diagnostics;
using Ntegrity.Models;

namespace Ntegrity.SourceControl
{
	public class MercurialSourceControlHelper : ISourceControlHelper
	{
		public string GetCurrentBranch()
		{
			Process p = new Process();

			p.StartInfo.UseShellExecute = false;
			p.StartInfo.RedirectStandardOutput = true;
			p.StartInfo.FileName = "hg";
			p.StartInfo.Arguments = "branch";
            p.StartInfo.CreateNoWindow = true;
            p.Start();

			string output = p.StandardOutput.ReadToEnd();
			p.WaitForExit();

			return output;
		}
	}
}