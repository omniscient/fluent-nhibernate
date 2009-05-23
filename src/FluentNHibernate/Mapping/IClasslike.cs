using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FluentNHibernate.Mapping
{
    public interface IClasslike
    {
        Type EntityType { get; }
        IEnumerable<IMappingPart> Parts { get; }
        IEnumerable<PropertyMap> Properties { get; }
        IEnumerable<IComponentBase> Components { get; }
        IEnumerable<ISubclass> Subclasses { get; }
        IEnumerable<IJoinedSubclass> JoinedSubclasses { get; }
        void AddSubclass(ISubclass subclass);
    }
}