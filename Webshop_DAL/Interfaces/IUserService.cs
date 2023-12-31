using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop_DAL.Models;

namespace Webshop_DAL.Interfaces
{
    public interface IUserService : IBaseService<User>
    {
        bool Update(AdminUpdateFormDTO user, int id);
        User Login(LoginUserFormDTO loginForm);
        void Register(CreateUserDTO user);
        bool Update(VendeurUpdateFormDTO vendeurUpdateForm, int id);
        bool Update(UserUpdateFormDTO userForm, int id);
        User CreateVendeur(CreateVendeurDTO newVendeur);
    }
}
