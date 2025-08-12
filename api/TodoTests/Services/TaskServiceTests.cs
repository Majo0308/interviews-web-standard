using AutoMapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TodoBack.Data;
using TodoBack.Models;
using TodoBack.Models.DTO;
using TodoBack.Services;

namespace TodoBack.Tests.Services
{
    public class TaskServiceIntegrationTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly TaskService _taskService;
        private readonly SubtaskService _subtaskService;
        private readonly TagService _tagService;
        private readonly SqliteConnection _connection;

        public TaskServiceIntegrationTests()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open(); 

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(_connection) 
                .Options;

            _context = new AppDbContext(options);
            _context.Database.EnsureCreated(); 

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Subtask, SubtaskDto>().ReverseMap()
                    .ForMember(dest => dest.TaskItem, opt => opt.Ignore())
                    .ForMember(dest => dest.SubtaskState, opt => opt.Ignore());

                cfg.CreateMap<TaskCreateDto, TaskItem>()
                    .ForMember(dest => dest.TaskTags, opt => opt.Ignore())
                    .ForMember(dest => dest.Subtasks, opt => opt.Ignore())
                    .ForAllMembers(opts =>
                        opts.Condition((src, dest, srcMember) =>
                            srcMember != null &&
                            (!(srcMember is string str) || !string.IsNullOrWhiteSpace(str))
                        )
                    );

                cfg.CreateMap<TaskItem, TaskDto>()
                    .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.TaskTags.Select(tt => tt.Tag)))
                    .ForMember(dest => dest.Subtasks, opt => opt.MapFrom(src => src.Subtasks));
            });
            _mapper = mapperConfig.CreateMapper();

            _subtaskService = new SubtaskService(_context, _mapper);
            _tagService = new TagService(_context);
            _taskService = new TaskService(_context, _mapper, _subtaskService, _tagService);
        }

        public void Dispose()
        {
            _context.Dispose();
            _connection.Close();
            _connection.Dispose();
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateTaskWithSubtasksAndTags()

        {
            var defaultState = new SubtaskState { StateName = "Pendant" };
            await _context.SubtaskStates.AddAsync(defaultState);
            await _context.SaveChangesAsync();

            var tag1 = new Tag { TagName = "Importante" };
            await _context.Tags.AddAsync(tag1);

            await _context.SaveChangesAsync();

            var newTaskDto = new TaskCreateDto
            {
                TaskName = "Tarea de Integración Completa",
                TaskDescription = "Descripción de la tarea",
                TaskPriority = 1,
                TaskDueDate = DateTime.UtcNow,
                Tags = new List<int> { tag1.TagId }, 
                Subtasks = new List<SubtaskDto>
                {
                    new SubtaskDto { SubtaskName = "Subtarea 1", SubtaskStateId=defaultState.SubtaskStateId}                }
            };

            var createdTaskDto = await _taskService.CreateAsync(newTaskDto);

            Assert.NotNull(createdTaskDto);
            Assert.Equal("Tarea de Integración Completa", createdTaskDto.TaskName);

            var taskCreated = await _taskService.GetByIdAsync(createdTaskDto.TaskItemId);
            
            Assert.NotNull(taskCreated);
            Assert.Single(taskCreated.Subtasks);
            Console.WriteLine(taskCreated.Subtasks.ToString());
            Assert.Equal("Subtarea 1", taskCreated.Subtasks.First().SubtaskName);
            Assert.Single(taskCreated.Subtasks); 
            Assert.Equal("Importante", taskCreated.Tags.First().TagName);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnTask_WhenExists()
        {
            var task = new TaskItem { TaskName = "Test Task" };
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            var result = await _taskService.GetByIdAsync(task.TaskItemId);

            Assert.NotNull(result);
            Assert.Equal("Test Task", result.TaskName);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
        {

            var result = await _taskService.GetByIdAsync(999);
            Assert.Null(result);
        }

        [Fact]
        public async Task AssignTagsAsync_ShouldCorrectlyAssignTagsToTask()
        {
            var task = new TaskItem { TaskName = "Task para Asignar Tags" };
            var tag1 = new Tag { TagName = "Frontend" };
            var tag2 = new Tag { TagName = "Bug" };
            _context.AddRange(task, tag1, tag2);
            await _context.SaveChangesAsync();

            var tagIdsToAssign = new List<int> { tag1.TagId, tag2.TagId };
            await _tagService.AssignTagsAsync(tagIdsToAssign, task);

            var taskFromDb = await _context.Tasks
                .Include(t => t.TaskTags)
                .FirstOrDefaultAsync(t => t.TaskItemId == task.TaskItemId);

            Assert.NotNull(taskFromDb);
            Assert.Equal(2, taskFromDb.TaskTags.Count);
            Assert.Contains(taskFromDb.TaskTags, tt => tt.TagId == tag1.TagId);
            Assert.Contains(taskFromDb.TaskTags, tt => tt.TagId == tag2.TagId);
        }
    }
}