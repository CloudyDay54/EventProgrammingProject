using System;
using System.Collections.Generic;
using System.Text;

namespace PTSLibrary
{
    class PTSAdminFacade : PTSSuperFacade
    {
        private DAO.ClientDAO dao;

        public PTSAdminFacade() : base(new DAO.AdminDAO())
        {
            dao = (DAO.AdminDAO)base.dao;
        }

        public TeamLeader Authenticate(string username, string password)
        {
            if (username == "" || password == "")
            {
                throw new Exception("Missing Data");
            }
            return dao.Authenticate(username, password);
        }

        public Project[] GetListOfProjects(int teamId)
        {
            return (dao.GetListOfProjects(teamId)).ToArray();
        }
    }
}

