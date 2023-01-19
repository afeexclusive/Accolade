using AccoladesData.Entities;
using Application.ViewModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IStudentService
    {
        Guid UserId { get; set; }

        Task<List<EmailHeadingVM>> CreateEmailMap(IFormFile file, string userId);
        Task<List<string>> CreateMapHeadings(IFormFile file, string userId);
        List<string> GetAllHeaders(string userID);
        List<Student> GetAllStudents(string userid);
        List<EmailHeadingVM> GetEmailHeaders(string userID);
        Task<List<string>> ReadAccolades(IFormFile file, string userId);
        ResponseViewModel UpdateStudentEmail(Student model, string userId);
        List<CorpMemberDefualtData> UploadCorperData(IFormFile file);
        ResponseViewModel UploadStudentData(Student model, string userId);
    }
}
