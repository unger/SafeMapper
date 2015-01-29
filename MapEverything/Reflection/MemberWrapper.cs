namespace MapEverything.Reflection
{
    using System;
    using System.Reflection;

    using Fasterflect;

    public enum MemberType
    {
        Undefined, Property, Field, StringIndexer    
    }

    public class MemberWrapper
    {
        public MemberWrapper(MemberInfo member) 
            : this(member.Name, member)
        {
        }
        
        public MemberWrapper(string name, MemberInfo member)
        {
            this.Name = name;
            this.Member = member;
            this.MemberType = MemberType.Undefined;
            if (member is PropertyInfo || member is FieldInfo)
            {
                this.Type = member.Type();
            }
            else
            {
                var method = member as MethodInfo;
                if (method != null)
                {
                    this.Type = method.ReturnType;
                }
            }

            if (member is PropertyInfo)
            {
                var indexParams = (member as PropertyInfo).GetIndexParameters();
                if (indexParams.Length == 0)
                {
                    this.MemberType = MemberType.Property;
                }
                else if (indexParams.Length == 1 && indexParams[0].ParameterType == typeof(string))
                {
                    this.MemberType = MemberType.StringIndexer;
                }

                this.CanRead = (member as PropertyInfo).CanRead;
                this.CanWrite = (member as PropertyInfo).CanWrite;
            }
            else
            {
                if (member is FieldInfo)
                {
                    this.MemberType = MemberType.Field;
                }

                this.CanRead = true;
                this.CanWrite = true;
            }
        }

        public MemberType MemberType { get; private set; }

        public string Name { get; private set; }

        public Type Type { get; private set; }

        public bool CanRead { get; private set; }

        public bool CanWrite { get; private set; }

        public MemberInfo Member { get; private set; }
    }
}
