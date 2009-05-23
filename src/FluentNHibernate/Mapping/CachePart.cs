using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping
{
    public class CachePart : ICacheMappingProvider
    {
        private readonly CacheMapping mapping = new CacheMapping();

        CacheMapping ICacheMappingProvider.GetCacheMapping()
        {
            return mapping;
        }

        public CachePart AsReadWrite()
        {
           mapping.Usage = "read-write";
           return this;
        }

        public CachePart AsNonStrictReadWrite()
        {
            mapping.Usage = "nonstrict-read-write";
            return this;
        }

        public CachePart AsReadOnly()
        {
            mapping.Usage = "read-only";
            return this;
        }

        public CachePart AsCustom(string custom)
        {
            mapping.Usage = custom;
            return this;
        }

        public CachePart Region(string name)
        {
            mapping.Region = name;
            return this;
        }

        public bool IsDirty
        {
            get { return mapping.Attributes.IsSpecified(x => x.Region) || mapping.Attributes.IsSpecified(x => x.Usage); }
        }
    }
}