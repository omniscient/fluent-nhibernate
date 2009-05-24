using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.Mapping.Providers
{
    public interface IIdMappingProvider
    {
        IdMapping GetIdMapping();
    }
}