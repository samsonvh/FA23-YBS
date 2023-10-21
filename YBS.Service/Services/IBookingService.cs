using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YBS.Data.Models;
using YBS.Service.Dtos.InputDtos;

namespace YBS.Service.Services
{
    public interface IBookingService
    {
        Task CreateGuestBooking (BookingInputDto pageRequest);
        Task<bool> ChangeStatusBookingNonMember(int id, string status);
    }
}