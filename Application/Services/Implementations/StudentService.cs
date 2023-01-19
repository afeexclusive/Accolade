using AccoladesData;
using AccoladesData.Entities;
using Application.ExcelHelper;
using Application.Services.Interfaces;
using Application.ViewModel;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Implementations
{
    public class StudentService : IStudentService
    {
        private readonly IStorageBroker<Student> storage;
        private readonly IStorageBroker<string> headerStore;
        private readonly IStorageBroker<EmailHeadingVM> emailStore;

        public Guid UserId { get; set; }

        public StudentService(IStorageBroker<Student> storage, IStorageBroker<string> headerStore, IStorageBroker<EmailHeadingVM> emailStore)
        {

            this.storage = storage;
            this.headerStore = headerStore;
            this.emailStore = emailStore;
        }

        public ResponseViewModel UploadStudentData(Student model, string userId)
        {
           
            if (!string.IsNullOrWhiteSpace(model.FileName))
            {
                if (model.FileName.EndsWith(".csv"))
                {
                    try
                    {
                        var fi = new FileInfo(($"./Data/{model.FileName}"));
                        var fs = fi.Open(FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);
                        using (var reader = new StreamReader(fs))
                        using (var csv = new CsvReader(reader))
                        {
                            csv.Read();
                            csv.ReadHeader();
                            string[] headerRow = csv.Context.HeaderRecord;

                            var map = new StudentsMap(model);

                            //csv.Configuration.BadDataFound = null;
                            //csv.Configuration.HeaderValidated = null;
                            //csv.Configuration.MissingFieldFound = null;

                            csv.Configuration.RegisterClassMap(map);
                            var records = csv.GetRecords<Student>().ToList();
                            records.ForEach(x => x.Id = Guid.NewGuid());



                            storage.UserId = userId;
                            storage.PostRange(records);

                            return new ResponseViewModel { Status = true, Response = "Successful", ReturnObject = records };
                        }
                    }
                    catch (Exception e)
                    {
                        return new ResponseViewModel { Status = false, Response = e.Message};
                    }
                }
                return new ResponseViewModel { Response = "Only csv file is allowed", Status = false };
            }
            return new ResponseViewModel { Response = "Uploaded file is empty", Status = false };
        }


        public List<CorpMemberDefualtData> UploadCorperData(IFormFile file)
        {

            if (file.Length > 0)
            {
                if (file.FileName.EndsWith(".csv"))
                {
                    try
                    {
                        using (var reader = new StreamReader(file.OpenReadStream()))
                        using (var csv = new CsvReader(reader))
                        {

                            //csv.Configuration.BadDataFound = null;
                            //csv.Configuration.HeaderValidated = null;
                            //csv.Configuration.MissingFieldFound = null;

                            var records = csv.GetRecords<CorpMember>().ToList();
                            var groupedRec = records.GroupBy(x => x.StateCode);
                            
                            List<CorpMemberDefualtData> defualtDatas = new List<CorpMemberDefualtData>();
                            foreach (var item in groupedRec)
                            {
                                var firstRec = item.First();
                                defualtDatas.Add(new CorpMemberDefualtData
                                {
                                    StateCode = item.Key,
                                    LastName = firstRec.LastName,
                                    LGA = firstRec.LGA,
                                    OtherNames = firstRec.OtherNames,
                                    PhoneNumber = firstRec.PhoneNumber,
                                    Months = string.Join(", ", item.Select(x=>x.Month).ToList())

                                });
                            }

                            return defualtDatas;
                        }
                    }
                    catch (Exception e)
                    {
                        return null;
                    }
                }
                return null;
            }
            return null;
        }

        public ResponseViewModel UpdateStudentEmail(Student model, string userId)
        {

            if (!string.IsNullOrWhiteSpace(model.FileName))
            {
                if (model.FileName.EndsWith(".csv"))
                {
                    try
                    {
                        var fi = new FileInfo(($"./Data/{model.FileName}"));
                        var fs = fi.Open(FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);
                        using (var reader = new StreamReader(fs))
                        using (var csv = new CsvReader(reader))
                        {
                            csv.Read();
                            csv.ReadHeader();
                            string[] headerRow = csv.Context.HeaderRecord;
                            
                            var map = new StudentEmailMap(model);

                            csv.Configuration.RegisterClassMap(map);
                            var records = csv.GetRecords<Student>().ToList();

                            storage.UserId = userId;
                            List<Student> students = storage.GetAll().ToList();

                            List<Student> studentsToPost = new List<Student>();

                            foreach (var student in students)
                            {
                                var isInRecord = records.FirstOrDefault(x => x.FirstName == student.FirstName);
                                if (isInRecord != null)
                                {
                                    student.Email = isInRecord.Email;
                                    student.DateOfBirth = isInRecord.DateOfBirth;
                                    studentsToPost.Add(student);
                                }
                                else
                                {
                                    studentsToPost.Add(student);
                                }
                            }

                            if (File.Exists($"./Data/student_{userId}.json"))
                            {
                                File.Delete($"./Data/student_{userId}.json");

                            }

                            storage.UserId = userId;
                            storage.PostRange(studentsToPost);

                            return new ResponseViewModel { Status = true, Response = "Successful", ReturnObject = records };
                        }
                    }
                    catch (Exception e)
                    {
                        return new ResponseViewModel { Status = false, Response = e.Message };
                    }
                }
                return new ResponseViewModel { Response = "Only csv file is allowed", Status = false };
            }
            return new ResponseViewModel { Response = "Uploaded file is empty", Status = false };
        }

        public async Task<List<string>> CreateMapHeadings(IFormFile file, string userId)
        {
            string[] headerRow;
            try
            {
                //Read the headers in the csv file
                using (var reader = new StreamReader(file.OpenReadStream()))
                using (var csv = new CsvReader(reader))
                {
                    csv.Read();
                    csv.ReadHeader();
                    headerRow = csv.Context.HeaderRecord;
                    
                }
                var headerList = headerRow.ToList();
                var savingFilename = $"std_{userId}_{file.FileName}";

                //Delete the csv file if it exists so as to clear the former upload ///==> May also look into deleting the files after certificate processing.
                if (File.Exists($"./Data/{savingFilename}"))
                {
                    File.Delete($"./Data/{savingFilename}");
                }

                //Save the csv file in the server to pull it during upload
                using (Stream fileStream = new FileStream($"./Data/{savingFilename}", FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }


                //Delete the header row file if it exists so as to clear the former data ///==> May also look into deleting the files after certificate processing.
                if (File.Exists($"./Data/string_{userId}.json"))
                {
                    File.Delete($"./Data/string_{userId}.json");

                }


                headerList.Add(savingFilename);
                headerStore.UserId = userId;
                headerStore.PostRange(headerList);
            }
            catch (Exception e) 
            {
                return new List<string>(){ e.Message };
            }
            return headerRow.ToList();
        }

        public async Task<List<EmailHeadingVM>> CreateEmailMap(IFormFile file, string userId)
        {
            string[] headerRow;
            List<EmailHeadingVM> emailHeaders = null;
            try
            {
                //Read the headers in the csv file
                using (var reader = new StreamReader(file.OpenReadStream()))
                using (var csv = new CsvReader(reader))
                {
                    csv.Read();
                    csv.ReadHeader();
                    headerRow = csv.Context.HeaderRecord;

                }
                var headerList = headerRow.ToList();
                var savingFilename = $"std_email_{userId}_{file.FileName}";

                //Delete the csv file if it exists so as to clear the former upload ///==> May also look into deleting the files after certificate processing.
                if (File.Exists($"./Data/{savingFilename}"))
                {
                    File.Delete($"./Data/{savingFilename}");
                }

                //Save the csv file in the server to pull it during upload
                using (Stream fileStream = new FileStream($"./Data/{savingFilename}", FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }


                //Delete the header row file if it exists so as to clear the former data ///==> May also look into deleting the files after certificate processing.
                
                if (File.Exists($"./Data/emailheadingvm_{userId}.json"))
                {
                   File.Delete($"./Data/emailheadingvm_{userId}.json");
                    
                }


                headerList.Add(savingFilename);
                emailStore.UserId = userId;
                emailHeaders = headerList.Select(x => new EmailHeadingVM { Heading = x, Sn = headerList.IndexOf(x) }).ToList();

                emailStore.PostRange(emailHeaders);
            }
            catch (Exception e)
            {
                return new List<EmailHeadingVM>() { new EmailHeadingVM { Sn = 0, Heading = e.Message } };
            }
            return emailHeaders;
        }

        public List<string> GetAllHeaders(string userID)
        {
            headerStore.UserId = userID;
            return headerStore.GetAll().ToList();
        }

        public List<EmailHeadingVM> GetEmailHeaders(string userID)
        {
            emailStore.UserId = userID;
            return emailStore.GetAll().ToList();
        }

        public List<Student> GetAllStudents(string userid)
        {
            storage.UserId = userid;
            return storage.GetAll().ToList();
        }

        public async Task<List<string>> ReadAccolades(IFormFile file, string userId)
        {
            string[] headerRow;
            try
            {
                //Read the headers in the csv file
                using (var reader = new StreamReader(file.OpenReadStream()))
                using (var csv = new CsvReader(reader))
                {
                    csv.Read();
                    csv.ReadHeader();
                    headerRow = csv.Context.HeaderRecord;

                    var rawData = csv.GetRecords<object>();
                    var rawDatas = csv.Context.RawRecord;
                    var rawDatasa = csv.Context.Record;


                }
                var headerList = headerRow.ToList();
                var savingFilename = $"std_{userId}_{file.FileName}";

                //Delete the csv file if it exists so as to clear the former upload ///==> May also look into deleting the files after certificate processing.
                if (File.Exists($"./Data/{savingFilename}"))
                {
                    File.Delete($"./Data/{savingFilename}");
                }

                //Save the csv file in the server to pull it during upload
                using (Stream fileStream = new FileStream($"./Data/{savingFilename}", FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }


                //Delete the header row file if it exists so as to clear the former data ///==> May also look into deleting the files after certificate processing.
                if (File.Exists($"./Data/string_{userId}.json"))
                {
                    File.Delete($"./Data/string_{userId}.json");

                }


                headerList.Add(savingFilename);
                headerStore.UserId = userId;
                headerStore.PostRange(headerList);
            }
            catch (Exception e)
            {
                return new List<string>() { e.Message };
            }
            return headerRow.ToList();
        }

    }


}


//if (file != null)
//{



//    if (file.FileName.EndsWith(".csv"))
//    {
//        try
//        {
//            using (var reader = new StreamReader(file.OpenReadStream()))
//            using (var csv = new CsvReader(reader))
//            {
//                csv.Read();
//                csv.ReadHeader();
//                string[] headerRow = csv.Context.HeaderRecord;

//                csv.Configuration.BadDataFound = null;
//                csv.Configuration.HeaderValidated = null;
//                csv.Configuration.MissingFieldFound = null;
//                csv.Configuration.RegisterClassMap<StudentsMap>();
//                var records = csv.GetRecords<Student>().ToList();
//                records.ForEach(x => x.Id = Guid.NewGuid());
//                storage.PostRange(records);

//                return new ResponseViewModel { Status = true, Response = "Successful", ReturnObject = records };
//            }
//        }
//        catch (Exception e)
//        {
//            return new ResponseViewModel { Status = false, Response = e.Message };
//        }
//    }
//    return new ResponseViewModel { Response = "Only csv file is allowed", Status = false };
//}