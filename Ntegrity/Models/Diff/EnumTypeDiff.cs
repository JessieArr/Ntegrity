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
		public List<IFieldDiff> ModifiedFields { get; }

		public List<IMethodData> AddedMethods { get; }
        public List<IMethodData> RemovedMethods { get; }
		public List<IMethodDiff> ModifiedMethods { get; }

		public EnumTypeDiff(IEnumTypeData beforeEnum, IEnumTypeData afterEnum)
        {
            AddedAttributes = new List<IAttributeData>();
            RemovedAttributes = new List<IAttributeData>();

            AddedFields = new List<IFieldData>();
			RemovedFields = new List<IFieldData>();
			ModifiedFields = new List<IFieldDiff>();

			AddedMethods = new List<IMethodData>();
            RemovedMethods = new List<IMethodData>();
			ModifiedMethods = new List<IMethodDiff>();

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
			foreach (var oldField in beforeEnum.FieldData)
			{
				var matchingNewField = afterEnum.FieldData.FirstOrDefault(
					x => x.FieldSignature == oldField.FieldSignature);
				if (matchingNewField != null)
				{
					var diff = new FieldDiff(oldField, matchingNewField);
					if (!diff.HasChanged)
					{
						continue;
					}
					ModifiedFields.Add(diff);
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
			foreach (var oldMethod in beforeEnum.MethodData)
			{
				var matchingNewField = afterEnum.MethodData.FirstOrDefault(
					x => x.MethodSignature == oldMethod.MethodSignature);
				if (matchingNewField != null)
				{
					var diff = new MethodDiff(oldMethod, matchingNewField);
					if (!diff.HasChanged)
					{
						continue;
					}
                    ModifiedMethods.Add(diff);
					HasChanged = true;
				}
			}
		}

        public override string ToString()
        {
            return ToString(new NtegrityOutputSettings());
        }

		private const string AttributesPrefix = "ATTRIBUTES:";
		private const string FieldsPrefix = "FIELDS:";
		private const string MethodsPrefix = "METHODS:";
		public string ToString(NtegrityOutputSettings outputSettings)
		{
			var returnString = "\t" + Name + Environment.NewLine;
			// ATTRIBUTES
			returnString += "\t" + AttributesPrefix + Environment.NewLine;
			if (RemovedAttributes.Any())
			{
				foreach (var removedAttribute in RemovedAttributes)
				{
					returnString += "-\t\t" + removedAttribute.Name + Environment.NewLine;
				}
				returnString += Environment.NewLine;
			}

			if (RemovedAttributes.Any())
			{
				foreach (var addedAttribute in AddedAttributes)
				{
					returnString += "+\t\t" + addedAttribute.Name + Environment.NewLine;
				}
				returnString += Environment.NewLine;
			}

			// FIELDS
			returnString += "\t" + FieldsPrefix + Environment.NewLine;
			if (RemovedFields.Any())
			{
				foreach (var removedField in RemovedFields)
				{
					returnString += "-\t\t" + removedField.FieldSignature + Environment.NewLine;
				}
				returnString += Environment.NewLine;
			}

			if (AddedFields.Any())
			{
				foreach (var addedField in AddedFields)
				{
					returnString += "+\t\t" + addedField.FieldSignature + Environment.NewLine;
				}
				returnString += Environment.NewLine;
			}

			if (ModifiedFields.Any())
			{
				foreach (var modifiedField in ModifiedFields)
				{
					returnString += "^\t\t" + modifiedField.ToString() + Environment.NewLine;
				}
				returnString += Environment.NewLine;
			}

			// STRUCTS
			returnString += "\t" + MethodsPrefix + Environment.NewLine;
			if (RemovedMethods.Any())
			{
				foreach (var removedMethod in RemovedMethods)
				{
					returnString += "-\t\t" + removedMethod.MethodSignature + Environment.NewLine;
				}
				returnString += Environment.NewLine;
			}

			if (AddedMethods.Any())
			{
				foreach (var addedMethod in AddedMethods)
				{
					returnString += "+\t\t" + addedMethod.MethodSignature + Environment.NewLine;
				}
				returnString += Environment.NewLine;
			}

			if (ModifiedMethods.Any())
			{
				foreach (var modifiedMethod in ModifiedMethods)
				{
					returnString += "^\t\t" + modifiedMethod.ToString() + Environment.NewLine;
				}
			}

			return returnString;
		}
	}
}
