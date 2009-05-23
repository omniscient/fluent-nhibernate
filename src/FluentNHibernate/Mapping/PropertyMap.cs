using System;
using System.Reflection;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using NHibernate.UserTypes;

namespace FluentNHibernate.Mapping
{
    public class PropertyMap : IPropertyMappingProvider
    {
        private readonly Type parentType;
        private readonly AccessStrategyBuilder<PropertyMap> access;
        private readonly ColumnNameCollection<PropertyMap> columnNames;
        private readonly AttributeStore<ColumnMapping> columnAttributes = new AttributeStore<ColumnMapping>();
        private bool nextBool = true;

        private readonly PropertyMapping mapping;

        public PropertyMap(PropertyMapping mapping, Type parentType)
        {
            columnNames = new ColumnNameCollection<PropertyMap>(this);
            access = new AccessStrategyBuilder<PropertyMap>(this, value => mapping.Access = value);

            this.mapping = mapping;
            this.parentType = parentType;
        }

        PropertyMapping IPropertyMappingProvider.GetPropertyMapping()
        {
            if (columnNames.List().Count == 0)
                columnNames.Add(mapping.Name);

            foreach (var column in columnNames.List())
            {
                var columnMapping = new ColumnMapping(columnAttributes.Clone())
                {
                    Name = column
                };

                mapping.AddColumn(columnMapping);
            }

            if (!mapping.Attributes.IsSpecified(x => x.Type))
                mapping.Type = TypeMapping.GetTypeString(Property.PropertyType);

            return mapping;
        }

        public PropertyInfo Property
        {
            get { return mapping.PropertyInfo; }
        }

        public Type PropertyType
        {
            get { return mapping.PropertyInfo.PropertyType; }
        }

        public Type EntityType
        {
            get { return parentType; }
        }

        public PropertyMap ColumnName(string columnName)
        {
            ColumnNames.Clear();
            ColumnNames.Add(columnName);
            return this;
        }

        public ColumnNameCollection<PropertyMap> ColumnNames
        {
            get { return columnNames; }
        }

        /// <summary>
        /// Set the access and naming strategy for this property.
        /// </summary>
        public AccessStrategyBuilder<PropertyMap> Access
        {
            get { return access; }
        }

        public PropertyMap Insert()
        {
            mapping.Insert = nextBool;
            nextBool = true;

            return this;
        }

        public PropertyMap Update()
        {
            mapping.Update = nextBool;
            nextBool = true;

            return this;
        }

        public PropertyMap WithLengthOf(int length)
        {
            columnAttributes.Set(x => x.Length, length);
            return this;
        }

        public PropertyMap Nullable()
        {
            columnAttributes.Set(x => x.NotNull, !nextBool);
            nextBool = true;
            return this;
        }

        public PropertyMap ReadOnly()
        {
            mapping.Insert = !nextBool;
            mapping.Update = !nextBool;
            nextBool = true;
            return this;
        }

        public PropertyMap FormulaIs(string formula) 
        {
            mapping.Formula = formula;
            return this;
        }

        /// <summary>
        /// Specifies that a custom type (an implementation of <see cref="IUserType"/>) should be used for this property for mapping it to/from one or more database columns whose format or type doesn't match this .NET property.
        /// </summary>
        /// <typeparam name="TCustomtype">A type which implements <see cref="IUserType"/>.</typeparam>
        /// <returns>This property mapping to continue the method chain</returns>
        public PropertyMap CustomTypeIs<TCustomtype>()
        {
            return CustomTypeIs(typeof(TCustomtype));
        }

        /// <summary>
        /// Specifies that a custom type (an implementation of <see cref="IUserType"/>) should be used for this property for mapping it to/from one or more database columns whose format or type doesn't match this .NET property.
        /// </summary>
        /// <param name="type">A type which implements <see cref="IUserType"/>.</param>
        /// <returns>This property mapping to continue the method chain</returns>
        public PropertyMap CustomTypeIs(Type type)
        {
            if (typeof(ICompositeUserType).IsAssignableFrom(type))
                AddColumnsFromCompositeUserType(type);

            return CustomTypeIs(TypeMapping.GetTypeString(type));
        }

        /// <summary>
        /// Specifies that a custom type (an implementation of <see cref="IUserType"/>) should be used for this property for mapping it to/from one or more database columns whose format or type doesn't match this .NET property.
        /// </summary>
        /// <param name="type">A type which implements <see cref="IUserType"/>.</param>
        /// <returns>This property mapping to continue the method chain</returns>
        public PropertyMap CustomTypeIs(string type)
        {
            mapping.Type = type;

            return this;
        }

        private void AddColumnsFromCompositeUserType(Type compositeUserType)
        {
            var inst = (ICompositeUserType)Activator.CreateInstance(compositeUserType);

            foreach (var name in inst.PropertyNames)
            {
                ColumnNames.Add(name);
            }
        }

        public PropertyMap CustomSqlTypeIs(string sqlType)
        {
            columnAttributes.Set(x => x.SqlType, sqlType);
            return this;
        }

        public PropertyMap Unique()
        {
            columnAttributes.Set(x => x.Unique, nextBool);
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Specifies the name of a multi-column unique constraint.
        /// </summary>
        /// <param name="keyName">Name of constraint</param>
        public PropertyMap UniqueKey(string keyName)
        {
            mapping.UniqueKey = keyName;
            return this;
        }

        /// <summary>
        /// Inverts the next boolean
        /// </summary>
        public PropertyMap Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }
    }
}
