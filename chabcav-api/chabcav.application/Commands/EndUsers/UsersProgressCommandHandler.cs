using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using chabcav.domain.Entities;
using chabcav.domain.Interfaces;
using MediatR;

namespace chabcav.application.Commands.EndUsers
{
    public class UsersProgressCommandHandler : IRequestHandler<UsersProgressCommand, bool>
    {
        private readonly IUsersProgressRepository _repository;

        public UsersProgressCommandHandler(IUsersProgressRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(UsersProgressCommand request, CancellationToken cancellationToken)
        {
            var progress = new UsersProgress
            {
                UserId = request.UserId,
                ChapterName = request.ChapterName,
                DateCompleted = DateTime.UtcNow,
                Status = "Chapter Completed"
            };

            return await _repository.AddUserProgressAsync(progress);
        }
    }


}
