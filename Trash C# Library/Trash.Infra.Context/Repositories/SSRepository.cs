using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trash.Infra.Context.Context;
using Trash.Infra.Context.Repositories.Common;
using Trash.Infra.Service.Models;
using Trash.Infra.User.Models;
using Trash.Infra.User.Models;

namespace Trash.Infra.Context.Repositories
{
    // Selected Services Repository
    public class SSRepository : Repository<SService>, ISSRepository
    {
        public static RepositoryResult RepositoryResult;
        public SSRepository(TrashContext _AppContxet)
            : base(_AppContxet)
        {
            RepositoryResult = new RepositoryResult();
        }

        #region SyncMethods
        /// <summary>
        /// return all Services used by given User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public IEnumerable<Guid> GetServices(User.Models.User user)
        {
            if (user is null)
                return null;
            return GetServicesIdByUserId(user._Id);
        }
        /// <summary>
        /// return all Services used by given User._Id
        /// </summary>
        /// <param name="_user_Id"></param>
        /// <returns>services id that used by given user.id</returns>
        public IEnumerable<Guid> GetServicesIdByUserId(Guid _user_Id)
        {
            if (_user_Id.ToString() is null)
                return null;
            return _Context.Where(x => x.User_Id == _user_Id).Select(x => x.Service_Id).ToList();
        }
        /// <summary>
        /// return all Services used by given User._Id
        /// </summary>
        /// <param name="_user_Id"></param>
        /// <returns>services object use by given user.id</returns>
        public IEnumerable<PService> GetServicesByUserId(Guid _user_Id)
        {
            if (_user_Id.ToString() is null)
                return null;
            var temp = _Context.Where(x => x.User_Id == _user_Id).Select(x => x.Service_Id).ToList();
            List<PService> userservices = new List<PService>();
            temp.ForEach(f => { userservices.Add(_AppContext.Set<PService>().Find(f)); });
            return userservices;
        }
        /// <summary>
        /// return the Id of users that use the given service
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public IEnumerable<Guid> GetUsers(PService service)
        {
            if (service is null)
                return null;
            return GetUsersByServiceId(service.Service_Id);
        }
        /// <summary>
        /// return the Id of users that use the given service_Id
        /// </summary>
        /// <param name="service_Id"></param>
        /// <returns></returns>
        public IEnumerable<Guid> GetUsersByServiceId(Guid service_Id)
        {
            if (service_Id.ToString() is null)
                return null;
            return _Context.Where(x => x.Service_Id == service_Id).Select(x => x.User_Id).ToList();
        }
        /// <summary>
        /// Add Service for given user
        /// </summary>
        /// <param name="service"></param>
        /// <param name="user"></param>
        /// <param name="Is_Currently_Use"> the user is using the service</param>
        /// <returns></returns>
        public RepositoryResult Add(PService service, User.Models.User user, bool Is_Currently_Use)
        {
            if (service is null || user is null)
                return SetToNull(StatusResult.NullArgument);
            return AddById(service.Service_Id, user._Id, Is_Currently_Use);
        }
        /// <summary>
        /// Add Service for given user By Id
        /// </summary>
        /// <param name="ServiceId"></param>
        /// <param name="UserId"></param>
        /// <param name="Is_Currently_Use"> the user is using the service</param>
        /// <returns></returns>
        public RepositoryResult AddById(Guid ServiceId, Guid UserId, bool Is_Currently_Use)
        {
            if (ServiceId.ToString() is null || UserId.ToString() is null)
                return SetToNull(StatusResult.BadArgument);
            RepositoryResult.Object = _Context.Add(new SService { Service_Id = ServiceId, User_Id = UserId, Is_CurrentlyUse = Is_Currently_Use, StartsFrom = DateTime.UtcNow });
            RepositoryResult.Result_Code = StatusResult.Success;
            return RepositoryResult;
        }
        /// <summary>
        /// delete the given service for given user
        /// </summary>
        /// <param name="service_Id"></param>
        /// <param name="user_Id"></param>
        /// <returns></returns>
        public RepositoryResult Delete(PService service, User.Models.User user)
        {
            if (service is null || user is null)
                return SetToNull(StatusResult.NullArgument);
            return DeleteById(service.Service_Id, user._Id);
        }
        /// <summary>
        /// delete the given service for given user
        /// </summary>
        /// <param name="service_Id"></param>
        /// <param name="user_Id"></param>
        /// <returns>deleted services</returns>
        public RepositoryResult DeleteById(Guid service_Id, Guid user_Id)
        {
            if (service_Id.ToString() is null || user_Id.ToString() is null)
                return SetToNull(StatusResult.BadArgument);
            RepositoryResult.Object = _Context.Select(x => x.Service_Id == service_Id && x.User_Id == user_Id).ToList();
            if (!((IEnumerable<SService>)RepositoryResult.Object).Any())
            {
                return SetToNull(StatusResult.NotFound);
            }
            ((List<SService>)RepositoryResult.Object).ForEach(f => { f.Is_CurrentlyUse = false; });
            RepositoryResult.Result_Code = StatusResult.Success;
            return RepositoryResult;
        }
        /// <summary>
        /// Delete all services fot given user
        /// </summary>
        /// <param name="user_Id"></param>
        /// <returns>deleted services</returns>
        public RepositoryResult DisableServicesById(Guid user_Id)
        {
            if (user_Id.ToString() is null)
                return SetToNull(StatusResult.BadArgument);
            RepositoryResult.Object = _Context.Where(x => x.User_Id == user_Id).ToList();
            if (!((IEnumerable<SService>)RepositoryResult.Object).Any())
            {
                return SetToNull(StatusResult.NotFound);
            }
            ((List<SService>)RepositoryResult.Object).ForEach(f => { f.Is_CurrentlyUse = false; });
            RepositoryResult.Result_Code = StatusResult.Success;
            return RepositoryResult;
        }

        public RepositoryResult DeleteServicesById(Guid user_Id)
        {
            if (user_Id.ToString() is null)
                return SetToNull(StatusResult.BadArgument);
            RepositoryResult.Object = _Context.Where(x => x.User_Id == user_Id).ToList();
            if (!((IEnumerable<SService>)RepositoryResult.Object).Any())
            {
                return SetToNull(StatusResult.NotFound);
            }
           ((List<SService>)RepositoryResult.Object).ForEach(f => { f.Is_Deleted = true; });
            RepositoryResult.Result_Code = StatusResult.Success;
            return RepositoryResult;
        }
        #endregion

        #region AsyncMethods
        /// <summary>
        /// get the Users that use the given Service
        /// </summary>
        /// <param name="service_Id"></param>
        /// <returns>return the users that use the given service</returns>
        public async Task<IEnumerable<User.Models.User>> GetUsersByServiceIdAsync(Guid service_Id)
        {
            if (service_Id.ToString() is null)
                return null;
            List<Guid> users_Id = _Context.Where(x => x.Service_Id == service_Id).Select(x => x.User_Id).ToList();
            List<User.Models.User> users = new List<User.Models.User>();
            //users_Id.ForEach(f => {users.Add(_AppContext.Set<User.Models.User>().FindAsync(f))});
            foreach (Guid userId in users_Id)
            {
                users.Add(await _AppContext.Set<User.Models.User>().FindAsync(userId));
            }
            return users;
        }
        #endregion

        #region Common
        public RepositoryResult SetToNull(StatusResult code)
        {
            RepositoryResult.Object = null;
            RepositoryResult.Result_Code = code;
            return RepositoryResult;
        }
        #endregion
    }
}
