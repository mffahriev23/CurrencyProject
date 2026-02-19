using Application.UnitOfWork;
using MediatR;
using UserService.Application.Interfaces;
using UserService.Application.Repositories;
using UserService.Domain.Entities;

namespace UserService.Application.Users.Commands.Registration
{
    public class RegistrationUserCommandHandler : IRequestHandler<RegistrationUserCommand>
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IHasher _passwordHasher;
        readonly IUserRepository _userRepository;

        public RegistrationUserCommandHandler(
            IUnitOfWork unitOfWork,
            IHasher passwordHasher,
            IUserRepository userRepository

        )
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
        }

        public async Task Handle(RegistrationUserCommand request, CancellationToken cancellationToken)
        {
            if (!request.Password.Equals(request.DoublePassword))
            {
                throw new ArgumentException("Введённые пароли не совпадают");
            }

            User user = new()
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Password = _passwordHasher.GetHash(request.Password),
            };

            _userRepository.Add(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
