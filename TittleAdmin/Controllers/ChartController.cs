using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TittleAdmin.Model.DTO;
using TittleAdmin.Service.Implementations;

namespace TittleAdmin.Controllers
{
    public class ChartController : ApiController
    {
        // GET api/<controller>/TotalUsers
        [HttpGet]
        [Route("api/chart/totalusers")]
        public HttpResponseMessage TotalUsers()
        {
            TittleUserServices us = new TittleUserServices();
            CustomChartData<string> cd = us.TotalUsersCount();
            return Request.CreateResponse(HttpStatusCode.OK, cd);
        }
        [HttpGet]
        [Route("api/chart/newusers")]
        public HttpResponseMessage NewUsers(int type)
        {
            TittleUserServices us = new TittleUserServices();
            List<CustomChartData<string>> cd = us.NewUsersCount(type);
            return Request.CreateResponse(HttpStatusCode.OK, cd);
        }
        [HttpGet]
        [Route("api/chart/activeusers")]
        public HttpResponseMessage ActiveUsers(int type)
        {
            TittleUserServices us = new TittleUserServices();
            List<CustomChartData<string>> cd = us.ActiveUsersCount(type);
            return Request.CreateResponse(HttpStatusCode.OK, cd);
        }
        [HttpGet]
        [Route("api/chart/terminatedusers")]
        public HttpResponseMessage TerminatedUsers(int type)
        {
            TittleUserServices us = new TittleUserServices();
            List<CustomChartData<string>> cd = us.DeactiveUsersCount(type);
            return Request.CreateResponse(HttpStatusCode.OK, cd);
        }
        [HttpGet]
        [Route("api/chart/revenuechart")]
        public HttpResponseMessage RevenueChart(int type)
        {
            TittlePackageServices us = new TittlePackageServices();
            List<CustomChartData<decimal>> cd = us.RevenueChart(type);
            return Request.CreateResponse(HttpStatusCode.OK, cd);
        }
        [HttpGet]
        [Route("api/chart/inappchart")]
        public HttpResponseMessage InappChart(int type)
        {
            TittlePackageServices us = new TittlePackageServices();
            List<CustomChartData<string>> cd = us.InappPurchaseChart(type);
            return Request.CreateResponse(HttpStatusCode.OK, cd);
        }
        [HttpGet]
        [Route("api/chart/addonfeature")]
        public HttpResponseMessage Addonfeature()
        {
            TittlePackageServices us = new TittlePackageServices();
            List<CustomAddOnData> cd = us.GetAddOnFeatures();
            return Request.CreateResponse(HttpStatusCode.OK, cd);
        }
        [HttpGet]
        [Route("api/chart/userplans")]
        public HttpResponseMessage userplans()
        {
            TittleUserServices us = new TittleUserServices();
            List<CustomChartData<int>> cd = us.UserPlansCount();
            return Request.CreateResponse(HttpStatusCode.OK, cd);
        }
        [HttpGet]
        [Route("api/chart/useractions")]
        public HttpResponseMessage useractions()
        {
            TittleUserServices us = new TittleUserServices();
            List<CustomChartData<int>> cd = us.UserActionsCount();
            return Request.CreateResponse(HttpStatusCode.OK, cd);
        }
        [HttpGet]
        [Route("api/chart/userpromocodes")]
        public HttpResponseMessage userpromocodes()
        {
            TittleUserServices us = new TittleUserServices();
            List<CustomChartData<int>> cd = us.UserPromoCodesCount();
            return Request.CreateResponse(HttpStatusCode.OK, cd);
        }
    }
}