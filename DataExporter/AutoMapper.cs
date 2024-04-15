using AutoMapper;
using DataExporter.Dtos;
using DataExporter.Model;

namespace DataExporter
{
    public class PolicyProfile : Profile
    {
        public PolicyProfile() 
        {
            CreateMap<Policy,ReadPolicyDto>();
            CreateMap<Policy, CreatePolicyDto>().ReverseMap();
            CreateMap<Policy, ExportDto>();
            CreateMap<Note, string>().ConvertUsing<NoteTypeConverter>();        }
    }

    public class NoteTypeConverter : ITypeConverter<Note, string>
    {
        public string Convert(Note source, string destination, ResolutionContext context)
        {
            return source.Text;
        }
    }
}
