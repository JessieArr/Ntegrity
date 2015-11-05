using System;
using System.Collections.Generic;
using System.Reflection;
using Ntegrity.Models.Reflection;
using Ntegrity.Models.Reflection.Interfaces;

namespace Ntegrity.Models
{
    public class PropertyData
    {
        public readonly string PropertySignature;
        public readonly AccessLevelEnum GetterAccessLevel;
        public readonly bool HasGetter;
        public readonly AccessLevelEnum SetterAccessLevel;
        public readonly bool HasSetter;
        public readonly List<AttributeData> AttributeData = new List<AttributeData>();

        public PropertyData(IPropertyInfoWrapper propertyInfo)
        {
            PropertySignature = propertyInfo.ToString();

            var getter = propertyInfo.GetGetMethod(true);
            HasGetter = getter != null;
            if (HasGetter)
            {
                if (getter.IsPrivate)
                {
                    GetterAccessLevel = AccessLevelEnum.Private;
                }
                if (getter.IsFamily)
                {
                    GetterAccessLevel = AccessLevelEnum.Protected;
                }
                if (getter.IsAssembly)
                {
                    GetterAccessLevel = AccessLevelEnum.Internal;
                }
                if (getter.IsPublic)
                {
                    GetterAccessLevel = AccessLevelEnum.Public;
                }
            }

            var setter = propertyInfo.GetSetMethod(true);
            HasSetter = setter != null;
            if (HasSetter)
            {
                if (setter.IsPrivate)
                {
                    SetterAccessLevel = AccessLevelEnum.Private;
                }
                if (setter.IsFamily)
                {
                    SetterAccessLevel = AccessLevelEnum.Protected;
                }
                if (setter.IsAssembly)
                {
                    SetterAccessLevel = AccessLevelEnum.Internal;
                }
                if (setter.IsPublic)
                {
                    SetterAccessLevel = AccessLevelEnum.Public;
                }
            }

            var attributes = propertyInfo.GetCustomAttributes();
            foreach (var attribute in attributes)
            {
                AttributeData.Add(new AttributeData(attribute));
            }
        }

        public PropertyData(string propertyString)
        {
            var sanitizedMethodInfo = propertyString.Replace("\t\t", "");
            var lines = sanitizedMethodInfo.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            for (var i = 0; i < lines.Length - 1; i++)
            {
                var attributeName = lines[i].Replace("[", "");
                attributeName = attributeName.Replace("]", "");
                AttributeData.Add(new AttributeData(attributeName));
            }

            var lastLine = lines[lines.Length - 1];
            PropertySignature = lastLine;
            var lastLineParts = lastLine.Split('}');
            lastLineParts = lastLineParts[0].Split('{');
            var getterAndSetter = lastLineParts[1];
            var splitGetterAndSetter = getterAndSetter.Split(';');
            var getter = splitGetterAndSetter[0].Trim();
            var setter = splitGetterAndSetter[1].Trim();

            switch (getter)
            {
                case "public get":
                    GetterAccessLevel = AccessLevelEnum.Public;
                    break;
                case "private get":
                    GetterAccessLevel = AccessLevelEnum.Private;
                    break;
                case "internal get":
                    GetterAccessLevel = AccessLevelEnum.Internal;
                    break;
                case "protected get":
                    GetterAccessLevel = AccessLevelEnum.Protected;
                    break;
            }

            switch (setter)
            {
                case "public set":
                    SetterAccessLevel = AccessLevelEnum.Public;
                    break;
                case "private set":
                    SetterAccessLevel = AccessLevelEnum.Private;
                    break;
                case "internal set":
                    SetterAccessLevel = AccessLevelEnum.Internal;
                    break;
                case "protected set":
                    SetterAccessLevel = AccessLevelEnum.Protected;
                    break;
            }
        }

        public new string ToString()
        {
            return ToString(new NtegrityOutputSettings());
        }

        public string ToString(NtegrityOutputSettings outputSettings)
        {
            var returnString = "";

            if (AttributeData.Count > 0)
            {
                foreach (var attribute in AttributeData)
                {
                    if (!outputSettings.ShowCompilerAttributes)
                    {
                        if (attribute.IsCompilerGenerated)
                        {
                            continue;
                        }
                    }

                    returnString += outputSettings.MemberPrefix + "[" + attribute.Name + "]" + Environment.NewLine;
                }
            }
            var accessorString = "{ ";
            if (HasGetter && GetterAccessLevel.HasAvailabilityEqualToOrGreaterThan(
                    outputSettings.ShowTypesAtOrAboveAccessLevel))
            {
                accessorString += GetterAccessLevel.GetKeywordFromEnum() + " get; ";
            }
            if (HasSetter && SetterAccessLevel.HasAvailabilityEqualToOrGreaterThan(
                    outputSettings.ShowTypesAtOrAboveAccessLevel))
            {
                accessorString += SetterAccessLevel.GetKeywordFromEnum() + " set; ";
            }
            accessorString += "}";

            return returnString + outputSettings.MemberPrefix + PropertySignature + " " + accessorString;
        }
    }
}