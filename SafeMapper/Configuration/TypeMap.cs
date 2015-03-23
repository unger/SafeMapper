namespace SafeMapper.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

    using SafeMapper.Reflection;
    using SafeMapper.Utils;

    public class TypeMap<TFrom, TTo> : ITypeMap<TFrom, TTo>
    {
        private readonly List<MemberMap> memberMaps = new List<MemberMap>();

        private MemberInfo getIndexer;

        private MemberInfo setIndexer;

        public TypeMapping GetTypeMapping()
        {
            return new TypeMapping(typeof(TFrom), typeof(TTo), this.memberMaps);
        }

        public void MapGetIndexer<TFromMember>(Expression<Func<TFrom, string, TFromMember>> fromIndexer)
        {
            this.getIndexer = ExpressionHelper.GetMember(fromIndexer);
        }

        public void MapSetIndexer<TToMember>(Expression<Action<TTo, string, TToMember>> toIndexer)
        {
            this.setIndexer = ExpressionHelper.GetMember(toIndexer);
        }

        public void Map<TFromMember, TToMember>(Expression<Func<TFrom, TFromMember>> from, Expression<Action<TTo, TToMember>> to)
        {
            var fromMember = ExpressionHelper.GetMember(from);
            var toMember = ExpressionHelper.GetMember(to);

            if (fromMember != null && toMember != null)
            {
                this.Map(new MemberGetter(fromMember), new MemberSetter(toMember));
            }
        }

        public void Map<TFromMember, TToMember>(Expression<Func<TFrom, TFromMember>> from, Expression<Func<TTo, TToMember>> to)
        {
            var fromMember = ExpressionHelper.GetMember(from);
            var toMember = ExpressionHelper.GetMember(to);

            if (fromMember != null && toMember != null)
            {
                this.Map(new MemberGetter(fromMember), new MemberSetter(toMember));
            }
        }

        public void Map<TFromMember, TToMember>(Expression<Func<TFrom, TFromMember>> from, string toName)
        {
            var fromMember = ExpressionHelper.GetMember(from);
            var toMember = (this.setIndexer != null)
                ? new MemberSetter(this.setIndexer, toName)
                : ReflectionUtils.GetMemberSetter(typeof(TTo), toName);

            if (fromMember != null && toMember != null)
            {
                this.Map(new MemberGetter(fromMember), toMember);
            }
        }

        public void Map<TFromMember, TToMember>(string fromName, Expression<Action<TTo, TToMember>> to)
        {
            var fromMember = (this.getIndexer != null) 
                ? new MemberGetter(this.getIndexer, fromName)
                : ReflectionUtils.GetMemberGetter(typeof(TFrom), fromName);

            var toMember = ExpressionHelper.GetMember(to);

            if (fromMember != null && toMember != null)
            {
                this.Map(fromMember, new MemberSetter(toMember));
            }
        }

        public void Map<TFromMember, TToMember>(string fromName, Expression<Func<TTo, TToMember>> to)
        {
            var fromMember = (this.getIndexer != null)
                ? new MemberGetter(this.getIndexer, fromName)
                : ReflectionUtils.GetMemberGetter(typeof(TFrom), fromName);

            var toMember = ExpressionHelper.GetMember(to);

            if (fromMember != null && toMember != null)
            {
                this.Map(fromMember, new MemberSetter(toMember));
            }
        }

        public void Map(string fromName, string toName)
        {
            var fromMember = (this.getIndexer != null)
                ? new MemberGetter(this.getIndexer, fromName)
                : ReflectionUtils.GetMemberGetter(typeof(TFrom), fromName);

            var toMember = (this.setIndexer != null)
                ? new MemberSetter(this.setIndexer, toName)
                : ReflectionUtils.GetMemberSetter(typeof(TTo), toName);
            
            this.Map(fromMember, toMember);
        }

        private void Map(MemberGetter fromMember, MemberSetter toMember)
        {
            if (fromMember != null && toMember != null)
            {
                this.memberMaps.Add(new MemberMap(fromMember, toMember));
            }
        }
    }
}
