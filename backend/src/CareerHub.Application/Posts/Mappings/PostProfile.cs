using AutoMapper;
using CareerHub.Application.Posts.DTOs;
using CareerHub.Domain.Posts.Entities;

namespace CareerHub.Application.Posts.Mappings;

public class PostProfile : Profile
{
    public PostProfile()
    {
        CreateMap<Post, PostDto>();
    }
}
