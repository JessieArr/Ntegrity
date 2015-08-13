using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

	public class TypeInterfaceData
	{
		public readonly TypeEnum Type;
		public readonly List<ConstructorData> ConstructorData;

		public TypeInterfaceData(Type typeToAnalyze)
		{
			if (typeToAnalyze.IsClass)
			{
				Type = TypeEnum.Class;
			}
			if (typeToAnalyze.IsInterface)
			{
				Type = TypeEnum.Interface;
			}
			if (Type == null)
			{
				throw new NtegrityException("Unable to determine data type for type: " + typeToAnalyze.AssemblyQualifiedName);
			}
		}
	}

	public enum TypeEnum
	{
		Class,
		Interface
	}

	public class ConstructorData
	{
		public string ConstructorSignature { get; set; }
		public List<AttributeData> AttributeData { get; set; } 
	}

	public class AttributeData
	{
		public string Name { get; set; }
	}
}
