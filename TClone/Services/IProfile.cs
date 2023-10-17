﻿using AutoMapper;
using TClone.Models;
using TClone.Repository;

namespace TClone.Services
{
    public interface IProfile 
    {
        Task<ProfileDto> GetProfile(string id);
        Task<string> DeleteProfile(Guid id);
        Task<string> UpdateProfile(Guid id, ProfileDto request);
    }
}
