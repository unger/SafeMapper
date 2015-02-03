namespace MapEverything.Reflection
{
    using System;
    using System.Reflection;

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

            this.Type = ReflectionUtils.GetMemberType(member);

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
                    this.CanWrite = !(member as FieldInfo).IsInitOnly;
                }

                this.CanRead = true;
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
