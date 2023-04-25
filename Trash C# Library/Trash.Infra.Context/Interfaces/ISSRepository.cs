using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trash.Infra.Context.Repositories.Common;
using Trash.Infra.Service.Models;
using Trash.Infra.User.Models;

namespace Trash.Infra.Context.Repositories
{
    public interface ISSRepository
    {
        RepositoryResult Add(PService service, User.Models.User user, bool Is_Currently_Use);
        RepositoryResult AddById(Guid ServiceId, Guid UserId, bool Is_Currently_Use);
        RepositoryResult Delete(PService service, User.Models.User user);
        RepositoryResult DeleteById(Guid service_Id, Guid user_Id);
        RepositoryResult DeleteServicesById(Guid user_Id);
        RepositoryResult DisableServicesById(Guid user_Id);
        IEnumerable<Guid> GetServices(User.Models.User user);
        IEnumerable<PService> GetServicesByUserId(Guid _user_Id);
        IEnumerable<Guid> GetServicesIdByUserId(Guid _user_Id);
        IEnumerable<Guid> GetUsers(PService service);
        IEnumerable<Guid> GetUsersByServiceId(Guid service_Id);
        Task<IEnumerable<User.Models.User>> GetUsersByServiceIdAsync(Guid service_Id);
        RepositoryResult SetToNull(StatusResult code);
    }
}