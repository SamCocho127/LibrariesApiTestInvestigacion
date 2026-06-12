using LibraryService.WebAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryService.WebAPI.Services
{
    public class FraudService : IFraudService
    {
        private readonly FraudContext _fraudContext;

        public FraudService(FraudContext fraudContext)
        {
            _fraudContext = fraudContext;
        }

        public async Task<IEnumerable<Fraud>> GetAllAsync()
        {
            return await _fraudContext.Frauds
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();
        }

        public async Task<Fraud> AddAsync(Fraud fraud)
        {
            fraud.CreatedAt = DateTime.UtcNow;
            await _fraudContext.Frauds.AddAsync(fraud);
            await _fraudContext.SaveChangesAsync();
            return fraud;
        }
    }

    public interface IFraudService
    {
        Task<IEnumerable<Fraud>> GetAllAsync();

        Task<Fraud> AddAsync(Fraud fraud);
    }
}
