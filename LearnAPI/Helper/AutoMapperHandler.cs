using AutoMapper;
using LearnAPI.Modal;
using LearnAPI.Repos.Models;

namespace LearnAPI.Helper
{
    public class AutoMapperHandler:Profile
    {
        public AutoMapperHandler()
        {
            //converting TblCustomer to Customermodal
            CreateMap<TblCustomer, Customermodal>().ForMember(item => item.Statusname, opt => opt.MapFrom(
                item => (item.IsActive !=null && item.IsActive.Value) ? "Active" : "In Active")).ReverseMap();


            CreateMap<CreateUser, TblUser>();
        }
    }
}
