using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ntegrity.Models.Diff.Interfaces;
using Ntegrity.Models.Interfaces;

namespace Ntegrity.Models.Diff
{
    public class MethodDiff : IMethodDiff
    {
        public bool HasChanged { get; private set; }
        public string MethodSignature { get; }

        public List<IAttributeData> AddedAttributes { get; }
        public List<IAttributeData> RemovedAttributes { get; }

        public MethodDiff(IMethodData oldField, IMethodData newField)
        {
            AddedAttributes = new List<IAttributeData>();
            RemovedAttributes = new List<IAttributeData>();

            if (oldField.MethodSignature != newField.MethodSignature)
            {
                throw new NtegrityException("Attempted to diff two different Enums!");
            }
            MethodSignature = oldField.MethodSignature;

            GetAddedAndRemovedAttributes(oldField, newField);
        }

        private void GetAddedAndRemovedAttributes(IMethodData beforeField, IMethodData afterField)
        {
            foreach (var oldAttribute in beforeField.AttributeData)
            {
                if (afterField.AttributeData.All(x => x.Name != oldAttribute.Name))
                {
                    RemovedAttributes.Add(oldAttribute);
                    HasChanged = true;
                }
            }
            foreach (var newAttribute in afterField.AttributeData)
            {
                if (beforeField.AttributeData.All(x => x.Name != newAttribute.Name))
                {
                    AddedAttributes.Add(newAttribute);
                    HasChanged = true;
                }
            }
        }

		public override string ToString()
		{
			return ToString(new NtegrityOutputSettings());
		}

		private const string AttributesPrefix = "ATTRIBUTES:";
		public string ToString(NtegrityOutputSettings outputSettings)
		{
			var returnString = MethodSignature + Environment.NewLine;
			// ATTRIBUTES
			returnString += "\t\t" + AttributesPrefix + Environment.NewLine;
			foreach (var removedAttribute in RemovedAttributes)
			{
				returnString += "-\t\t\t" + removedAttribute.Name + Environment.NewLine;
			}
			returnString += Environment.NewLine;

			foreach (var addedAttribute in AddedAttributes)
			{
				returnString += "+\t\t\t" + addedAttribute.Name + Environment.NewLine;
			}

			return returnString;
		}
	}
}
