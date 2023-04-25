using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trash.Infra.Context.Repositories;
using Trash.Infra.Context.Repositories.Common;
using Trash.Infra.Service.Models;
using Trash.Infra.Service.Repositories;

namespace Trash.Services.UserServices.Services
{
    public class SService
    {
        private readonly SSRepository sSRepository;
        private readonly PSRepository pSRepository;
        private RepositoryResult repositoryResult;

        public SService(SSRepository ssRepository,PSRepository psRepository)
        {
            sSRepository = ssRepository;
            pSRepository = psRepository;
            repositoryResult = new RepositoryResult();
        }
        /// <summary>
        /// get all services for given user.id
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public IEnumerable<PService> GetUserServices(Guid user_id)
        {
            IEnumerable<Guid> services_id = sSRepository.GetServicesIdByUserId(user_id);
            if(!services_id.Any())
            {
                return null;
            }
            return pSRepository.GetServiceByIdRange(services_id);
        }
        /// <summary>
        /// delete all current services for given user.id
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public IEnumerable<SService> DeleteServicesByUserId(Guid user_id)
        {
            repositoryResult = sSRepository.DeleteServicesById(user_id);
            return ((List<SService>)repositoryResult.Object);
        }
        /// <summary>
        /// add given service.id for given user.id
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="service_id"></param>
        /// <returns></returns>
        public SService AddService(Guid user_id,Guid service_id)
        {
            repositoryResult.Object = sSRepository.AddById(service_id, user_id, Is_Currently_Use: true);
            if(sSRepository.GetServicesIdByUserId(user_id).Contains(service_id))
            {
                return ((SService)repositoryResult.Object);
            }
            return null;
        }
        /// <summary>
        /// add range of services.ids for given user.id
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="services"></param>
        /// <returns></returns>
        public IEnumerable<SService> AddServicesRange(Guid user_id , IEnumerable<PService> services)
        {
            foreach(PService item in services)
            {
                ((List<SService>)repositoryResult.Object).Add(((SService)sSRepository.AddById(item.Service_Id,user_id,Is_Currently_Use:true).Object));
            }
            return ((List<SService>)repositoryResult.Object);
        }


    }
}
