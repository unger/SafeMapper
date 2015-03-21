namespace SafeMapper.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using SafeMapper.Reflection;
    using SafeMapper.Utils;

    public class TypeMap<TFrom, TTo>
    {
        private readonly List<MemberMap> memberMaps = new List<MemberMap>();

        public TypeMapping GetTypeMapping()
        {
            return new TypeMapping(typeof(TFrom), typeof(TTo), this.memberMaps);
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
            var toMember = ReflectionUtils.GetMemberSetter(typeof(TTo), toName);

            if (fromMember != null && toMember != null)
            {
                this.Map(new MemberGetter(fromMember), toMember);
            }
        }

        public void Map<TFromMember, TToMember>(string fromName, Expression<Action<TTo, TToMember>> to)
        {
            var fromMember = ReflectionUtils.GetMemberGetter(typeof(TFrom), fromName);
            var toMember = ExpressionHelper.GetMember(to);

            if (fromMember != null && toMember != null)
            {
                this.Map(fromMember, new MemberSetter(toMember));
            }
        }

        public void Map<TFromMember, TToMember>(string fromName, Expression<Func<TTo, TToMember>> to)
        {
            var fromMember = ReflectionUtils.GetMemberGetter(typeof(TFrom), fromName);
            var toMember = ExpressionHelper.GetMember(to);

            if (fromMember != null && toMember != null)
            {
                this.Map(fromMember, new MemberSetter(toMember));
            }
        }

        public void Map(string fromName, string toName)
        {
            var fromMember = ReflectionUtils.GetMemberGetter(typeof(TFrom), fromName);
            var toMember = ReflectionUtils.GetMemberSetter(typeof(TTo), toName);

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
