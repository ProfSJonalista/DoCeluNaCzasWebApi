﻿using System.Net.Http;
using System.Threading.Tasks;

namespace DCNC.DataAccess.PublicTransport
{
    public interface IPublicTransportRepository
    {
        Task<string> DownloadData(string url);
        Task<string> DownloadData(string url, HttpClient client);
    }
}
