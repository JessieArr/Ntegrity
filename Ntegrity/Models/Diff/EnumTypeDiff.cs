using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ntegrity.Models.Diff.Interfaces;
using Ntegrity.Models.Interfaces;

namespace Ntegrity.Models.Diff
{
    public class EnumTypeDiff : IEnumTypeDiff
    {
        public bool HasChanged { get; private set; }
        public string Name { get; }

        public List<IAttributeData> AddedAttributes { get;  }
        public List<IAttributeData> RemovedAttributes { get; }

        public List<IFieldData> AddedFields { get; }
        public List<IFieldData> RemovedFields { get; }

        public List<IMethodData> AddedMethods { get; }
        public List<IMethodData> RemovedMethods { get; }

        public EnumTypeDiff(IEnumTypeData beforeEnum, IEnumTypeData afterEnum)
        {
            AddedAttributes = new List<IAttributeData>();
            RemovedAttributes = new List<IAttributeData>();

            AddedFields = new List<IFieldData>();
            RemovedFields = new List<IFieldData>();

            AddedMethods = new List<IMethodData>();
            RemovedMethods = new List<IMethodData>();

            if (beforeEnum.Name != afterEnum.Name)
            {
                throw new NtegrityException("Attempted to diff two different Enums!");
            }
            Name = beforeEnum.Name;

            GetAddedAndRemovedAttributes(beforeEnum, afterEnum);
            GetAddedAndRemovedFields(beforeEnum, afterEnum);
            GetAddedAndRemovedMethods(beforeEnum, afterEnum);
        }

        private void GetAddedAndRemovedAttributes(IEnumTypeData beforeEnum, IEnumTypeData afterEnum)
        {
            foreach (var oldAttribute in beforeEnum.AttributeData)
            {
                if (afterEnum.AttributeData.All(x => x.Name != oldAttribute.Name))
                {
                    RemovedAttributes.Add(oldAttribute);
                    HasChanged = true;
                }
            }
            foreach (var newAttribute in afterEnum.AttributeData)
            {
                if (beforeEnum.AttributeData.All(x => x.Name != newAttribute.Name))
                {
                    AddedAttributes.Add(newAttribute);
                    HasChanged = true;
                }
            }
        }

        private void GetAddedAndRemovedFields(IEnumTypeData beforeEnum, IEnumTypeData afterEnum)
        {
            foreach (var oldField in beforeEnum.FieldData)
            {
                if (afterEnum.FieldData.All(x => x.FieldSignature != oldField.FieldSignature))
                {
                    RemovedFields.Add(oldField);
                    HasChanged = true;
                }
            }
            foreach (var newField in afterEnum.FieldData)
            {
                if (beforeEnum.FieldData.All(x => x.FieldSignature != newField.FieldSignature))
                {
                    AddedFields.Add(newField);
                    HasChanged = true;
                }
            }
        }

        private void GetAddedAndRemovedMethods(IEnumTypeData beforeEnum, IEnumTypeData afterEnum)
        {
            foreach (var oldMethod in beforeEnum.MethodData)
            {
                if (afterEnum.MethodData.All(x => x.MethodSignature != oldMethod.MethodSignature))
                {
                    RemovedMethods.Add(oldMethod);
                    HasChanged = true;
                }
            }
            foreach (var newMethod in afterEnum.MethodData)
            {
                if (beforeEnum.MethodData.All(x => x.MethodSignature != newMethod.MethodSignature))
                {
                    AddedMethods.Add(newMethod);
                    HasChanged = true;
                }
            }
        }

        public override string ToString()
        {
            return ToString(new NtegrityOutputSettings());
        }

        public string ToString(NtegrityOutputSettings outputSettings)
        {
            return Name;
        }
    }
}
