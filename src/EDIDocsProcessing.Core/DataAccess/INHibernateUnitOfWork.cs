using NHibernate;

namespace EDIDocsProcessing.Core.DataAccess
{
    public interface INHibernateUnitOfWork
    {
        ISession CurrentSession { get; }
        void Start();
    }
}