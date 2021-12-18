using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBank.Server.Entities
{
    internal interface IApplicantRepository
    {
        public Task<Response> ApplyToProjectAsync(int projectId, int userId);

        public Task<Response> HasAlreadyAppliedToProjectAsync(int projectId, int userId);

        public  Task<List<SimplifiedProjectDTO>> ShowListOfAppliedProjectsAsync(int userId);

        public  Task<int> SelectNrOfProjectApplicationsAsync(int projectId);

        public  Task<Response> DeleteApplicationsAsync(int projectId);

    }
}
