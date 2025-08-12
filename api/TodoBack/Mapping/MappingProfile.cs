using AutoMapper;
using TodoBack.Models;
using TodoBack.Models.DTO;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Subtask, SubtaskDto>();
        CreateMap<SubtaskDto, Subtask>()
            .ForMember(dest => dest.TaskItem, opt => opt.Ignore()) 
            .ForMember(dest => dest.SubtaskState, opt => opt.Ignore()); 
                                                                        
        CreateMap<TaskCreateDto, TaskItem>()
          .ForMember(dest => dest.TaskTags, opt => opt.Ignore())
          .ForMember(dest => dest.Subtasks, opt => opt.Ignore())
          .ForAllMembers(opts =>
              opts.Condition((src, dest, srcMember) =>
                  srcMember != null &&
                  (!(srcMember is string str) || !string.IsNullOrWhiteSpace(str))
              )
          );

        CreateMap<TaskItem, TaskDto>()
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.TaskTags.Select(tt => tt.Tag)))
            .ForMember(dest => dest.Subtasks, opt => opt.MapFrom(src => src.Subtasks)); 
    }
}
