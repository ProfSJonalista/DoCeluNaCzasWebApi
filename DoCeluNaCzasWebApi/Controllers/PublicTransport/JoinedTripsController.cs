﻿using DoCeluNaCzasWebApi.Models.PublicTransport;
using DoCeluNaCzasWebApi.Services.UpdateService;
using System.Collections.Generic;
using System.Web.Http;

namespace DoCeluNaCzasWebApi.Controllers.PublicTransport
{
    public class JoinedTripsController : ApiController
    {
        public List<JoinedTripsModel> Get()
        {
            return UpdateDataService.GetJoinedTrips();
        }
    }
}
