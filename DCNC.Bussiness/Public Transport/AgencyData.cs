using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCNC.Bussiness.Public_Transport
{
    public class AgencyData
    {
        public string lastUpdate { get; set; }
        public IList<Agency> agency { get; set; }
    }

    public class Agency
    {
        public int agencyId { get; set; }
        public string agencyName { get; set; }
        public string agencyUrl { get; set; }
        public string agencyTimezone { get; set; }
        public string agencyLang { get; set; }
        public string agencyPhone { get; set; }
        public string agencyFareUrl { get; set; }
        public string agencyEmail { get; set; }
        public IList<Topology> topologies { get; set; }
    }

    public class Topology
    {
        public int versionNumber { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
    }
}