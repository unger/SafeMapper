namespace SafeMapper.Abstractions
{
    using System;
    using System.Collections.Generic;

    using SafeMapper.Reflection;

    public interface ITypeMapping
    {
        Type FromType { get; }

        Type ToType { get; }

        List<MemberMap> MemberMaps { get; }
    }
}