﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WyvernWatch.Models.API;
using WyvernWatch.Shared.DTO;

namespace WyvernWatch.Interfaces
{
    public interface IAPIClient
    {
        Task<string> FetchAsync();
    }
}
