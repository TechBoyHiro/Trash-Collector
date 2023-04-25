using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Trash.Infra.Context.Context;
using Trash.Infra.Context.Repositories.Common;
using Trash.Infra.Service.Models;

namespace Trash.Infra.Service.Repositories
{
    [Display(Name = "Provided Service Repository")]
    public class PSRepository : Repository<PService>, IPSRepository
    {
        public static RepositoryResult RepositoryResult;
        public PSRepository(TrashContext _AppConext)
            : base(_AppConext)
        {
            RepositoryResult = new RepositoryResult();
        }

        #region SyncMethods
        public PService GetServiceById(Guid _Id)
        {
            return _Context.Find(_Id);
        }
        /// <summary>
        /// return the service by given name
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public RepositoryResult GetServiceByName(string serviceName)
        {
            RepositoryResult.Object = _Context.Where(x => x.ServiceName == serviceName).First();
            if (RepositoryResult.Object == null)
            {
                return SetToNull(StatusResult.NotFound);
            }
            RepositoryResult.Result_Code = StatusResult.Success;
            return RepositoryResult;
        }
        /// <summary>
        /// return all services with given ids
        /// </summary>
        /// <param name="service_ids"></param>
        /// <returns></returns>
        public IEnumerable<PService> GetServiceByIdRange(IEnumerable<Guid> service_ids)
        {
            return _Context.Where(x => service_ids.Contains(x.Service_Id)).ToList();
        }

        public RepositoryResult AddService(PService service)
        {
            if (service != null)
            {
                RepositoryResult.Object = _Context.Add(service);
                if (RepositoryResult.Object is null)
                {
                    RepositoryResult.Result_Code = StatusResult.BadArgument;
                    return RepositoryResult;
                }
                _AppContext.SaveChanges();
                RepositoryResult.Result_Code = StatusResult.Success;
                return RepositoryResult;
            }
            RepositoryResult.Object = null;
            RepositoryResult.Result_Code = StatusResult.NullArgument;
            return RepositoryResult;
        }

        public RepositoryResult DeleteService(PService service)
        {
            return DeleteServiceById(service.Service_Id);
        }

        public RepositoryResult DeleteServiceById(Guid service_Id)
        {
            if (service_Id.ToString() is null)
            {
                return SetToNull(StatusResult.BadArgument);
            }
            var temp = _Context.Find(service_Id);
            if (temp is null)
            {
                return SetToNull(StatusResult.NotFound);
            }
            if (temp.IsDeleted == true)
                return SetToNull(StatusResult.BadArgument);
            temp.IsDeleted = true;
            _Context.Update(temp);
            _AppContext.SaveChanges();
            RepositoryResult.Object = temp;
            RepositoryResult.Result_Code = StatusResult.Success;
            return RepositoryResult;
        }
        public RepositoryResult DeleteServiceByName(string servicename)
        {
            if (servicename == null)
            {
                return SetToNull(StatusResult.NullArgument);
            }
            RepositoryResult temp = GetServiceByName(servicename);
            if (RepositoryResult.Result_Code == StatusResult.Success)
            {
                PService pService = (PService)RepositoryResult.Object;
                return DeleteServiceById(pService.Service_Id);
            }
            return SetToNull(StatusResult.NotFound);
        }

        public RepositoryResult UpdateServiceByName(string ServiceName, PService ToUpdate)
        {
            if (ServiceName is null || ToUpdate is null)
            {
                return SetToNull(StatusResult.NullArgument);
            }
            RepositoryResult.Object = _Context.Update(ToUpdate);
            RepositoryResult.Result_Code = StatusResult.Success;
            return RepositoryResult;
        }
        #endregion

        #region AsyncMethods
        /// <summary>
        /// get all available services
        /// </summary>
        /// <returns></returns>
        public async Task<RepositoryResult> GetAll()
        {
            RepositoryResult.Object = await _Context.ToArrayAsync();
            RepositoryResult.Result_Code = StatusResult.Success;
            return RepositoryResult;
        }

        public async Task<RepositoryResult> DeleteByIdAsync(Guid service_Id, CancellationToken cancellationToken)
        {
            if (service_Id.ToString() is null)
                return SetToNull(StatusResult.BadArgument);

            var temp = await _Context.FindAsync(service_Id);
            if (temp is null)
                return SetToNull(StatusResult.NotFound);

            if (temp.IsDeleted == true)
                return SetToNull(StatusResult.BadArgument);

            temp.IsDeleted = true;
            _Context.Update(temp);
            await _AppContext.SaveChangesAsync(cancellationToken);
            RepositoryResult.Object = temp;
            RepositoryResult.Result_Code = StatusResult.Success;
            return RepositoryResult;
        }

        public async Task<RepositoryResult> AddServiceAsync(PService service, CancellationToken cancellationToken)
        {
            if (service is null)
                return SetToNull(StatusResult.NullArgument);
            RepositoryResult.Object = _Context.AddAsync(service, cancellationToken);
            _AppContext.SaveChangesAsync(cancellationToken);
            RepositoryResult.Result_Code = StatusResult.Success;
            return RepositoryResult;
        }

        public async Task<RepositoryResult> AddRangeAsync(IEnumerable<PService> services, CancellationToken cancellationToken)
        {
            if (!services.Any())
                return SetToNull(StatusResult.NullArgument); // could be either RepositoryResult.BadArgument
            RepositoryResult.Object = _Context.AddRangeAsync(services, cancellationToken);
            RepositoryResult.Result_Code = StatusResult.Success;
            return RepositoryResult;
        }
        #endregion

        #region Common
        /// <summary>
        /// set the RepositoryResult.Object To Null
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public RepositoryResult SetToNull(StatusResult status)
        {
            RepositoryResult.Object = null;
            RepositoryResult.Result_Code = status;
            return RepositoryResult;
        }
        #endregion
    }
}





// .... Extra Methods
///// <summary>
///// if any service exist with the given name
///// </summary>
///// <param name="Name"></param>
///// <returns></returns>
//public RepositoryResult IsThereAnyByName(string Name)
//{
//    RepositoryResult.Object =  _Context.Where(x => x.ServiceName == Name || x.ServiceName.Contains(Name)).ToArray();

//    if(!((IEnumerable<PService>)RepositoryResult.Object).Any())
//    {
//        RepositoryResult.Object = null;
//        RepositoryResult.Result_Code = StatusResult.NotFound;
//    }
//    RepositoryResult.Result_Code = StatusResult.Success;
//    return RepositoryResult;
//}
