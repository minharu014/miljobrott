using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Miljobrott.Models.Models;
using System.Runtime.Intrinsics.Arm;

namespace Miljobrott.Models
{
    public class EFRepository : IRepository
    {
        private readonly ApplicationDbContext context;
        private IHttpContextAccessor contextAcc;
    

        public EFRepository(ApplicationDbContext ctx, IHttpContextAccessor cont)
        {
            context = ctx;
            contextAcc = cont;
        }

        public IQueryable<Errand> Errands => context.Errands.Include(e => e.Samples).Include(e => e.Pictures);
        public IQueryable<Department> Departments => context.Departments;
        public IQueryable<Employee> Employees => context.Employees;
        public IQueryable<ErrandStatus> ErrandStatuses => context.ErrandStatuses;
        public IQueryable<Sequence> Sequences => context.Sequences;

        public Task<Errand> GetErrandDet(int id)
        {
            return Task.Run(() =>
            {
                var errandDetail = Errands.Where(td => td.ErrandId == id).First();
                return errandDetail;
            });
        }


        public void UpdateManager(int eId, string employeeId, bool check, string comment)
        {
            var errand = context.Errands.Find(eId);
            if (check)
            {
                errand.StatusId = "S_B";
                errand.InvestigatorInfo = comment;
            }
            else
            {
                if (!(employeeId == "Välj"))
                {
                    errand.EmployeeId = employeeId;
                }
            }
            context.SaveChanges();
        }


        public void SaveErrand(Errand errand)
        {
            if (errand.ErrandId == 0) 
            { 
            Sequence seq = Sequences.First();
            errand.StatusId = "S_A";
            errand.RefNumber = "2023-45-" + seq.CurrentValue;
            context.Errands.Add(errand);
            seq.CurrentValue++;
            }
            context.SaveChanges();
            }

        public void UpdateDepartment(int eId, string depId)
        {
            var errand = context.Errands.Find(eId);
            if (!(depId == "Välj" || depId == "D00"))
            {
                errand.DepartmentId = depId;
            }
            context.SaveChanges();
        }


        public void UpdateInvestigator(int eId, string events, string information, string StatusId)
        {
            var errand = context.Errands.Find(eId);
            if (events != null)
            {
                errand.InvestigatorAction = errand.InvestigatorAction + " " + events;
            }
            if (information != null)
            {
                errand.InvestigatorInfo = errand.InvestigatorInfo + " " + information;

            }
            if (!(StatusId == "Välj"))
            {
                errand.StatusId = StatusId;
            }
            context.SaveChanges();
        }

        // spara sample
        public void SaveSample(string sampleName, int eId)
        {
            Sample sample = new Sample();
            sample.SampleName = sampleName;
            sample.ErrandID = eId;
            var sam = context.Samples.Add(sample);

            context.SaveChanges();
        }

        //spara bild
        public void SaveImage(string picName, int eId)
        {
            Picture picture = new Picture();
            picture.PictureName = picName;
            picture.ErrandID = eId;
            var pic = context.Pictures.Add(picture);

            context.SaveChanges();
        }
        /*
         * hämta alla erands.
         video: rätt data
         */
        public IQueryable<MyErrand> GetErrandListDetail()
        {

            var errandList = from err in Errands
                             join stat in ErrandStatuses on err.StatusId equals stat.StatusId

                             join dep in Departments on err.DepartmentId equals dep.DepartmentId
                                into departmentErrand from deptE in departmentErrand.DefaultIfEmpty()

                             join em in Employees on err.EmployeeId equals em.EmployeeId
                                into employeeErrand from empE in employeeErrand.DefaultIfEmpty()

                             orderby err.RefNumber descending


                            select new MyErrand()
                            {
                                DateOfObservation = err.DateOfObservation,
                                ErrandId = err.ErrandId,
                                RefNumber = err.RefNumber,
                                TypeOfCrime = err.TypeOfCrime,
                                StatusName = stat.StatusName,
                                DepartmentName = (err.DepartmentId == null ? "ej tillsatt" : deptE.DepartmentName),
                                EmployeeName = (err.EmployeeId == null ? "ej tillsatt" : empE.EmployeeName)
                            };      
            return errandList;
        }

        /*
         hämta namn och användarnamn på en användare.
         */
        public string GetNameAndLogin()
        {
            var userName = contextAcc.HttpContext.User.Identity.Name;
            var employee = Employees.FirstOrDefault(e => e.EmployeeId == userName);
            var name = employee.EmployeeName;
            var NameAndID = userName + " " + name;
            return NameAndID;
        }

        /*
         * hämta dep av användare
         */
        public string GetCurrentDep(string userName)
        {
            var userDep = from emp in Employees
                          where emp.EmployeeId == userName
                          join dep in Departments on emp.DepartmentId equals dep.DepartmentId
                          select dep.DepartmentName;
            return userDep.First();
        }

        /***
         hämta ärande lista för specifik manager
         */
        public IQueryable<MyErrand> GetManagerList()
        {
            var userName = contextAcc.HttpContext.User.Identity.Name; //e100
            var employee = Employees.FirstOrDefault(e => e.EmployeeId == userName);
            var department = Departments.FirstOrDefault(d => d.DepartmentId == employee.DepartmentId);
            var managerList = GetErrandListDetail().Where(err => err.DepartmentName == department.DepartmentName);
            return managerList;
        }

        /*
         hämta ärande lista för specifik investigator
         */
        public IQueryable<MyErrand> GetInvestigatorList()
        {
            var userName = contextAcc.HttpContext.User.Identity.Name;
            var employee = Employees.FirstOrDefault(e => e.EmployeeId == userName); 
            var department = Departments.FirstOrDefault(d => d.DepartmentId == employee.DepartmentId);
            var investigatorList = GetErrandListDetail().Where(err => err.EmployeeName == employee.EmployeeName);
            return investigatorList;
        }
        /*
         hämta investigators som är i samma department.
         */
        public IQueryable<Employee> GetDepartmentEmp()
        {
            var userName = contextAcc.HttpContext.User.Identity.Name;
            var employee = Employees.FirstOrDefault(e => e.EmployeeId == userName);
            var department = Departments.FirstOrDefault(d => d.DepartmentId == employee.DepartmentId);
            var investigatorList = Employees.Where(emp => emp.DepartmentId == department.DepartmentId && emp.RoleTitle == "Investigator");
            return investigatorList;
        }

    }
}