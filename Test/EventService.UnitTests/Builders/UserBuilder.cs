using EventService.Application.UseCases.Events.ViewModels;
using EventService.Domain.Entities;

namespace EventService.UnitTests.Builders
{
    public class UserBuilder
    {
        private readonly UserData _user;

        public UserBuilder()
        {
            _user = DefaultUser();
        }

        public UserData BuildUser()
        {
            return _user;
        }

        public UserData DefaultUser()
        {
            return new UserData {
                Id = 1,
                Name = "Leanne Graham"
            };
        }
    }
}
