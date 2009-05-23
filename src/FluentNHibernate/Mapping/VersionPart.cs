using System;
using System.Reflection;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping
{
    public class VersionPart : IVersionMappingProvider
    {
        public PropertyInfo Property { get; private set; }
        public Type EntityType { get; private set; }
        private readonly AccessStrategyBuilder<VersionPart> access;
        private readonly VersionGeneratedBuilder<VersionPart> generated;

        private readonly VersionMapping mapping = new VersionMapping();

        public VersionPart(Type entity, PropertyInfo property)
        {
            EntityType = entity;
            access = new AccessStrategyBuilder<VersionPart>(this, value => mapping.Access = value);
            generated = new VersionGeneratedBuilder<VersionPart>(this, value => mapping.Generated = value);
            Property = property;
        }

        VersionMapping IVersionMappingProvider.GetVersionMapping()
        {
            mapping.Name = Property.Name;
            mapping.Type = Property.PropertyType == typeof(DateTime) ? "timestamp" : Property.PropertyType.AssemblyQualifiedName;

            if (!mapping.Attributes.IsSpecified(x => x.Column))
                mapping.Column = Property.Name;

            return mapping;
        }

        public VersionGeneratedBuilder<VersionPart> Generated
        {
            get { return generated; }
        }

        public AccessStrategyBuilder<VersionPart> Access
        {
            get { return access; }
        }

        public VersionPart ColumnName(string name)
        {
            mapping.Column = name;
            return this;
        }

        public string GetColumnName()
        {
            return mapping.Column;
        }

        public VersionPart UnsavedValue(string value)
        {
            mapping.UnsavedValue = value;
            return this;
        }
    }
}