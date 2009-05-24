using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    public class IdentityPart : IIdMappingProvider
    {
        private readonly AttributeStore<ColumnMapping> columnAttributes = new AttributeStore<ColumnMapping>();
        private readonly IList<string> columns = new List<string>();
		private readonly PropertyInfo property;
        private readonly AccessStrategyBuilder<IIdMappingProvider> access;

        private readonly IdMapping mapping = new IdMapping();

        public IdentityPart(Type entity, PropertyInfo property, string columnName)
		{
            this.property = property;

            access = new AccessStrategyBuilder<IIdMappingProvider>(this, value => mapping.Access = value);
            GeneratedBy = new IdentityGenerationStrategyBuilder<IIdMappingProvider>(this, property.PropertyType);

            ColumnName(columnName);

            SetDefaultGenerator();
		}

        public IdentityPart(Type entity, PropertyInfo property)
            : this(entity, property, property.Name)
		{}

        private void SetDefaultGenerator()
        {
            if (property.PropertyType == typeof(Guid))
                GeneratedBy.GuidComb();
            else if (property.PropertyType == typeof(int) || property.PropertyType == typeof(long))
                GeneratedBy.Identity();
            else
                GeneratedBy.Assigned();
        }

        IdMapping IIdMappingProvider.GetIdMapping()
        {
            foreach (var column in columns)
                mapping.AddColumn(new ColumnMapping(columnAttributes.Clone()) { Name = column });

            mapping.Name = property.Name;
            mapping.Type = property.PropertyType.AssemblyQualifiedName;
            mapping.Generator = GeneratedBy.GetGeneratorMapping();

            return mapping;
        }

        public IdentityGenerationStrategyBuilder<IIdMappingProvider> GeneratedBy { get; private set; }

        /// <summary>
        /// Set the access and naming strategy for this identity.
        /// </summary>
        public AccessStrategyBuilder<IIdMappingProvider> Access
	    {
	        get { return access; }
	    }

        /// <summary>
        /// Sets the unsaved-value of the identity.
        /// </summary>
        /// <param name="unsavedValue">Value that represents an unsaved value.</param>
        public IIdMappingProvider UnsavedValue(object unsavedValue)
        {
            mapping.UnsavedValue = unsavedValue.ToString();
            return this;
        }

        /// <summary>
        /// Sets the column name for the identity field.
        /// </summary>
        /// <param name="columnName">Column name</param>
        public IIdMappingProvider ColumnName(string columnName)
        {
            columns.Clear(); // only currently support one column for ids
            columns.Add(columnName);
            return this;
        }
    }
}
