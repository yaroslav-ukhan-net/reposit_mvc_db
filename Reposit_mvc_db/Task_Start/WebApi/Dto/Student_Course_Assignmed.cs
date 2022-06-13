using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Dto
{
    public class Student_Course_Assignmed
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int PassCredits { get; set; }

        public List<AssignmedStudent_Viewmodel> Students { get; set; }
    }

    public class AssignmedStudent_Viewmodel
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public bool IsAssigned { get; set; }
    }
}
