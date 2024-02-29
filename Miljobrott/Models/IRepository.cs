

namespace Miljobrott.Models
{
    public interface IRepository
    {
        IQueryable<Department> Departments { get; }
        IQueryable<Employee> Employees { get; }
        IQueryable<Errand> Errands{ get; }
        IQueryable<ErrandStatus> ErrandStatuses { get; }



        //Read
        Task<Errand> GetErrandDet(int id);
        //Create and update
        void SaveErrand(Errand errand);

        void UpdateManager(int eId, string employeeId, bool check, string comment);


        void UpdateDepartment(int eId, string depId);

        void SaveSample(string sampleName, int eId);

        void SaveImage(string picName, int eId);

        void UpdateInvestigator(int eId, string events, string information, string StatusId);

        IQueryable<MyErrand> GetErrandListDetail();

        IQueryable<MyErrand> GetManagerList();
        IQueryable<MyErrand> GetInvestigatorList();
        IQueryable<Employee> GetDepartmentEmp();

        string GetNameAndLogin();

    }
}
