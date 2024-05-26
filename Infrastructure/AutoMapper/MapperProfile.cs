using AutoMapper;
using Domain.DTOs.BookingDto;
using Domain.DTOs.PaymentDto;
using Domain.DTOs.RoleDto;
using Domain.DTOs.UserRoleDto;
using Domain.Entities;

namespace Infrastructure.AutoMapper;

public class MapperProfile:Profile
{
    public MapperProfile()
    {
        CreateMap<Booking, AddBookingDto>().ReverseMap();
        CreateMap<Booking, GetBookingDto>().ReverseMap();
        CreateMap<Booking, UpdateBookingDto>().ReverseMap();

        CreateMap<Payment, AddPaymentDto>().ReverseMap();
        CreateMap<Payment, GetPaymentDto>().ReverseMap();
        CreateMap<Payment, UpdatePaymentDto>().ReverseMap();

        CreateMap<UserRole, AddUserRoleDto>().ReverseMap();
        CreateMap<UserRole, GetUserRoleDto>().ReverseMap();
        CreateMap<UserRole, UpdateUserRoleDto>().ReverseMap();

        CreateMap<Role, AddRoleDto>().ReverseMap();
        CreateMap<Role, GetRoleDto>().ReverseMap();
        CreateMap<Role, UpdateRoleDto>().ReverseMap();

    }
}
