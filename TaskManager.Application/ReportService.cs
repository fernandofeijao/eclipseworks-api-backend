using FluentResults;
using AutoMapper;
using System.Net;
using TaskManager.DomainCore;
using AutoMapper.Internal.Mappers;

namespace TaskManager.Application;

public class ReportService : IReportService
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IReportRepository _reportRepository;

    public ReportService(IMapper mapper, IReportRepository reportRepository, IUserRepository userRepository)
    {
        _mapper = mapper;
        _reportRepository = reportRepository;
        _userRepository = userRepository;
    }

    public async Task<Result<List<TaskReportDTO>>> GetTaskByUser(ReportFilterDTO reportDto)
    {
        //Partindo da premissa que a API terá autenticação, essa validação é desnecessária
        var userAction = await _userRepository.GetAsync(reportDto.ActionUser ?? "");
        if (userAction is null)
            return Result.Fail("O usuário informado no header não existe.");

        if (userAction.Profile != (byte)UserProfileEnum.Manager)
            return Result.Fail("Não autorizado.");

        return await _reportRepository.GetTaskByUserAsync(reportDto);
    }
}
