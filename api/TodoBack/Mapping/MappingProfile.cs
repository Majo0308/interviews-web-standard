using AutoMapper;
using TodoBack.Models;
using TodoBack.Models.DTO;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Subtask mappings
        CreateMap<Subtask, SubtaskDto>();
        CreateMap<SubtaskDto, Subtask>()
            .ForMember(dest => dest.TaskItem, opt => opt.Ignore()) // Ignorar TaskItem al crear
            .ForMember(dest => dest.SubtaskState, opt => opt.Ignore()); // Ignorar SubtaskState al crear
        // Task mappings
        CreateMap<TaskCreateDto, TaskItem>()
            .ForMember(dest => dest.TaskTags, opt => opt.Ignore()) // Ignorar para no mapear tags al crear
            .ForMember(dest => dest.Subtasks, opt => opt.Ignore()); // Ignorar subtasks al crear

        CreateMap<TaskItem, TaskDto>()
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.TaskTags.Select(tt => tt.Tag)))
            .ForMember(dest => dest.Subtasks, opt => opt.MapFrom(src => src.Subtasks)); // Mapear subtasks
    }
}
