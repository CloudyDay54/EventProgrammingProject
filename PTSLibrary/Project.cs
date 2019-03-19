using System;
using System.Collections.Generic;
using System.Text;

namespace PTSLibrary
{
    class Project
    {
        private string name;
        private DateTime expectedStartDate;
        private DateTime expectedEndDate;
        private Customer theCustomer;
        private Guid projectId;
        private List<Task> tasks;

        public Project(string name, DateTime startDate, DateTime endDate, Guid projectId, Customer customer)
        {
            this.name = name;
            this.expectedStartDate = startDate;
            this.expectedEndDate = endDate;
            this.projectId = projectId;
            this.theCustomer = customer;
        }

        public Project(string name, DateTime startDate, DateTime endDate, Guid projectId, List<Task> tasks)
        {
            this.name = name;
            this.expectedStartDate = startDate;
            this.expectedEndDate = endDate;
            this.projectId = projectId;
            this.tasks = tasks;
        }
    }
}
