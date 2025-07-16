using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Models.Dtos;
using WebAPI.Repositories.Results;

namespace WebAPI.Services
{
    public class StatsControllerService
    {
        private readonly AppDbContext appDbContext;
        public StatsControllerService(AppDbContext _appDbContext)
        {
            this.appDbContext = _appDbContext;
        }


        public async Task<IDataResult<DifferenceLabeledDto>> GetDifferenceLabeledImageCount(int doctor1Id, int doctor2Id)
        {
            try
            {
                var doctor1Birads = await appDbContext.BreastBiradsEntities
                    .Where(b => b.DoctorId == doctor1Id)
                    .GroupBy(b => b.BreastBiradsId)
                    .Select(g => new LabelCountDto
                    {
                        EtiketId = (int)g.Key,
                        Count = g.Count()
                    })
                    .ToListAsync();

                var doctor2Birads = await appDbContext.BreastBiradsEntities
                    .Where(b => b.DoctorId == doctor2Id)
                    .GroupBy(b => b.BreastBiradsId)
                    .Select(g => new LabelCountDto
                    {
                        EtiketId = (int)g.Key,
                        Count = g.Count()
                    })
                    .ToListAsync();

                var doctor1Findings = await appDbContext.FindingCategoriesEntities
                    .Where(f => f.DoctorId == doctor1Id)
                    .GroupBy(f => f.FindingCategoriesId)
                    .Select(g => new LabelCountDto
                    {
                        EtiketId = g.Key,
                        Count = g.Count()
                    })
                    .ToListAsync();

                var doctor2Findings = await appDbContext.FindingCategoriesEntities
                    .Where(f => f.DoctorId == doctor2Id)
                    .GroupBy(f => f.FindingCategoriesId)
                    .Select(g => new LabelCountDto
                    {
                        EtiketId = g.Key,
                        Count = g.Count()
                    })
                    .ToListAsync();

                var result = new DifferenceLabeledDto
                {
                    Doctor1Id = doctor1Id,
                    Doctor2Id = doctor2Id,
                    Doctor1BiradsCounts = doctor1Birads,
                    Doctor2BiradsCounts = doctor2Birads,
                    Doctor1FindingCounts = doctor1Findings,
                    Doctor2FindingCounts = doctor2Findings
                };

                return new SuccessDataResult<DifferenceLabeledDto>(result, "Etiket istatistikleri başarıyla getirildi.");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<DifferenceLabeledDto>($"Hata oluştu: {ex.Message}");
            }
        }
    }
}
