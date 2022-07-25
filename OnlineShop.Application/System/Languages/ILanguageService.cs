using OnlineShop.ViewModels.Common;
using OnlineShop.ViewModels.System.Languages;
using OnlineShop.ViewModels.System.Users;
using OnlineShop.ViewModels.Utilities.Slides;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.System.Languages
{
    public interface ILanguageService
    {
        Task<ApiResult<List<LanguageViewModel>>> GetAll();
    }
}
