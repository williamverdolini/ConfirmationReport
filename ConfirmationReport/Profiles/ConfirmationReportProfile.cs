using System;
using System.Threading.Tasks;
using AutoMapper;
using ConfirmRep.Infrastructure.Common;
using ConfirmRep.Models.Domain;
using ConfirmRep.Models.View;
using ConfirmRep.Repositories;

namespace ConfirmRep.Profiles
{
    public class ConfirmationReportProfile : Profile
    {
        private readonly IAuthRepository userRepo;

        public ConfirmationReportProfile(IAuthRepository userRepo)
        {
            Contract.Requires<ArgumentNullException>(userRepo != null, "userRepo");
            this.userRepo = userRepo;
        }

        protected override void Configure()
        {
            CreateMap<ConfirmationReportViewModel, ConfirmationReport>();
            CreateMap<ConfirmationReportDetailViewModel, ConfirmationReportDetail>();
            CreateMap<ConfirmationReport, ConfirmationReportViewModel>()
                .ForMember(dest => dest.OwnerCompleteName, opt => opt.ResolveUsing((res, src) => {
                    var user = Task.Run(() => userRepo.GetUserInfoByUsername(src.OwnerName)).Result;
                    return user.Name + " "+ user.Surname; 
                }));
            CreateMap<ConfirmationReportDetail, ConfirmationReportDetailViewModel>();
        }
    }
}