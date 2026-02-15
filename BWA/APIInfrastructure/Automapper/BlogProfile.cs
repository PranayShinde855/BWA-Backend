using AutoMapper;
using BWA.APIInfrastructure.Requests;
using BWA.ServiceEntities;

namespace BWA.APIInfrastructure.Automapper
{
    public class BlogProfile : Profile
    {
        public BlogProfile()
        {
            CreateMap<GetBlogsRequest, GetBlogsDto>();
            CreateMap<AddBlogRequest, AddBlogDto>();
            CreateMap<GetBlogByIdRequest, GetBlogByIdDto>();
            CreateMap<UpdateBlogRequest, UpdateBlogDto>();
            CreateMap<DeleteBlogRequest, DeleteBlogDto>();
        }
    }
}
