using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Trash.Infra.Context.Repositories.Common;
using Trash.Infra.Service.Models;

namespace Trash.Infra.Service.Repositories
{
    public interface IPSRepository
    {
        Task<RepositoryResult> AddRangeAsync(IEnumerable<PService> services, CancellationToken cancellationToken);
        RepositoryResult AddService(PService service);
        Task<RepositoryResult> AddServiceAsync(PService service, CancellationToken cancellationToken);
        Task<RepositoryResult> DeleteByIdAsync(Guid service_Id, CancellationToken cancellationToken);
        RepositoryResult DeleteService(PService service);
        RepositoryResult DeleteServiceById(Guid service_Id);
        RepositoryResult DeleteServiceByName(string servicename);
        Task<RepositoryResult> GetAll();
        PService GetServiceById(Guid _Id);
        RepositoryResult GetServiceByName(string serviceName);
        RepositoryResult SetToNull(StatusResult status);
        RepositoryResult UpdateServiceByName(string ServiceName, PService ToUpdate);
    }
}