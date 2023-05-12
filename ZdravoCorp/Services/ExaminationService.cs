using ZdravoCorp.Repositories;
using System.Collections.Generic;
using ZdravoCorp.Domain;

namespace ZdravoCorp.Services;

public class ExaminationService
{
    private ExaminationRepository _examinationRepository;

    public ExaminationService()
    {
        _examinationRepository = new ExaminationRepository();
        _examinationRepository.LoadExaminationsFromJson();
    }

    public List<Examination> GetAllExaminations()
    {
        return _examinationRepository.GetAllExaminations();
    }
    
    public Examination GetExaminationById(int id)
    {
        return _examinationRepository.GetExaminationById(id);
    }

    public void AddExamination(Examination examination)
    {
        _examinationRepository.AddExamination(examination);
    }

    public int GenerateId()
    {
        return _examinationRepository.GenerateId();
    }

    public void Refresh()
    {
        _examinationRepository.LoadExaminationsFromJson();
    }
    
}