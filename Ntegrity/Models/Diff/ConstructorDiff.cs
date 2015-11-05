using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ntegrity.Models.Diff.Interfaces;
using Ntegrity.Models.Interfaces;

namespace Ntegrity.Models.Diff
{
    public class ConstructorDiff : IConstructorDiff
    {
        public bool HasChanged { get; private set; }
        public string ConstructorSignature { get; }

        public List<IAttributeData> AddedAttributes { get; }
        public List<IAttributeData> RemovedAttributes { get; }

        public ConstructorDiff(IConstructorData oldField, IConstructorData newField)
        {
            AddedAttributes = new List<IAttributeData>();
            RemovedAttributes = new List<IAttributeData>();

            if (oldField.ConstructorSignature != newField.ConstructorSignature)
            {
                throw new NtegrityException("Attempted to diff two different Enums!");
            }
            ConstructorSignature = oldField.ConstructorSignature;

            GetAddedAndRemovedAttributes(oldField, newField);
        }

        private void GetAddedAndRemovedAttributes(IConstructorData beforeField, IConstructorData afterField)
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
    }
}
