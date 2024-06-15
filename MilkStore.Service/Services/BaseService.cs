using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using MilkStore.Repository.Interfaces;
using MilkStore.Service.Common;
using MilkStore.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Services
{
    public abstract class BaseService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;
        protected readonly ICurrentTime _currentTime;
        protected readonly IClaimsService _claimsService;
        protected readonly AppConfiguration _appConfiguration;
        protected readonly ISmsSender _smsSender;
        protected readonly IMemoryCache _cache;

        protected BaseService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ICurrentTime currentTime,
            IClaimsService claimsService,
            AppConfiguration appConfiguration,
            ISmsSender smsSender,
            IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _appConfiguration = appConfiguration;
            _smsSender = smsSender;
            _cache = cache;
        }
    }
}
