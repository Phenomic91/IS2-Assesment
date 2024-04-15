using AutoMapper;
using DataExporter.Dtos;
using DataExporter.Model;
using Microsoft.EntityFrameworkCore;


namespace DataExporter.Services
{
    public class PolicyService
    {
        private ExporterDbContext _dbContext;
        private IMapper _mapper;

        public PolicyService(ExporterDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _dbContext.Database.EnsureCreated();
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new policy from the DTO.
        /// </summary>
        /// <param name="policy"></param>
        /// <returns>Returns a ReadPolicyDto representing the new policy, if succeded. Returns null, otherwise.</returns>
        public async Task<ReadPolicyDto?> CreatePolicyAsync(CreatePolicyDto createPolicyDto)
        {
            var newPolicy = _mapper.Map<Policy>(createPolicyDto);
            _dbContext.Policies.Add(newPolicy);
            await _dbContext.SaveChangesAsync();


            return await ReadPolicyAsync(newPolicy.Id);
        }

        /// <summary>
        /// Retrives all policies.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns a list of ReadPoliciesDto.</returns>
        public async Task<IList<ReadPolicyDto>> ReadPoliciesAsync()
        {
            var policies = await _dbContext.Policies.ToListAsync();
            return _mapper.Map<List<ReadPolicyDto>>(policies);
        }

        /// <summary>
        /// Retrieves a policy by id.
        /// 
        /// Possible Imrpovements:
        /// Logging
        /// Auto-mapping from Model to dto
        /// Cancellation tokens
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns a ReadPolicyDto.</returns>
        public async Task<ReadPolicyDto?> ReadPolicyAsync(int id)
        {
            var policy = await _dbContext.Policies.SingleOrDefaultAsync(x => x.Id == id); // Use single or default here to return null for 0 elements
            if (policy == null)
            {
                return null;
            }

            var policyDto = new ReadPolicyDto()
            {
                Id = policy.Id,
                PolicyNumber = policy.PolicyNumber,
                Premium = policy.Premium,
                StartDate = policy.StartDate
            };

            return policyDto;
        }

        public async Task<IList<ExportDto>> ExportPoliciesAsync(DateTime startDate,  DateTime endDate)
        {
            var policies = await _dbContext.Policies.Where(p => p.StartDate >= startDate && p.StartDate <= endDate).Include(p => p.Notes).ToListAsync();
            return _mapper.Map<List<ExportDto>>(policies);        
        }
    }
}
