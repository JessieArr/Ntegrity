using Ntegrity.SourceControl;

namespace Ntegrity
{
	public class Ntegrity
	{
		private ISourceControlHelper _SourceControlHelper;
		public ISourceControlHelper SourceControlHelper
		{
			get
			{
				if (_SourceControlHelper == null)
				{
					throw new NtegrityException("This Ntegrity instance has no SourceControlHelper specified. Please provide the approprite SourceControlTypeEnum in the object's constructor, or set the SourceControlHelper property to an object that implements the ISourceControlHelper interface.");
				}
				return _SourceControlHelper;
			}
			set { _SourceControlHelper = value; }
		}

		public Ntegrity()
		{
			
		}

		public Ntegrity(SourceControlTypeEnum sourceControlType)
		{
			switch (sourceControlType)
			{
				case SourceControlTypeEnum.Git:
					_SourceControlHelper = new GitSourceControlHelper();
					return;
				case SourceControlTypeEnum.Mercurial:
					_SourceControlHelper = new MercurialSourceControlHelper();
					return;
			}
		}
	}
}
