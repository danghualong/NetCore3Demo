using AutoMapper;
using EFTest.Configs;
using EFTest.Models.Dtos;
using EFTest.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFTest.Profiles
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserInfo>()
                .ForMember(dest => dest.Id, options => options.MapFrom(src => src.UserId))
                .ForMember(dest => dest.IsAdmin, options =>
                {
                    options.MapFrom(src => src.UserType == (int)UserType.SuperAdmin);
                });
        }
    }
}
