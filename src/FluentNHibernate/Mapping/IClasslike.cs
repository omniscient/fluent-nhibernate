using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.Mapping.Providers;

namespace FluentNHibernate.Mapping
{
    public interface IClasslike
    {
        Type EntityType { get; }
        IEnumerable<IMappingPart> Parts { get; }
        IEnumerable<IPropertyMappingProvider> Properties { get; }
        IEnumerable<IComponentBase> Components { get; }
        IEnumerable<ISubclass> Subclasses { get; }
        IEnumerable<IJoinedSubclass> JoinedSubclasses { get; }
        IEnumerable<IVersionMappingProvider> Versions { get; }
        void AddSubclass(ISubclass subclass);
    }
}