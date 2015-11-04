using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ntegrity.Models.Diff
{
    public class EnumTypeDiff
    {
        public bool HasChanged = false;
        public string Name;

        public List<AttributeData> AddedAttributes = new List<AttributeData>();
        public List<AttributeData> RemovedAttributes = new List<AttributeData>();

        public List<FieldData> AddedFields = new List<FieldData>();
        public List<FieldData> RemovedFields = new List<FieldData>();

        public List<MethodData> AddedMethods = new List<MethodData>();
        public List<MethodData> RemovedMethods = new List<MethodData>();

        public EnumTypeDiff(EnumTypeData beforeEnum, EnumTypeData afterEnum)
        {
            if (beforeEnum.Name != afterEnum.Name)
            {
                throw new NtegrityException("Attempted to diff two different Enums!");
            }
            Name = beforeEnum.Name;

            GetAddedAndRemovedAttributes(beforeEnum, afterEnum);
            GetAddedAndRemovedFields(beforeEnum, afterEnum);
            GetAddedAndRemovedMethods(beforeEnum, afterEnum);
        }

        private void GetAddedAndRemovedAttributes(EnumTypeData beforeEnum, EnumTypeData afterEnum)
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

        private void GetAddedAndRemovedFields(EnumTypeData beforeEnum, EnumTypeData afterEnum)
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

        private void GetAddedAndRemovedMethods(EnumTypeData beforeEnum, EnumTypeData afterEnum)
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
